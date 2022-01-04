namespace Bing.Elasticsearch.ConsoleSample
{
    /// <summary>
    /// 上下文
    /// </summary>
    public class SampleContext
    {
        /// <summary>
        /// 中间库
        /// </summary>
        public IFreeSql Orm { get; set; }

        /// <summary>
        /// ES客户端
        /// </summary>
        public IElasticsearchClient ESClient { get; set; }
    }
}
