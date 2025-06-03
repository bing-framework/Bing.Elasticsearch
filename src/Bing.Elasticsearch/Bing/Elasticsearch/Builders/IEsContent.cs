using Nest;

namespace Bing.Elasticsearch.Builders;

/// <summary>
/// ES内容
/// </summary>
public interface IEsContent
{
    /// <summary>
    /// 添加到查询请求
    /// </summary>
    /// <param name="builder">查询请求</param>
    void AppendTo(ISearchRequest builder);
}