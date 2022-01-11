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
        /// ES上下文
        /// </summary>
        public IElasticsearchContext ESContext { get; set; }

        public SampleContext(IFreeSql orm, IElasticsearchContext context)
        {
            Orm = orm;
            ESContext = context;
        }
    }
}
