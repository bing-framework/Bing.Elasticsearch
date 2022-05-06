using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Bing.Elasticsearch.Exceptions;
using Bing.Elasticsearch.Internals;
using Bing.Elasticsearch.Model;
using Bing.Elasticsearch.Options;
using Microsoft.Extensions.Options;
using Nest;

namespace Bing.Elasticsearch.Repositories
{
    /// <summary>
    /// ES仓储基类
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    public class EsRepository<TEntity> : EsReadOnlyRepositoryBase<TEntity>, IEsRepository<TEntity>
        where TEntity : class
    {
        /// <summary>
        /// 初始化一个<see cref="EsRepository{TEntity}"/>类型的实例
        /// </summary>
        /// <param name="context">ES上下文</param>
        /// <param name="options">ES选项配置</param>
        public EsRepository(IElasticsearchContext context, IOptions<ElasticsearchOptions> options)
            : base(context, options)
        {
        }

        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="cancellationToken">取消令牌</param>
        public virtual async Task InsertAsync(TEntity entity, CancellationToken cancellationToken = default)
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
        public virtual async Task InsertManyAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            var indexName = Helper.SafeIndexName<TEntity>(IndexName);
            await ExistOrCreateAsync(indexName, cancellationToken);
            var response = await Context.BulkSaveAsync(entities, indexName, cancellationToken: cancellationToken);
            if (!response.IsValid)
                throw new ElasticsearchException($"索引[{indexName}]批量新增数据失败 : {response.ServerError.Error.Reason}");
        }

        /// <summary>
        /// 批量插入
        /// </summary>
        /// <param name="documents">文档集合</param>
        /// <param name="chunkSize">每次批量请求的数量。默认：51000</param>
        /// <param name="backOffTime">重试等待时间。默认：30s</param>
        /// <param name="retries">重试次数。默认：3</param>
        /// <param name="maxRuntime">最大运行时间。默认：15分钟</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <remarks>
        /// 参考链接：https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/indexing-documents.html
        /// </remarks>
        public virtual async Task BulkInsertAsync(IEnumerable<TEntity> documents, int chunkSize = 1000, int backOffTime = 30, int retries = 3, double maxRuntime = 15, CancellationToken cancellationToken = default)
        {
            var docs = documents?.ToList();
            if (docs == null || docs.Any(d => d == null))
                throw new ArgumentNullException(nameof(documents));
            if (docs.Count == 0)
                return;
            var indexName = Helper.SafeIndexName<TEntity>(IndexName);
            if (docs.Count <= chunkSize)
            {
                var response = await Client.BulkAsync(x =>
                {
                    x.Index(indexName);
                    x.IndexMany(docs);
                    return x;
                }, cancellationToken);
                if (response.IsValid)
                {
                    _logger.LogRequest(response);
                }
                else
                {
                    var message = "Error adding document";
                    if (response.ServerError?.Status == 400)
                        throw new DuplicateDocumentException(response.GetErrorMessage(message), response.OriginalException);
                    throw new DocumentException(response.GetErrorMessage(message), response.OriginalException);
                }
            }
            else
            {
                long totalCount = 0;
                Client.BulkAll(docs, x =>
                {
                    x.Index(indexName)
                        .BackOffTime($"{backOffTime}s")
                        .BackOffRetries(retries)
                        .RefreshOnCompleted()
                        .MaxDegreeOfParallelism(Environment.ProcessorCount)
                        .Size(chunkSize);
                    return x;
                }).Wait(TimeSpan.FromMinutes(maxRuntime), next =>
                {
                    Interlocked.Add(ref totalCount, next.Items.Count);
                });
            }
        }

        /// <summary>
        /// 批量操作
        /// </summary>
        /// <param name="entities">实体集合</param>
        /// <param name="cancellationToken">取消令牌</param>
        public virtual async Task BulkAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
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
        public virtual async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
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
        public virtual async Task DeleteAsync(object id, CancellationToken cancellationToken = default)
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
        public virtual async Task DeleteByQueryAsync(DeleteByQueryDescriptor<TEntity> descriptor, CancellationToken cancellationToken = default)
        {
            var indexName = Helper.SafeIndexName<TEntity>(IndexName);
            descriptor = descriptor.Index(Context.GetIndexName(IndexName));
            Func<DeleteByQueryDescriptor<TEntity>, IDeleteByQueryRequest> selector = x => descriptor;
            var response = await Context.DeleteByQueryAsync(selector, cancellationToken);
            if (!response.IsValid)
                throw new ElasticsearchException($"索引[{indexName}]删除数据失败 : {response.ServerError.Error.Reason}");
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="cancellationToken">取消令牌</param>
        public virtual async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
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
        public virtual async Task UpdateAsync(object id, TEntity entity, CancellationToken cancellationToken = default)
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
        public virtual async Task UpdateByQueryAsync(UpdateByQueryDescriptor<TEntity> descriptor, CancellationToken cancellationToken = default)
        {
            var indexName = Helper.SafeIndexName<TEntity>(IndexName);
            descriptor = descriptor.Index(Context.GetIndexName(IndexName));
            Func<UpdateByQueryDescriptor<TEntity>, IUpdateByQueryRequest> selector = x => descriptor;
            var response = await Context.UpdateByQueryAsync(selector, cancellationToken);
            if (!response.IsValid)
                throw new ElasticsearchException($"索引[{indexName}]更新数据失败 : {response.ServerError.Error.Reason}");
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="descriptor">查询描述器</param>
        /// <param name="cancellationToken">取消令牌</param>
        public virtual async Task<IQueryResult<TEntity>> SearchAsync(SearchDescriptor<TEntity> descriptor, CancellationToken cancellationToken = default)
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
        public virtual async Task<IEnumerable<IHit<TEntity>>> HitsSearchAsync(SearchDescriptor<TEntity> descriptor, CancellationToken cancellationToken = default)
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
        public virtual async Task<TermsAggregate<string>> AggregationsSearchAsync(SearchDescriptor<TEntity> descriptor, string key, CancellationToken cancellationToken = default)
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
        protected virtual async Task ExistOrCreateAsync(string indexName, CancellationToken cancellationToken = default)
        {
            if (!Options.CheckIndex)
                return;
            indexName = Context.GetIndexName(indexName);
            var result = await Client.Indices.ExistsAsync(indexName, null, cancellationToken);
            _logger.LogRequest(result);
            if (result.Exists)
                return;
            await Context.CreateIndexAsync<TEntity>(indexName, null, cancellationToken);
        }
    }
}
