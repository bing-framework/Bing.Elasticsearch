using System;
using System.Linq;
using System.Threading.Tasks;
using Bing.Data;
using Bing.Data.Queries;
using Bing.Extensions;
using Nest;

namespace Bing.Elasticsearch.Models;

/// <summary>
/// ES分页查询模型
/// </summary>
/// <typeparam name="TResult">查询结果类型</typeparam>
public class EsPageSearch<TResult> : EsSearchBase<EsPageSearch<TResult>, TResult>
    where TResult : class
{
    /// <summary>
    /// 查询参数
    /// </summary>
    private readonly IQueryParameter _queryParam;

    /// <summary>
    /// 初始化一个<see cref="EsPageSearch{TResult}"/>类型的实例
    /// </summary>
    /// <param name="context">ES上下文</param>
    /// <param name="query">查询参数</param>
    public EsPageSearch(IElasticsearchContext context, IQueryParameter query)
        : base(context)
    {
        query.CheckNull(nameof(query));
        _queryParam = query;
    }

    /// <summary>
    /// 获取分页结果
    /// </summary>
    public async Task<PagerList<TResult>> GetPageResultAsync()
    {
        var result = await GetEsResultAsync();
        return CreateResult(result);
    }

    /// <summary>
    /// 创建分页结果
    /// </summary>
    /// <param name="result">查询响应结果</param>
    private PagerList<TResult> CreateResult(ISearchResponse<TResult> result)
    {
        _queryParam.TotalCount = Convert.ToInt32(result.Total);
        return new PagerList<TResult>(_queryParam, result.Documents.ToList());
    }

    /// <summary>
    /// 获取起始行数
    /// </summary>
    protected override int GetFrom() => _from > 0 ? _from.SafeValue() : _queryParam.GetStartNumber() - 1;

    /// <summary>
    /// 获取分页大小
    /// </summary>
    protected override int GetSize() => _size > 0 ? _size.SafeValue() : _queryParam.PageSize;
}