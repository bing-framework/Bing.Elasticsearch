using System;
using System.Linq.Expressions;
using Bing.Data;

namespace Bing.Elasticsearch.Builders.Clauses;

public interface IWhereClause : IEsCondition
{
    /// <summary>
    /// And连接条件
    /// </summary>
    /// <param name="condition">查询条件</param>
    void And(IEsCondition condition);

    /// <summary>
    /// Or连接条件
    /// </summary>
    /// <param name="condition">查询条件</param>
    void Or(IEsCondition condition);

    /// <summary>
    /// 设置查询条件
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="expression">查询条件表达式</param>
    void Where<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : class;

    /// <summary>
    /// 设置查询条件
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="expression">列名表达式</param>
    /// <param name="value">值</param>
    /// <param name="operator">运算符</param>
    void Where<TEntity>(Expression<Func<TEntity,object>> expression,object value, Operator @operator = Operator.Equal) where TEntity : class;
}