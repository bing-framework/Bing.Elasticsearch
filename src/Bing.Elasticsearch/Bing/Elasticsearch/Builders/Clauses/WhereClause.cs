using System;
using System.Linq.Expressions;
using Bing.Data;
using Bing.Data.Queries.Conditions;
using Nest;

namespace Bing.Elasticsearch.Builders.Clauses;

public class WhereClause : IWhereClause
{
    /// <summary>
    /// 查询条件
    /// </summary>
    private IEsCondition _condition;

    public WhereClause(IEsCondition condition = null)
    {
        _condition = condition;
    }

    /// <summary>
    /// 获取查询条件
    /// </summary>
    public QueryContainer GetCondition()
    {
        return _condition?.GetCondition();
    }

    /// <summary>
    /// And连接条件
    /// </summary>
    /// <param name="condition">查询条件</param>
    public void And(IEsCondition condition)
    {
        _condition = new AndEsCondition(_condition, condition);
    }

    /// <summary>
    /// Or连接条件
    /// </summary>
    /// <param name="condition">查询条件</param>
    public void Or(IEsCondition condition)
    {
        _condition = new OrEsCondition(_condition, condition);
    }

    /// <summary>
    /// 设置查询条件
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="expression">查询条件表达式</param>
    public void Where<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : class
    {
        if (expression == null)
            throw new ArgumentNullException(nameof(expression));
    }

    /// <summary>
    /// 设置查询条件
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="expression">列名表达式</param>
    /// <param name="value">值</param>
    /// <param name="operator">运算符</param>
    public void Where<TEntity>(Expression<Func<TEntity, object>> expression, object value, Operator @operator = Operator.Equal) where TEntity : class
    {
        throw new NotImplementedException();
    }
}