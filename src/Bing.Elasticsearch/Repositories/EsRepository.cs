using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Bing.Elasticsearch.Model;
using Bing.Elasticsearch.Options;
using Bing.Elasticsearch.Provider;
using Microsoft.Extensions.Options;
using Nest;

namespace Bing.Elasticsearch.Repositories
{
    /// <summary>
    /// ES仓储基类
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    public class EsRepository<TEntity> : IEsRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// ES客户端
        /// </summary>
        private readonly IElasticClient _client;

        /// <summary>
        /// ES客户端提供程序
        /// </summary>
        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        private readonly IElasticClientProvider _provider;

        /// <summary>
        /// ES选项配置
        /// </summary>
        private readonly ElasticsearchOptions _options;

        /// <summary>
        /// 索引名称
        /// </summary>
        public string IndexName { get; protected set; }

        /// <summary>
        /// 初始化一个<see cref="EsRepository{TEntity}"/>类型的实例
        /// </summary>
        /// <param name="provider">ES客户端提供程序</param>
        /// <param name="options">ES选项配置</param>
        public EsRepository(IElasticClientProvider provider, IOptions<ElasticsearchOptions> options)
        {
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
            _options = options.Value ?? throw new ArgumentNullException(nameof(options));
            IndexName = typeof(TEntity).Name.ToLower();
            _client = _provider.GetClient();
        }

        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="cancellationToken">取消令牌</param>
        public Task InsertAsync(TEntity entity, CancellationToken cancellationToken = default) =>
            InsertAsync(entity, null, cancellationToken);

        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="indexName">索引名称。注意：必须小写</param>
        /// <param name="cancellationToken">取消令牌</param>
        public async Task InsertAsync(TEntity entity, string indexName, CancellationToken cancellationToken = default)
        {
            var targetIndexName = GetSafeIndexName(indexName);
            await ExistOrCreateAsync(targetIndexName, cancellationToken);
            var response = await _client.IndexAsync(entity, x => x.Index(targetIndexName), cancellationToken);
            if (!response.IsValid)
                throw new ElasticsearchException($"索引[{targetIndexName}]新增数据失败 : {response.ServerError.Error.Reason}");
        }

