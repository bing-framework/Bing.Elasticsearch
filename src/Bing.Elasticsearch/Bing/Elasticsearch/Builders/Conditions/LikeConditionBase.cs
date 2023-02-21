using System;
using Nest;

namespace Bing.Elasticsearch.Builders.Conditions;

/// <summary>
/// 模糊查询条件基类
/// </summary>
public abstract class LikeConditionBase : EsConditionBase
{
    /// <summary>
    /// 初始化一个<see cref="LikeConditionBase"/>类型的实例
    /// </summary>
    /// <param name="column">列名</param>
    /// <param name="value">值</param>
    /// <exception cref="ArgumentNullException"></exception>
    protected LikeConditionBase(Field column, object value) 
        : base(column, value)
    {
    }

    /// <summary>
    /// 获取查询条件
    /// </summary>
    public override QueryContainer GetCondition()
    {
        return new WildcardQuery
        {
            Field = Column,
            Value = GetValue()
        };
    }

    /// <summary>
    /// 获取值
    /// </summary>
    protected abstract object GetValue();
}