using Bing.Extensions;
using System.Linq.Expressions;
using System;

namespace Bing.Elasticsearch.Builders;

public static class EsBuilderExtensions
{
    /// <summary>
    /// 设置列名
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="source">源</param>
    /// <param name="column">列名，范例：x=>appId</param>
    public static IEsBuilder Select<TEntity>(this IEsBuilder source, Expression<Func<TEntity, object>> column)
        where TEntity : class
    {
        source.CheckNull(nameof(source));
        if (source is IEsPartAccessor accessor)
            accessor.SelectClause.Select(column);
        return source;
    }

    /// <summary>
    /// 移除列名
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="source">源</param>
    /// <param name="column">列名，范例：x=>appId</param>
    public static IEsBuilder RemoveSelect<TEntity>(this IEsBuilder source, Expression<Func<TEntity, object>> column)
        where TEntity : class
    {
        source.CheckNull(nameof(source));
        if (source is IEsPartAccessor accessor)
            accessor.SelectClause.RemoveSelect(column);
        return source;
    }

    /// <summary>
    /// 设置表名
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="source">源</param>
    public static IEsBuilder From<TEntity>(this IEsBuilder source) 
        where TEntity : class
    {
        source.CheckNull(nameof(source));
        if (source is IEsPartAccessor accessor)
            accessor.FromClause.From<TEntity>();
        return source;
    }

    public static IEsBuilder Where<TEntity>(this IEsBuilder source, Expression<Func<TEntity, object>> expression, object value, Operator @operator = Operator.Equal)
        where TEntity : class
    {
        source.CheckNull(nameof(source));
        if (source is IEsPartAccessor accessor)
            accessor.WhereClause.Where(expression, value, @operator);
        return source;
    }
}