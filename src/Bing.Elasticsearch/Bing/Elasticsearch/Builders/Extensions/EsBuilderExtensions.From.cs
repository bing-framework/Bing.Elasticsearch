using Bing.Extensions;

namespace Bing.Elasticsearch.Builders;

/// <summary>
/// <see cref="IEsBuilder"/> 查询扩展
/// </summary>
public static partial class EsBuilderExtensions
{
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
}