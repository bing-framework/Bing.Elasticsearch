using Bing.Data;
using Nest;
using System.Collections.Generic;

namespace Bing.Elasticsearch.Builders.Conditions;

/// <summary>
/// Not In查询条件
/// </summary>
public class NotInCondition : IEsCondition
{
    /// <summary>
    /// 字段
    /// </summary>
    private readonly Field _field;

    /// <summary>
    /// 值集合
    /// </summary>
    private readonly IEnumerable<object> _values;

    public NotInCondition(string field, IEnumerable<object> values)
    {
        _field = new Field(field);
        _values = values;
    }

    public NotInCondition(Field field, IEnumerable<object> values)
    {
        _field = field;
        _values = values;
    }

    /// <summary>
    /// 获取查询条件
    /// </summary>
    public QueryContainer GetCondition()
    {
        return new BoolQuery
        {
            MustNot = new List<QueryContainer>
            {
                new TermsQuery
                {
                    Field = _field,
                    Terms = _values
                }
            }
        };
    }
}