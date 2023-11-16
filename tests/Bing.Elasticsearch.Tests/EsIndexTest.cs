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
            await _context.DeleteIndexAsync("test_model_1");
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
            await _context.DeleteIndexAsync("test_model_5");
        }

        /// <summary>
        /// 测试 - 上下文 - 创建索引
        /// </summary>
        [Fact]
        public async Task Test_Context_CreateIndexAsync_1()
        {
            await _context.CreateIndexAsync<TestModel1>("test_model_1");
            await _context.CreateIndexAsync<TestModel5>("test_model_5");
        }

        /// <summary>
        /// 测试 - 上下文 - 创建索引及设置别名
        /// </summary>
        [Fact]
        public async Task Test_Context_CreateIndex_With_Alias_Async_1()
        {
            await _context.CreateIndexAsync<TestModel1>("test_model_1", "test_1");
            await _context.CreateIndexAsync<TestModel5>("test_model_5", "test_5");
        }

        /// <summary>
        /// 测试 - 上下文 - 删除索引
        /// </summary>
        [Fact]
        public async Task Test_Context_DeleteIndexAsync_1()
        {
            await _context.CreateIndexAsync<TestModel1>("test_model_1");
            await _context.CreateIndexAsync<TestModel5>("test_model_5");

            await _context.DeleteIndexAsync("test_model_1");
            await _context.DeleteIndexAsync("test_model_5");
        }

        /// <summary>
        /// 测试 - 上下文 - 通过别名删除索引集合
        /// </summary>
        [Fact]
        public async Task Test_Context_DeleteIndexesByAliasAsync_1()
        {
            await _context.CreateIndexAsync<TestModel1>("test_model_1", "test_1");
            await _context.CreateIndexAsync<TestModel1>("test_model_2", "test_1");
            await _context.CreateIndexAsync<TestModel1>("test_model_3", "test_1");
            await _context.CreateIndexAsync<TestModel1>("test_model_4", "test_1");
            await _context.CreateIndexAsync<TestModel1>("test_model_5", "test_1");

            await _context.DeleteIndexesByAliasAsync("test_1");
        }
    }
}