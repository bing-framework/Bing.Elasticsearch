using System;
using System.Linq.Expressions;
using Bing.Data;
using Bing.Data.Queries;
using Bing.Data.Queries.Conditions;
using Bing.Elasticsearch.Builders.Conditions;
using Bing.Elasticsearch.Common.Constants;
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
    /// <param name="appendKeyword">是否追加keyword关键词，主要用于模糊查询</param>
    public IEsCondition Create<TEntity>(Expression<Func<TEntity, object>> column, object value, Operator @operator, bool appendKeyword = false) 
        where TEntity : class
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
                if (appendKeyword)
                    return new StartsCondition(Nest.ExpressionExtensions.AppendSuffix(column, BaseEsConst.KEYWORD), value);
                return new StartsCondition(column, value);
            case Operator.Ends:
                if (appendKeyword)
                    return new EndsCondition(Nest.ExpressionExtensions.AppendSuffix(column, BaseEsConst.KEYWORD), value);
                return new EndsCondition(column, value);
            case Operator.Contains:
                if (appendKeyword)
                    return new ContainsCondition(Nest.ExpressionExtensions.AppendSuffix(column, BaseEsConst.KEYWORD), value);
                return new ContainsCondition(column, value);
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
    public IEsCondition CreateBetween<TEntity>(Expression<Func<TEntity, object>> column, object minValue, object maxValue, Boundary boundary) 
        where TEntity : class
    {
        return CreateBetweenCondition(column, minValue, maxValue, boundary);
    }

    /// <summary>
    /// 创建范围条件
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="column">列名</param>
    /// <param name="minValue">最小值</param>
    /// <param name="maxValue">最大值</param>
    /// <param name="boundary">包含边界</param>
    private IEsCondition CreateBetweenCondition<TEntity>(Expression<Func<TEntity, object>> column, object minValue, object maxValue, Boundary boundary)
    {
        if (minValue == null && maxValue == null)
            return NullEsCondition.Instance;
        var type = minValue != null
            ? TypeConv.GetNonNullableType(minValue.GetType())
            : TypeConv.GetNonNullableType(maxValue.GetType());
        // 整数类型
        if (type.IsIntegerType())
            return new LongRangeCondition(column, minValue, maxValue, boundary);
        // 日期类型
        if (type == TypeClass.DateTimeClazz)
            return new DateTimeRangeCondition(column, minValue, maxValue, boundary);
        // 小数类型
        if (TypeJudgment.IsNumericType(type))
            return new NumericRangeCondition(column, minValue, maxValue, boundary);
        throw new NotSupportedException($"尚未支持[{type.FullName}]类型的条件比较");
    }
}