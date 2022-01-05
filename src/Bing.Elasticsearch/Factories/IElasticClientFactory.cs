using Elasticsearch.Net;
using Nest;

namespace Bing.Elasticsearch.Factories
{
    /// <summary>
    /// ES客户端工厂
    /// </summary>
    public interface IElasticClientFactory
    {
        /// <summary>
        /// 覆盖索引
        /// </summary>
        /// <param name="indexName">索引名</param>
        void OverlapIndex(string indexName);

        /// <summary>
        /// 获取索引名
        /// </summary>
        /// <typeparam name="TDocument">文档类型</typeparam>
        string GetIndexName<TDocument>() where TDocument : class;

        /// <summary>
        /// 获取ES客户端
        /// </summary>
        /// <param name="indexName">索引名</param>
        IElasticClient GetClient(string indexName);

        /// <summary>
        /// 获取ES客户端
        /// </summary>
        /// <typeparam name="TDocument">文档类型</typeparam>
        IElasticClient GetClient<TDocument>() where TDocument : class;

        /// <summary>
        /// 获取降级后的ES客户端
        /// </summary>
        /// <param name="indexName">索引名</param>
        IElasticLowLevelClient GetLowLevelClient(string indexName);

        /// <summary>
        /// 降级后的ES客户端
        /// </summary>
        IElasticLowLevelClient LowLevelClient { get; }
    }
}
