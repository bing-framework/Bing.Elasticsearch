using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Bing.Data;
using Bing.Data.Queries;

namespace Bing.Elasticsearch.Builders.Clauses;

/// <summary>
/// Where子句
/// </summary>
public interface IWhereClause : IEsCondition, IEsClause
{
    /// <summary>
    /// And连接条件
    /// </summary>
    /// <param name="condition">查询条件</param>
    void And(IEsCondition condition);

    /// <summary>
    /// Or连接条件
    /// </summary>
    /// <param name="condition">查询条件</param>
    void Or(IEsCondition condition);

    /// <summary>
    /// 设置查询条件
    /// </summary>
    /// <param name="condition">查询条件</param>
    void Where(IEsCondition condition);

    /// <summary>
    /// 设置查询条件
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="expression">列名表达式</param>
    /// <param name="value">值</param>
    /// <param name="operator">运算符</param>
    /// <param name="appendKeyword">是否追加keyword关键词，主要用于模糊查询</param>
    void Where<TEntity>(Expression<Func<TEntity, object>> expression, object value, Operator @operator = Operator.Equal, bool appendKeyword = false) where TEntity : class;

    /// <summary>
    /// 添加范围查询条件
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="expression">列名表达式</param>
    /// <param name="min">最小值</param>
    /// <param name="max">最大值</param>
    /// <param name="boundary">包含边界</param>
    void Between<TEntity>(Expression<Func<TEntity, object>> expression, int? min, int? max, Boundary boundary)
        where TEntity : class;

    /// <summary>
    /// 添加范围查询条件
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="expression">列名表达式</param>
    /// <param name="min">最小值</param>
    /// <param name="max">最大值</param>
    /// <param name="boundary">包含边界</param>
    void Between<TEntity>(Expression<Func<TEntity, object>> expression, long? min, long? max, Boundary boundary)
        where TEntity : class;

    /// <summary>
    /// 添加范围查询条件
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="expression">列名表达式</param>
    /// <param name="min">最小值</param>
    /// <param name="max">最大值</param>
    /// <param name="boundary">包含边界</param>
    void Between<TEntity>(Expression<Func<TEntity, object>> expression, float? min, float? max, Boundary boundary)
        where TEntity : class;

    /// <summary>
    /// 添加范围查询条件
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="expression">列名表达式</param>
    /// <param name="min">最小值</param>
    /// <param name="max">最大值</param>
    /// <param name="boundary">包含边界</param>
    void Between<TEntity>(Expression<Func<TEntity, object>> expression, double? min, double? max, Boundary boundary)
        where TEntity : class;

    /// <summary>
    /// 添加范围查询条件
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="expression">列名表达式</param>
    /// <param name="min">最小值</param>
    /// <param name="max">最大值</param>
    /// <param name="boundary">包含边界</param>
    void Between<TEntity>(Expression<Func<TEntity, object>> expression, decimal? min, decimal? max, Boundary boundary)
        where TEntity : class;

    /// <summary>
    /// 添加范围查询条件
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="expression">列名表达式</param>
    /// <param name="min">最小值</param>
    /// <param name="max">最大值</param>
    /// <param name="includeTime">是否包含时间</param>
    /// <param name="boundary">包含边界</param>
    void Between<TEntity>(Expression<Func<TEntity, object>> expression, DateTime? min, DateTime? max, bool includeTime, Boundary? boundary)
        where TEntity : class;

    /// <summary>
    /// 设置In条件
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="expression">列名表达式</param>
    /// <param name="values">值集合</param>
    void In<TEntity>(Expression<Func<TEntity, object>> expression, IEnumerable<object> values) where TEntity : class;

    /// <summary>
    /// 设置Not In条件
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="expression">列名表达式</param>
    /// <param name="values">值集合</param>
    void NotIn<TEntity>(Expression<Func<TEntity, object>> expression, IEnumerable<object> values) where TEntity : class;
}