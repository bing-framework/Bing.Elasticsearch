using System;
using Bing.Elasticsearch.Internals;
using Nest;

namespace Bing.Elasticsearch.Builders.Clauses;

/// <summary>
/// From 子句
/// </summary>
public class FromClause : ClauseBase, IFromClause
{
    /// <summary>
    /// 表名
    /// </summary>
    private string _table;

    /// <summary>
    /// 初始化一个<see cref="FromClause"/>类型的实例
    /// </summary>
    /// <param name="esBuilder">ES生成器</param>
    /// <exception cref="ArgumentNullException"></exception>
    public FromClause(EsBuilderBase esBuilder) : base(esBuilder)
    {
    }

    /// <summary>
    /// 设置表名
    /// </summary>
    /// <param name="table">表名</param>
    public void From(string table)
    {
        _table = table;
    }

    /// <summary>
    /// 设置表名
    /// </summary>
    /// <param name="type">实体类型</param>
    public void From(Type type)
    {
        _table = EsBuilder.IndexNameResolver.GetIndexName(Helper.SafeIndexName(type));
    }

    /// <summary>
    /// 设置表名
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    public void From<TEntity>()
    {
        _table = EsBuilder.IndexNameResolver.GetIndexName(Helper.SafeIndexName<TEntity>());
    }

    /// <summary>
    /// 添加到查询请求
    /// </summary>
    /// <param name="builder">查询请求</param>
    public void AppendTo(ISearchRequest builder)
    {
    }

    /// <summary>
    /// 预先初始化
    /// </summary>
    public void PreInit()
    {
        EsBuilder.InitIndex(_table);
    }
}