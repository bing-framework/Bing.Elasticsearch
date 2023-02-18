using Nest;

namespace Bing.Data.Queries.Conditions;

public class NullEsCondition : IEsCondition
{
    /// <summary>
    /// 空查询条件实例
    /// </summary>
    public static readonly NullEsCondition Instance = new NullEsCondition();

    /// <summary>
    /// 获取查询条件
    /// </summary>
    public QueryContainer GetCondition()
    {
        return null;
    }
}