using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bing.Elasticsearch.Exceptions;
using Bing.Elasticsearch.Internals;
using Bing.Elasticsearch.Mapping;
using Bing.Elasticsearch.Options;
using Bing.Elasticsearch.Provider;
using Bing.Elasticsearch.Repositories;
using Bing.Extensions;
using Elasticsearch.Net;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
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
        /// ES映射工厂
        /// </summary>
        private readonly IElasticMappingFactory _mappingFactory;

        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// 初始化一个<see cref="ElasticsearchContext"/>类型的实例
        /// </summary>
        /// <param name="provider">ES客户端提供程序</param>
        /// <param name="resolver">索引名称解析器</param>
        /// <param name="mappingFactory">ES映射工厂</param>
        /// <param name="options">ES选项配置</param>
        /// <param name="loggerFactory">日志工厂</param>
        public ElasticsearchContext(
            IElasticClientProvider provider,
            IIndexNameResolver resolver,
            IElasticMappingFactory mappingFactory,
            IOptions<ElasticsearchOptions> options,
            ILoggerFactory loggerFactory = null)
        {
            LoggerFactory = loggerFactory ?? NullLoggerFactory.Instance;
            _provider = provider;
            _resolver = resolver;
            _mappingFactory = mappingFactory;
            _client = provider.GetClient();
            _options = options.Value;
            _logger = loggerFactory.CreateLogger(GetType());
        }

        /// <summary>
        /// 日志工厂
        /// </summary>
        public ILoggerFactory LoggerFactory { get; }

        /// <summary>
        /// 是否存在指定索引别名
        /// </summary>
        /// <param name="alias">别名名称。注意：必须小写</param>
        /// <param name="cancellationToken">取消令牌</param>
        public async Task<bool> AliasExistsAsync(string alias, CancellationToken cancellationToken = default)
        {
            if (alias.IsEmpty())
                return false;
            var response = await _client.Indices.AliasExistsAsync(alias, ct: cancellationToken);
            if (response.IsValid)
                _logger.LogRequest(response);
            return response.Exists;
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
            index = GetIndexName(index);
            var response = await _client.Indices.ExistsAsync(index, ct: cancellation);
            if (response.IsValid)
                _logger.LogRequest(response);
            return response.Exists;
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
            if (result.IsValid)
                _logger.LogRequest(result);
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
            index = GetIndexName<TDocument>(index);
            var mapping = _mappingFactory.GetMapping<TDocument>();
            var result = await _client.Indices.CreateAsync(index, x =>
            {
                mapping.Map(x);
                return x;
            }, cancellationToken);
            if (result.IsValid || result.ServerError?.Status == 400 &&
                (result.ServerError.Error.Type == "index_already_exists_exception" ||
                 result.ServerError.Error.Type == "resource_already_exists_exception"))
            {
                _logger.LogRequest(result);
                if (alias.IsEmpty() == false)
                {
                    var aliasResult = await _client.Indices.PutAliasAsync(index, alias, ct: cancellationToken);
                    if (aliasResult.IsValid)
                        _logger.LogRequest(aliasResult);
                }
                return;
            }

            throw new EsRepositoryException(result.GetErrorMessage($"Error creating the index {index}"), result.OriginalException);
        }

        /// <summary>
        /// 删除索引
        /// </summary>
        /// <typeparam name="TDocument">文档类型</typeparam>
        /// <param name="cancellationToken">取消令牌</param>
        public async Task<DeleteIndexResponse> DeleteIndexAsync<TDocument>(CancellationToken cancellationToken = default) where TDocument : class
        {
            var index = Helper.SafeIndexName<TDocument>();
            return await DeleteIndexAsync(index, cancellationToken);
        }

        /// <summary>
        /// 删除索引
        /// </summary>
        /// <param name="index">索引名称。注意：必须小写</param>
        /// <param name="cancellationToken">取消令牌</param>
        public async Task<DeleteIndexResponse> DeleteIndexAsync(string index, CancellationToken cancellationToken = default)
        {
            index = GetIndexName(index);
            var response = await _client.Indices.DeleteAsync(index, ct: cancellationToken);
            if (response.IsValid)
                _logger.LogRequest(response);
            return response;
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
            var indexes = await GetIndexesByAliasAsync(alias, cancellationToken);
            await DeleteIndexesAsync(indexes.ToArray(), cancellationToken);
        }

        /// <summary>
        /// 删除多个索引
        /// </summary>
        /// <param name="names">索引名称数组</param>
        /// <param name="cancellationToken">取消令牌</param>
        protected async Task DeleteIndexesAsync(string[] names, CancellationToken cancellationToken = default)
        {
            if (names == null || names.Length == 0)
                throw new ArgumentNullException(nameof(names));
            var response = await _client.Indices.DeleteAsync(Indices.Index(names), i => i.IgnoreUnavailable(), cancellationToken);
            if (response.IsValid)
            {
                _logger.LogRequest(response);
                return;
            }

            throw new EsRepositoryException(response.GetErrorMessage("Error deleting the index {names}"), response.OriginalException);
        }

        /// <summary>
        /// 删除全部索引
        /// </summary>
        /// <param name="cancellationToken">取消令牌</param>
        public async Task<DeleteIndexResponse> DeleteAllIndexAsync(CancellationToken cancellationToken = default)
        {
            var response = await _client.Indices.DeleteAsync("_all", ct: cancellationToken);
            if (response.IsValid)
                _logger.LogRequest(response);
            return response;
        }

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
                await _client.Indices.PutAliasAsync(GetIndexName(index), alias);
        }

        /// <summary>
        /// 从别名中移除索引列表
        /// </summary>
        /// <param name="alias">别名名称。注意：必须小写</param>
        /// <param name="indexes">索引名称列表。注意：必须小写</param>
        public async Task RemoveIndexesFromAliasAsync(string alias, params string[] indexes)
        {
            if (alias.IsEmpty())
                return;
            foreach (var index in indexes)
                await _client.Indices.DeleteAliasAsync(GetIndexName(index), alias);
        }

        /// <summary>
        /// 获取索引名称
        /// </summary>
        /// <typeparam name="TDocument">文档类型</typeparam>
        /// <param name="index">索引名称。注意：必须小写</param>
        public string GetIndexName<TDocument>(string index = null) => _resolver.GetIndexName(Helper.SafeIndexName<TDocument>(index));

        /// <summary>
        /// 获取索引名称
        /// </summary>
        /// <param name="index">索引名称。注意：必须小写</param>
        public string GetIndexName(string index) => _resolver.GetIndexName(index);

        /// <summary>
        /// 获取全部数据。
        /// </summary>
        /// <typeparam name="TDocument">文档类型</typeparam>
        /// <param name="index">索引名称。注意：必须小写</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <remarks>说明：最多返回10000条</remarks>
        public async Task<List<TDocument>> GetAllAsync<TDocument>(string index = null, CancellationToken cancellationToken = default)
            where TDocument : class
        {
            index = GetIndexName(Helper.SafeIndexName<TDocument>(index));
            var response = await _client.SearchAsync<TDocument>(s => s.Index(index).Size(10000).Query(q => q.MatchAll()),
                cancellationToken);
            if (response.IsValid)
                _logger.LogRequest(response);
            return response.Documents.ToList();
        }

        /// <summary>
        /// 通过标识查找
        /// </summary>
        /// <typeparam name="TDocument">文档类型</typeparam>
        /// <param name="id">文档标识</param>
        /// <param name="index">索引名称。注意：必须小写</param>
        /// <param name="cancellationToken">取消令牌</param>
        public async Task<TDocument> FindByIdAsync<TDocument>(object id, string index = null, CancellationToken cancellationToken = default) where TDocument : class
        {
            index = GetIndexName(Helper.SafeIndexName<TDocument>(index));
            var response = await _client.GetAsync<TDocument>(Helper.GetEsId(id), x => x.Index(index), cancellationToken);
            _logger.LogRequest(response);
            return response.IsValid ? response.Source : null;
        }

        /// <summary>
        /// 通过标识集合查找
        /// </summary>
        /// <typeparam name="TDocument">文档类型</typeparam>
        /// <param name="ids">文档标识集合</param>
        /// <param name="index">索引名称。注意：必须小写</param>
        /// <param name="cancellationToken">取消令牌</param>
        public async Task<IEnumerable<TDocument>> FindByIdsAsync<TDocument>(IEnumerable<string> ids, string index = null, CancellationToken cancellationToken = default)
            where TDocument : class
        {
            index = GetIndexName(Helper.SafeIndexName<TDocument>(index));
            var result = new List<TDocument>();
            var response = await _client.GetManyAsync<TDocument>(ids, index, cancellationToken);
            if ((response?.Count() ?? 0) != 0)
                result.AddRange(response.Select(x => x.Source));
            return result;
        }

        /// <summary>
        /// 通过标识集合查找
        /// </summary>
        /// <typeparam name="TDocument">文档类型</typeparam>
        /// <param name="ids">文档标识集合</param>
        /// <param name="index">索引名称。注意：必须小写</param>
        /// <param name="cancellationToken">取消令牌</param>
        public async Task<IEnumerable<TDocument>> FindByIdsAsync<TDocument>(IEnumerable<long> ids, string index = null, CancellationToken cancellationToken = default)
            where TDocument : class
        {
            index = GetIndexName(Helper.SafeIndexName<TDocument>(index));
            var result = new List<TDocument>();
            var response = await _client.GetManyAsync<TDocument>(ids, index, cancellationToken);
            if ((response?.Count() ?? 0) != 0)
                result.AddRange(response.Select(x => x.Source));
            return result;
        }

        /// <summary>
        /// 通过标识集合查找
        /// </summary>
        /// <typeparam name="TDocument">文档类型</typeparam>
        /// <param name="index">索引名称。注意：必须小写</param>
        /// <param name="ids">文档标识集合</param>
        public Task<IEnumerable<TDocument>> FindByIdsAsync<TDocument>(string index, params string[] ids)
            where TDocument : class => FindByIdsAsync<TDocument>((IEnumerable<string>)ids, index);

        /// <summary>
        /// 通过标识集合查找
        /// </summary>
        /// <typeparam name="TDocument">文档类型</typeparam>
        /// <param name="index">索引名称。注意：必须小写</param>
        /// <param name="ids">文档标识集合</param>
        public Task<IEnumerable<TDocument>> FindByIdsAsync<TDocument>(string index, params long[] ids)
            where TDocument : class => FindByIdsAsync<TDocument>((IEnumerable<long>)ids, index);

        /// <summary>
        /// 查询
        /// </summary>
        /// <typeparam name="TDocument">文档类型</typeparam>
        /// <param name="searchTerms">查询条件</param>
        /// <param name="index">索引名称。注意：必须小写</param>
        /// <param name="cancellationToken">取消令牌</param>
        public async Task<ISearchResponse<TDocument>> SearchAsync<TDocument>(Func<QueryContainerDescriptor<TDocument>, QueryContainer> searchTerms = null, string index = null, CancellationToken cancellationToken = default)
            where TDocument : class
        {
            index = GetIndexName(Helper.SafeIndexName<TDocument>(index));
            var response = await _client.SearchAsync<TDocument>(x => x.Query(searchTerms).Index(index).Size(20), cancellationToken);
            _logger.LogRequest(response);
            return response;
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <typeparam name="TDocument">文档类型</typeparam>
        /// <param name="selector">查询表达式</param>
        /// <param name="cancellationToken">取消令牌</param>
        public async Task<ISearchResponse<TDocument>> SearchAsync<TDocument>(Func<SearchDescriptor<TDocument>, ISearchRequest> selector = null, CancellationToken cancellationToken = default)
            where TDocument : class
        {
            var response = await _client.SearchAsync<TDocument>(selector, cancellationToken);
            _logger.LogRequest(response);
            return response;
        }

        /// <summary>
        /// 获取客户端
        /// </summary>
        public IElasticClient GetClient() => _client;

        /// <summary>
        /// 获取客户端
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        public IElasticClient GetClient<T>() => _provider.GetClient(GetIndexName(Helper.SafeIndexName<T>()));

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
            index = GetIndexName(Helper.SafeIndexName<TDocument>(index));
            var response = id == null
                ? await _client.IndexAsync(document, x => x.Index(index), cancellationToken)
                : await _client.IndexAsync(document, x => x.Index(index).Id(Helper.GetEsId(id)), cancellationToken);
            _logger.LogRequest(response);
            return response;
        }

        /// <summary>
        /// 批量保存文档
        /// </summary>
        /// <typeparam name="TDocument">文档类型</typeparam>
        /// <param name="documents">文档对象列表</param>
        /// <param name="index">索引名称。注意：必须小写</param>
        /// <param name="timeout">超时时间间隔。单位：毫秒，默认值：300000，即5分钟</param>
        /// <param name="cancellationToken">取消令牌</param>
        public async Task<BulkResponse> BulkSaveAsync<TDocument>(IEnumerable<TDocument> documents, string index = null, double timeout = 300000, CancellationToken cancellationToken = default)
            where TDocument : class
        {
            index = GetIndexName(Helper.SafeIndexName<TDocument>(index));
            var response = await _client.BulkAsync(x => x
                     .Index(index)
                     .IndexMany(documents)
                     .Timeout(timeout),
                cancellationToken);
            _logger.LogRequest(response);
            return response;
        }

        /// <summary>
        /// 删除文档
        /// </summary>
        /// <typeparam name="TDocument">文档类型</typeparam>
        /// <param name="id">文档标识</param>
        /// <param name="index">索引名称。注意：必须小写</param>
        /// <param name="cancellationToken">取消令牌</param>
        public async Task<DeleteResponse> DeleteAsync<TDocument>(object id, string index = null, CancellationToken cancellationToken = default)
            where TDocument : class
        {
            index = GetIndexName(Helper.SafeIndexName<TDocument>(index));
            var response = await _client.DeleteAsync<TDocument>(Helper.GetEsId(id), x => x.Index(index), cancellationToken);
            _logger.LogRequest(response);
            return response;
        }

        /// <summary>
        /// 删除文档
        /// </summary>
        /// <typeparam name="TDocument">文档类型</typeparam>
        /// <param name="document">文档</param>
        /// <param name="index">索引名称。注意：必须小写</param>
        /// <param name="cancellationToken">取消令牌</param>
        public async Task<DeleteResponse> DeleteAsync<TDocument>(TDocument document, string index = null, CancellationToken cancellationToken = default)
            where TDocument : class
        {
            index = GetIndexName(Helper.SafeIndexName<TDocument>(index));
            var response = await _client.DeleteAsync<TDocument>(Helper.GetEsId(document), x => x.Index(index), cancellationToken);
            _logger.LogRequest(response);
            return response;
        }

        /// <summary>
        /// 按查询条件删除文档
        /// </summary>
        /// <typeparam name="TDocument">文档类型</typeparam>
        /// <param name="selector">删除表达式</param>
        /// <param name="cancellationToken">取消令牌</param>
        public async Task<DeleteByQueryResponse> DeleteByQueryAsync<TDocument>(Func<DeleteByQueryDescriptor<TDocument>, IDeleteByQueryRequest> selector, CancellationToken cancellationToken = default) where TDocument : class
        {
            var response = await _client.DeleteByQueryAsync(selector, cancellationToken);
            _logger.LogRequest(response);
            return response;
        }

        /// <summary>
        /// 更新文档
        /// </summary>
        /// <typeparam name="TDocument">文档类型</typeparam>
        /// <param name="document">文档</param>
        /// <param name="index">索引名称。注意：必须小写</param>
        /// <param name="cancellationToken">取消令牌</param>
        public async Task<UpdateResponse<TDocument>> UpdateAsync<TDocument>(TDocument document, string index = null, CancellationToken cancellationToken = default)
            where TDocument : class
        {
            index = GetIndexName(Helper.SafeIndexName<TDocument>(index));
            var response = await _client.UpdateAsync<TDocument>(Helper.GetEsId(document), x => x.Index(index).Doc(document), cancellationToken);
            _logger.LogRequest(response);
            return response;
        }

        /// <summary>
        /// 更新文档
        /// </summary>
        /// <typeparam name="TDocument">文档类型</typeparam>
        /// <param name="id">文档标识</param>
        /// <param name="document">文档</param>
        /// <param name="index">索引名称。注意：必须小写</param>
        /// <param name="cancellationToken">取消令牌</param>
        public async Task<UpdateResponse<TDocument>> UpdateAsync<TDocument>(object id, TDocument document, string index = null, CancellationToken cancellationToken = default)
            where TDocument : class
        {
            index = GetIndexName(Helper.SafeIndexName<TDocument>(index));
            var response = await _client.UpdateAsync<TDocument>(Helper.GetEsId(id), x => x.Index(index).Doc(document), cancellationToken);
            _logger.LogRequest(response);
            return response;
        }

        /// <summary>
        /// 按查询条件更新文档
        /// </summary>
        /// <typeparam name="TDocument">文档类型</typeparam>
        /// <param name="selector">更新表达式</param>
        /// <param name="cancellationToken">取消令牌</param>
        public async Task<UpdateByQueryResponse> UpdateByQueryAsync<TDocument>(Func<UpdateByQueryDescriptor<TDocument>, IUpdateByQueryRequest> selector, CancellationToken cancellationToken = default) where TDocument : class
        {
            var response = await _client.UpdateByQueryAsync(selector, cancellationToken);
            _logger.LogRequest(response);
            return response;
        }

        /// <summary>
        /// 是否存在指定文档标识
        /// </summary>
        /// <typeparam name="TDocument">文档类型</typeparam>
        /// <param name="id">文档标识</param>
        /// <param name="index">索引名称。注意：必须小写</param>
        /// <param name="cancellationToken">取消令牌</param>
        public async Task<bool> ExistsAsync<TDocument>(object id, string index = null, CancellationToken cancellationToken = default) where TDocument : class
        {
            index = GetIndexName(Helper.SafeIndexName<TDocument>(index));
            var response = await _client.DocumentExistsAsync<TDocument>(Helper.GetEsId(id), ct: cancellationToken);
            _logger.LogRequest(response);
            return response.Exists;
        }

        /// <summary>
        /// 获取文档计数
        /// </summary>
        /// <typeparam name="TDocument">文档类型</typeparam>
        /// <param name="index">索引名称。注意：必须小写</param>
        /// <param name="cancellationToken">取消令牌</param>
        public async Task<long> GetTotalCountAsync<TDocument>(string index = null, CancellationToken cancellationToken = default) where TDocument : class
        {
            index = GetIndexName(Helper.SafeIndexName<TDocument>(index));
            var search = new SearchDescriptor<TDocument>().MatchAll();
            var response = await _client.SearchAsync<TDocument>(search, cancellationToken);
            _logger.LogRequest(response);
            if (response.IsValid)
                return response.Total;
            throw new ElasticsearchClientException($"索引[{index}]获取文档计数失败 : {response.ServerError.Error.Reason}");
        }
    }
}
