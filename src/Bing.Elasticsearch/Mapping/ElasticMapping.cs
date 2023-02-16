using System;
using Bing.Elasticsearch.Internals;
using Bing.Elasticsearch.Options;
using Nest;

namespace Bing.Elasticsearch.Mapping;

/// <summary>
/// ES 映射
/// </summary>
public class ElasticMapping : IElasticMapping
{
    /// <summary>
    /// 类型
    /// </summary>
    public Type Type { get; protected set; }

    /// <summary>
    /// 索引名称
    /// </summary>
    public string Name { get; protected set; }

    /// <summary>
    /// 是否拥有多个索引
    /// </summary>
    public bool HasMultipleIndexes { get; protected set; }

    /// <summary>
    /// ES选项配置
    /// </summary>
    protected ElasticsearchOptions Options { get; }

    /// <summary>
    /// 初始化一个<see cref="ElasticMapping"/>类型的实例
    /// </summary>
    /// <param name="type">类型</param>
    /// <param name="options">ES选项配置</param>
    public ElasticMapping(Type type, ElasticsearchOptions options)
    {
        Type = type;
        Name = Helper.SafeIndexName(type);
        Options = options;
    }

    /// <summary>
    /// 映射
    /// </summary>
    /// <param name="descriptor">创建索引描述符</param>
    public void Map(CreateIndexDescriptor descriptor)
    {
        ConfigureIndex(descriptor);
    }

    /// <summary>
    /// 配置索引
    /// </summary>
    /// <param name="idx">创建索引描述符</param>
    public virtual CreateIndexDescriptor ConfigureIndex(CreateIndexDescriptor idx)
    {
        return idx
            .Aliases(ConfigureIndexAliases)
            .Settings(ConfigureIndexSettings);
    }

    /// <summary>
    /// 配置索引别名
    /// </summary>
    /// <param name="aliases">别名描述符</param>
    public virtual IPromise<IAliases> ConfigureIndexAliases(AliasesDescriptor aliases)
    {
        return aliases;
    }

    /// <summary>
    /// 配置索引设置
    /// </summary>
    /// <param name="settings">索引设置描述符</param>
    public virtual IPromise<IIndexSettings> ConfigureIndexSettings(IndexSettingsDescriptor settings)
    {
        return settings;
    }
}

/// <summary>
/// ES映射
/// </summary>
/// <typeparam name="T">实体类型</typeparam>
public class ElasticMapping<T> : ElasticMapping, IElasticMapping<T> where T : class
{
    /// <summary>
    /// 初始化一个<see cref="ElasticMapping"/>类型的实例
    /// </summary>
    /// <param name="type">类型</param>
    /// <param name="options">ES选项配置</param>
    public ElasticMapping(Type type, ElasticsearchOptions options) : base(type, options)
    {
        Type = typeof(T);
        Name = Helper.SafeIndexName(type);
    }

    /// <summary>
    /// 配置索引映射
    /// </summary>
    /// <param name="map">映射</param>
    public virtual TypeMappingDescriptor<T> ConfigureIndexMapping(TypeMappingDescriptor<T> map)
    {
        return map.AutoMap<T>();
    }

    /// <summary>
    /// 配置索引
    /// </summary>
    /// <param name="idx">创建索引描述符</param>
    public override CreateIndexDescriptor ConfigureIndex(CreateIndexDescriptor idx)
    {
        idx = base.ConfigureIndex(idx);
        return idx.Map<T>(ConfigureIndexMapping);
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