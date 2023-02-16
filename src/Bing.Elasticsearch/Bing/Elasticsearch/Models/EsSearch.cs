namespace Bing.Elasticsearch.Models;

/// <summary>
/// ES查询模型
/// </summary>
/// <typeparam name="TResult">查询结果类型</typeparam>
public class EsSearch<TResult> : EsSearchBase<EsSearch<TResult>, TResult>
    where TResult : class
{
    /// <summary>
    /// 初始化一个<see cref="EsSearch{TResult}"/>类型的实例
    /// </summary>
    /// <param name="context">ES上下文</param>
    public EsSearch(IElasticsearchContext context) : base(context)
    {
    }
}