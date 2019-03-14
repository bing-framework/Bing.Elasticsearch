using System.Threading.Tasks;

namespace Bing.Elasticsearch.Configs
{
    /// <summary>
    /// Elasticsearch 配置提供程序
    /// </summary>
    public class ElasticsearchConfigProvider:IElasticsearchConfigProvider
    {
        /// <summary>
        /// 配置
        /// </summary>
        private readonly ElasticsearchConfig _config;

        /// <summary>
        /// 初始化一个<see cref="ElasticsearchConfigProvider"/>类型的实例
        /// </summary>
        /// <param name="config">Elasticsearch 连接配置</param>
        public ElasticsearchConfigProvider(ElasticsearchConfig config)
        {
            _config = config;
        }
        
        /// <summary>
        /// 获取配置
        /// </summary>
        /// <returns></returns>
        public Task<ElasticsearchConfig> GetConfigAsync()
        {
            return Task.FromResult(_config);
        }
    }
}
