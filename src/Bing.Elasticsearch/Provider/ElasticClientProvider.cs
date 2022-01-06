using System;
using System.Collections.Concurrent;
using System.Linq;
using Bing.Elasticsearch.Options;
using Elasticsearch.Net;
using Microsoft.Extensions.Options;
using Nest;

namespace Bing.Elasticsearch.Provider
{
    /// <summary>
    /// ES客户端提供程序
    /// </summary>
    public class ElasticClientProvider : IElasticClientProvider
    {
        /// <summary>
        /// ES选项配置
        /// </summary>
        private readonly ElasticsearchOptions _options;

        /// <summary>
        /// ES连接池
        /// </summary>
        private readonly IConnectionPool _connectionPool;

        /// <summary>
        /// ES客户端字典
        /// </summary>
        private readonly ConcurrentDictionary<string, IElasticClient> _esClientDict = new ConcurrentDictionary<string, IElasticClient>();

        /// <summary>
        /// 初始化一个<see cref="ElasticClientProvider"/>类型的实例
        /// </summary>
        /// <param name="options">ES选项配置</param>
        public ElasticClientProvider(IOptions<ElasticsearchOptions> options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));
            _options = options.Value ?? throw new ArgumentNullException(nameof(options.Value));
            _connectionPool = CreateConnectionPool();
            var settings = new ConnectionSettings(_connectionPool);
            ConfigSettings(settings);
            var client = new ElasticClient(settings);
            _esClientDict.TryAdd(_options.DefaultIndex, client);
        }

        /// <summary>
        /// 创建连接池
        /// </summary>
        private IConnectionPool CreateConnectionPool()
        {
            var urls = _options.Urls.Select(x => new Uri(x));
            switch (_options.PoolType)
            {
                // 只请求正常节点，异常节点恢复后可以请求,默认随机请求服务  静态链接池，支持ping,适合小型集群
                case ElasticsearchConnectionPoolType.Static:
                    return new StaticConnectionPool(urls)
                    {
                        SniffedOnStartup = true
                    };
                case ElasticsearchConnectionPoolType.SingleNode:
                    return new SingleNodeConnectionPool(urls.FirstOrDefault());
                case ElasticsearchConnectionPoolType.Sniffing:
                    return new SniffingConnectionPool(urls);
                case ElasticsearchConnectionPoolType.Sticky:
                    return new StickyConnectionPool(urls);
                case ElasticsearchConnectionPoolType.StickySniffing:
                    return new StickySniffingConnectionPool(urls, x => 1.0F);
                default:
                    return new StaticConnectionPool(urls)
                    {
                        SniffedOnStartup = true
                    };
            }
        }

        /// <summary>
        /// 配置连接设置
        /// </summary>
        /// <param name="settings">连接设置</param>
        private void ConfigSettings(ConnectionSettings settings)
        {
            // 启用验证
            if (!string.IsNullOrWhiteSpace(_options.UserName) && !string.IsNullOrWhiteSpace(_options.Password))
                settings.BasicAuthentication(_options.UserName, _options.Password);

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
            settings.DisablePing(_options.DisablePing);

            // ping超时设置
            //settings.PingTimeout(new TimeSpan(10000));

            // 选择节点
            //settings.NodePredicate(node => { return true; });

            // 默认操作索引
            settings.DefaultIndex(_options.DefaultIndex);

            // 字段名规则 与model字段同名
            //settings.DefaultFieldNameInferrer(name => name);

            // 根据Type获取类型名
            //settings.DefaultTypeNameInferrer(name => name.Name);

            // 请求超时设置
            //settings.RequestTimeout(new TimeSpan(10000));

            // 调试信息
            settings.DisableDirectStreaming(_options.DisableDebugInfo);
            //settings.EnableDebugMode((apiCallDetails) =>
            //{
            //    // 请求完成 返回 apiCallDetails
            //});

            // 抛出异常，默认false，错误信息在每个操作的response中
            settings.ThrowExceptions(_options.ThrowExceptions);
            //settings.OnRequestCompleted(apiCallDetails =>
            //{
            //    // 请求完成 返回apiCallDetails
            //});
            //settings.OnRequestDataCreated(requestData =>
            //{
            //    // 请求的数据创建完成 返回请求的数据
            //});
        }

        /// <summary>
        /// 获取ES客户端
        /// </summary>
        public IElasticClient GetClient()
        {
            return _esClientDict.GetOrAdd(_options.DefaultIndex, key =>
            {
                var settings = new ConnectionSettings(_connectionPool).DefaultIndex(_options.DefaultIndex);
                return new ElasticClient(settings);
            });
        }

        /// <summary>
        /// 获取ES客户端
        /// </summary>
        /// <param name="indexName">索引名</param>
        public IElasticClient GetClient(string indexName)
        {
            if (string.IsNullOrEmpty(indexName))
                throw new ArgumentNullException(nameof(indexName));
            return _esClientDict.GetOrAdd(indexName, key =>
            {
                var settings = new ConnectionSettings(_connectionPool).DefaultIndex(indexName);
                return new ElasticClient(settings);
            });
        }

        /// <summary>
        /// 获取降级后的ES客户端
        /// </summary>
        /// <param name="indexName">索引名</param>
        public IElasticLowLevelClient GetLowLowLevelClient(string indexName)
        {
            if (string.IsNullOrEmpty(indexName))
                throw new ArgumentNullException(nameof(indexName));
            return GetClient(indexName).LowLevel;
        }
    }
}
