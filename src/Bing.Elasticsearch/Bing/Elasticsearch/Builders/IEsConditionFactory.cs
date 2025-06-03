using System;
using System.Linq.Expressions;
using Bing.Data;
using Bing.Data.Queries;

namespace Bing.Elasticsearch.Builders;

/// <summary>
/// ES条件工厂
/// </summary>
public interface IEsConditionFactory
{
    /// <summary>
    /// 创建ES条件
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="column">列名</param>
    /// <param name="value">值</param>
    /// <param name="operator">操作符</param>
    /// <param name="appendKeyword">是否追加keyword关键词，主要用于模糊查询</param>
    IEsCondition Create<TEntity>(Expression<Func<TEntity, object>> column, object value, Operator @operator, bool appendKeyword = false) where TEntity : class;

    /// <summary>
    /// 创建ES范围条件
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="column">列名</param>
    /// <param name="minValue">最小值</param>
    /// <param name="maxValue">最大值</param>
    /// <param name="boundary">包含边界</param>
    IEsCondition CreateBetween<TEntity>(Expression<Func<TEntity, object>> column, object minValue, object maxValue, Boundary boundary) where TEntity : class;
}