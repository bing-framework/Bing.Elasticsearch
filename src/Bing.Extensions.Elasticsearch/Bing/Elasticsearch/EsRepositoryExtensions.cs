using Bing.Data.Queries;
using Bing.Elasticsearch.Models;
using Bing.Elasticsearch.Repositories;
using Bing.Extensions;

namespace Bing.Elasticsearch
{
    /// <summary>
    /// ES仓储(<see cref="IEsRepository{TEntity}"/>) 扩展
    /// </summary>
    public static class EsRepositoryExtensions
    {
        /// <summary>
        /// 搜索
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="repository">仓储</param>
        /// <param name="query">查询参数</param>
        public static EsPageSearch<TEntity> Search<TEntity>(this IEsRepository<TEntity> repository, IQueryParameter query)
            where TEntity : class
        {
            repository.CheckNull(nameof(repository));
            query.CheckNull(nameof(query));
            return new EsPageSearch<TEntity>(repository.GetContext(), query);
        }
    }
}