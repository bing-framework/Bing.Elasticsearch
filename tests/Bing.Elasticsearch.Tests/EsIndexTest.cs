using System.Threading.Tasks;
using Bing.Elasticsearch.Mapping;
using Bing.Elasticsearch.Tests.Models;
using Xunit;

namespace Bing.Elasticsearch.Tests
{
    /// <summary>
    /// ES索引测试
    /// </summary>
    public class EsIndexTest
    {
        /// <summary>
        /// es mapping factory
        /// </summary>
        private readonly IElasticMappingFactory _factory;

        /// <summary>
        /// es context
        /// </summary>
        private readonly IElasticsearchContext _context;

        public EsIndexTest(IElasticMappingFactory factory, IElasticsearchContext context)
        {
            _factory = factory;
            _context = context;
        }

        [Fact]
        public async Task Test_CreateIndexAsync()
        {
            var mapping = _factory.GetMapping<TestModel1>();
            var result = await _context.GetClient().Indices.CreateAsync("test_model_1", (x) =>
             {
                 mapping.Map(x);
                 return x;
             });
            Assert.True(result.IsValid);
        }

        [Fact]
        public async Task Test_CreateIndexAsync_Default()
        {
            var mapping = _factory.GetMapping<TestModel5>();
            var result = await _context.GetClient().Indices.CreateAsync("test_model_5", (x) =>
            {
                mapping.Map(x);
                return x;
            });
            Assert.True(result.IsValid);
        }
    }
}