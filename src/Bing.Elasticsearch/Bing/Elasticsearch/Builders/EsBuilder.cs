using Bing.Elasticsearch.Repositories;

namespace Bing.Elasticsearch.Builders;

/// <summary>
/// ES生成器
/// </summary>
public class EsBuilder : EsBuilderBase
{
    public EsBuilder(IIndexNameResolver indexNameResolver) : base(indexNameResolver)
    {
    }
}