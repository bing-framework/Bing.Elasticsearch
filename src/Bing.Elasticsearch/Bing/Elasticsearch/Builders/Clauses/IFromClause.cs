using System;

namespace Bing.Elasticsearch.Builders.Clauses;

/// <summary>
/// From 子句
/// </summary>
public interface IFromClause : IEsClause, IPreInitClause
{
    /// <summary>
    /// 设置表名
    /// </summary>
    /// <param name="table">表名</param>
    void From(string table);

    /// <summary>
    /// 设置表名
    /// </summary>
    /// <param name="type">实体类型</param>
    void From(Type type);

    /// <summary>
    /// 设置表名
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    void From<TEntity>();
}