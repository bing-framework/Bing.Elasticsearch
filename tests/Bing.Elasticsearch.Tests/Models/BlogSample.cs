using System;
using Nest;
using Newtonsoft.Json;

namespace Bing.Elasticsearch.Tests.Models
{
    [ElasticsearchType(RelationName = "test_blog",IdProperty = "Id")]
    public class BlogSample
    {
        /// <summary>
        /// 博客ID
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        [JsonProperty("time")]
        public DateTime Time { get; set; }
    }
}