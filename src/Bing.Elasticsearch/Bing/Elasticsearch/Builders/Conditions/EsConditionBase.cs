using System;
using Bing.Data;
using Nest;

namespace Bing.Elasticsearch.Builders.Conditions;

/// <summary>
/// Elasticsearch 查询条件基类
/// </summary>
public abstract class EsConditionBase : IEsCondition
{
    /// <summary>
    /// 列名
    /// </summary>
    protected readonly Field Column;

    /// <summary>
    /// 值
    /// </summary>
    protected readonly object Value;

    /// <summary>
    /// 初始化一个<see cref="EsConditionBase"/>类型的实例
    /// </summary>
    /// <param name="column">列名</param>
    /// <param name="value">值</param>
    /// <exception cref="ArgumentNullException"></exception>
    protected EsConditionBase(Field column, object value)
    {
        if (column == null)
            throw new ArgumentNullException(nameof(column));
        Column = column;
        Value = value;
    }

    /// <summary>
    /// 获取查询条件
    /// </summary>
    public abstract QueryContainer GetCondition();
}