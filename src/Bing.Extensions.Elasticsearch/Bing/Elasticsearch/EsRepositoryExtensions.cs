using Bing.Data.Queries;
using Bing.Elasticsearch.Models;
using Bing.Elasticsearch.Repositories;
using Bing.Extensions;

namespace Bing.Elasticsearch;

/// <summary>
/// ES仓储(<see cref="IEsRepository{TEntity}"/>) 扩展
/// </summary>
public static class EsRepositoryExtensions
{
    /// <summary>
    /// 搜索
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="context">ES上下文</param>
    public static EsSearch<TEntity> Search<TEntity>(this IEsRepository<TEntity> context)
        where TEntity : class
    {
        context.CheckNull(nameof(context));
        return new EsSearch<TEntity>(context.GetContext());
    }

    /// <summary>
    /// 分页搜索
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="repository">仓储</param>
    /// <param name="query">查询参数</param>
    public static EsPageSearch<TEntity> PageSearch<TEntity>(this IEsRepository<TEntity> repository, IQueryParameter query)
        where TEntity : class
    {
        repository.CheckNull(nameof(repository));
        query.CheckNull(nameof(query));
        return new EsPageSearch<TEntity>(repository.GetContext(), query);
    }

    /// <summary>
    /// 滚动搜索
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="repository">仓储</param>
    public static EsScrollAllSearch<TEntity> ScrollAllSearch<TEntity>(this IEsRepository<TEntity> repository)
        where TEntity : class
    {
        repository.CheckNull(nameof(repository));
        return new EsScrollAllSearch<TEntity>(repository.GetContext());
    }

    /// <summary>
    /// 滚动搜索
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TResult">结果类型</typeparam>
    /// <param name="repository">仓储</param>
    public static EsScrollAllSearch<TEntity, TResult> ScrollAllSearch<TEntity, TResult>(this IEsRepository<TEntity> repository)
        where TEntity : class
        where TResult : class
    {
        repository.CheckNull(nameof(repository));
        return new EsScrollAllSearch<TEntity, TResult>(repository.GetContext());
    }
}