        /// <summary>
        /// 批量插入
        /// </summary>
        /// <param name="entities">实体集合</param>
        /// <param name="cancellationToken">取消令牌</param>
        public Task InsertManyAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default) =>
            InsertManyAsync(entities, null, cancellationToken);

        /// <summary>
        /// 批量插入
        /// </summary>
        /// <param name="entities">实体集合</param>
        /// <param name="indexName">索引名称。注意：必须小写</param>
        /// <param name="cancellationToken">取消令牌</param>
        public async Task InsertManyAsync(IEnumerable<TEntity> entities, string indexName, CancellationToken cancellationToken = default)
        {
            var targetIndexName = GetSafeIndexName(indexName);
            await ExistOrCreateAsync(targetIndexName, cancellationToken);
            var response = await _client.IndexManyAsync(entities, targetIndexName, cancellationToken);
            if (!response.IsValid)
                throw new ElasticsearchException($"索引[{targetIndexName}]批量新增数据失败 : {response.ServerError.Error.Reason}");
        }

        /// <summary>
        /// 批量操作
        /// </summary>
        /// <param name="entities">实体集合</param>
        /// <param name="cancellationToken">取消令牌</param>
        public Task BulkAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default) =>
            BulkAsync(entities, null, cancellationToken);

        /// <summary>
        /// 批量操作
        /// </summary>
        /// <param name="entities">实体集合</param>
        /// <param name="indexName">索引名称。注意：必须小写</param>
        /// <param name="cancellationToken">取消令牌</param>
        public async Task BulkAsync(IEnumerable<TEntity> entities, string indexName, CancellationToken cancellationToken = default)
        {
            var targetIndexName = GetSafeIndexName(indexName);
            await ExistOrCreateAsync(targetIndexName, cancellationToken);
            var response = await _client.BulkAsync(x => x.Index(targetIndexName).IndexMany(entities), cancellationToken);
            if (!response.IsValid)
                throw new ElasticsearchException($"索引[{targetIndexName}]批量保存数据失败：{response.ServerError.Error.Reason}");
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="cancellationToken">取消令牌</param>
        public Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default) =>
            DeleteAsync(entity, null, cancellationToken);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="indexName">索引名称。注意：必须小写</param>
        /// <param name="cancellationToken">取消令牌</param>
        public async Task DeleteAsync(TEntity entity, string indexName, CancellationToken cancellationToken = default)
        {
            var targetIndexName = GetSafeIndexName(indexName);
            var response = await _client.DeleteAsync<TEntity>(new Id(entity), x => x.Index(targetIndexName), cancellationToken);
            if (!response.IsValid)
                throw new ElasticsearchException($"索引[{targetIndexName}]删除数据失败 : {response.ServerError.Error.Reason}");
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">标识</param>
        /// <param name="cancellationToken">取消令牌</param>
        public Task DeleteAsync(object id, CancellationToken cancellationToken = default) =>
            DeleteAsync(id, null, cancellationToken);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">标识</param>
        /// <param name="indexName">索引名称。注意：必须小写</param>
        /// <param name="cancellationToken">取消令牌</param>
        public async Task DeleteAsync(object id, string indexName, CancellationToken cancellationToken = default)
        {
            var targetIndexName = GetSafeIndexName(indexName);
            var response = await _client.DeleteAsync<TEntity>(GetEsId(id), x => x.Index(targetIndexName), cancellationToken);
            if (!response.IsValid)
                throw new ElasticsearchException($"索引[{targetIndexName}]删除数据失败 : {response.ServerError.Error.Reason}");
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="cancellationToken">取消令牌</param>
        public Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default) =>
            UpdateAsync(entity, null, cancellationToken);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="indexName">索引名称。注意：必须小写</param>
        /// <param name="cancellationToken">取消令牌</param>
        public async Task UpdateAsync(TEntity entity, string indexName, CancellationToken cancellationToken = default)
        {
            var targetIndexName = GetSafeIndexName(indexName);
            var response = await _client.UpdateAsync<TEntity>(new Id(entity), x => x.Index(targetIndexName).Doc(entity), cancellationToken);
            if (!response.IsValid)
                throw new ElasticsearchException($"索引[{targetIndexName}]更新数据失败 : {response.ServerError.Error.Reason}");
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="id">标识</param>
        /// <param name="entity">实体</param>
        /// <param name="cancellationToken">取消令牌</param>
        public Task UpdateAsync(object id, TEntity entity, CancellationToken cancellationToken = default) =>
            UpdateAsync(id, entity, null, cancellationToken);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="id">标识</param>
        /// <param name="entity">实体</param>
        /// <param name="indexName">索引名称。注意：必须小写</param>
        /// <param name="cancellationToken">取消令牌</param>
        public async Task UpdateAsync(object id, TEntity entity, string indexName, CancellationToken cancellationToken = default)
        {
            var targetIndexName = GetSafeIndexName(indexName);
            var response = await _client.UpdateAsync<TEntity>(GetEsId(id), x => x.Index(targetIndexName).Doc(entity), cancellationToken);
            if (!response.IsValid)
                throw new ElasticsearchException($"索引[{targetIndexName}]更新数据失败 : {response.ServerError.Error.Reason}");
        }

        /// <summary>
        /// 通过标识查找
        /// </summary>
        /// <param name="id">标识</param>
        /// <param name="cancellationToken">取消令牌</param>
        public Task<TEntity> FindByIdAsync(object id, CancellationToken cancellationToken = default) =>
            FindByIdAsync(id, null, cancellationToken);

        /// <summary>
        /// 通过标识查找
        /// </summary>
        /// <param name="id">标识</param>
        /// <param name="indexName">索引名称。注意：必须小写</param>
        /// <param name="cancellationToken">取消令牌</param>
        public async Task<TEntity> FindByIdAsync(object id, string indexName, CancellationToken cancellationToken = default)
        {
            var targetIndexName = GetSafeIndexName(indexName);
            var response = await _client.GetAsync<TEntity>(GetEsId(id), x => x.Index(targetIndexName), cancellationToken);
            return response.IsValid ? response.Source : null;
        }

        /// <summary>
        /// 通过标识集合查找
        /// </summary>
        /// <param name="ids">标识集合</param>
        public Task<IEnumerable<TEntity>> FindByIdsAsync(params string[] ids) => FindByIdsAsync((IEnumerable<string>)ids);

        /// <summary>
        /// 通过标识集合查找
        /// </summary>
        /// <param name="indexName">索引名称。注意：必须小写</param>
        /// <param name="ids">标识集合</param>
        public Task<IEnumerable<TEntity>> FindByIdsAsync(string indexName, params string[] ids) =>
            FindByIdsAsync((IEnumerable<string>) ids, indexName);

        /// <summary>
        /// 通过标识集合查找
        /// </summary>
        /// <param name="ids">标识集合</param>
        public Task<IEnumerable<TEntity>> FindByIdsAsync(params long[] ids) => FindByIdsAsync((IEnumerable<long>)ids);

        /// <summary>
        /// 通过标识集合查找
        /// </summary>
        /// <param name="indexName">索引名称。注意：必须小写</param>
        /// <param name="ids">标识集合</param>
        public Task<IEnumerable<TEntity>> FindByIdsAsync(string indexName, params long[] ids) =>
            FindByIdsAsync((IEnumerable<long>) ids, indexName);

        /// <summary>
        /// 通过标识集合查找
        /// </summary>
        /// <param name="ids">标识集合</param>
        /// <param name="cancellationToken">取消令牌</param>
        public Task<IEnumerable<TEntity>> FindByIdsAsync(IEnumerable<string> ids,
            CancellationToken cancellationToken = default) => FindByIdsAsync(ids, null, cancellationToken);

        /// <summary>
        /// 通过标识集合查找
        /// </summary>
        /// <param name="ids">标识集合</param>
        /// <param name="indexName">索引名称。注意：必须小写</param>
        /// <param name="cancellationToken">取消令牌</param>
        public async Task<IEnumerable<TEntity>> FindByIdsAsync(IEnumerable<string> ids, string indexName, CancellationToken cancellationToken = default)
        {
            var targetIndexName = GetSafeIndexName(indexName);
            var result = new List<TEntity>();
            var response = await _client.GetManyAsync<TEntity>(ids, targetIndexName, cancellationToken);
            if ((response?.Count() ?? 0) != 0)
                result.AddRange(response.Select(x => x.Source));
            return result;
        }

        /// <summary>
        /// 通过标识集合查找
        /// </summary>
        /// <param name="ids">标识集合</param>
        /// <param name="cancellationToken">取消令牌</param>
        public Task<IEnumerable<TEntity>> FindByIdsAsync(IEnumerable<long> ids,
            CancellationToken cancellationToken = default) => FindByIdsAsync(ids, null, cancellationToken);

        /// <summary>
        /// 通过标识集合查找
        /// </summary>
        /// <param name="ids">标识集合</param>
        /// <param name="indexName">索引名称。注意：必须小写</param>
        /// <param name="cancellationToken">取消令牌</param>
        public async Task<IEnumerable<TEntity>> FindByIdsAsync(IEnumerable<long> ids, string indexName, CancellationToken cancellationToken = default)
        {
            var targetIndexName = GetSafeIndexName(indexName);
            var result = new List<TEntity>();
            var response = await _client.GetManyAsync<TEntity>(ids, targetIndexName, cancellationToken);
            if ((response?.Count() ?? 0) != 0)
                result.AddRange(response.Select(x => x.Source));
            return result;
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="descriptor">查询描述器</param>
        /// <param name="cancellationToken">取消令牌</param>
        public async Task<IQueryResult<TEntity>> SearchAsync(SearchDescriptor<TEntity> descriptor, CancellationToken cancellationToken = default)
        {
            var result = new List<TEntity>();
            descriptor = descriptor.Index(IndexName);
            Func<SearchDescriptor<TEntity>, ISearchRequest> selector = x => descriptor;
            var response = await _client.SearchAsync(selector, cancellationToken);
            if (response.ApiCall.RequestBodyInBytes != null)
            {
                var responseJson = Encoding.UTF8.GetString(response.ApiCall.RequestBodyInBytes);
            }

            if (!response.IsValid)
                return new CustomQueryResult<TEntity>();
            result.AddRange(response.Hits.Select(x => x.Source));
            return new CustomQueryResult<TEntity>
            {
                Data = result,
                Took = response.Took,
                TotalCount = response.Total
            };
        }

        /// <summary>
        /// 命中查询
        /// </summary>
        /// <param name="descriptor">查询描述符</param>
        /// <param name="cancellationToken">取消令牌</param>
        public async Task<IEnumerable<IHit<TEntity>>> HitsSearchAsync(SearchDescriptor<TEntity> descriptor, CancellationToken cancellationToken = default)
        {
            descriptor = descriptor.Index(IndexName);
            Func<SearchDescriptor<TEntity>, ISearchRequest> selector = x => descriptor;
            var response = await _client.SearchAsync(selector, cancellationToken);
            return response.IsValid ? response.Hits : null;
        }

        /// <summary>
        /// 聚合查询
        /// </summary>
        /// <param name="descriptor">查询描述符</param>
        /// <param name="key">键名</param>
        /// <param name="cancellationToken">取消令牌</param>
        public async Task<TermsAggregate<string>> AggregationsSearchAsync(SearchDescriptor<TEntity> descriptor, string key, CancellationToken cancellationToken = default)
        {
            descriptor = descriptor.Index(IndexName);
            Func<SearchDescriptor<TEntity>, ISearchRequest> selector = x => descriptor;
            var response = await _client.SearchAsync(selector, cancellationToken);
            return response.IsValid ? response.Aggregations.Terms(key) : null;
        }

        /// <summary>
        /// 不存在则创建
        /// </summary>
        /// <param name="indexName">索引名称</param>
        /// <param name="cancellationToken">取消令牌</param>
        protected async Task ExistOrCreateAsync(string indexName, CancellationToken cancellationToken = default)
        {
            var result = await _client.Indices.ExistsAsync(indexName, null, cancellationToken);
            if (result.Exists)
                return;
            await _client.CreateIndexAsync<TEntity>(indexName, _options.NumberOfShards, _options.NumberOfReplicas, cancellationToken);
        }

        /// <summary>
        /// 获取ES标识
        /// </summary>
        /// <param name="id">标识</param>
        protected Id GetEsId(object id)
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
        /// 获取安全的索引名称
        /// </summary>
        /// <param name="indexName">索引名称</param>
        protected string GetSafeIndexName(string indexName) => string.IsNullOrWhiteSpace(indexName) ? IndexName : indexName;
    }
}
