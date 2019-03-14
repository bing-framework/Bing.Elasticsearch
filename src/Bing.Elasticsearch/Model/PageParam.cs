
namespace Bing.Elasticsearch.Model
{
    /// <summary>
    /// 分页参数
    /// </summary>
    public interface IPageParam
    {
        /// <summary>
        /// 页数，即第几页，从1开始
        /// </summary>
        int Page { get; set; }

        /// <summary>
        /// 每页显示行数
        /// </summary>
        int PageSize { get; set; }

        /// <summary>
        /// 关键词
        /// </summary>
        string Keyword { get; set; }

        /// <summary>
        /// 获取跳过的行数
        /// </summary>
        /// <returns></returns>
        int GetSkipCount();

        /// <summary>
        /// 运算符
        /// </summary>
        Nest.Operator Operator { get; set; }

        /// <summary>
        /// 高亮参数
        /// </summary>
        HighlightParam Highlight { get; set; }
    }

    /// <summary>
    /// 分页参数
    /// </summary>
    public class PageParam:IPageParam
    {
        /// <summary>
        /// 页数，即第几页，从1开始
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// 每页显示行数
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 关键词
        /// </summary>
        public string Keyword { get; set; }

        /// <summary>
        /// 获取跳过的行数
        /// </summary>
        /// <returns></returns>
        public int GetSkipCount() => (Page - 1) * PageSize;

        /// <summary>
        /// 运算符
        /// </summary>
        public Nest.Operator Operator { get; set; } = Nest.Operator.And;

        /// <summary>
        /// 高亮参数
        /// </summary>
        public HighlightParam Highlight { get; set; }
    }

    /// <summary>
    /// 指定字段查询
    /// </summary>
    public class PageParamWithSearch : PageParam
    {
        /// <summary>
        /// 查询字段列表
        /// </summary>
        public string[] SearchKeys { get; set; }
    }
}
