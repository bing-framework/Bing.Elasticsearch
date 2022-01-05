using Elasticsearch.Net;
using Nest;

namespace Bing.Elasticsearch.Provider
{
    /// <summary>
    /// ES客户端提供程序
    /// </summary>
    public interface IElasticClientProvider
    {
        /// <summary>
        /// 获取ES客户端
        /// </summary>
        /// <param name="indexName">索引名</param>
        IElasticClient GetClient(string indexName);

        /// <summary>
        /// 获取降级后的ES客户端
        /// </summary>
        /// <param name="indexName">索引名</param>
        IElasticLowLevelClient GetLowLowLevelClient(string indexName);
    }
}
