using System;
using System.Collections.Generic;
using Bing.Elasticsearch.Options;
using Bing.Elasticsearch.Provider;
using Bing.Elasticsearch.Repositories;
using Bing.Elasticsearch.Tests.Models;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Bing.Elasticsearch.Tests
{
    /// <summary>
    /// ES仓储测试
    /// </summary>
    public class EsRepositoryTest
    {
        /// <summary>
        /// 服务提供程序
        /// </summary>
        protected IServiceProvider ServiceProvider { get; }

        public EsRepositoryTest()
        {
            ServiceProvider = GetServiceProvider();
        }

        protected IServiceProvider GetServiceProvider()
        {
            var services = new ServiceCollection();
            services.AddOptions().Configure<ElasticsearchOptions>(x =>
            {
                x.Urls = new List<string>
                {
                    "http://10.186.132.138:9200"
                };
                x.DefaultIndex = "bing_es_sample";
            });
            services.AddSingleton<IElasticClientProvider, ElasticClientProvider>();
            services.AddScoped(typeof(IEsRepository<>), typeof(EsRepositoryBase<>));
            return services.BuildServiceProvider();
        }

        [Fact]
        public void Test_GetEsRepository()
        {
            using var scope = ServiceProvider.CreateScope();
            var resp = scope.ServiceProvider.GetService<IEsRepository<UserSample>>();
            Assert.NotNull(resp);

            Assert.IsType<EsRepositoryBase<UserSample>>(resp);
        }
    }
}
