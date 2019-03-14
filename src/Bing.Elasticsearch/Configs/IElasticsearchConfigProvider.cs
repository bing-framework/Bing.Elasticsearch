using System.Threading.Tasks;

namespace Bing.Elasticsearch.Configs
{
    /// <summary>
    /// Elasticsearch 配置提供程序
    /// </summary>
    public interface IElasticsearchConfigProvider
    {
        /// <summary>
        /// 获取配置
        /// </summary>
        /// <returns></returns>
        Task<ElasticsearchConfig> GetConfigAsync();
    }
}
