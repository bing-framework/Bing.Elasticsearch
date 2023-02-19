using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Bing.Extensions;
using Nest;

namespace Bing.Elasticsearch.Builders.Clauses;

/// <summary>
/// Select 子句
/// </summary>
public class SelectClause : ClauseBase, ISelectClause
{
    /// <summary>
    /// 包含字段集合
    /// </summary>
    private readonly List<Field> _includeFields;

    /// <summary>
    /// 排除字段集合
    /// </summary>
    private readonly List<Field> _excludeFields;

    /// <summary>
    /// 初始化一个<see cref="ClauseBase"/>类型的实例
    /// </summary>
    /// <param name="esBuilder">ES生成器</param>
    /// <exception cref="ArgumentNullException"></exception>
    public SelectClause(EsBuilderBase esBuilder) : base(esBuilder)
    {
        _includeFields = new List<Field>();
        _excludeFields = new List<Field>();
    }

    /// <summary>
    /// 设置星号*列
    /// </summary>
    public void Select()
    {
        _includeFields.Clear();
        _excludeFields.Clear();
    }

    /// <summary>
    /// 设置列名
    /// </summary>
    /// <param name="columns">列名</param>
    public void Select(string columns)
    {
        if (columns.IsEmpty())
            return;
        var items = columns.Split(',');
        foreach (var item in items) 
            _includeFields.Add(new Field(item));
    }

    /// <summary>
    /// 设置列名
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="column">表达式</param>
    public void Select<TEntity>(Expression<Func<TEntity, object>> column) where TEntity : class
    {
        if (column == null)
            return;
        _includeFields.Add(new Field(column));
    }

    /// <summary>
    /// 移除列名
    /// </summary>
    /// <param name="columns">列名</param>
    public void RemoveSelect(string columns)
    {
        if (columns.IsEmpty())
            return;
        var items = columns.Split(',');
        foreach (var item in items) 
            _excludeFields.Add(new Field(item));
    }

    /// <summary>
    /// 移除列名
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="column">表达式</param>
    public void RemoveSelect<TEntity>(Expression<Func<TEntity, object>> column) where TEntity : class
    {
        if (column == null)
            return;
        _excludeFields.Add(new Field(column));
    }

    /// <summary>
    /// 添加到查询请求
    /// </summary>
    /// <param name="builder">查询请求</param>
    public void AppendTo(ISearchRequest builder)
    {
        builder.Source = GetSource();
    }

    /// <summary>
    /// 获取源过滤器
    /// </summary>
    private SourceFilter GetSource()
    {
        return new SourceFilter
        {
            Includes = GetIncludeFields(),
            Excludes = GetExcludeFields()
        };
    }

    /// <summary>
    /// 获取包含字段
    /// </summary>
    private Fields GetIncludeFields() => _includeFields.Count == 0 ? "*" : _includeFields.ToArray();

    /// <summary>
    /// 获取排除字段
    /// </summary>
    private Fields GetExcludeFields() => _excludeFields.ToArray();
}