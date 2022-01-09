using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bing.Elasticsearch.Options;
using Bing.Elasticsearch.Provider;
using Bing.Elasticsearch.Repositories;
using Bing.Extensions;
using Microsoft.Extensions.Options;
using Nest;

namespace Bing.Elasticsearch
{
    /// <summary>
    /// ES上下文
    /// </summary>
    public class ElasticsearchContext : IElasticsearchContext
    {
        /// <summary>
        /// ES客户端提供程序
        /// </summary>
        private readonly IElasticClientProvider _provider;

        /// <summary>
        /// 索引名称解析器
        /// </summary>
        private readonly IIndexNameResolver _resolver;

        /// <summary>
        /// ES客户端
        /// </summary>
        private readonly IElasticClient _client;

        /// <summary>
        /// ES选项配置
        /// </summary>
        private readonly ElasticsearchOptions _options;

        /// <summary>
        /// 初始化一个<see cref="ElasticsearchContext"/>类型的实例
        /// </summary>
        /// <param name="provider">ES客户端提供程序</param>
        /// <param name="resolver">索引名称解析器</param>
        /// <param name="options">ES选项配置</param>
        public ElasticsearchContext(IElasticClientProvider provider, IIndexNameResolver resolver, IOptions<ElasticsearchOptions> options)
        {
            _provider = provider;
            _resolver = resolver;
            _client = provider.GetClient();
            _options = options.Value;
        }

        /// <summary>
        /// 是否存在指定索引别名
        /// </summary>
        /// <param name="alias">别名名称。注意：必须小写</param>
        /// <param name="cancellationToken">取消令牌</param>
        public async Task<bool> AliasExistsAsync(string alias, CancellationToken cancellationToken = default)
        {
            if (alias.IsEmpty())
                return false;
            var result = await _client.Indices.AliasExistsAsync(alias, ct: cancellationToken);
            return result.Exists;
        }

        /// <summary>
        /// 是否存在指定索引名称
        /// </summary>
        /// <param name="index">索引名称。注意：必须小写</param>
        /// <param name="cancellation">取消令牌</param>
        public async Task<bool> IndexExistsAsync(string index, CancellationToken cancellation = default)
        {
            if (index.IsEmpty())
                throw new ArgumentNullException(nameof(index));
            var result = await _client.Indices.ExistsAsync(_resolver.GetIndexName(index), ct: cancellation);
            return result.Exists;
        }

        /// <summary>
        /// 获取索引名称列表
        /// </summary>
        /// <param name="alias">别名名称。注意：必须小写</param>
        /// <param name="cancellationToken">取消令牌</param>
        public async Task<List<string>> GetIndexesByAliasAsync(string alias, CancellationToken cancellationToken = default)
        {
            if (alias.IsEmpty())
                return new List<string>();
            var result = await _client.GetIndicesPointingToAliasAsync(alias);
            return result.ToList();
        }

        /// <summary>
        /// 创建索引
        /// </summary>
        /// <param name="index">索引名称。注意：必须小写</param>
        /// <param name="alias">别名名称。注意：必须小写</param>
        /// <param name="selector">映射操作</param>
        /// <param name="cancellationToken">取消令牌</param>
        public async Task<CreateIndexResponse> CreateIndexAsync(string index, string alias = null, Func<CreateIndexDescriptor, ICreateIndexRequest> selector = null, CancellationToken cancellationToken = default)
        {
            var result = await _client.Indices.CreateAsync(_resolver.GetIndexName(index), selector, cancellationToken);
            if (alias.IsEmpty() == false)
                await _client.Indices.PutAliasAsync(_resolver.GetIndexName(index), alias, ct: cancellationToken);
            return result;
        }

        /// <summary>
        /// 创建索引
        /// </summary>
        /// <typeparam name="TDocument">文档类型</typeparam>
        /// <param name="index">索引名称。注意：必须小写</param>
        /// <param name="alias">别名名称。注意：必须小写</param>
        /// <param name="cancellationToken">取消令牌</param>
        public async Task CreateIndexAsync<TDocument>(string index, string alias = null, CancellationToken cancellationToken = default) where TDocument : class
        {
            await _client.CreateIndexAsync<TDocument>(index, _options.NumberOfShards, _options.NumberOfReplicas,cancellationToken);
            if (alias.IsEmpty() == false)
                await _client.Indices.PutAliasAsync(index, alias, ct: cancellationToken);
        }

        /// <summary>
        /// 删除索引
        /// </summary>
        /// <typeparam name="TDocument">文档类型</typeparam>
        /// <param name="cancellationToken">取消令牌</param>
        public async Task<DeleteIndexResponse> DeleteIndexAsync<TDocument>(CancellationToken cancellationToken = default) where TDocument : class
        {
            var index = GetIndexName<TDocument>();
            return await _client.Indices.DeleteAsync(index, ct: cancellationToken);
        }

        /// <summary>
        /// 删除索引
        /// </summary>
        /// <param name="index">索引名称。注意：必须小写</param>
        /// <param name="cancellationToken">取消令牌</param>
        public async Task<DeleteIndexResponse> DeleteIndexAsync(string index, CancellationToken cancellationToken = default)
        {
            return await _client.Indices.DeleteAsync(_resolver.GetIndexName(index), ct: cancellationToken);
        }

