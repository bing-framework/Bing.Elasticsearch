using System;
using Bing.Data;
using Bing.Elasticsearch.Builders;
using Bing.Elasticsearch.Builders.Operations;

namespace Bing.Elasticsearch;

/// <summary>
/// Where子句操作扩展
/// </summary>
public static class WhereClauseExtensions
{
    /// <summary>
    /// And连接条件
    /// </summary>
    /// <typeparam name="T">源类型</typeparam>
    /// <param name="source">源</param>
    /// <param name="condition">条件</param>
    public static T And<T>(this T source, IEsCondition condition)
        where T : IWhere
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));
        if (source is IEsPartAccessor accessor)
            accessor.WhereClause.And(condition);
        return source;
    }

    /// <summary>
    /// Or连接条件
    /// </summary>
    /// <typeparam name="T">源类型</typeparam>
    /// <param name="source">源</param>
    /// <param name="condition">条件</param>
    public static T Or<T>(this T source, IEsCondition condition)
        where T : IWhere
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));
        if (source is IEsPartAccessor accessor)
            accessor.WhereClause.Or(condition);
        return source;
    }

    /// <summary>
    /// Or连接条件
    /// </summary>
    /// <typeparam name="T">源类型</typeparam>
    /// <param name="source">源</param>
    /// <param name="predicate">条件</param>
    /// <param name="condition">该值为true时添加查询条件，否则忽略</param>
    public static T OrIf<T>(this T source, IEsCondition predicate, bool condition) where T : IWhere => condition ? Or(source, predicate) : source;

    /// <summary>
    /// 设置查询条件
    /// </summary>
    /// <typeparam name="T">源类型</typeparam>
    /// <param name="source">源</param>
    /// <param name="condition">条件</param>
    public static T Where<T>(this T source, IEsCondition condition)
        where T : IWhere
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));
        if (source is IEsPartAccessor accessor)
            accessor.WhereClause.Where(condition);
        return source;
    }
}