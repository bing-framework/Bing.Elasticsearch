using System;
using System.Collections.Generic;
using Bing.Elasticsearch.Options;
using Microsoft.Extensions.Options;

namespace Bing.Elasticsearch.Mapping;

/// <summary>
/// ES映射工厂
/// </summary>
public class ElasticMappingFactory : IElasticMappingFactory
{
    /// <summary>
    /// 反射实例类型
    /// </summary>
    private static readonly Type RefInstanceType = typeof(ElasticMapping<>);

    /// <summary>
    /// ES选项配置
    /// </summary>
    private readonly ElasticsearchOptions _options;

    /// <summary>
    /// ES映射 类型查找器
    /// </summary>
    private readonly IElasticMappingTypeFinder _finder;

    /// <summary>
    /// 映射字典
    /// </summary>
    protected Dictionary<Type, IElasticMapping> MappingDict { get; }

    /// <summary>
    /// 初始化一个<see cref="ElasticMappingFactory"/>类型的实例
    /// </summary>
    /// <param name="options">ES选项配置</param>
    /// <param name="finder">ES映射 类型查找器</param>
    public ElasticMappingFactory(IOptions<ElasticsearchOptions> options, IElasticMappingTypeFinder finder)
    {
        _options = options.Value;
        _finder = finder;
        MappingDict = new Dictionary<Type, IElasticMapping>();
        InitMapping();
    }

    /// <summary>
    /// 初始化映射
    /// </summary>
    protected virtual void InitMapping()
    {
        var maps = _finder.FindAll(true);
        foreach (var map in maps)
        {
            var interfaces = map.GetInterfaces();
            foreach (var @interface in interfaces)
            {
                var genericArgs = @interface.GetGenericArguments();
                if (genericArgs.Length == 1)
                {
                    var targetType = genericArgs[0];
                    if (targetType.FullName == null)
                        continue;
                    var implementType = RefInstanceType.MakeGenericType(targetType);
                    MappingDict[targetType] = (IElasticMapping)Activator.CreateInstance(implementType, targetType, _options);
                }
            }


        }
    }

    /// <summary>
    /// 获取ES映射类
    /// </summary>
    /// <param name="type">类型</param>
    public IElasticMapping GetMapping(Type type)
    {
        if (MappingDict.TryGetValue(type, out var mapping))
        {
            return mapping;
        }
        return new DefaultElasticMapping(type, _options);
    }

    /// <summary>
    /// 获取ES映射类
    /// </summary>
    /// <typeparam name="T">泛型类型</typeparam>
    public IElasticMapping GetMapping<T>() => GetMapping(typeof(T));
}