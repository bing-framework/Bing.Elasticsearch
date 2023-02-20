using System;
using Nest;

namespace Bing.Elasticsearch.Builders.Conditions;

/// <summary>
/// 相等查询条件
/// </summary>
public class EqualCondition : EsConditionBase
{
    /// <summary>
    /// 初始化一个<see cref="EqualCondition"/>类型的实例
    /// </summary>
    /// <param name="column">列名</param>
    /// <param name="value">值</param>
    public EqualCondition(Field column, object value) : base(column, value)
    {
    }

    /// <summary>
    /// 获取查询条件
    /// </summary>
    public override QueryContainer GetCondition()
    {
        if (Value == null)
            throw new ArgumentNullException(nameof(Value), "值不能为空");
        if (Value is string value)
            return new MatchPhraseQuery
            {
                Field = Column,
                Query = value,
                Slop = 0
            };
        return new TermQuery
        {
            Field = Column,
            Value = Value
        };
    }
}