namespace Bing.Elasticsearch.Options;

/// <summary>
/// ES 连接池类型。
/// 支持ping-说明能够发现节点的状态；
/// 支持嗅探-说明能够发现新的节点
/// </summary>
public enum ElasticsearchConnectionPoolType
{
    /// <summary>
    /// 静态连接池。推荐使用，应用于已知集群，请求时随机请求各个正常节点，支持ping，不支持嗅探
    /// </summary>
    Static,
    /// <summary>
    /// 单节点连接池
    /// </summary>
    SingleNode,
    /// <summary>
    /// 嗅探连接池。可动态嗅探集群，随机请求，支持嗅探、ping
    /// </summary>
    Sniffing,
    /// <summary>
    /// 固定连接池。选择一个可用节点作为请求主节点，支持ping，不支持嗅探
    /// </summary>
    Sticky,
    /// <summary>
    /// 固定嗅探连接池。选择一个可用节点作为请求主节点，支持ping，支持嗅探
    /// </summary>
    StickySniffing
}