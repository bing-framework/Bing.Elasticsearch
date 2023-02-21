using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Bing.Data;
using Bing.Data.Queries;
using Bing.Elasticsearch.Builders.Conditions;
using Bing.Elasticsearch.Builders.Internal;
using Bing.Extensions;
using Nest;

namespace Bing.Elasticsearch.Builders.Clauses;

/// <summary>
/// Where子句
/// </summary>
public class WhereClause : ClauseBase, IWhereClause
{
    /// <summary>
    /// 查询条件
    /// </summary>
    private IEsCondition _condition;

    private readonly Helper _helper;

    /// <summary>
    /// 初始化一个<see cref="WhereClause"/>类型的实例
    /// </summary>
    /// <param name="esBuilder">ES生成器</param>
    /// <exception cref="ArgumentNullException"></exception>
    public WhereClause(EsBuilderBase esBuilder) : base(esBuilder)
    {
        _helper = new Helper();
    }

    /// <summary>
    /// 获取查询条件
    /// </summary>
    public QueryContainer GetCondition()
    {
        return _condition?.GetCondition();
    }

    /// <summary>
    /// And连接条件
    /// </summary>
    /// <param name="condition">查询条件</param>
    public void And(IEsCondition condition)
    {
        _condition = new AndCondition(_condition, condition);
    }

    /// <summary>
    /// Or连接条件
    /// </summary>
    /// <param name="condition">查询条件</param>
    public void Or(IEsCondition condition)
    {
        _condition = new OrCondition(_condition, condition);
    }

    /// <summary>
    /// 设置查询条件
    /// </summary>
    /// <param name="condition">查询条件</param>
    public void Where(IEsCondition condition) => And(condition);

    /// <summary>
    /// 设置查询条件
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="expression">列名表达式</param>
    /// <param name="value">值</param>
    /// <param name="operator">运算符</param>
    /// <param name="appendKeyword">是否追加keyword关键词，主要用于模糊查询</param>
    public void Where<TEntity>(Expression<Func<TEntity, object>> expression, object value, Operator @operator = Operator.Equal, bool appendKeyword = false) where TEntity : class
    {
        var condition = EsBuilder.ConditionFactory.Create(expression, value, @operator);
        And(condition);
    }

    /// <summary>
    /// 添加范围查询条件
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="expression">列名表达式</param>
    /// <param name="min">最小值</param>
    /// <param name="max">最大值</param>
    /// <param name="boundary">包含边界</param>
    public void Between<TEntity>(Expression<Func<TEntity, object>> expression, int? min, int? max, Boundary boundary) where TEntity : class
    {
        if (min > max)
        {
            Where(EsBuilder.ConditionFactory.CreateBetween(expression, max, min, boundary));
            return;
        }
        Where(EsBuilder.ConditionFactory.CreateBetween(expression, min, max, boundary));
    }

    /// <summary>
    /// 添加范围查询条件
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="expression">列名表达式</param>
    /// <param name="min">最小值</param>
    /// <param name="max">最大值</param>
    /// <param name="boundary">包含边界</param>
    public void Between<TEntity>(Expression<Func<TEntity, object>> expression, long? min, long? max, Boundary boundary) where TEntity : class
    {
        if (min > max)
        {
            Where(EsBuilder.ConditionFactory.CreateBetween(expression, max, min, boundary));
            return;
        }
        Where(EsBuilder.ConditionFactory.CreateBetween(expression, min, max, boundary));
    }

    /// <summary>
    /// 添加范围查询条件
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="expression">列名表达式</param>
    /// <param name="min">最小值</param>
    /// <param name="max">最大值</param>
    /// <param name="boundary">包含边界</param>
    public void Between<TEntity>(Expression<Func<TEntity, object>> expression, float? min, float? max, Boundary boundary) where TEntity : class
    {
        if (min > max)
        {
            Where(EsBuilder.ConditionFactory.CreateBetween(expression, max, min, boundary));
            return;
        }
        Where(EsBuilder.ConditionFactory.CreateBetween(expression, min, max, boundary));
    }

