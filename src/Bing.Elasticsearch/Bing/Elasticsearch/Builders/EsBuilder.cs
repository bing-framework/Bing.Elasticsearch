using Bing.Elasticsearch.Repositories;
using Nest;

namespace Bing.Elasticsearch.Builders;

/// <summary>
/// ES生成器
/// </summary>
public class EsBuilder : EsBuilderBase
{
    /// <summary>
    /// 初始化一个<see cref="EsBuilder"/>类型的实例
    /// </summary>
    /// <param name="indexNameResolver">索引名称解析器</param>
    public EsBuilder(IIndexNameResolver indexNameResolver) : base(indexNameResolver)
    {
    }
}

/// <summary>
/// ES生成器
/// </summary>
/// <typeparam name="TDocument">文档类型</typeparam>
public class EsBuilder<TDocument> : EsBuilderBase, IEsBuilder<TDocument>
{
    /// <summary>
    /// 初始化一个<see cref="EsBuilder{TDocument}"/>类型的实例
    /// </summary>
    /// <param name="indexNameResolver">索引名称解析器</param>
    public EsBuilder(IIndexNameResolver indexNameResolver) : base(indexNameResolver)
    {
    }

    /// <summary>
    /// 获取查询请求
    /// </summary>
    ISearchRequest<TDocument> IEsBuilder<TDocument>.GetSearchRequest()
    {
        FromClause.PreInit();
        var request = new SearchRequest<TDocument>(_index);
        AppendTo(request);
        return request;
    }
}