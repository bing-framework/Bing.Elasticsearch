using Bing.Data.Queries;
using Bing.Elasticsearch.Models;
using Bing.Extensions;

namespace Bing.Elasticsearch;

/// <summary>
/// ES上下文(<see cref="IElasticsearchContext"/>) 扩展
/// </summary>
public static partial class ElasticsearchContextExtensions
{
    /// <summary>
    /// 搜索
    /// </summary>
    /// <typeparam name="TResult">结果类型</typeparam>
    /// <param name="context">ES上下文</param>
    public static EsSearch<TResult> Search<TResult>(this IElasticsearchContext context)
        where TResult : class
    {
        context.CheckNull(nameof(context));
        return new EsSearch<TResult>(context);
    }

    /// <summary>
    /// 分页搜索
    /// </summary>
    /// <typeparam name="TResult">结果类型</typeparam>
    /// <param name="context">ES上下文</param>
    /// <param name="query">查询参数</param>
    public static EsPageSearch<TResult> PageSearch<TResult>(this IElasticsearchContext context, IQueryParameter query)
        where TResult : class
    {
        context.CheckNull(nameof(context));
        query.CheckNull(nameof(query));
        return new EsPageSearch<TResult>(context, query);
    }

    /// <summary>
    /// 滚动搜索
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="context">ES上下文</param>
    public static EsScrollAllSearch<TEntity> ScrollAllSearch<TEntity>(this IElasticsearchContext context)
        where TEntity : class
    {
        context.CheckNull(nameof(context));
        return new EsScrollAllSearch<TEntity>(context);
    }

    /// <summary>
    /// 滚动搜索
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TResult">结果类型</typeparam>
    /// <param name="context">ES上下文</param>
    public static EsScrollAllSearch<TEntity, TResult> ScrollAllSearch<TEntity, TResult>(this IElasticsearchContext context)
        where TEntity : class
        where TResult : class
    {
        context.CheckNull(nameof(context));
        return new EsScrollAllSearch<TEntity, TResult>(context);
    }
}