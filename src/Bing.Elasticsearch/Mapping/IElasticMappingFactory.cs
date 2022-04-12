using System;

namespace Bing.Elasticsearch.Mapping
{
    /// <summary>
    /// ES映射工厂
    /// </summary>
    public interface IElasticMappingFactory
    {
        /// <summary>
        /// 获取ES映射类
        /// </summary>
        /// <param name="type">类型</param>
        IElasticMapping GetMapping(Type type);

        /// <summary>
        /// 获取ES映射类
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        IElasticMapping GetMapping<T>();
    }
}