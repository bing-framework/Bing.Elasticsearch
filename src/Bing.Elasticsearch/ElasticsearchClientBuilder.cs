using System;
using System.Linq;
using System.Threading.Tasks;
using Bing.Elasticsearch.Configs;
using Elasticsearch.Net;
using Nest;

namespace Bing.Elasticsearch
{
    /// <summary>
    /// ES客户端生成器
    /// </summary>
    internal class ElasticsearchClientBuilder
    {
        /// <summary>
        /// ES客户端
        /// </summary>
        private IElasticClient _client;

        /// <summary>
        /// 配置提供程序
        /// </summary>
        private readonly IElasticsearchConfigProvider _configProvider;

        /// <summary>
        /// 对象锁
        /// </summary>
        private static object _lock = new object();

        /// <summary>
        /// 初始化一个<see cref="ElasticsearchClientBuilder"/>类型的实例
        /// </summary>
        /// <param name="configProvider">配置提供程序</param>
        public ElasticsearchClientBuilder(IElasticsearchConfigProvider configProvider)
        {
            _configProvider = configProvider;
        }

        /// <summary>
        /// 获取ES客户端
        /// </summary>
        /// <returns></returns>
        public async Task<IElasticClient> GetClientAsync()
        {
            if (_client == null)
            {
                var config = await _configProvider.GetConfigAsync();
                lock (_lock)
                {
                    if (_client == null)
                    {
                        if (config.Nodes == null)
                        {
                            throw new ArgumentException("请设置ES客户端节点");
                        }
                        _client = CreateClient(config);
                    }
                }
            }

            return _client;
        }

        /// <summary>
        /// 创建ES客户端
        /// </summary>
        /// <param name="config">配置</param>
        /// <returns></returns>
        private IElasticClient CreateClient(ElasticsearchConfig config)
        {
            var connectionPool = CreateConnectionPool(config);
            var settings = new ConnectionSettings(connectionPool);
            ConfigSettings(settings, config);
            return new ElasticClient(settings);
        }

        /// <summary>
        /// 创建连接池
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        private IConnectionPool CreateConnectionPool(ElasticsearchConfig config)
        {
            var nodes = config.Nodes;
            switch (config.PoolType)
            {
                case ElasticsearchConnectionPoolType.Static:
                    return new StaticConnectionPool(nodes);
                case ElasticsearchConnectionPoolType.SingleNode:
                    return new SingleNodeConnectionPool(nodes.FirstOrDefault());
                case ElasticsearchConnectionPoolType.Sniffing:
                    return new SniffingConnectionPool(nodes);
                case ElasticsearchConnectionPoolType.Sticky:
                    return new StickyConnectionPool(nodes);
                case ElasticsearchConnectionPoolType.StickySniffing:
                    return new StickySniffingConnectionPool(nodes, x => 1.0F);
                default:
                    return new StaticConnectionPool(nodes);
            }
        }

        /// <summary>
        /// 配置连接设置
        /// </summary>
        /// <param name="settings">连接设置</param>
        /// <param name="config">配置</param>
        private void ConfigSettings(ConnectionSettings settings, ElasticsearchConfig config)
        {
            // 启用验证
            if (!string.IsNullOrWhiteSpace(config.UserName) && !string.IsNullOrWhiteSpace(config.Password))
            {
                settings.BasicAuthentication(config.UserName, config.Password);
            }
            // 验证证书
            //settings.ClientCertificate("");
            //settings.ClientCertificates(new System.Security.Cryptography.X509Certificates.X509CertificateCollection());
            //settings.ServerCertificateValidationCallback();

            // 开启第一次使用时进行嗅探，需连接池支持
            //settings.SniffOnStartup(false);

            // 链接最大并发数
            //settings.ConnectionLimit(80);

            // 标记为死亡节点的超时时间
            //settings.DeadTimeout(new TimeSpan(10000));
            //settings.MaxDeadTimeout(new TimeSpan(10000));

            // 最大重试次数
            //settings.MaximumRetries(5);

            // 重试超时时间，默认是RequestTimeout
            //settings.MaxRetryTimeout(new TimeSpan(50000));

            // 禁用代理自动检测
            //settings.DisableAutomaticProxyDetection(true);

            // 禁用ping，第一次使用节点或使用被标记死亡的节点进行ping
            settings.DisablePing(config.DisablePing);

            // ping超时设置
            //settings.PingTimeout(new TimeSpan(10000));

            // 选择节点
            //settings.NodePredicate(node => { return true; });

            // 默认操作索引
            //settings.DefaultIndex("");

            // 字段名规则 与model字段同名
            //settings.DefaultFieldNameInferrer(name => name);

            // 根据Type获取类型名
            //settings.DefaultTypeNameInferrer(name => name.Name);

            // 请求超时设置
            //settings.RequestTimeout(new TimeSpan(10000));

            // 调试信息
            settings.DisableDirectStreaming(config.DisableDebugInfo);
            //settings.EnableDebugMode((apiCallDetails) =>
            //{
            //    // 请求完成 返回 apiCallDetails
            //});

            // 抛出异常，默认false，错误信息在每个操作的response中
            settings.ThrowExceptions(config.ThrowExceptions);
            //settings.OnRequestCompleted(apiCallDetails =>
            //{
            //    // 请求完成 返回apiCallDetails
            //});
            //settings.OnRequestDataCreated(requestData =>
            //{
            //    // 请求的数据创建完成 返回请求的数据
            //});
        }
    }
}
