using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Bing.Elasticsearch.Model;
using Nest;

namespace Bing.Elasticsearch.Repositories
{
    /// <summary>
    /// ES仓储
    /// </summary>
    /// <typeparam name="TEntity">实体类类型</typeparam>
    public interface IEsRepository<TEntity> : IEsReadOnlyRepository<TEntity>
        where TEntity : class
    {
        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="cancellationToken">取消令牌</param>
        Task InsertAsync(TEntity entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// 批量插入
        /// </summary>
        /// <param name="entities">实体集合</param>
        /// <param name="cancellationToken">取消令牌</param>
        Task InsertManyAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

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
        Task BulkInsertAsync(IEnumerable<TEntity> documents, int chunkSize = 1000, int backOffTime = 30, int retries = 3, double maxRuntime = 15, CancellationToken cancellationToken = default);

        /// <summary>
        /// 批量操作
        /// </summary>
        /// <param name="entities">实体集合</param>
        /// <param name="cancellationToken">取消令牌</param>
        Task BulkAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="cancellationToken">取消令牌</param>
        Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">标识</param>
        /// <param name="cancellationToken">取消令牌</param>
        Task DeleteAsync(object id, CancellationToken cancellationToken = default);

        /// <summary>
        /// 按查询条件删除
        /// </summary>
        /// <param name="descriptor">查询删除描述符</param>
        /// <param name="cancellationToken">取消令牌</param>
        Task DeleteByQueryAsync(DeleteByQueryDescriptor<TEntity> descriptor, CancellationToken cancellationToken = default);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="cancellationToken">取消令牌</param>
        Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="id">标识</param>
        /// <param name="entity">实体</param>
        /// <param name="cancellationToken">取消令牌</param>
        Task UpdateAsync(object id, TEntity entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// 按查询条件更新
        /// </summary>
        /// <param name="descriptor">查询更新描述符</param>
        /// <param name="cancellationToken">取消令牌</param>
        Task UpdateByQueryAsync(UpdateByQueryDescriptor<TEntity> descriptor, CancellationToken cancellationToken = default);

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="descriptor">查询描述器</param>
        /// <param name="cancellationToken">取消令牌</param>
        Task<IQueryResult<TEntity>> SearchAsync(SearchDescriptor<TEntity> descriptor, CancellationToken cancellationToken = default);

        /// <summary>
        /// 命中查询
        /// </summary>
        /// <param name="descriptor">查询描述符</param>
        /// <param name="cancellationToken">取消令牌</param>
        Task<IEnumerable<IHit<TEntity>>> HitsSearchAsync(SearchDescriptor<TEntity> descriptor, CancellationToken cancellationToken = default);

        /// <summary>
        /// 聚合查询
        /// </summary>
        /// <param name="descriptor">查询描述符</param>
        /// <param name="key">键名</param>
        /// <param name="cancellationToken">取消令牌</param>
        Task<TermsAggregate<string>> AggregationsSearchAsync(SearchDescriptor<TEntity> descriptor, string key, CancellationToken cancellationToken = default);

        /// <summary>
        /// 获取ES上下文
        /// </summary>
        IElasticsearchContext GetContext();
    }

}
