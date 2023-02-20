using System;
using Bing.Data;
using Nest;

namespace Bing.Elasticsearch.Builders.Conditions;

/// <summary>
/// ES查询条件基类
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