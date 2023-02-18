using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Bing.Data;
using Bing.Data.Queries.Conditions;
using Bing.Utils.Parameters;
using Nest;

namespace Bing.Elasticsearch.Builders.Internal;

public class Helper
{
    /// <summary>
    /// 获取列名
    /// </summary>
    /// <param name="expression">表达式</param>
    /// <param name="type">实体类型</param>
    public Field GetColumn(Expression expression, Type type)
    {
        if (expression == null)
            return null;
        return new Field(expression);
    }

    /// <summary>
    /// 获取列名
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="expression">表达式</param>
    public Field GetColumn<TEntity>(Expression<Func<TEntity, object>> expression)
    {
        if (expression == null)
            return null;
        return new Field(expression);
    }

    public IEsCondition CreateCondition(Expression expression, object value, Operator @operator)
    {
    }

    public IEsCondition CreateCondition(Field column, object value, Operator @operator)
    {
        if (column == null)
            throw new ArgumentNullException(nameof(column));

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
        if (@operator == Operator.Contains && value != null && Reflection.Reflections.IsCollection(value.GetType()))
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
    /// 创建In条件
    /// </summary>
    /// <param name="column">列名</param>
    /// <param name="values">值列表</param>
    /// <param name="notIn">是否Not In条件</param>
    private IEsCondition CreateInCondition(string column, IEnumerable values, bool notIn = false)
    {
        if (values == null)
            return NullEsCondition.Instance;
        var paramNames = new List<string>();
        foreach (var value in values)
        {
        }
        if (notIn)
            return new NotInCondition(column, paramNames);
        return new InCondition(column, paramNames);
    }
}
