using System;
using System.Linq.Expressions;
using Bing.Data;
using Bing.Data.Queries;
using Bing.Elasticsearch.Builders.Conditions;
using Bing.Extensions;
using Bing.Judgments;
using Bing.Reflection;

namespace Bing.Elasticsearch.Builders;

/// <summary>
/// ES条件工厂
/// </summary>
public class EsConditionFactory : IEsConditionFactory
{
    /// <summary>
    /// 创建ES条件
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="column">列名</param>
    /// <param name="value">值</param>
    /// <param name="operator">操作符</param>
    public IEsCondition Create<TEntity>(Expression<Func<TEntity, object>> column, object value, Operator @operator) where TEntity : class
    {
        if (IsInCondition(@operator, value))
            return new InCondition(column, value);
        if (IsNotInCondition(@operator, value))
            return new NotInCondition(column, value);
        switch (@operator)
        {
            case Operator.Equal:
                return new EqualCondition(column, value);
            case Operator.NotEqual:
                return new NotEqualCondition(column, value);
            case Operator.Greater:
            case Operator.GreaterEqual:
            case Operator.Less:
            case Operator.LessEqual:
                return CreateCompareCondition(column, value, @operator);
            case Operator.Starts:
                break;
            case Operator.Ends:
                break;
            case Operator.Contains:
                break;
        }
        throw new NotImplementedException($"运算符 {@operator.Description()} 尚未实现");
    }

    /// <summary>
    /// 创建比较条件
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="column">列名</param>
    /// <param name="value">值</param>
    /// <param name="operator">操作符</param>
    private IEsCondition CreateCompareCondition<TEntity>(Expression<Func<TEntity, object>> column, object value, Operator @operator)
    {
        var type = TypeConv.GetNonNullableType(value.GetType());
        // 整数类型
        if (type.IsIntegerType())
           return new LongCompareCondition(column, value, @operator);
        // 日期类型
        if (type == TypeClass.DateTimeClazz)
            return new DateTimeCompareCondition(column, value, @operator);
        // 小数类型
        if (TypeJudgment.IsNumericType(type))
            return new NumericCompareCondition(column, value, @operator);
        throw new NotSupportedException($"尚未支持[{type.FullName}]类型的条件比较");
    }

    /// <summary>
    /// 是否In条件
    /// </summary>
    /// <param name="operator">运算符</param>
    /// <param name="value">值</param>
    private bool IsInCondition(Operator @operator, object value)
    {
        if (@operator == Operator.In)
            return true;
        return false;
    }

    /// <summary>
    /// 是否Not In条件
    /// </summary>
    /// <param name="operator">运算符</param>
    /// <param name="value">值</param>
    private bool IsNotInCondition(Operator @operator, object value)
    {
        if (@operator == Operator.NotIn)
            return true;
        return false;
    }

    /// <summary>
    /// 创建ES范围条件
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="column">列名</param>
    /// <param name="minValue">最小值</param>
    /// <param name="maxValue">最大值</param>
    /// <param name="boundary">包含边界</param>
    public IEsCondition Create<TEntity>(Expression<Func<TEntity, object>> column, object minValue, object maxValue, Boundary boundary) where TEntity : class
    {
        throw new NotImplementedException();
    }
}