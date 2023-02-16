using Nest;

namespace Bing.Data;

/// <summary>
/// ES搜索条件
/// </summary>
public interface IEsCondition
{
    /// <summary>
    /// 获取查询条件
    /// </summary>
    QueryContainer GetCondition();
}