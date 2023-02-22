using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bing.Elasticsearch.Builders;
using Bing.Elasticsearch.Internals;
using Bing.Extensions;
using Nest;

namespace Bing.Elasticsearch;

/// <summary>
/// ES上下文扩展
/// </summary>
public static partial class ElasticsearchContextExtensions
{
    /// <summary>
    /// 查询。单一条件查询，一般是精确查询
    /// </summary>
    /// <typeparam name="TDocument">文档类型</typeparam>
    /// <param name="context">ES上下文</param>
    /// <param name="field">字段名</param>
    /// <param name="value">值</param>
    /// <param name="index">索引名称。注意：必须小写</param>
    /// <param name="cancellationToken">取消令牌</param>
    public static async Task<IEnumerable<TDocument>> SearchAsync<TDocument>(this IElasticsearchContext context, string field, object value, string index = null, CancellationToken cancellationToken = default)
        where TDocument : class
    {
        if (field.IsEmpty())
            throw new ArgumentNullException(nameof(field));
        index = context.GetIndexName(Helper.SafeIndexName<TDocument>(index));
        var descriptor = new SearchDescriptor<TDocument>();
        descriptor.Index(index)
            .PostFilter(f => f.Term(x => x.Field(field).Value(value)));
        Func<SearchDescriptor<TDocument>, ISearchRequest> selector = x => descriptor;
        var response = await context.SearchAsync(selector, cancellationToken);
        return response.Documents;
    }

    /// <summary>
    /// 滚动查询
    /// </summary>
    /// <typeparam name="TDocument">文档类型</typeparam>
    /// <param name="context">ES上下文</param>
    /// <param name="builder">ES生成器</param>
    /// <param name="timeout">超时时间，单位：秒。默认：10秒</param>
    /// <param name="maximumRunTime">查询最大超时时间，单位：分钟。默认：20分钟</param>
    /// <param name="maxDegreeOfParallelism">最大并行度。默认：3</param>
    /// <param name="cancellationToken">取消令牌</param>
    public static Task<List<TDocument>> ScrollAllAsync<TDocument>(this IElasticsearchContext context, IEsBuilder builder, int timeout = 10, double maximumRunTime = 20, int maxDegreeOfParallelism = 3, CancellationToken cancellationToken = default)
        where TDocument : class
    {
        var result = context.ScrollAll<TDocument>(x => builder.GetSearchRequest(), timeout, maximumRunTime, maxDegreeOfParallelism, cancellationToken).ToList();
        return Task.FromResult(result);
    }

    /// <summary>
    /// 创建ES生成器
    /// </summary>
    /// <param name="context">ES上下文</param>
    public static IEsBuilder CreateBuilder(this IElasticsearchContext context) => new EsBuilder(context.GetIndexNameResolver());

    /// <summary>
    /// 创建ES生成器
    /// </summary>
    /// <typeparam name="TDocument">文档类型</typeparam>
    /// <param name="context">ES上下文</param>
    public static IEsBuilder CreateBuilder<TDocument>(this IElasticsearchContext context)
        where TDocument : class
    {
        var builder = new EsBuilder(context.GetIndexNameResolver());
        builder.From<TDocument>();
        return builder;
    }
}