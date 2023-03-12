using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Bing.Data.Queries;
using Bing.Extensions;

namespace Bing.Elasticsearch.Builders;

/// <summary>
/// <see cref="IEsBuilder"/> 查询扩展
/// </summary>
public static partial class EsBuilderExtensions
{
    #region IEsBuilder

    /// <summary>
    /// 设置查询条件
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="source">源</param>
    /// <param name="expression">列名表达式。范例：t => t.Name</param>
    /// <param name="value">值</param>
    /// <param name="operator">运算符</param>
    /// <param name="appendKeyword">是否追加keyword关键词，如果字段类型被识别成 text 类型，同时还附带 keyword 子字段，则需要追加 keyword 的子字段来实现精确匹配。</param>
    public static IEsBuilder Where<TEntity>(this IEsBuilder source, Expression<Func<TEntity, object>> expression, object value, Operator @operator = Operator.Equal, bool appendKeyword = false)
        where TEntity : class
    {
        source.CheckNull(nameof(source));
        if (source is IEsPartAccessor accessor)
            accessor.WhereClause.Where(expression, value, @operator, appendKeyword);
        return source;
    }

    /// <summary>
    /// 设置查询条件
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="source">源</param>
    /// <param name="expression">列名表达式。范例：t => t.Name</param>
    /// <param name="value">值</param>
    /// <param name="condition">该值为true时添加查询条件，否则忽略</param>
    /// <param name="operator">运算符</param>
    /// <param name="appendKeyword">是否追加keyword关键词，如果字段类型被识别成 text 类型，同时还附带 keyword 子字段，则需要追加 keyword 的子字段来实现精确匹配。</param>
    public static IEsBuilder WhereIf<TEntity>(this IEsBuilder source, Expression<Func<TEntity, object>> expression, object value, bool condition, Operator @operator = Operator.Equal, bool appendKeyword = false)
        where TEntity : class =>
        condition ? source.Where(expression, value, @operator, appendKeyword) : source;

    /// <summary>
    /// 设置查询条件
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="source">源</param>
    /// <param name="expression">列名表达式。范例：t => t.Name</param>
    /// <param name="value">值</param>
    /// <param name="operator">运算符</param>
    /// <param name="appendKeyword">是否追加keyword关键词，如果字段类型被识别成 text 类型，同时还附带 keyword 子字段，则需要追加 keyword 的子字段来实现精确匹配。</param>
    public static IEsBuilder WhereIfNotEmpty<TEntity>(this IEsBuilder source, Expression<Func<TEntity, object>> expression, object value, Operator @operator = Operator.Equal, bool appendKeyword = false)
        where TEntity : class
    {
        source.CheckNull(nameof(source));
        if (string.IsNullOrWhiteSpace(value.SafeString()))
            return source;
        if (source is IEsPartAccessor accessor)
            accessor.WhereClause.Where(expression, value, @operator, appendKeyword);
        return source;
    }

