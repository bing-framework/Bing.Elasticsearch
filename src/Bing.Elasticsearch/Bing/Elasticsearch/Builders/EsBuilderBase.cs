using System;
using Bing.Elasticsearch.Builders.Clauses;
using Bing.Elasticsearch.Repositories;
using Nest;

namespace Bing.Elasticsearch.Builders;

/// <summary>
/// ES生成器基类
/// </summary>
public abstract class EsBuilderBase : IEsBuilder, IEsPartAccessor
{
    #region 字段

    /// <summary>
    /// 索引
    /// </summary>
    private string _index;

    /// <summary>
    /// Select子句
    /// </summary>
    private ISelectClause _selectClause;

    /// <summary>
    /// From子句
    /// </summary>
    private IFromClause _fromClause;

    /// <summary>
    /// Where子句
    /// </summary>
    private IWhereClause _whereClause;

    /// <summary>
    /// OrderBy子句
    /// </summary>
    private IOrderByClause _orderByClause;

    /// <summary>
    /// 结束子句
    /// </summary>
    private IEndClause _endClause;

    /// <summary>
    /// 索引名称解析器
    /// </summary>
    private IIndexNameResolver _indexNameResolver;

    #endregion

    #region 构造函数

    protected EsBuilderBase(IIndexNameResolver indexNameResolver)
    {
        _indexNameResolver = indexNameResolver;
    }

    #endregion

    #region 属性

    /// <summary>
    /// Select子句
    /// </summary>
    public ISelectClause SelectClause => _selectClause ??= CreateSelectClause();

    /// <summary>
    /// From子句
    /// </summary>
    public IFromClause FromClause => _fromClause ??= CreateFromClause();

    /// <summary>
    /// Where子句
    /// </summary>
    public IWhereClause WhereClause => _whereClause ??= CreateWhereClause();

    /// <summary>
    /// OrderBy子句
    /// </summary>
    public IOrderByClause OrderByClause => _orderByClause ??= CreateOrderByClause();

    /// <summary>
    /// 结束子句
    /// </summary>
    public IEndClause EndClause => _endClause ??= CreateEndClause();

    /// <summary>
    /// 索引名称解析器
    /// </summary>
    public IIndexNameResolver IndexNameResolver => _indexNameResolver;

    #endregion

    #region 工厂方法

    /// <summary>
    /// 创建From子句
    /// </summary>
    protected virtual ISelectClause CreateSelectClause() => new SelectClause(this);

    /// <summary>
    /// 创建From子句
    /// </summary>
    protected virtual IFromClause CreateFromClause() => new FromClause(this);

    /// <summary>
    /// 创建Where子句
    /// </summary>
    protected virtual IWhereClause CreateWhereClause() => new WhereClause(this);

    /// <summary>
    /// 创建OrderBy子句
    /// </summary>
    protected virtual IOrderByClause CreateOrderByClause() => new OrderByClause(this);

    /// <summary>
    /// 创建结束子句
    /// </summary>
    protected virtual IEndClause CreateEndClause() => new EndClause(this);

    #endregion

    #region GetSearchRequest(获取查询请求)

    /// <summary>
    /// 获取查询请求
    /// </summary>
    public ISearchRequest GetSearchRequest()
    {
        FromClause.PreInit();
        var request = new SearchRequest(_index);
        AppendTo(request);
        return request;
    }

    /// <summary>
    /// 初始化索引
    /// </summary>
    /// <param name="index">索引名称</param>
    public void InitIndex(string index)
    {
        if (string.IsNullOrWhiteSpace(index))
            throw new ArgumentNullException(nameof(index));
        _index = index;
    }

    #endregion

    #region AppendTo(添加到查询请求)

    /// <summary>
    /// 添加到查询请求
    /// </summary>
    /// <param name="builder">查询请求</param>
    public void AppendTo(ISearchRequest builder)
    {
        AppendEs(builder, _selectClause);
        AppendEs(builder, _fromClause);
        AppendEs(builder, _whereClause);
        AppendEs(builder, _orderByClause);
        AppendEs(builder, _endClause);
    }

    /// <summary>
    /// 添加ES
    /// </summary>
    protected void AppendEs(ISearchRequest builder, IEsClause content)
    {
        content.AppendTo(builder);
    }

    #endregion
}