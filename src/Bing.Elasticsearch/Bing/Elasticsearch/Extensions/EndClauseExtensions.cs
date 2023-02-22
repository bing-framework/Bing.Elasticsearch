using Bing.Elasticsearch.Builders;
using Bing.Elasticsearch.Builders.Operations;
using Bing.Extensions;

namespace Bing.Elasticsearch;

/// <summary>
/// 结束子句操作扩展
/// </summary>
public static class EndClauseExtensions
{
    #region Skip(设置跳过行数)

    /// <summary>
    /// 设置跳过行数
    /// </summary>
    /// <param name="source">源</param>
    /// <param name="count">跳过的行数</param>
    public static T Skip<T>(this T source, int count) where T : IEnd
    {
        source.CheckNull(nameof(source));
        if (source is IEsPartAccessor accessor)
            accessor.EndClause.Skip(count);
        return source;
    }

    #endregion

    #region Take(设置获取行数)

    /// <summary>
    /// 设置获取行数
    /// </summary>
    /// <param name="source">源</param>
    /// <param name="count">获取的行数</param>
    public static T Take<T>(this T source, int count) where T : IEnd
    {
        source.CheckNull(nameof(source));
        if (source is IEsPartAccessor accessor)
            accessor.EndClause.Take(count);
        return source;
    }

    #endregion
}