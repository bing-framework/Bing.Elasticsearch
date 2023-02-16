using System;
using Bing.Elasticsearch.Mapping;
using Bing.Elasticsearch.Options;
using Bing.Elasticsearch.Provider;
using Bing.Elasticsearch.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Bing.Elasticsearch;

/// <summary>
/// Elasticsearch 扩展
/// </summary>
public static partial class Extensions
{
    /// <summary>
    /// 注册Elasticsearch日志操作
    /// </summary>
    /// <param name="services">服务集合</param>
    /// <param name="setupAction">配置操作</param>
    public static void AddElasticsearch(this IServiceCollection services, Action<ElasticsearchOptions> setupAction)
    {
        services.Configure(setupAction);
        services.GetOrAddAllAssemblyFinder();
        services.GetOrAddTypeFinder<IElasticMappingTypeFinder>(assemblyFinder => new ElasticMappingTypeFinder(assemblyFinder));
        services.TryAddSingleton<IIndexNameResolver, IndexNameResolver>();
        services.TryAddSingleton<IElasticClientProvider, ElasticClientProvider>();
        services.TryAddScoped<IElasticsearchContext, ElasticsearchContext>();
        services.TryAddSingleton<IElasticMappingFactory, ElasticMappingFactory>();
        services.TryAddScoped(typeof(IEsRepository<>), typeof(EsRepository<>));
    }
}