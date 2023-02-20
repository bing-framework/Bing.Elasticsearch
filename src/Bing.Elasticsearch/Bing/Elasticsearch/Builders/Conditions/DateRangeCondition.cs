using System;
using Bing.Data.Queries;
using Bing.Extensions;
using Nest;

namespace Bing.Elasticsearch.Builders.Conditions;

/// <summary>
/// 日期范围过滤条件 - 不包含时间
/// </summary>
public class DateRangeCondition : DateTimeRangeCondition
{
    /// <summary>
    /// 初始化一个<see cref="DateRangeCondition"/>类型的实例
    /// </summary>
    /// <param name="column">列名</param>
    /// <param name="minValue">最小值</param>
    /// <param name="maxValue">最大值</param>
    /// <param name="boundary">包含边界</param>
    public DateRangeCondition(Field column, DateTime? minValue, DateTime? maxValue, Boundary boundary = Boundary.Both)
        : base(column, minValue, maxValue, boundary)
    {
    }

    /// <summary>
    /// 获取最小值
    /// </summary>
    protected override DateTime? GetMinValue() => base.GetMinValue().SafeValue().Date;

    /// <summary>
    /// 获取最大值
    /// </summary>
    protected override DateTime? GetMaxValue() => base.GetMaxValue().SafeValue().Date.AddDays(1);
}