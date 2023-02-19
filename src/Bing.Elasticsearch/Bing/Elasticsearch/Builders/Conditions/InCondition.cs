using System.Collections.Generic;
using Bing.Data;
using Nest;

namespace Bing.Elasticsearch.Builders.Conditions;

/// <summary>
/// In查询条件
/// </summary>
public class InCondition : IEsCondition
{
    /// <summary>
    /// 字段
    /// </summary>
    private readonly Field _field;

    /// <summary>
    /// 值集合
    /// </summary>
    private readonly IEnumerable<object> _values;

    public InCondition(string field, IEnumerable<object> values)
    {
        _field = new Field(field);
        _values = values;
    }

    public InCondition(Field field, IEnumerable<object> values)
    {
        _field = field;
        _values = values;
    }

    /// <summary>
    /// 获取查询条件
    /// </summary>
    public QueryContainer GetCondition()
    {
        return new TermsQuery
        {
            Field = _field,
            Terms = _values
        };
    }
}