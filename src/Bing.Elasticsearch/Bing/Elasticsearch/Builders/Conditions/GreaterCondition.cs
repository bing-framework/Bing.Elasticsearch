using System;
using Bing.Helpers;
using Bing.Reflection;
using Nest;

namespace Bing.Elasticsearch.Builders.Conditions;

/// <summary>
/// 大于查询条件
/// </summary>
public class GreaterCondition : EsConditionBase
{
    public GreaterCondition(Field column, object value) : base(column, value)
    {
    }

    /// <summary>
    /// 获取查询条件
    /// </summary>
    public override QueryContainer GetCondition()
    {
        if (Reflections.IsDate(Column.Property))
        {
            return new DateRangeQuery
            {
                Field = Column,
                GreaterThan = Conv.ToDate(Value)
            };
        }


        if (Value is double doubleValue)
        {
            return new NumericRangeQuery
            {
                Field = Column,
                GreaterThan = doubleValue
            };
        }

        return new LongRangeQuery
        {
            Field = Column,
            GreaterThan = Convert.ToInt64(Value.ToString())
        };
    }

}