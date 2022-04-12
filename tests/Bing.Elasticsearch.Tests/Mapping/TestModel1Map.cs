using System;
using Bing.Elasticsearch.Mapping;
using Bing.Elasticsearch.Options;
using Bing.Elasticsearch.Tests.Models;
using Nest;

namespace Bing.Elasticsearch.Tests.Mapping
{
    public class TestModel1Map : ElasticMapping<TestModel1>
    {
        /// <summary>
        /// 初始化一个<see cref="ElasticMapping"/>类型的实例
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="options">ES选项配置</param>
        public TestModel1Map(Type type, ElasticsearchOptions options) : base(type, options)
        {
        }

        /// <summary>
        /// 配置索引映射
        /// </summary>
        /// <param name="map">映射</param>
        public override TypeMappingDescriptor<TestModel1> ConfigureIndexMapping(TypeMappingDescriptor<TestModel1> map)
        {
            return map.AutoMap<TestModel1>()
                .Properties(p => p.Number(f => f.Name(e => e.Id).Type(NumberType.Long)));
        }
    }
}