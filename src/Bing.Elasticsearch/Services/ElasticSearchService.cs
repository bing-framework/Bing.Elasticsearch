using System;
using Bing.Elasticsearch.Provider;
using Nest;

namespace Bing.Elasticsearch.Services
{
    /// <summary>
    /// ElasticSearch 服务
    /// </summary>
    public class ElasticSearchService //: IElasticSearch
    {
        /// <summary>
        /// ES客户端
        /// </summary>
        internal IElasticClient Client { get; }

        /// <summary>
        /// 初始化一个<see cref="ElasticSearchService"/>类型的实例
        /// </summary>
        /// <param name="provider">ES客户端</param>
        public ElasticSearchService(IElasticClientProvider provider)
        {
            if (provider == null)
                throw new ArgumentNullException(nameof(provider));
            Client = provider.GetClient();
        }

    }
}
