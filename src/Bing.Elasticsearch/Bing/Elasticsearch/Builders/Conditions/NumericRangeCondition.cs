using Bing.Data.Queries;
using Bing.Helpers;
using Nest;

namespace Bing.Elasticsearch.Builders.Conditions;

/// <summary>
/// double范围过滤条件
/// </summary>
public class NumericRangeCondition : RangeConditionBase<double, NumericRangeQuery>
{
    /// <summary>
    /// 初始化一个<see cref="NumericRangeCondition"/>类型的实例
    /// </summary>
    /// <param name="column">列名</param>
    /// <param name="minValue">最小值</param>
    /// <param name="maxValue">最大值</param>
    /// <param name="boundary">包含边界</param>
    public NumericRangeCondition(Field column, object minValue, object maxValue, Boundary boundary = Boundary.Both)
        : base(column, minValue, maxValue, boundary)
    {
    }

    /// <summary>
    /// 获取值
    /// </summary>
    /// <param name="value">值</param>
    protected override double? GetValue(object value) => Conv.ToDoubleOrNull(value);

    /// <summary>
    /// 最小值是否大于最大值
    /// </summary>
    /// <param name="min">最小值</param>
    /// <param name="max">最大值</param>
    protected override bool IsMinGreaterMax(double? min, double? max) => min > max;

    /// <summary>
    /// 设置大于等于操作
    /// </summary>
    /// <param name="condition">查询条件</param>
    /// <param name="min">最小值</param>
    protected override void SetGreaterEqual(NumericRangeQuery condition, double? min) => condition.GreaterThanOrEqualTo = min;

    /// <summary>
    /// 设置大于操作
    /// </summary>
    /// <param name="condition">查询条件</param>
    /// <param name="min">最小值</param>
    protected override void SetGreater(NumericRangeQuery condition, double? min) => condition.GreaterThan = min;

    /// <summary>
    /// 设置小于等于操作
    /// </summary>
    /// <param name="condition">查询条件</param>
    /// <param name="max">最大值</param>
    protected override void SetLessEqual(NumericRangeQuery condition, double? max) => condition.LessThanOrEqualTo = max;

    /// <summary>
    /// 设置小于操作
    /// </summary>
    /// <param name="condition">查询条件</param>
    /// <param name="max">最大值</param>
    protected override void SetLess(NumericRangeQuery condition, double? max) => condition.LessThan = max;
}