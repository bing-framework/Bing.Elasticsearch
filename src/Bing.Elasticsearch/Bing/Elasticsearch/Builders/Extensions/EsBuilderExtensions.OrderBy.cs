using System;
using System.Linq.Expressions;

namespace Bing.Elasticsearch.Builders;

/// <summary>
/// <see cref="IEsBuilder"/> 查询扩展
/// </summary>
public static partial class EsBuilderExtensions
{
    /// <summary>
    /// 排序
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="source">Sql生成器</param>
    /// <param name="column">排序列。范例：t => t.Name</param>
    /// <param name="desc">是否倒排</param>
    /// <param name="appendKeyword">是否追加keyword关键词。text 类型字段不能直接使用，否则将会产生异常</param>
    public static IEsBuilder OrderBy<TEntity>(this IEsBuilder source, Expression<Func<TEntity, object>> column, bool desc = false, bool appendKeyword = false)
        where TEntity : class
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));
        if (source is IEsPartAccessor accessor)
            accessor.OrderByClause.OrderBy(column, desc, appendKeyword);
        return source;
    }

    /// <summary>
    /// 降序排序
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="source">Sql生成器</param>
    /// <param name="column">排序列。范例：t => t.Name</param>
    /// <param name="appendKeyword">是否追加keyword关键词。text 类型字段不能直接使用，否则将会产生异常</param>
    public static IEsBuilder OrderByDescending<TEntity>(this IEsBuilder source, Expression<Func<TEntity, object>> column, bool appendKeyword = false)
        where TEntity : class
    {
        return OrderBy(source, column, true, appendKeyword);
    }

    /// <summary>
    /// 升序排序
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="source">Sql生成器</param>
    /// <param name="column">排序列。范例：t => t.Name</param>
    /// <param name="appendKeyword">是否追加keyword关键词。text 类型字段不能直接使用，否则将会产生异常</param>
    public static IEsBuilder OrderByAscending<TEntity>(this IEsBuilder source, Expression<Func<TEntity, object>> column, bool appendKeyword = false)
        where TEntity : class
    {
        return OrderBy(source, column, false, appendKeyword);
    }
}