        /// <summary>
        /// 通过别名删除索引集合
        /// </summary>
        /// <param name="alias">别名名称。注意：必须小写</param>
        /// <param name="cancellationToken">取消令牌</param>
        public async Task DeleteIndexesByAliasAsync(string alias, CancellationToken cancellationToken = default)
        {
            if (alias.IsEmpty())
                return;
            var indexes = await GetIndexesByAliasAsync(alias,cancellationToken);
            foreach (var index in indexes) 
                await DeleteIndexAsync(index,cancellationToken);
        }

        /// <summary>
        /// 删除全部索引
        /// </summary>
        /// <param name="cancellationToken">取消令牌</param>
        public async Task<DeleteIndexResponse> DeleteAllIndexAsync(CancellationToken cancellationToken = default) => await _client.Indices.DeleteAsync("_all",ct:cancellationToken);

        /// <summary>
        /// 添加索引列表到别名
        /// </summary>
        /// <param name="alias">别名名称。注意：必须小写</param>
        /// <param name="indexes">索引名称列表。注意：必须小写</param>
        public async Task AddIndexesToAliasAsync(string alias, params string[] indexes)
        {
            if (alias.IsEmpty())
                return;
            foreach (var index in indexes) 
                await _client.Indices.PutAliasAsync(_resolver.GetIndexName(index), alias);
        }

        /// <summary>
        /// 从别名中移除索引列表
        /// </summary>
        /// <param name="alias">别名名称。注意：必须小写</param>
        /// <param name="indexes">索引名称列表。注意：必须小写</param>
        public async Task RemoveIndexesFromAliasAsync(string alias, params string[] indexes)
        {
            if(alias.IsEmpty())
                return;
            foreach (var index in indexes) 
                await _client.Indices.DeleteAliasAsync(_resolver.GetIndexName(index), alias);
        }

        /// <summary>
        /// 获取索引名称
        /// </summary>
        /// <typeparam name="TDocument">文档类型</typeparam>
        /// <param name="index">索引名称。注意：必须小写</param>
        public string GetIndexName<TDocument>(string index = null)
        {
            if (index.IsEmpty() == false)
                return _resolver.GetIndexName(index);
            return _resolver.GetIndexName(typeof(TDocument).Name.ToLower());
        }

        /// <summary>
        /// 获取ES标识
        /// </summary>
        /// <param name="id">标识</param>
        public Id GetEsId(object id)
        {
            Id entityId;
            switch (id)
            {
                case long longId:
                    entityId = new Id(longId);
                    break;
                case string stringId:
                    entityId = new Id(stringId);
                    break;
                default:
                    entityId = new Id(id);
                    break;
            }

            return entityId;
        }

        /// <summary>
        /// 获取全部数据。
        /// </summary>
        /// <typeparam name="TResult">结果类型</typeparam>
        /// <param name="index">索引名称。注意：必须小写</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <remarks>说明：最多返回10000条</remarks>
        public async Task<List<TResult>> GetAllAsync<TResult>(string index = null, CancellationToken cancellationToken = default) where TResult : class
        {
            index = GetIndexName<TResult>(index);
            var result = await _client.SearchAsync<TResult>(s => s.Index(index).Size(10000).Query(q => q.MatchAll()),
                cancellationToken);
            return result.Documents.ToList();
        }

        /// <summary>
        /// 获取客户端
        /// </summary>
        public IElasticClient GetClient() => _client;

        /// <summary>
        /// 获取客户端
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        public IElasticClient GetClient<T>() => _provider.GetClient(GetIndexName<T>());

        /// <summary>
        /// 添加文档
        /// </summary>
        /// <typeparam name="TDocument">文档类型</typeparam>
        /// <param name="document">文档对象</param>
        /// <param name="index">索引名称。注意：必须小写</param>
        /// <param name="id">文档标识</param>
        /// <param name="cancellationToken">取消令牌</param>
        public async Task<IndexResponse> AddAsync<TDocument>(TDocument document, string index = null, object id = null, CancellationToken cancellationToken = default) 
            where TDocument : class
        {
            index = GetIndexName<TDocument>(index);
            if (id == null)
                return await _client.IndexAsync(document, x => x.Index(index), cancellationToken);
            return await _client.IndexAsync(document, x => x.Index(index).Id(GetEsId(id)), cancellationToken);
        }

        /// <summary>
        /// 批量保存文档
        /// </summary>
        /// <typeparam name="TDocument">文档类型</typeparam>
        /// <param name="documents">文档对象列表</param>
        /// <param name="index">索引名称。注意：必须小写</param>
        /// <param name="timeout">超时时间间隔。单位：毫秒，默认值：300000，即5分钟</param>
        /// <param name="cancellationToken">取消令牌</param>
        public async Task<BulkResponse> BulkSaveAsync<TDocument>(IEnumerable<TDocument> documents, string index = null, double timeout = 300000, CancellationToken cancellationToken = default) where TDocument : class
        {
            index = GetIndexName<TDocument>(index);
            return await _client.BulkAsync(x => x
                    .Index(index)
                    .IndexMany(documents)
                    .Timeout(timeout),
                cancellationToken);
        }

        /// <summary>
        /// 删除文档
        /// </summary>
        /// <typeparam name="TDocument">文档类型</typeparam>
        /// <param name="id">文档标识</param>
        /// <param name="index">索引名称。注意：必须小写</param>
        /// <param name="cancellationToken">取消令牌</param>
        public async Task<DeleteResponse> DeleteAsync<TDocument>(object id, string index = null, CancellationToken cancellationToken = default) where TDocument : class
        {
            index = GetIndexName<TDocument>(index);
            return await _client.DeleteAsync<TDocument>(GetEsId(id), x => x.Index(index), cancellationToken);
        }
    }
}
