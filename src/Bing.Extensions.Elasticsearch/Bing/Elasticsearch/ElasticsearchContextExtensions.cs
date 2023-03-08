using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bing.Data;
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

    /// <summary>
    /// 分页查询
    /// </summary>
    /// <typeparam name="TDocument">文档类型</typeparam>
    /// <param name="context">ES上下文</param>
    /// <param name="builder">ES生成器</param>
    /// <param name="page">页码</param>
    /// <param name="pageSize">每页记录数</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static async Task<PagerList<TDocument>> PageSearchAsync<TDocument>(this IElasticsearchContext context, IEsBuilder<TDocument> builder, int page, int pageSize = 20, CancellationToken cancellationToken = default)
        where TDocument : class
    {
        context.CheckNull(nameof(context));
        if (page < 1)
            throw new ArgumentOutOfRangeException(nameof(page), "页码不能小于1");
        if (pageSize <= 0)
            throw new ArgumentOutOfRangeException(nameof(pageSize), "每页记录数不能小于等于0");
        builder
            .Skip((page - 1) * pageSize)
            .Take(pageSize);
        var response = await context.SearchAsync<TDocument>(x => builder.GetSearchRequest(), cancellationToken);
        var totalCount = response.Total;
        var pageCount = totalCount / pageSize + 1;
        var result = new PagerList<TDocument>
        {
            Data = response.Documents.ToList(),
            Page = page,
            PageSize = pageSize,
            TotalCount = Convert.ToInt32(totalCount),
            PageCount = Convert.ToInt32(pageCount)
        };
        return result;
    }
}