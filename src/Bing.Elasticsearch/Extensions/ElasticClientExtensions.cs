using System.Threading;
using System.Threading.Tasks;
using Nest;

// ReSharper disable once CheckNamespace
namespace Bing.Elasticsearch
{
    /// <summary>
    /// ES客户端(<see cref="IElasticClient"/>) 扩展
    /// </summary>
    internal static class ElasticClientExtensions
    {
        /// <summary>
        /// 初始化索引映射
        /// </summary>
        /// <param name="client">ES客户端</param>
        /// <param name="indexName">索引名</param>
        public static async Task InitializeIndexMapAsync(this IElasticClient client, string indexName)
        {
            //var newName = indexName + DateTime.Now.Ticks;
            var newName = indexName;
            var result = await client.Indices.CreateAsync(newName,
                x => x.Index(newName)
                    .Settings(o =>
                        o.NumberOfShards(1)
                            .NumberOfReplicas(1)
                            .Setting("max_result_window", int.MaxValue)));
            if (result.Acknowledged)
            {
                await client.Indices.PutAliasAsync(newName, indexName);
                return;
            }
            throw new ElasticsearchException($"创建索引 {indexName} 失败：{result.ServerError.Error.Reason}");
        }

        /// <summary>
        /// 初始化索引映射
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="client">ES客户端</param>
        /// <param name="indexName">索引名</param>
        public static async Task InitializeIndexMapAsync<T>(this IElasticClient client, string indexName) where T : class
        {
            //var newName = indexName + DateTime.Now.Ticks;
            var newName = indexName;
            var result = await client.Indices.CreateAsync(newName,
                x => x.Index(newName)
                    .Settings(o =>
                        o.NumberOfShards(1)
                            .NumberOfReplicas(1)
                            .Setting("max_result_window", int.MaxValue))
                    .Map(m => m.AutoMap<T>()));
            if (result.Acknowledged)
            {
                await client.Indices.PutAliasAsync(newName, indexName);
                return;
            }
            throw new ElasticsearchException($"创建索引 {indexName} 失败：{result.ServerError.Error.Reason}");
        }

        /// <summary>
        /// 创建索引
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="client">ES客户端</param>
        /// <param name="indexName">索引名</param>
        /// <param name="numberOfShards">主分片数量</param>
        /// <param name="numberOfReplicas">每个主分片的副分片数量</param>
        /// <param name="cancellationToken">取消令牌</param>
        public static async Task CreateIndexAsync<T>(this IElasticClient client,
            string indexName,
            int numberOfShards = 1,
            int numberOfReplicas = 1,
            CancellationToken cancellationToken = default)
            where T : class
        {
            var existsResult = await client.Indices.ExistsAsync(indexName, null, cancellationToken);
            if (existsResult.Exists)
                return;

            var result = await client.Indices.CreateAsync(
                indexName,
                x => x.Index(indexName)
                    .Settings(o =>
                        o.NumberOfShards(numberOfShards)
                            .NumberOfReplicas(numberOfReplicas)
                            .Setting("max_result_window", int.MaxValue))
                    .Map(m => m.AutoMap<T>()),
                cancellationToken);
            if (!result.Acknowledged)
                throw new ElasticsearchException($"索引[{indexName}]创建失败：{result.ServerError.Error.Reason}");
        }
    }
}
