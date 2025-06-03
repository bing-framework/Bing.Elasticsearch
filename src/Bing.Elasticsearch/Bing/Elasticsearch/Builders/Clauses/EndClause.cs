using System;
using Bing.Extensions;
using Nest;

namespace Bing.Elasticsearch.Builders.Clauses;

/// <summary>
/// 结束子句
/// </summary>
public class EndClause : ClauseBase, IEndClause
{
    #region 字段

    /// <summary>
    /// 分页大小
    /// </summary>
    private int? _size;

    /// <summary>
    /// 起始行数
    /// </summary>
    private int? _from;

    #endregion

    /// <summary>
    /// 初始化一个<see cref="EndClause"/>类型的实例
    /// </summary>
    /// <param name="esBuilder">ES生成器</param>
    /// <exception cref="ArgumentNullException"></exception>
    public EndClause(EsBuilderBase esBuilder) : base(esBuilder)
    {
    }

    /// <summary>
    /// 设置跳过行数
    /// </summary>
    /// <param name="count">跳过行数</param>
    public void Skip(int count) => _from = count;

    /// <summary>
    /// 设置获取行数
    /// </summary>
    /// <param name="count">获取的行数</param>
    public void Take(int count) => _size = count;

    /// <summary>
    /// 添加到查询请求
    /// </summary>
    /// <param name="builder">查询请求</param>
    public void AppendTo(ISearchRequest builder)
    {
        builder.From = GetFrom();
        builder.Size = GetSize();
    }

    /// <summary>
    /// 获取起始行数
    /// </summary>
    private int GetFrom() => _from > 0 ? _from.SafeValue() : 0;

    /// <summary>
    /// 获取分页大小
    /// </summary>
    private int GetSize() => _size > 0 ? _size.SafeValue() : 10;

    
}