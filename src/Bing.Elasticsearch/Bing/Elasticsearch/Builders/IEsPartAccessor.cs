using Bing.Elasticsearch.Builders.Clauses;

namespace Bing.Elasticsearch.Builders;

/// <summary>
/// ES子句访问器
/// </summary>
public interface IEsPartAccessor
{
    /// <summary>
    /// Select子句
    /// </summary>
    ISelectClause SelectClause { get; }

    /// <summary>
    /// From子句
    /// </summary>
    IFromClause FromClause { get; }

    /// <summary>
    /// Where子句
    /// </summary>
    IWhereClause WhereClause { get; }

    /// <summary>
    /// OrderBy子句
    /// </summary>
    IOrderByClause OrderByClause { get; }

    /// <summary>
    /// 结束子句
    /// </summary>
    IEndClause EndClause { get; }
}