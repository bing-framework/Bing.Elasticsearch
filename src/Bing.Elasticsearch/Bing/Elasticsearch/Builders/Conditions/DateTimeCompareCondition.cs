using System;
using Bing.Helpers;
using Nest;

namespace Bing.Elasticsearch.Builders.Conditions;

/// <summary>
/// 日期比较过滤条件
/// </summary>
public class DateTimeCompareCondition : CompareConditionBase<DateTime, DateRangeQuery>
{
    /// <summary>
    /// 初始化一个<see cref="DateTimeCompareCondition"/>类型的实例
    /// </summary>
    /// <param name="column">列名</param>
    /// <param name="value">值</param>
    /// <param name="operator">查询操作符</param>
    public DateTimeCompareCondition(Field column, object value, Operator @operator) : base(column, value, @operator)
    {
    }

    /// <summary>
    /// 获取值
    /// </summary>
    protected override DateTime? GetValue() => Conv.ToDateOrNull(Value);

    /// <summary>
    /// 设置小于操作
    /// </summary>
    /// <param name="condition">查询条件</param>
    /// <param name="max">最大值</param>
    protected override void SetLess(DateRangeQuery condition, DateTime? max)
    {
        condition.LessThan = max;
    }

    /// <summary>
    /// 设置小于等于操作
    /// </summary>
    /// <param name="condition">查询条件</param>
    /// <param name="max">最大值</param>
    protected override void SetLessEqual(DateRangeQuery condition, DateTime? max)
    {
        condition.LessThanOrEqualTo = max;
    }

    /// <summary>
    /// 设置大于操作
    /// </summary>
    /// <param name="condition">查询条件</param>
    /// <param name="min">最小值</param>
    protected override void SetGreater(DateRangeQuery condition, DateTime? min)
    {
        condition.GreaterThan = min;
    }

    /// <summary>
    /// 设置大于等于操作
    /// </summary>
    /// <param name="condition">查询条件</param>
    /// <param name="min">最小值</param>
    protected override void SetGreaterEqual(DateRangeQuery condition, DateTime? min)
    {
        condition.GreaterThanOrEqualTo = min;
    }
}