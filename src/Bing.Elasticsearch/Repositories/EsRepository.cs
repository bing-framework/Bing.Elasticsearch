using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Bing.Elasticsearch.Internals;
using Bing.Elasticsearch.Model;
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
        /// ES上下文
        /// </summary>
        protected IElasticsearchContext Context { get;  set; }

        /// <summary>
        /// 索引名称
        /// </summary>
        protected virtual string IndexName { get; set; }

        /// <summary>
        /// 初始化一个<see cref="EsRepository{TEntity}"/>类型的实例
        /// </summary>
        /// <param name="context">ES上下文</param>
        public EsRepository(IElasticsearchContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
            _client = Context.GetClient();
            IndexName = Helper.SafeIndexName<TEntity>(IndexName);
        }

        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="cancellationToken">取消令牌</param>
        public async Task InsertAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            var indexName = Helper.SafeIndexName<TEntity>(IndexName);
            await ExistOrCreateAsync(indexName, cancellationToken);
            var response = await Context.AddAsync(entity, indexName, cancellationToken: cancellationToken);
            if (!response.IsValid)
                throw new ElasticsearchException($"索引[{indexName}]新增数据失败 : {response.ServerError.Error.Reason}");
        }

        /// <summary>
        /// 批量插入
        /// </summary>
        /// <param name="entities">实体集合</param>
        /// <param name="cancellationToken">取消令牌</param>
        public async Task InsertManyAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            var indexName = Helper.SafeIndexName<TEntity>(IndexName);
            await ExistOrCreateAsync(indexName, cancellationToken);
            var response = await Context.BulkSaveAsync(entities, indexName, cancellationToken: cancellationToken);
            if (!response.IsValid)
                throw new ElasticsearchException($"索引[{indexName}]批量新增数据失败 : {response.ServerError.Error.Reason}");
        }

        /// <summary>
        /// 批量操作
        /// </summary>
        /// <param name="entities">实体集合</param>
        /// <param name="cancellationToken">取消令牌</param>
        public async Task BulkAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            var indexName = Helper.SafeIndexName<TEntity>(IndexName);
            await ExistOrCreateAsync(indexName, cancellationToken);
            var response = await Context.BulkSaveAsync(entities, indexName, cancellationToken: cancellationToken);
            if (!response.IsValid)
                throw new ElasticsearchException($"索引[{indexName}]批量保存数据失败：{response.ServerError.Error.Reason}");
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="cancellationToken">取消令牌</param>
        public async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            var indexName = Helper.SafeIndexName<TEntity>(IndexName);
            var response = await Context.DeleteAsync<TEntity>(entity, indexName, cancellationToken);
            if (!response.IsValid)
                throw new ElasticsearchException($"索引[{indexName}]删除数据失败 : {response.ServerError.Error.Reason}");
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">标识</param>
        /// <param name="cancellationToken">取消令牌</param>
        public async Task DeleteAsync(object id, CancellationToken cancellationToken = default)
        {
            var indexName = Helper.SafeIndexName<TEntity>(IndexName);
            var response = await Context.DeleteAsync<TEntity>(id, indexName, cancellationToken);
            if (!response.IsValid)
                throw new ElasticsearchException($"索引[{indexName}]删除数据失败 : {response.ServerError.Error.Reason}");
        }

        /// <summary>
        /// 按查询条件删除
        /// </summary>
        /// <param name="descriptor">查询删除描述符</param>
        /// <param name="cancellationToken">取消令牌</param>
        public async Task DeleteByQueryAsync(DeleteByQueryDescriptor<TEntity> descriptor, CancellationToken cancellationToken = default)
        {
            var indexName = Helper.SafeIndexName<TEntity>(IndexName);
            descriptor = descriptor.Index(Context.GetIndexName<TEntity>(IndexName));
            Func<DeleteByQueryDescriptor<TEntity>, IDeleteByQueryRequest> selector = x => descriptor;
            var response = await Context.DeleteByQueryAsync(selector, cancellationToken);
            if(!response.IsValid)
                throw new ElasticsearchException($"索引[{indexName}]删除数据失败 : {response.ServerError.Error.Reason}");
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="cancellationToken">取消令牌</param>
        public async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            var indexName = Helper.SafeIndexName<TEntity>(IndexName);
            var response = await Context.UpdateAsync(entity, indexName, cancellationToken);
            if (!response.IsValid)
                throw new ElasticsearchException($"索引[{indexName}]更新数据失败 : {response.ServerError.Error.Reason}");
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="id">标识</param>
        /// <param name="entity">实体</param>
        /// <param name="cancellationToken">取消令牌</param>
        public async Task UpdateAsync(object id, TEntity entity, CancellationToken cancellationToken = default)
        {
            var indexName = Helper.SafeIndexName<TEntity>(IndexName);
            var response = await Context.UpdateAsync(id, entity, indexName, cancellationToken);
            if (!response.IsValid)
                throw new ElasticsearchException($"索引[{indexName}]更新数据失败 : {response.ServerError.Error.Reason}");
        }

        /// <summary>
        /// 按查询条件更新
        /// </summary>
        /// <param name="descriptor">查询更新描述符</param>
        /// <param name="cancellationToken">取消令牌</param>
        public async Task UpdateByQueryAsync(UpdateByQueryDescriptor<TEntity> descriptor, CancellationToken cancellationToken = default)
        {
            var indexName = Helper.SafeIndexName<TEntity>(IndexName);
            descriptor = descriptor.Index(Context.GetIndexName<TEntity>(IndexName));
            Func<UpdateByQueryDescriptor<TEntity>, IUpdateByQueryRequest> selector = x => descriptor;
            var response = await Context.UpdateByQueryAsync(selector, cancellationToken);
            if (!response.IsValid)
                throw new ElasticsearchException($"索引[{indexName}]更新数据失败 : {response.ServerError.Error.Reason}");
        }

        /// <summary>
        /// 通过标识查找
        /// </summary>
        /// <param name="id">标识</param>
        /// <param name="cancellationToken">取消令牌</param>
        public async Task<TEntity> FindByIdAsync(object id, CancellationToken cancellationToken = default)
        {
            var indexName = Helper.SafeIndexName<TEntity>(IndexName);
            return await Context.FindByIdAsync<TEntity>(id, indexName, cancellationToken);
        }

        /// <summary>
        /// 通过标识集合查找
        /// </summary>
        /// <param name="ids">标识集合</param>
        public Task<IEnumerable<TEntity>> FindByIdsAsync(params string[] ids) => 
            FindByIdsAsync((IEnumerable<string>)ids);

        /// <summary>
        /// 通过标识集合查找
        /// </summary>
        /// <param name="ids">标识集合</param>
        public Task<IEnumerable<TEntity>> FindByIdsAsync(params long[] ids) => FindByIdsAsync((IEnumerable<long>)ids);

        /// <summary>
        /// 通过标识集合查找
        /// </summary>
        /// <param name="ids">标识集合</param>
        /// <param name="cancellationToken">取消令牌</param>
        public async Task<IEnumerable<TEntity>> FindByIdsAsync(IEnumerable<string> ids, CancellationToken cancellationToken = default)
        {
            var indexName = Helper.SafeIndexName<TEntity>(IndexName);
            return await Context.FindByIdsAsync<TEntity>(ids, indexName, cancellationToken);
        }

        /// <summary>
        /// 通过标识集合查找
        /// </summary>
        /// <param name="ids">标识集合</param>
        /// <param name="cancellationToken">取消令牌</param>
        public async Task<IEnumerable<TEntity>> FindByIdsAsync(IEnumerable<long> ids, CancellationToken cancellationToken = default)
        {
            var indexName = Helper.SafeIndexName<TEntity>(IndexName);
            return await Context.FindByIdsAsync<TEntity>(ids, indexName, cancellationToken);
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="descriptor">查询描述器</param>
        /// <param name="cancellationToken">取消令牌</param>
        public async Task<IQueryResult<TEntity>> SearchAsync(SearchDescriptor<TEntity> descriptor, CancellationToken cancellationToken = default)
        {
            var result = new List<TEntity>();
            descriptor = descriptor.Index(Context.GetIndexName<TEntity>(IndexName));
            Func<SearchDescriptor<TEntity>, ISearchRequest> selector = x => descriptor;
            var response = await Context.SearchAsync(selector, cancellationToken);
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
            descriptor = descriptor.Index(Context.GetIndexName<TEntity>(IndexName));
            Func<SearchDescriptor<TEntity>, ISearchRequest> selector = x => descriptor;
            var response = await Context.SearchAsync(selector, cancellationToken);
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
            descriptor = descriptor.Index(Context.GetIndexName<TEntity>(IndexName));
            Func<SearchDescriptor<TEntity>, ISearchRequest> selector = x => descriptor;
            var response = await Context.SearchAsync(selector, cancellationToken);
            return response.IsValid ? response.Aggregations.Terms(key) : null;
        }

        /// <summary>
        /// 获取ES上下文
        /// </summary>
        public IElasticsearchContext GetContext() => Context;

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
            await Context.CreateIndexAsync<TEntity>(indexName, null, cancellationToken);
        }
    }
}
