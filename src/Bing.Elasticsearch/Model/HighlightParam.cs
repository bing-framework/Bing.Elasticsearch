namespace Bing.Elasticsearch.Model
{
    /// <summary>
    /// 高亮参数
    /// </summary>
    public class HighlightParam
    {
        /// <summary>
        /// 高亮字段
        /// </summary>
        public string[] Keys { get; set; }

        /// <summary>
        /// 高亮标签
        /// </summary>
        public string PreTags { get; set; } = "<em>";

        /// <summary>
        /// 高亮标签
        /// </summary>
        public string PostTags { get; set; } = "</em>";

        /// <summary>
        /// 高亮字段前缀。
        /// 例如：title 高亮值赋值给 h_title
        /// </summary>
        public string PrefixOfKey { get; set; } = string.Empty;

        /// <summary>
        /// 是否替换原来的值
        /// </summary>
        public bool ReplaceAuto { get; set; } = true;
    }
}
