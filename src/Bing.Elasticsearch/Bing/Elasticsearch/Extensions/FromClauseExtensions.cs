using System;
using Bing.Elasticsearch.Builders;
using Bing.Elasticsearch.Builders.Operations;
using Bing.Extensions;

namespace Bing.Elasticsearch;

/// <summary>
/// From子句扩展
/// </summary>
public static class FromClauseExtensions
{
    /// <summary>
    /// 设置表名
    /// </summary>
    /// <typeparam name="T">源类型</typeparam>
    /// <param name="source">源</param>
    /// <param name="type">表类型</param>
    public static T From<T>(this T source, Type type) where T : IFrom
    {
        source.CheckNull(nameof(source));
        if (source is IEsPartAccessor accessor)
            accessor.FromClause.From(type);
        return source;
    }
}