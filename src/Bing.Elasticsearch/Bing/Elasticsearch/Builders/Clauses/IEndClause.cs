namespace Bing.Elasticsearch.Builders.Clauses;

/// <summary>
/// 结束子句
/// </summary>
public interface IEndClause : IEsClause
{
    /// <summary>
    /// 设置跳过行数
    /// </summary>
    /// <param name="count">跳过行数</param>
    void Skip(int count);

    /// <summary>
    /// 设置获取行数
    /// </summary>
    /// <param name="count">获取的行数</param>
    void Take(int count);
}