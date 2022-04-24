using Nest;

namespace Bing.Elasticsearch
{
    /// <summary>
    /// ES映射扩展
    /// </summary>
    public static class ElasticMappingExtensions
    {
        /// <summary>
        /// 添加关键词(keyword)字段。注意：不能与 AddSortField 链式调用，如要调用则 AddKeywordAndSortFields。
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="descriptor">文本属性描述符</param>
        public static TextPropertyDescriptor<T> AddKeywordField<T>(this TextPropertyDescriptor<T> descriptor)
            where T : class
        {
            return descriptor.Fields(f => f.Keyword(s => s.Name(KeywordFieldName).IgnoreAbove(256)));
        }

        /// <summary>
        /// 添加排序(sort)字段。注意：不能与 AddKeywordField 链式调用，如要调用则 AddKeywordAndSortFields。
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="descriptor">文本属性描述符</param>
        /// <param name="normalizer">排序规范器</param>
        public static TextPropertyDescriptor<T> AddSortField<T>(this TextPropertyDescriptor<T> descriptor, string normalizer = "sort") 
            where T : class
        {
            return descriptor.Fields(f => f.Keyword(s => s.Normalizer(SortFieldName).Normalizer(normalizer).IgnoreAbove(256)));
        }

        /// <summary>
        /// 添加关键词(keyword)及排序(sort)字段
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="descriptor">文本属性描述符</param>
        /// <param name="sortNormalizer">排序规范器</param>
        public static TextPropertyDescriptor<T> AddKeywordAndSortFields<T>(this TextPropertyDescriptor<T> descriptor, string sortNormalizer = "sort")
            where T : class
        {
            return descriptor.Fields(f => f
                .Keyword(s => s.Name(KeywordFieldName).IgnoreAbove(256))
                .Keyword(s => s.Name(SortFieldName).Normalizer(sortNormalizer).IgnoreAbove(256))
            );
        }

        /// <summary>
        /// 添加排序规范器
        /// </summary>
        /// <param name="descriptor">分析描述符</param>
        public static AnalysisDescriptor AddSortNormalizer(this AnalysisDescriptor descriptor)
        {
            return descriptor.Normalizers(d => d.Custom("sort", n => n.Filters("lowercase", "asciifolding")));
        }

        /// <summary>
        /// keyword 字段名
        /// </summary>
        public static string KeywordFieldName = "keyword";

        /// <summary>
        /// sort 字段名
        /// </summary>
        public static string SortFieldName = "sort";
    }
}