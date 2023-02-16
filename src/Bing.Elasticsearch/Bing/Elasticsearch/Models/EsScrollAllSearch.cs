using Nest;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System;
using System.Linq;

namespace Bing.Elasticsearch.Models;


/// <summary>
/// ES滚动查询模型
/// </summary>
public class EsScrollAllSearch<TResult> : EsScrollAllSearch<TResult, TResult>
    where TResult : class
{
    /// <summary>
    /// 初始化一个<see cref="EsScrollAllSearch{TResult}"/>类型的实例
    /// </summary>
    /// <param name="context">ES上下文</param>
    public EsScrollAllSearch(IElasticsearchContext context) : base(context)
    {
    }
}

/// <summary>
/// ES滚动查询模型
/// </summary>
public class EsScrollAllSearch<TEntity, TResult> : EsSearchBase<EsScrollAllSearch<TEntity, TResult>, TResult>
    where TResult : class
    where TEntity : class
{
    /// <summary>
    /// 初始化一个<see cref="EsScrollAllSearch{TEntity,TResult}"/>类型的实例
    /// </summary>
    /// <param name="context">ES上下文</param>
    public EsScrollAllSearch(IElasticsearchContext context) : base(context)
    {
    }

    /// <summary>
    /// 获取大批量数据
    /// </summary>
    /// <param name="timeout">超时时间，单位：秒。默认：10秒</param>
    /// <param name="maximumRunTime">查询最大超时时间，单位：分钟。默认：20分钟</param>
    /// <param name="maxParallelism">最大并行度。默认：3</param>
    /// <param name="cancellationToken">取消令牌</param>
    public Task<List<TResult>> GetLargeListAsync(int timeout = 10, double maximumRunTime = 20, int maxParallelism = 3, CancellationToken cancellationToken = default)
    {
        Func<SearchDescriptor<TResult>, ISearchRequest> selector = x => BuildSearchRequest();
        var result = _context.ScrollAll<TResult>(selector, timeout, maximumRunTime, maxParallelism, cancellationToken);
        return Task.FromResult(result.ToList());
    }
}