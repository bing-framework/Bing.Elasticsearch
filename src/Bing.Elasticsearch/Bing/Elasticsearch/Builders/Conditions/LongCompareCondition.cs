using Bing.Helpers;
using Nest;

namespace Bing.Elasticsearch.Builders.Conditions;

/// <summary>
/// long比较过滤条件
/// </summary>
public class LongCompareCondition : CompareConditionBase<long, LongRangeQuery>
{
    /// <summary>
    /// 初始化一个<see cref="LongCompareCondition"/>类型的实例
    /// </summary>
    /// <param name="column">列名</param>
    /// <param name="value">值</param>
    /// <param name="operator">查询操作符</param>
    public LongCompareCondition(Field column, object value, Operator @operator) : base(column, value, @operator)
    {
    }

    /// <summary>
    /// 获取值
    /// </summary>
    protected override long? GetValue() => Conv.ToLongOrNull(Value);

    /// <summary>
    /// 设置小于操作
    /// </summary>
    /// <param name="condition">查询条件</param>
    /// <param name="max">最大值</param>
    protected override void SetLess(LongRangeQuery condition, long? max)
    {
        condition.LessThan = max;
    }

    /// <summary>
    /// 设置小于等于操作
    /// </summary>
    /// <param name="condition">查询条件</param>
    /// <param name="max">最大值</param>
    protected override void SetLessEqual(LongRangeQuery condition, long? max)
    {
        condition.LessThanOrEqualTo = max;
    }

    /// <summary>
    /// 设置大于操作
    /// </summary>
    /// <param name="condition">查询条件</param>
    /// <param name="min">最小值</param>
    protected override void SetGreater(LongRangeQuery condition, long? min)
    {
        condition.GreaterThan = min;
    }

    /// <summary>
    /// 设置大于等于操作
    /// </summary>
    /// <param name="condition">查询条件</param>
    /// <param name="min">最小值</param>
    protected override void SetGreaterEqual(LongRangeQuery condition, long? min)
    {
        condition.GreaterThanOrEqualTo = min;
    }
}