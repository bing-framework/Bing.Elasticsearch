namespace Bing.Elasticsearch.Builders.Operations;

/// <summary>
/// 查询操作
/// </summary>
public interface IQueryOperation : ISelect, IFrom, IWhere, IOrderBy, IEnd
{
}