    /// <summary>
    /// 添加范围查询条件
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="expression">列名表达式</param>
    /// <param name="min">最小值</param>
    /// <param name="max">最大值</param>
    /// <param name="boundary">包含边界</param>
    public void Between<TEntity>(Expression<Func<TEntity, object>> expression, double? min, double? max, Boundary boundary) where TEntity : class
    {
        if (min > max)
        {
            Where(EsBuilder.ConditionFactory.CreateBetween(expression, max, min, boundary));
            return;
        }
        Where(EsBuilder.ConditionFactory.CreateBetween(expression, min, max, boundary));
    }

    /// <summary>
    /// 添加范围查询条件
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="expression">列名表达式</param>
    /// <param name="min">最小值</param>
    /// <param name="max">最大值</param>
    /// <param name="boundary">包含边界</param>
    public void Between<TEntity>(Expression<Func<TEntity, object>> expression, decimal? min, decimal? max, Boundary boundary) where TEntity : class
    {
        if (min > max)
        {
            Where(EsBuilder.ConditionFactory.CreateBetween(expression, max, min, boundary));
            return;
        }
        Where(EsBuilder.ConditionFactory.CreateBetween(expression, min, max, boundary));
    }

    /// <summary>
    /// 添加范围查询条件
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="expression">列名表达式</param>
    /// <param name="min">最小值</param>
    /// <param name="max">最大值</param>
    /// <param name="includeTime">是否包含时间</param>
    /// <param name="boundary">包含边界</param>
    public void Between<TEntity>(Expression<Func<TEntity, object>> expression, DateTime? min, DateTime? max, bool includeTime, Boundary? boundary) where TEntity : class
    {
        Where(EsBuilder.ConditionFactory.CreateBetween(expression, GetMin(min, max, includeTime), GetMax(min, max, includeTime), GetBoundary(boundary, includeTime)));
    }

    /// <summary>
    /// 获取最小日期
    /// </summary>
    /// <param name="min">最小值</param>
    /// <param name="max">最大值</param>
    /// <param name="includeTime">是否包含时间</param>
    private DateTime? GetMin(DateTime? min, DateTime? max, bool includeTime)
    {
        if (min == null)
            return null;
        var result = min;
        if (min > max)
            result = min;
        if (includeTime)
            return result;
        return result.SafeValue().Date;
    }

    /// <summary>
    /// 获取最大日期
    /// </summary>
    /// <param name="min">最小值</param>
    /// <param name="max">最大值</param>
    /// <param name="includeTime">是否包含时间</param>
    private DateTime? GetMax(DateTime? min, DateTime? max, bool includeTime)
    {
        if (max == null)
            return null;
        var result = max;
        if (min > max)
            result = min;
        if (includeTime)
            return result;
        return result.SafeValue().Date.AddDays(1);
    }

    /// <summary>
    /// 获取日期范围查询条件边界
    /// </summary>
    /// <param name="boundary">包含边界</param>
    /// <param name="includeTime">是否包含时间</param>
    private Boundary GetBoundary(Boundary? boundary, bool includeTime)
    {
        if (boundary != null)
            return boundary.SafeValue();
        if (includeTime)
            return Boundary.Both;
        return Boundary.Left;
    }

    /// <summary>
    /// 设置In条件
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="expression">列名表达式</param>
    /// <param name="values">值集合</param>
    public void In<TEntity>(Expression<Func<TEntity, object>> expression, IEnumerable<object> values) where TEntity : class => Where(expression, values, Operator.In);

    /// <summary>
    /// 设置Not In条件
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="expression">列名表达式</param>
    /// <param name="values">值集合</param>
    public void NotIn<TEntity>(Expression<Func<TEntity, object>> expression, IEnumerable<object> values) where TEntity : class => Where(expression, values, Operator.NotIn);

    /// <summary>
    /// 添加到查询请求
    /// </summary>
    /// <param name="builder">查询请求</param>
    public void AppendTo(ISearchRequest builder) => builder.Query = GetCondition();
}