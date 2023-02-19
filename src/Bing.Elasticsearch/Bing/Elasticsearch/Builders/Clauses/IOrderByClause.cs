using System;
using System.Linq.Expressions;

namespace Bing.Elasticsearch.Builders.Clauses;

/// <summary>
/// Order By子句
/// </summary>
public interface IOrderByClause : IEsClause
{
    /// <summary>
    /// 排序
    /// </summary>
    /// <param name="column">列名</param>
    /// <param name="desc">是否倒序</param>
    void OrderBy(string column, bool desc = false);

    /// <summary>
    /// 排序
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="column">排序列</param>
    /// <param name="desc">是否倒序</param>
    void OrderBy<TEntity>(Expression<Func<TEntity, object>> column, bool desc = false) where TEntity : class;
}