using System;
using Bing.Data.Queries;
using Nest;

namespace Bing.Elasticsearch.Builders.Conditions;

/// <summary>
/// 日期范围过滤条件 - 包含时间
/// </summary>
public class DateTimeRangeCondition : RangeConditionBase<DateTime, DateRangeQuery>
{
    /// <summary>
    /// 初始化一个<see cref="DateTimeRangeCondition"/>类型的实例
    /// </summary>
    /// <param name="column">列名</param>
    /// <param name="minValue">最小值</param>
    /// <param name="maxValue">最大值</param>
    /// <param name="boundary">包含边界</param>
    public DateTimeRangeCondition(Field column, DateTime? minValue, DateTime? maxValue, Boundary boundary = Boundary.Both)
        : base(column, minValue, maxValue, boundary)
    {
    }

    /// <summary>
    /// 最小值是否大于最大值
    /// </summary>
    /// <param name="min">最小值</param>
    /// <param name="max">最大值</param>
    protected override bool IsMinGreaterMax(DateTime? min, DateTime? max) => min > max;

    /// <summary>
    /// 设置大于等于操作
    /// </summary>
    /// <param name="condition">查询条件</param>
    /// <param name="min">最小值</param>
    protected override void SetGreaterEqual(DateRangeQuery condition, DateTime? min) => condition.GreaterThanOrEqualTo = min;

    /// <summary>
    /// 设置大于操作
    /// </summary>
    /// <param name="condition">查询条件</param>
    /// <param name="min">最小值</param>
    protected override void SetGreater(DateRangeQuery condition, DateTime? min) => condition.GreaterThan = min;

    /// <summary>
    /// 设置小于等于操作
    /// </summary>
    /// <param name="condition">查询条件</param>
    /// <param name="max">最大值</param>
    protected override void SetLessEqual(DateRangeQuery condition, DateTime? max) => condition.LessThanOrEqualTo = max;

    /// <summary>
    /// 设置小于操作
    /// </summary>
    /// <param name="condition">查询条件</param>
    /// <param name="max">最大值</param>
    protected override void SetLess(DateRangeQuery condition, DateTime? max) => condition.LessThan = max;
}