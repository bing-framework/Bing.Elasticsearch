using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Bing.Elasticsearch.Repositories
{
    /// <summary>
    /// ES只读仓储
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    public interface IEsReadOnlyRepository<TEntity>
        where TEntity : class
    {
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
    }
}