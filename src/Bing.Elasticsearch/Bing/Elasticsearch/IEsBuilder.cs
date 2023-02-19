using Bing.Elasticsearch.Builders;
using Bing.Elasticsearch.Builders.Operations;
using Nest;

namespace Bing.Elasticsearch;

/// <summary>
/// ES生成器
/// </summary>
public interface IEsBuilder : IEsContent, IQueryOperation
{
    /// <summary>
    /// 获取查询请求
    /// </summary>
    ISearchRequest GetSearchRequest();
}