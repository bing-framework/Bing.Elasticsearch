using Nest;

namespace Bing.Elasticsearch.Builders.Conditions;

/// <summary>
/// Elasticsearch 不相等查询条件
/// </summary>
public class NotEqualCondition : EsConditionBase
{

    public NotEqualCondition(Field column, object value) : base(column, value)
    {
    }

    /// <summary>
    /// 获取查询条件
    /// </summary>
    public override QueryContainer GetCondition()
    {
        throw new System.NotImplementedException();
    }
}