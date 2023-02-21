using System;
using System.Linq.Expressions;
using Bing.Extensions;

namespace Bing.Elasticsearch.Builders;

/// <summary>
/// <see cref="IEsBuilder"/> 查询扩展
/// </summary>
public static partial class EsBuilderExtensions
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
}