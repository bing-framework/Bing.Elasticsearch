using System;

namespace Bing.Elasticsearch.Configs
{
    /// <summary>
    /// Elasticsearch 节点
    /// </summary>
    public class ElasticsearchNode
    {
        /// <summary>
        /// 主机
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// 端口号
        /// </summary>
        public uint Port { get; set; }        

        /// <summary>
        /// 输出字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var port = Port == 0 ? "" : $":{Port}";
            var result = $"{Host}{port}".ToLowerInvariant();
            return result.IndexOf("http", StringComparison.OrdinalIgnoreCase) > -1 ? result : $"http://{result}";
        }
    }
}
