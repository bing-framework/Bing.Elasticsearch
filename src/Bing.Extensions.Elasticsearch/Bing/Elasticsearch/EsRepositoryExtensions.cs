using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
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

    /// <summary>
    /// 滚动查询
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="repository">仓储</param>
    /// <param name="builder">ES生成器</param>
    /// <param name="timeout">超时时间，单位：秒。默认：10秒</param>
    /// <param name="maximumRunTime">查询最大超时时间，单位：分钟。默认：20分钟</param>
    /// <param name="maxDegreeOfParallelism">最大并行度。默认：3</param>
    /// <param name="cancellationToken">取消令牌</param>
    public static Task<List<TEntity>> ScrollAllAsync<TEntity>(this IEsRepository<TEntity> repository, IEsBuilder builder, int timeout = 10, double maximumRunTime = 20, int maxDegreeOfParallelism = 3, CancellationToken cancellationToken = default)
        where TEntity : class
    {
        var result = repository.GetContext().ScrollAll<TEntity>(x => builder.GetSearchRequest(), timeout, maximumRunTime, maxDegreeOfParallelism, cancellationToken);
        return Task.FromResult(result.ToList());
    }

    /// <summary>
    /// 创建ES生成器
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="repository">仓储</param>
    public static IEsBuilder CreateBuilder<TEntity>(IEsRepository<TEntity> repository)
        where TEntity : class
    {
        return repository.GetContext().CreateBuilder<TEntity>();
    }
}