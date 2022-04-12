using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace Bing.Elasticsearch.Tests
{
    /// <summary>
    /// ES测试基类
    /// </summary>
    public class EsTestBase
    {
        /// <summary>
        /// 服务提供程序
        /// </summary>
        protected IServiceProvider ServiceProvider { get; }

        public EsTestBase() => ServiceProvider = GetServiceProvider();

        /// <summary>
        /// 获取服务提供程序
        /// </summary>
        protected IServiceProvider GetServiceProvider()
        {
            var services = new ServiceCollection();
            services.AddElasticsearch(o =>
            {
                o.Urls = new List<string>
                {
                    "http://10.186.103.244:9201",
                    "http://10.186.103.244:9202",
                    "http://10.186.103.244:9203",
                };
                o.DefaultIndex = "bing_es_sample";
                o.Prefix = "bing_sample";
                o.UserName = "elastic";
                o.Password = "gzdevops2022";
                o.NumberOfShards = 3;
                o.NumberOfReplicas = 1;
            });
            return services.BuildServiceProvider();
        }
    }
}