    /// <summary>
    /// 设置相等查询条件
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="source">Sql生成器</param>
    /// <param name="expression">列名表达式。范例：t => t.Name</param>
    /// <param name="value">值</param>
    /// <param name="appendKeyword">是否追加keyword关键词，如果字段类型被识别成 text 类型，同时还附带 keyword 子字段，则需要追加 keyword 的子字段来实现精确匹配。</param>
    public static IEsBuilder Equal<TEntity>(this IEsBuilder source, Expression<Func<TEntity, object>> expression, object value, bool appendKeyword = false)
        where TEntity : class
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));
        return source.Where(expression, value, Operator.Equal, appendKeyword);
    }

    /// <summary>
    /// 设置不相等查询条件
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="source">Sql生成器</param>
    /// <param name="expression">列名表达式。范例：t => t.Name</param>
    /// <param name="value">值</param>
    /// <param name="appendKeyword">是否追加keyword关键词，如果字段类型被识别成 text 类型，同时还附带 keyword 子字段，则需要追加 keyword 的子字段来实现精确匹配。</param>
    public static IEsBuilder NotEqual<TEntity>(this IEsBuilder source, Expression<Func<TEntity, object>> expression, object value, bool appendKeyword = false)
        where TEntity : class
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));
        return source.Where(expression, value, Operator.NotEqual, appendKeyword);
    }

    /// <summary>
    /// 设置大于查询条件
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="source">Sql生成器</param>
    /// <param name="expression">列名表达式。范例：t => t.Name</param>
    /// <param name="value">值</param>
    public static IEsBuilder Greater<TEntity>(this IEsBuilder source, Expression<Func<TEntity, object>> expression, object value)
        where TEntity : class
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));
        return source.Where(expression, value, Operator.Greater);
    }

    /// <summary>
    /// 设置小于查询条件
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="source">Sql生成器</param>
    /// <param name="expression">列名表达式。范例：t => t.Name</param>
    /// <param name="value">值</param>
    public static IEsBuilder Less<TEntity>(this IEsBuilder source, Expression<Func<TEntity, object>> expression, object value)
        where TEntity : class
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));
        return source.Where(expression, value, Operator.Less);
    }

    /// <summary>
    /// 设置大于等于查询条件
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="source">Sql生成器</param>
    /// <param name="expression">列名表达式。范例：t => t.Name</param>
    /// <param name="value">值</param>
    public static IEsBuilder GreaterEqual<TEntity>(this IEsBuilder source, Expression<Func<TEntity, object>> expression, object value)
        where TEntity : class
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));
        return source.Where(expression, value, Operator.GreaterEqual);
    }

    /// <summary>
    /// 设置小于等于查询条件
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="source">Sql生成器</param>
    /// <param name="expression">列名表达式。范例：t => t.Name</param>
    /// <param name="value">值</param>
    public static IEsBuilder LessEqual<TEntity>(this IEsBuilder source, Expression<Func<TEntity, object>> expression, object value)
        where TEntity : class
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));
        return source.Where(expression, value, Operator.LessEqual);
    }

    /// <summary>
    /// 设置模糊匹配查询条件
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="source">Sql生成器</param>
    /// <param name="expression">列名表达式。范例：t => t.Name</param>
    /// <param name="value">值</param>
    /// <param name="appendKeyword">是否追加keyword关键词，如果字段类型被识别成 text 类型，同时还附带 keyword 子字段，则需要追加 keyword 的子字段来实现精确匹配。</param>
    public static IEsBuilder Contains<TEntity>(this IEsBuilder source, Expression<Func<TEntity, object>> expression, object value, bool appendKeyword = false)
        where TEntity : class
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));
        return source.Where(expression, value, Operator.Contains, appendKeyword);
    }

    /// <summary>
    /// 设置头匹配查询条件
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="source">Sql生成器</param>
    /// <param name="expression">列名表达式。范例：t => t.Name</param>
    /// <param name="value">值</param>
    /// <param name="appendKeyword">是否追加keyword关键词，如果字段类型被识别成 text 类型，同时还附带 keyword 子字段，则需要追加 keyword 的子字段来实现精确匹配。</param>
    public static IEsBuilder Starts<TEntity>(this IEsBuilder source, Expression<Func<TEntity, object>> expression, object value, bool appendKeyword = false)
        where TEntity : class
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));
        return source.Where(expression, value, Operator.Starts, appendKeyword);
    }

    /// <summary>
    /// 设置尾匹配查询条件
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="source">Sql生成器</param>
    /// <param name="expression">列名表达式。范例：t => t.Name</param>
    /// <param name="value">值</param>
    /// <param name="appendKeyword">是否追加keyword关键词，如果字段类型被识别成 text 类型，同时还附带 keyword 子字段，则需要追加 keyword 的子字段来实现精确匹配。</param>
    public static IEsBuilder Ends<TEntity>(this IEsBuilder source, Expression<Func<TEntity, object>> expression, object value, bool appendKeyword = false)
        where TEntity : class
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));
        return source.Where(expression, value, Operator.Ends, appendKeyword);
    }

    /// <summary>
    /// 设置In条件
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="source">Sql生成器</param>
    /// <param name="expression">列名表达式。范例：t => t.Name</param>
    /// <param name="values">值集合</param>
    /// <param name="appendKeyword">是否追加keyword关键词，如果字段类型被识别成 text 类型，同时还附带 keyword 子字段，则需要追加 keyword 的子字段来实现精确匹配。</param>
    public static IEsBuilder In<TEntity>(this IEsBuilder source, Expression<Func<TEntity, object>> expression, IEnumerable<object> values, bool appendKeyword = false)
        where TEntity : class
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));
        if (source is IEsPartAccessor accessor)
            accessor.WhereClause.Where(expression, values, Operator.In, appendKeyword);
        return source;
    }

    /// <summary>
    /// 设置Not In条件
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="source">Sql生成器</param>
    /// <param name="expression">列名表达式。范例：t => t.Name</param>
    /// <param name="values">值集合</param>
    /// <param name="appendKeyword">是否追加keyword关键词，如果字段类型被识别成 text 类型，同时还附带 keyword 子字段，则需要追加 keyword 的子字段来实现精确匹配。</param>
    public static IEsBuilder NotIn<TEntity>(this IEsBuilder source, Expression<Func<TEntity, object>> expression, IEnumerable<object> values, bool appendKeyword = false)
        where TEntity : class
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));
        if (source is IEsPartAccessor accessor)
            accessor.WhereClause.Where(expression, values, Operator.NotIn, appendKeyword);
        return source;
    }

    /// <summary>
    /// 添加范围查询条件
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="source">Sql生成器</param>
    /// <param name="expression">列名表达式。范例：t => t.Name</param>
    /// <param name="min">最小值</param>
    /// <param name="max">最大值</param>
    /// <param name="boundary">包含边界</param>
    public static IEsBuilder Between<TEntity>(this IEsBuilder source, Expression<Func<TEntity, object>> expression, int? min, int? max, Boundary boundary = Boundary.Both)
        where TEntity : class
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));
        if (source is IEsPartAccessor accessor)
            accessor.WhereClause.Between(expression, min, max, boundary);
        return source;
    }

    /// <summary>
    /// 添加范围查询条件
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="source">Sql生成器</param>
    /// <param name="expression">列名表达式。范例：t => t.Name</param>
    /// <param name="min">最小值</param>
    /// <param name="max">最大值</param>
    /// <param name="boundary">包含边界</param>
    public static IEsBuilder Between<TEntity>(this IEsBuilder source, Expression<Func<TEntity, object>> expression, long? min, long? max, Boundary boundary = Boundary.Both)
        where TEntity : class
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));
        if (source is IEsPartAccessor accessor)
            accessor.WhereClause.Between(expression, min, max, boundary);
        return source;
    }

    /// <summary>
    /// 添加范围查询条件
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="source">Sql生成器</param>
    /// <param name="expression">列名表达式。范例：t => t.Name</param>
    /// <param name="min">最小值</param>
    /// <param name="max">最大值</param>
    /// <param name="boundary">包含边界</param>
    public static IEsBuilder Between<TEntity>(this IEsBuilder source, Expression<Func<TEntity, object>> expression, float? min, float? max, Boundary boundary = Boundary.Both)
        where TEntity : class
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));
        if (source is IEsPartAccessor accessor)
            accessor.WhereClause.Between(expression, min, max, boundary);
        return source;
    }

    /// <summary>
    /// 添加范围查询条件
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="source">Sql生成器</param>
    /// <param name="expression">列名表达式。范例：t => t.Name</param>
    /// <param name="min">最小值</param>
    /// <param name="max">最大值</param>
    /// <param name="boundary">包含边界</param>
    public static IEsBuilder Between<TEntity>(this IEsBuilder source, Expression<Func<TEntity, object>> expression, double? min, double? max, Boundary boundary = Boundary.Both)
        where TEntity : class
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));
        if (source is IEsPartAccessor accessor)
            accessor.WhereClause.Between(expression, min, max, boundary);
        return source;
    }

    /// <summary>
    /// 添加范围查询条件
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="source">Sql生成器</param>
    /// <param name="expression">列名表达式。范例：t => t.Name</param>
    /// <param name="min">最小值</param>
    /// <param name="max">最大值</param>
    /// <param name="boundary">包含边界</param>
    public static IEsBuilder Between<TEntity>(this IEsBuilder source, Expression<Func<TEntity, object>> expression, decimal? min, decimal? max, Boundary boundary = Boundary.Both)
        where TEntity : class
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));
        if (source is IEsPartAccessor accessor)
            accessor.WhereClause.Between(expression, min, max, boundary);
        return source;
    }

    /// <summary>
    /// 添加范围查询条件
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="source">Sql生成器</param>
    /// <param name="expression">列名表达式。范例：t => t.Name</param>
    /// <param name="min">最小值</param>
    /// <param name="max">最大值</param>
    /// <param name="includeTime">是否包含时间</param>
    /// <param name="boundary">包含边界</param>
    public static IEsBuilder Between<TEntity>(this IEsBuilder source, Expression<Func<TEntity, object>> expression, DateTime? min, DateTime? max, bool includeTime = true, Boundary? boundary = Boundary.Both)
        where TEntity : class
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));
        if (source is IEsPartAccessor accessor)
            accessor.WhereClause.Between(expression, min, max, includeTime, boundary);
        return source;
    }

    #endregion

    #region IEsBuilder`

    /// <summary>
    /// 设置查询条件
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="source">源</param>
    /// <param name="expression">列名表达式。范例：t => t.Name</param>
    /// <param name="value">值</param>
    /// <param name="operator">运算符</param>
    /// <param name="appendKeyword">是否追加keyword关键词，如果字段类型被识别成 text 类型，同时还附带 keyword 子字段，则需要追加 keyword 的子字段来实现精确匹配。</param>
    public static IEsBuilder<TEntity> Where<TEntity>(this IEsBuilder<TEntity> source, Expression<Func<TEntity, object>> expression, object value, Operator @operator = Operator.Equal, bool appendKeyword = false)
        where TEntity : class
    {
        source.CheckNull(nameof(source));
        if (source is IEsPartAccessor accessor)
            accessor.WhereClause.Where(expression, value, @operator, appendKeyword);
        return source;
    }

    /// <summary>
    /// 设置查询条件
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="source">源</param>
    /// <param name="expression">列名表达式。范例：t => t.Name</param>
    /// <param name="value">值</param>
    /// <param name="condition">该值为true时添加查询条件，否则忽略</param>
    /// <param name="operator">运算符</param>
    /// <param name="appendKeyword">是否追加keyword关键词，如果字段类型被识别成 text 类型，同时还附带 keyword 子字段，则需要追加 keyword 的子字段来实现精确匹配。</param>
    public static IEsBuilder<TEntity> WhereIf<TEntity>(this IEsBuilder<TEntity> source, Expression<Func<TEntity, object>> expression, object value, bool condition, Operator @operator = Operator.Equal, bool appendKeyword = false)
        where TEntity : class =>
        condition ? source.Where(expression, value, @operator, appendKeyword) : source;

    /// <summary>
    /// 设置查询条件
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="source">源</param>
    /// <param name="expression">列名表达式。范例：t => t.Name</param>
    /// <param name="value">值，如果该值为空，则忽略该查询条件</param>
    /// <param name="operator">运算符</param>
    /// <param name="appendKeyword">是否追加keyword关键词，如果字段类型被识别成 text 类型，同时还附带 keyword 子字段，则需要追加 keyword 的子字段来实现精确匹配。</param>
    public static IEsBuilder<TEntity> WhereIfNotEmpty<TEntity>(this IEsBuilder<TEntity> source, Expression<Func<TEntity, object>> expression, object value, Operator @operator = Operator.Equal, bool appendKeyword = false)
        where TEntity : class
    {
        source.CheckNull(nameof(source));
        if (string.IsNullOrWhiteSpace(value.SafeString()))
            return source;
        if (source is IEsPartAccessor accessor)
            accessor.WhereClause.Where(expression, value, @operator, appendKeyword);
        return source;
    }

    /// <summary>
    /// 设置相等查询条件
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="source">Sql生成器</param>
    /// <param name="expression">列名表达式。范例：t => t.Name</param>
    /// <param name="value">值</param>
    /// <param name="appendKeyword">是否追加keyword关键词，如果字段类型被识别成 text 类型，同时还附带 keyword 子字段，则需要追加 keyword 的子字段来实现精确匹配。</param>
    public static IEsBuilder<TEntity> Equal<TEntity>(this IEsBuilder<TEntity> source, Expression<Func<TEntity, object>> expression, object value, bool appendKeyword = false)
        where TEntity : class
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));
        return source.Where(expression, value, Operator.Equal, appendKeyword);
    }

    /// <summary>
    /// 设置不相等查询条件
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="source">Sql生成器</param>
    /// <param name="expression">列名表达式。范例：t => t.Name</param>
    /// <param name="value">值</param>
    /// <param name="appendKeyword">是否追加keyword关键词，如果字段类型被识别成 text 类型，同时还附带 keyword 子字段，则需要追加 keyword 的子字段来实现精确匹配。</param>
    public static IEsBuilder<TEntity> NotEqual<TEntity>(this IEsBuilder<TEntity> source, Expression<Func<TEntity, object>> expression, object value, bool appendKeyword = false)
        where TEntity : class
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));
        return source.Where(expression, value, Operator.NotEqual, appendKeyword);
    }

    /// <summary>
    /// 设置大于查询条件
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="source">Sql生成器</param>
    /// <param name="expression">列名表达式。范例：t => t.Name</param>
    /// <param name="value">值</param>
    public static IEsBuilder<TEntity> Greater<TEntity>(this IEsBuilder<TEntity> source, Expression<Func<TEntity, object>> expression, object value)
        where TEntity : class
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));
        return source.Where(expression, value, Operator.Greater);
    }

    /// <summary>
    /// 设置小于查询条件
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="source">Sql生成器</param>
    /// <param name="expression">列名表达式。范例：t => t.Name</param>
    /// <param name="value">值</param>
    public static IEsBuilder<TEntity> Less<TEntity>(this IEsBuilder<TEntity> source, Expression<Func<TEntity, object>> expression, object value)
        where TEntity : class
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));
        return source.Where(expression, value, Operator.Less);
    }

    /// <summary>
    /// 设置大于等于查询条件
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="source">Sql生成器</param>
    /// <param name="expression">列名表达式。范例：t => t.Name</param>
    /// <param name="value">值</param>
    public static IEsBuilder<TEntity> GreaterEqual<TEntity>(this IEsBuilder<TEntity> source, Expression<Func<TEntity, object>> expression, object value)
        where TEntity : class
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));
        return source.Where(expression, value, Operator.GreaterEqual);
    }

    /// <summary>
    /// 设置小于等于查询条件
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="source">Sql生成器</param>
    /// <param name="expression">列名表达式。范例：t => t.Name</param>
    /// <param name="value">值</param>
    public static IEsBuilder<TEntity> LessEqual<TEntity>(this IEsBuilder<TEntity> source, Expression<Func<TEntity, object>> expression, object value)
        where TEntity : class
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));
        return source.Where(expression, value, Operator.LessEqual);
    }

    /// <summary>
    /// 设置模糊匹配查询条件
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="source">Sql生成器</param>
    /// <param name="expression">列名表达式。范例：t => t.Name</param>
    /// <param name="value">值</param>
    /// <param name="appendKeyword">是否追加keyword关键词，如果字段类型被识别成 text 类型，同时还附带 keyword 子字段，则需要追加 keyword 的子字段来实现精确匹配。</param>
    public static IEsBuilder<TEntity> Contains<TEntity>(this IEsBuilder<TEntity> source, Expression<Func<TEntity, object>> expression, object value, bool appendKeyword = false)
        where TEntity : class
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));
        return source.Where(expression, value, Operator.Contains, appendKeyword);
    }

    /// <summary>
    /// 设置头匹配查询条件
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="source">Sql生成器</param>
    /// <param name="expression">列名表达式。范例：t => t.Name</param>
    /// <param name="value">值</param>
    /// <param name="appendKeyword">是否追加keyword关键词，如果字段类型被识别成 text 类型，同时还附带 keyword 子字段，则需要追加 keyword 的子字段来实现精确匹配。</param>
    public static IEsBuilder<TEntity> Starts<TEntity>(this IEsBuilder<TEntity> source, Expression<Func<TEntity, object>> expression, object value, bool appendKeyword = false)
        where TEntity : class
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));
        return source.Where(expression, value, Operator.Starts, appendKeyword);
    }

    /// <summary>
    /// 设置尾匹配查询条件
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="source">Sql生成器</param>
    /// <param name="expression">列名表达式。范例：t => t.Name</param>
    /// <param name="value">值</param>
    /// <param name="appendKeyword">是否追加keyword关键词，如果字段类型被识别成 text 类型，同时还附带 keyword 子字段，则需要追加 keyword 的子字段来实现精确匹配。</param>
    public static IEsBuilder<TEntity> Ends<TEntity>(this IEsBuilder<TEntity> source, Expression<Func<TEntity, object>> expression, object value, bool appendKeyword = false)
        where TEntity : class
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));
        return source.Where(expression, value, Operator.Ends, appendKeyword);
    }

    /// <summary>
    /// 设置In条件
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="source">Sql生成器</param>
    /// <param name="expression">列名表达式。范例：t => t.Name</param>
    /// <param name="values">值集合</param>
    /// <param name="appendKeyword">是否追加keyword关键词，如果字段类型被识别成 text 类型，同时还附带 keyword 子字段，则需要追加 keyword 的子字段来实现精确匹配。</param>
    public static IEsBuilder<TEntity> In<TEntity>(this IEsBuilder<TEntity> source, Expression<Func<TEntity, object>> expression, IEnumerable<object> values, bool appendKeyword = false)
        where TEntity : class
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));
        if (source is IEsPartAccessor accessor)
            accessor.WhereClause.Where(expression, values, Operator.In, appendKeyword);
        return source;
    }

    /// <summary>
    /// 设置Not In条件
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="source">Sql生成器</param>
    /// <param name="expression">列名表达式。范例：t => t.Name</param>
    /// <param name="values">值集合</param>
    /// <param name="appendKeyword">是否追加keyword关键词，如果字段类型被识别成 text 类型，同时还附带 keyword 子字段，则需要追加 keyword 的子字段来实现精确匹配。</param>
    public static IEsBuilder<TEntity> NotIn<TEntity>(this IEsBuilder<TEntity> source, Expression<Func<TEntity, object>> expression, IEnumerable<object> values, bool appendKeyword = false)
        where TEntity : class
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));
        if (source is IEsPartAccessor accessor)
            accessor.WhereClause.Where(expression, values, Operator.NotIn, appendKeyword);
        return source;
    }

    /// <summary>
    /// 添加范围查询条件
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="source">Sql生成器</param>
    /// <param name="expression">列名表达式。范例：t => t.Name</param>
    /// <param name="min">最小值</param>
    /// <param name="max">最大值</param>
    /// <param name="boundary">包含边界</param>
    public static IEsBuilder<TEntity> Between<TEntity>(this IEsBuilder<TEntity> source, Expression<Func<TEntity, object>> expression, int? min, int? max, Boundary boundary = Boundary.Both)
        where TEntity : class
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));
        if (source is IEsPartAccessor accessor)
            accessor.WhereClause.Between(expression, min, max, boundary);
        return source;
    }

    /// <summary>
    /// 添加范围查询条件
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="source">Sql生成器</param>
    /// <param name="expression">列名表达式。范例：t => t.Name</param>
    /// <param name="min">最小值</param>
    /// <param name="max">最大值</param>
    /// <param name="boundary">包含边界</param>
    public static IEsBuilder<TEntity> Between<TEntity>(this IEsBuilder<TEntity> source, Expression<Func<TEntity, object>> expression, long? min, long? max, Boundary boundary = Boundary.Both)
        where TEntity : class
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));
        if (source is IEsPartAccessor accessor)
            accessor.WhereClause.Between(expression, min, max, boundary);
        return source;
    }

    /// <summary>
    /// 添加范围查询条件
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="source">Sql生成器</param>
    /// <param name="expression">列名表达式。范例：t => t.Name</param>
    /// <param name="min">最小值</param>
    /// <param name="max">最大值</param>
    /// <param name="boundary">包含边界</param>
    public static IEsBuilder<TEntity> Between<TEntity>(this IEsBuilder<TEntity> source, Expression<Func<TEntity, object>> expression, float? min, float? max, Boundary boundary = Boundary.Both)
        where TEntity : class
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));
        if (source is IEsPartAccessor accessor)
            accessor.WhereClause.Between(expression, min, max, boundary);
        return source;
    }

    /// <summary>
    /// 添加范围查询条件
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="source">Sql生成器</param>
    /// <param name="expression">列名表达式。范例：t => t.Name</param>
    /// <param name="min">最小值</param>
    /// <param name="max">最大值</param>
    /// <param name="boundary">包含边界</param>
    public static IEsBuilder<TEntity> Between<TEntity>(this IEsBuilder<TEntity> source, Expression<Func<TEntity, object>> expression, double? min, double? max, Boundary boundary = Boundary.Both)
        where TEntity : class
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));
        if (source is IEsPartAccessor accessor)
            accessor.WhereClause.Between(expression, min, max, boundary);
        return source;
    }

    /// <summary>
    /// 添加范围查询条件
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="source">Sql生成器</param>
    /// <param name="expression">列名表达式。范例：t => t.Name</param>
    /// <param name="min">最小值</param>
    /// <param name="max">最大值</param>
    /// <param name="boundary">包含边界</param>
    public static IEsBuilder<TEntity> Between<TEntity>(this IEsBuilder<TEntity> source, Expression<Func<TEntity, object>> expression, decimal? min, decimal? max, Boundary boundary = Boundary.Both)
        where TEntity : class
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));
        if (source is IEsPartAccessor accessor)
            accessor.WhereClause.Between(expression, min, max, boundary);
        return source;
    }

    /// <summary>
    /// 添加范围查询条件
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="source">Sql生成器</param>
    /// <param name="expression">列名表达式。范例：t => t.Name</param>
    /// <param name="min">最小值</param>
    /// <param name="max">最大值</param>
    /// <param name="includeTime">是否包含时间</param>
    /// <param name="boundary">包含边界</param>
    public static IEsBuilder<TEntity> Between<TEntity>(this IEsBuilder<TEntity> source, Expression<Func<TEntity, object>> expression, DateTime? min, DateTime? max, bool includeTime = true, Boundary? boundary = Boundary.Both)
        where TEntity : class
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));
        if (source is IEsPartAccessor accessor)
            accessor.WhereClause.Between(expression, min, max, includeTime, boundary);
        return source;
    }

    #endregion

}