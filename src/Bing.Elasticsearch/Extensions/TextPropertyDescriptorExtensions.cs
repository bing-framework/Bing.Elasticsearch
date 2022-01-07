using Nest;

// ReSharper disable once CheckNamespace
namespace Bing.Elasticsearch
{
    /// <summary>
    /// 文本属性描述器<see cref="TextPropertyDescriptor{T}"/> 扩展
    /// </summary>
    public static class TextPropertyDescriptorExtensions
    {
        /// <summary>
        /// 设置分析器为 ik_max_word，设置搜索分析器为 ik_smart
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="source">文本属性</param>
        public static TextPropertyDescriptor<T> IkAnalyzer<T>(this TextPropertyDescriptor<T> source)
            where T : class =>
            source.MaxWordAnalyzer().SmartSearchAnalyzer();

        /// <summary>
        /// 设置分析器为 ik_max_word
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="source">文本属性</param>
        public static TextPropertyDescriptor<T> MaxWordAnalyzer<T>(this TextPropertyDescriptor<T> source)
            where T : class =>
            source.Analyzer("ik_max_word");

        /// <summary>
        /// 设置搜索分析器为 ik_smart
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="source">文本属性</param>
        public static TextPropertyDescriptor<T> SmartSearchAnalyzer<T>(this TextPropertyDescriptor<T> source)
            where T : class =>
            source.SearchAnalyzer("ik_smart");
    }
}
