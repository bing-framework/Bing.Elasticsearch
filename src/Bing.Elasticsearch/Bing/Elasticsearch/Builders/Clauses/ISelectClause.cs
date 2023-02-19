using System;
using System.Linq.Expressions;

namespace Bing.Elasticsearch.Builders.Clauses;

/// <summary>
/// Select 子句
/// </summary>
public interface ISelectClause : IEsClause
{
    /// <summary>
    /// 设置星号*列
    /// </summary>
    void Select();

    /// <summary>
    /// 设置列名
    /// </summary>
    /// <param name="columns">列名</param>
    void Select(string columns);

    /// <summary>
    /// 设置列名
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="column">列名表达式</param>
    void Select<TEntity>(Expression<Func<TEntity, object>> column) where TEntity : class;

    /// <summary>
    /// 移除列名
    /// </summary>
    /// <param name="columns">列名</param>
    void RemoveSelect(string columns);

    /// <summary>
    /// 移除列名
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="column">列名表达式</param>
    void RemoveSelect<TEntity>(Expression<Func<TEntity, object>> column) where TEntity : class;
}