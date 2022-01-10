using System;
using Bing.Elasticsearch.Options;
using Microsoft.Extensions.Options;

namespace Bing.Elasticsearch.Repositories
{
    /// <summary>
    /// 索引名称解析器
    /// </summary>
    public class IndexNameResolver : IIndexNameResolver
    {
        /// <summary>
        /// ES选项配置
        /// </summary>
        private readonly ElasticsearchOptions _options;

        /// <summary>
        /// 初始化一个<see cref="IndexNameResolver"/>类型的实例
        /// </summary>
        /// <param name="options">ES选项配置</param>
        public IndexNameResolver(IOptions<ElasticsearchOptions> options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));
            _options = options.Value ?? throw new ArgumentNullException(nameof(options.Value));
        }

        /// <summary>
        /// 获取索引名称
        /// </summary>
        /// <param name="indexName">索引名称</param>
        public string GetIndexName(string indexName)
        {
            if (string.IsNullOrEmpty(indexName))
                throw new ArgumentNullException(nameof(indexName));
            if (string.IsNullOrEmpty(_options.Prefix))
                return indexName;
            return $"{_options.Prefix}_{indexName}";
        }
    }
}
