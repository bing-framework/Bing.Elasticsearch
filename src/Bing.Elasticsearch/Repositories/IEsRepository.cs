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
    public interface IEsRepository<TEntity> where TEntity : class
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
        /// 通过标识查找
        /// </summary>
        /// <param name="id">标识</param>
        /// <param name="cancellationToken">取消令牌</param>
        Task<TEntity> FindByIdAsync(object id, CancellationToken cancellationToken = default);

        /// <summary>
        /// 通过标识集合查找
        /// </summary>
        /// <param name="ids">标识集合</param>
        Task<IEnumerable<TEntity>> FindByIdsAsync(params string[] ids);

        /// <summary>
        /// 通过标识集合查找
        /// </summary>
        /// <param name="ids">标识集合</param>
        Task<IEnumerable<TEntity>> FindByIdsAsync(params long[] ids);

        /// <summary>
        /// 通过标识集合查找
        /// </summary>
        /// <param name="ids">标识集合</param>
        /// <param name="cancellationToken">取消令牌</param>
        Task<IEnumerable<TEntity>> FindByIdsAsync(IEnumerable<string> ids, CancellationToken cancellationToken = default);

        /// <summary>
        /// 通过标识集合查找
        /// </summary>
        /// <param name="ids">标识集合</param>
        /// <param name="cancellationToken">取消令牌</param>
        Task<IEnumerable<TEntity>> FindByIdsAsync(IEnumerable<long> ids, CancellationToken cancellationToken = default);

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
    }

}
