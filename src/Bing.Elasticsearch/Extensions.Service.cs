using System;
using Bing.Elasticsearch.Configs;
using Bing.Elasticsearch.Options;
using Bing.Elasticsearch.Provider;
using Bing.Elasticsearch.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Bing.Elasticsearch
{
    /// <summary>
    /// Elasticsearch 扩展
    /// </summary>
    public static partial class Extensions
    {
        ///// <summary>
        ///// 注册Elasticsearch日志操作
        ///// </summary>
        ///// <param name="services">服务集合</param>
        ///// <param name="setupAction">配置操作</param>
        //[Obsolete]
        //public static void AddElasticsearch(this IServiceCollection services, Action<ElasticsearchConfig> setupAction)
        //{
        //    var config = new ElasticsearchConfig();
        //    setupAction?.Invoke(config);
        //    services.TryAddSingleton<IElasticsearchConfigProvider>(new ElasticsearchConfigProvider(config));
        //    services.TryAddScoped<IElasticsearchClient, ElasticsearchClient>();
        //}

        ///// <summary>
        ///// 注册Elasticsearch日志操作
        ///// </summary>
        ///// <typeparam name="TElasticSearchConfigProvider">Elasticsearch配置提供器</typeparam>
        ///// <param name="services">服务集合</param>
        //[Obsolete]
        //public static void AddElasticsearch<TElasticSearchConfigProvider>(this IServiceCollection services)
        //    where TElasticSearchConfigProvider : class, IElasticsearchConfigProvider
        //{
        //    services.TryAddScoped<IElasticsearchConfigProvider, TElasticSearchConfigProvider>();
        //    services.TryAddScoped<IElasticsearchClient, ElasticsearchClient>();
        //}

        /// <summary>
        /// 注册Elasticsearch日志操作
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <param name="setupAction">配置操作</param>
        public static void AddElasticsearch(this IServiceCollection services, Action<ElasticsearchOptions> setupAction)
        {
            services.Configure(setupAction);
            services.TryAddSingleton<IIndexNameResolver, IndexNameResolver>();
            services.TryAddSingleton<IElasticClientProvider, ElasticClientProvider>();
            services.TryAddScoped<IElasticsearchContext, ElasticsearchContext>();
            services.TryAddScoped(typeof(IEsRepository<>), typeof(EsRepository<>));
        }
    }
}
