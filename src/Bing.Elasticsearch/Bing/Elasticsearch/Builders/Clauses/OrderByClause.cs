using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Bing.Elasticsearch.Common.Constants;
using Nest;

namespace Bing.Elasticsearch.Builders.Clauses;

/// <summary>
/// Order By子句
/// </summary>
public class OrderByClause : ClauseBase, IOrderByClause
{
    /// <summary>
    /// 排序列表
    /// </summary>
    private readonly List<ISort> _sorts;

    /// <summary>
    /// 初始化一个<see cref="OrderByClause"/>类型的实例
    /// </summary>
    /// <param name="esBuilder">ES生成器</param>
    /// <exception cref="ArgumentNullException"></exception>
    public OrderByClause(EsBuilderBase esBuilder) : base(esBuilder)
    {
        _sorts = new List<ISort>();
    }

    /// <summary>
    /// 排序
    /// </summary>
    /// <param name="column">列名</param>
    /// <param name="desc">是否倒序</param>
    public void OrderBy(string column, bool desc = false)
    {
        if (string.IsNullOrWhiteSpace(column))
            return;
        _sorts.Add(new FieldSort { Field = new Field(column), Order = GetOrder(desc) });
    }

    /// <summary>
    /// 排序
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="column">排序列</param>
    /// <param name="desc">是否倒序</param>
    /// <param name="appendKeyword">是否追加keyword关键词。text 类型字段不能直接使用，否则将会产生异常</param>
    public void OrderBy<TEntity>(Expression<Func<TEntity, object>> column, bool desc = false, bool appendKeyword = false) where TEntity : class
    {
        if (column is null)
            return;
        var targetColumn = appendKeyword ? column.AppendSuffix(BaseEsConst.KEYWORD) : column;
        _sorts.Add(new FieldSort { Field = new Field(targetColumn), Order = GetOrder(desc) });
    }

    /// <summary>
    /// 获取排序方向
    /// </summary>
    /// <param name="desc">是否降序排序</param>
    private SortOrder GetOrder(bool desc) => desc ? SortOrder.Descending : SortOrder.Ascending;

    /// <summary>
    /// 添加到查询请求
    /// </summary>
    /// <param name="builder">查询请求</param>
    public void AppendTo(ISearchRequest builder)
    {
        builder.Sort = _sorts;
    }
}