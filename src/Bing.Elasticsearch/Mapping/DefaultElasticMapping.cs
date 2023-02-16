using System;
using Bing.Elasticsearch.Options;
using Nest;

namespace Bing.Elasticsearch.Mapping;

/// <summary>
/// 默认映射
/// </summary>
public class DefaultElasticMapping : ElasticMapping, IElasticMapping
{
    /// <summary>
    /// 初始化一个<see cref="ElasticMapping"/>类型的实例
    /// </summary>
    /// <param name="type">类型</param>
    /// <param name="options">ES选项配置</param>
    public DefaultElasticMapping(Type type, ElasticsearchOptions options) : base(type, options)
    {
    }

    /// <summary>
    /// 配置索引
    /// </summary>
    /// <param name="idx">创建索引描述符</param>
    public override CreateIndexDescriptor ConfigureIndex(CreateIndexDescriptor idx)
    {
        idx = base.ConfigureIndex(idx);
        return idx.Map(x => x.AutoMap(Type));
    }

    /// <summary>
    /// 配置索引设置
    /// </summary>
    /// <param name="settings">索引设置描述符</param>
    public override IPromise<IIndexSettings> ConfigureIndexSettings(IndexSettingsDescriptor settings)
    {
        return settings.NumberOfShards(Options.NumberOfShards)
            .NumberOfReplicas(Options.NumberOfReplicas)
            .Setting("max_result_window", int.MaxValue);
    }
}