using System;
using System.Linq.Expressions;
using Bing.Data;
using Bing.Elasticsearch.Builders.Conditions;
using Bing.Elasticsearch.Builders.Internal;
using Nest;

namespace Bing.Elasticsearch.Builders.Clauses;

/// <summary>
/// Where子句
/// </summary>
public class WhereClause : ClauseBase, IWhereClause
{
    /// <summary>
    /// 查询条件
    /// </summary>
    private IEsCondition _condition;

    private readonly Helper _helper;

    /// <summary>
    /// 初始化一个<see cref="WhereClause"/>类型的实例
    /// </summary>
    /// <param name="esBuilder">ES生成器</param>
    /// <exception cref="ArgumentNullException"></exception>
    public WhereClause(EsBuilderBase esBuilder) : base(esBuilder)
    {
        _helper = new Helper();
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
        _condition = new AndCondition(_condition, condition);
    }

    /// <summary>
    /// Or连接条件
    /// </summary>
    /// <param name="condition">查询条件</param>
    public void Or(IEsCondition condition)
    {
        _condition = new OrCondition(_condition, condition);
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
        var condition = EsBuilder.ConditionFactory.Create(expression, value, @operator);
        And(condition);
    }

    /// <summary>
    /// 添加到查询请求
    /// </summary>
    /// <param name="builder">查询请求</param>
    public void AppendTo(ISearchRequest builder)
    {
        builder.Query = GetCondition();
    }
}