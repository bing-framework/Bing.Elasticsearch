using System;
using Nest;

namespace Bing.Elasticsearch.Mapping
{
    /// <summary>
    /// ES映射
    /// </summary>
    public interface IElasticMapping
    {
        /// <summary>
        /// 类型
        /// </summary>
        Type Type { get; }

        /// <summary>
        /// 索引名称
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 是否拥有多个索引
        /// </summary>
        bool HasMultipleIndexes { get; }

        /// <summary>
        /// 映射
        /// </summary>
        /// <param name="descriptor">创建索引描述符</param>
        void Map(CreateIndexDescriptor descriptor);
    }

    /// <summary>
    /// ES映射
    /// </summary>
    /// <typeparam name="T">实体类型</typeparam>
    public interface IElasticMapping<T> : IElasticMapping where T : class
    {
        /// <summary>
        /// 配置索引映射
        /// </summary>
        /// <param name="map">映射</param>
        TypeMappingDescriptor<T> ConfigureIndexMapping(TypeMappingDescriptor<T> map);
    }
}