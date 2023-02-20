using Nest;

namespace Bing.Elasticsearch.Builders.Conditions;

/// <summary>
/// 比较条件基类
/// </summary>
/// <typeparam name="TValue">值类型</typeparam>
/// <typeparam name="TQuery">字段名称查询类型</typeparam>
public abstract class CompareConditionBase<TValue, TQuery> : EsConditionBase
    where TValue : struct
    where TQuery : FieldNameQueryBase, new()
{
    /// <summary>
    /// 查询操作符
    /// </summary>
    private readonly Operator _operator;

    /// <summary>
    /// 初始化一个<see cref="CompareConditionBase{TValue,TQuery}"/>类型的实例
    /// </summary>
    /// <param name="column">列名</param>
    /// <param name="value">值</param>
    /// <param name="operator">操作符</param>
    protected CompareConditionBase(Field column, object value, Operator @operator) 
        : base(column, value)
    {
        _operator = @operator;
    }

    /// <summary>
    /// 创建查询条件
    /// </summary>
    protected TQuery CreateCondition() => new TQuery();

    /// <summary>
    /// 获取查询条件
    /// </summary>
    public override QueryContainer GetCondition()
    {
        var condition = CreateCondition();
        condition.Field = Column;
        var value = GetValue();
        switch (_operator)
        {
            case Operator.Greater:
                SetGreater(condition, value);
                break;
            case Operator.GreaterEqual:
                SetGreaterEqual(condition, value);
                break;
            case Operator.Less:
                SetLess(condition, value);
                break;
            case Operator.LessEqual:
                SetLessEqual(condition, value);
                break;
        }
        return condition;
    }

    /// <summary>
    /// 获取值
    /// </summary>
    protected abstract TValue? GetValue();

    /// <summary>
    /// 设置小于操作
    /// </summary>
    /// <param name="condition">查询条件</param>
    /// <param name="max">最大值</param>
    protected abstract void SetLess(TQuery condition, TValue? max);

    /// <summary>
    /// 设置小于等于操作
    /// </summary>
    /// <param name="condition">查询条件</param>
    /// <param name="max">最大值</param>
    protected abstract void SetLessEqual(TQuery condition, TValue? max);

    /// <summary>
    /// 设置大于操作
    /// </summary>
    /// <param name="condition">查询条件</param>
    /// <param name="min">最小值</param>
    protected abstract void SetGreater(TQuery condition, TValue? min);

    /// <summary>
    /// 设置大于等于操作
    /// </summary>
    /// <param name="condition">查询条件</param>
    /// <param name="min">最小值</param>
    protected abstract void SetGreaterEqual(TQuery condition, TValue? min);
}