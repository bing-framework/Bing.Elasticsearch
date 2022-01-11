using System.Threading;
using System.Threading.Tasks;
using Bing.Elasticsearch.Internals;
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
        /// 创建索引
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="client">ES客户端</param>
        /// <param name="indexName">索引名</param>
        /// <param name="numberOfShards">主分片数量</param>
        /// <param name="numberOfReplicas">每个主分片的副分片数量</param>
        /// <param name="cancellationToken">取消令牌</param>
        public static async Task CreateIndexAsync<T>(this IElasticClient client,
            string indexName = "",
            int numberOfShards = 1,
            int numberOfReplicas = 1,
            CancellationToken cancellationToken = default)
            where T : class
        {
            indexName = Helper.SafeIndexName<T>(indexName);
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
