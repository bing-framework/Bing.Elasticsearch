using System;

namespace Bing.Elasticsearch.Builders.Clauses;

/// <summary>
/// ES子句基类
/// </summary>
public abstract class ClauseBase
{
    /// <summary>
    /// ES生成器
    /// </summary>
    protected readonly EsBuilderBase EsBuilder;

    /// <summary>
    /// 初始化一个<see cref="ClauseBase"/>类型的实例
    /// </summary>
    /// <param name="esBuilder">ES生成器</param>
    /// <exception cref="ArgumentNullException"></exception>
    protected ClauseBase(EsBuilderBase esBuilder)
    {
        EsBuilder = esBuilder ?? throw new ArgumentNullException(nameof(esBuilder));
    }
}