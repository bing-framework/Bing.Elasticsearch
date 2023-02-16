namespace Bing.Elasticsearch.Repositories;

/// <summary>
/// 索引名称解析器
/// </summary>
public interface IIndexNameResolver
{
    /// <summary>
    /// 获取索引名称
    /// </summary>
    /// <param name="indexName">索引名称</param>
    string GetIndexName(string indexName);
}