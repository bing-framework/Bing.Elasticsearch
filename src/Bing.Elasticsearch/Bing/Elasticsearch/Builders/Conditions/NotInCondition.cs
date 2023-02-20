using Nest;
using System.Collections;
using System.Collections.Generic;

namespace Bing.Elasticsearch.Builders.Conditions;

/// <summary>
/// Not In查询条件
/// </summary>
public class NotInCondition : EsConditionBase
{
    public NotInCondition(Field column, object value) : base(column, value)
    {
    }

    /// <summary>
    /// 获取查询条件
    /// </summary>
    public override QueryContainer GetCondition()
    {
        return new BoolQuery
        {
            MustNot = new List<QueryContainer>
            {
                new TermsQuery
                {
                    Field = Column,
                    Terms = GetValues()
                }
            }
        };
    }

    /// <summary>
    /// 获取值
    /// </summary>
    private List<object> GetValues()
    {
        var values = Value as IEnumerable;
        if (values == null)
            return null;
        var result = new List<object>();
        foreach (var value in values)
        {
            if (value == null)
                continue;
            result.Add(value);
        }
        return result;
    }
}