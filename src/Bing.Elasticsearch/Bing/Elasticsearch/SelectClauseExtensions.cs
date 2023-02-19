using Bing.Elasticsearch.Builders;
using Bing.Elasticsearch.Builders.Operations;
using Bing.Extensions;

namespace Bing.Elasticsearch;

/// <summary>
/// Select子句操作扩展
/// </summary>
public static class SelectClauseExtensions
{
    /// <summary>
    /// 设置星号*列
    /// </summary>
    /// <typeparam name="T">源类型</typeparam>
    /// <param name="source">源</param>
    public static T Select<T>(this T source) where T : ISelect
    {
        source.CheckNull(nameof(source));
        if(source is IEsPartAccessor accessor)
            accessor.SelectClause.Select();
        return source;
    }

    /// <summary>
    /// 设置列名
    /// </summary>
    /// <typeparam name="T">源类型</typeparam>
    /// <param name="source">源</param>
    /// <param name="columns">列名，范例：appId,id,name</param>
    public static T Select<T>(this T source, string columns) where T : ISelect
    {
        source.CheckNull(nameof(source));
        if(source is IEsPartAccessor accessor)
            accessor.SelectClause.Select(columns);
        return source;
    }

    /// <summary>
    /// 移除列名
    /// </summary>
    /// <typeparam name="T">源类型</typeparam>
    /// <param name="source">源</param>
    /// <param name="columns">列名，范例：appId,id,name</param>
    public static T RemoveSelect<T>(this T source, string columns) where T : ISelect
    {
        source.CheckNull(nameof(source));
        if(source is IEsPartAccessor accessor)
            accessor.SelectClause.Select(columns);
        return source;
    }
}