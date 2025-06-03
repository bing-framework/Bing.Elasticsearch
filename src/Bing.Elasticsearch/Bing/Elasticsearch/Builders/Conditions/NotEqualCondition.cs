using System.Collections.Generic;
using Nest;

namespace Bing.Elasticsearch.Builders.Conditions;

/// <summary>
/// Elasticsearch 不相等查询条件
/// </summary>
public class NotEqualCondition : EqualCondition
{
    /// <summary>
    /// 初始化一个<see cref="NotEqualCondition"/>类型的实例
    /// </summary>
    /// <param name="column">列名</param>
    /// <param name="value">值</param>
    public NotEqualCondition(Field column, object value) : base(column, value)
    {
    }

    /// <summary>
    /// 获取查询条件
    /// </summary>
    public override QueryContainer GetCondition()
    {
        var condition = new BoolQuery();
        var baseCondition = base.GetCondition();
        condition.MustNot = new List<QueryContainer> { baseCondition };
        return condition;
    }
}