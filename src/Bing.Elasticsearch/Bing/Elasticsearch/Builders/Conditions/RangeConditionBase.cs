using Bing.Data;
using Bing.Data.Queries;
using Nest;

namespace Bing.Elasticsearch.Builders.Conditions;

/// <summary>
/// 范围过滤条件
/// </summary>
/// <typeparam name="TValue">值类型</typeparam>
/// <typeparam name="TQuery">查询类型</typeparam>
public abstract class RangeConditionBase<TValue, TQuery> : IEsCondition
    where TValue : struct
    where TQuery : FieldNameQueryBase, new()
{
    /// <summary>
    /// 列名
    /// </summary>
    private readonly Field Column;

    /// <summary>
    /// 最小值
    /// </summary>
    private TValue? MinValue;

    /// <summary>
    /// 最大值
    /// </summary>
    private TValue? MaxValue;

    /// <summary>
    /// 包含边界
    /// </summary>
    private readonly Boundary _boundary;

    /// <summary>
    /// 初始化一个<see cref="RangeConditionBase{TValue,TQuery}"/>类型的实例
    /// </summary>
    /// <param name="column">列名</param>
    /// <param name="minValue">最小值</param>
    /// <param name="maxValue">最大值</param>
    /// <param name="boundary">包含边界</param>
    protected RangeConditionBase(Field column, TValue? minValue, TValue? maxValue, Boundary boundary)
    {
        Column = column;
        MinValue = minValue;
        MaxValue = maxValue;
        _boundary = boundary;
    }

    /// <summary>
    /// 获取查询条件
    /// </summary>
    public QueryContainer GetCondition()
    {
        if (MinValue == null && MaxValue == null)
            return null;
        Adjust(MinValue, MaxValue);
        var condition = CreateCondition();
        condition.Field = Column;
        SetLeftOperation(condition);
        SetRightOperation(condition);
        return condition;
    }

    /// <summary>
    /// 当最小值大于最大值时进行校正
    /// </summary>
    /// <param name="min">最小值</param>
    /// <param name="max">最大值</param>
    private void Adjust(TValue? min, TValue? max)
    {
        if (IsMinGreaterMax(min, max) == false)
            return;
        MinValue = max;
        MaxValue = min;
    }

    /// <summary>
    /// 最小值是否大于最大值
    /// </summary>
    /// <param name="min">最小值</param>
    /// <param name="max">最大值</param>
    protected abstract bool IsMinGreaterMax(TValue? min, TValue? max);

    /// <summary>
    /// 创建查询条件
    /// </summary>
    protected TQuery CreateCondition() => new TQuery();

    /// <summary>
    /// 设置左操作
    /// </summary>
    /// <param name="condition">查询条件</param>
    private void SetLeftOperation(TQuery condition)
    {
        if (MinValue == null)
            return;
        if (_boundary == Boundary.Left || _boundary == Boundary.Both)
        {
            SetGreaterEqual(condition, GetMinValue());
            return;
        }
        SetGreater(condition, GetMinValue());
    }

    /// <summary>
    /// 设置大于等于操作
    /// </summary>
    /// <param name="condition">查询条件</param>
    /// <param name="min">最小值</param>
    protected abstract void SetGreaterEqual(TQuery condition, TValue? min);

    /// <summary>
    /// 获取最小值
    /// </summary>
    protected virtual TValue? GetMinValue() => MinValue;

    /// <summary>
    /// 设置大于操作
    /// </summary>
    /// <param name="condition">查询条件</param>
    /// <param name="min">最小值</param>
    protected abstract void SetGreater(TQuery condition, TValue? min);

    /// <summary>
    /// 设置右操作
    /// </summary>
    /// <param name="condition">查询条件</param>
    private void SetRightOperation(TQuery condition)
    {
        if (MaxValue == null)
            return;
        if (_boundary == Boundary.Right || _boundary == Boundary.Both)
        {
            SetLessEqual(condition, GetMaxValue());
            return;
        }

        SetLess(condition, GetMaxValue());
    }

    /// <summary>
    /// 设置小于等于操作
    /// </summary>
    /// <param name="condition">查询条件</param>
    /// <param name="max">最大值</param>
    protected abstract void SetLessEqual(TQuery condition, TValue? max);

    /// <summary>
    /// 获取最大值
    /// </summary>
    protected virtual TValue? GetMaxValue() => MaxValue;

    /// <summary>
    /// 设置小于操作
    /// </summary>
    /// <param name="condition">查询条件</param>
    /// <param name="max">最大值</param>
    protected abstract void SetLess(TQuery condition, TValue? max);
}