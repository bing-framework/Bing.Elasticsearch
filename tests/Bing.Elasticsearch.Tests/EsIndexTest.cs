using System.Threading.Tasks;
using Bing.Elasticsearch.Mapping;
using Bing.Elasticsearch.Tests.Models;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Bing.Elasticsearch.Tests
{
    /// <summary>
    /// ES索引测试
    /// </summary>
    public class EsIndexTest: EsTestBase
    {
        [Fact]
        public async Task Test_CreateIndexAsync()
        {
            using var scope = ServiceProvider.CreateScope();
            var factory = scope.ServiceProvider.GetService<IElasticMappingFactory>();
            var mapping = factory.GetMapping<TestModel1>();
            var context = scope.ServiceProvider.GetService<IElasticsearchContext>();
            var result=await context.GetClient().Indices.CreateAsync("test_model_1",  (x) =>
            {
                mapping.Map(x);
                return x;
            });
            Assert.True(result.IsValid);
        }

        [Fact]
        public async Task Test_CreateIndexAsync_Default()
        {
            using var scope = ServiceProvider.CreateScope();
            var factory = scope.ServiceProvider.GetService<IElasticMappingFactory>();
            var mapping = factory.GetMapping<TestModel5>();
            var context = scope.ServiceProvider.GetService<IElasticsearchContext>();
            var result = await context.GetClient().Indices.CreateAsync("test_model_5", (x) =>
            {
                mapping.Map(x);
                return x;
            });
            Assert.True(result.IsValid);
        }
    }
}