using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Bing.Data.Queries;
using Bing.Elasticsearch.Repositories;
using Bing.Elasticsearch.Tests.Models;
using Bing.Extensions;
using Bing.IO;
using Bing.MockData.Core.Options;
using Bing.MockData.Factories;
using Bing.Utils.IdGenerators.Core;
using Bing.Utils.Json;
using Microsoft.Extensions.Logging;
using Xunit;

namespace Bing.Elasticsearch.Tests
{
    /// <summary>
    /// blog sample 查询测试
    /// </summary>
    public class BlogSampleSearchTest
    {
        /// <summary>
        /// es context
        /// </summary>
        private readonly IElasticsearchContext _context;

        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger<BlogSampleSearchTest> _logger;

        /// <summary>
        /// blog sample repo
        /// </summary>
        private readonly IEsRepository<BlogSample> _blogRepo;

        /// <summary>
        /// 初始化
        /// </summary>
        public BlogSampleSearchTest(IElasticsearchContext context
            , ILogger<BlogSampleSearchTest> logger
            , IEsRepository<BlogSample> blogRepo)
        {
            _context = context;
            _logger = logger;
            _blogRepo = blogRepo;
        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        [Fact]
        public async Task InitAsync()
        {
            var rootPath = Directory.GetCurrentDirectory();
            var jsonFilePath = Path.Combine(rootPath, "Resources", "BlogSample.json");
            var json = await FileHelper.ReadToStringAsync(jsonFilePath);
            var blogs = JsonHelper.ToObject<List<BlogSample>>(json);
            await _blogRepo.InsertManyAsync(blogs);
        }

        /// <summary>
        /// 测试 - 查询 - 模糊匹配词条
        /// </summary>
        [Fact]
        public async Task Test_Search_MatchAsync()
        {

            await SearchByMatchAsync("java", 5);
            await SearchByMatchAsync("入门", 3);
            await SearchByMatchAsync("java入门", 7);
        }

        private async Task SearchByMatchAsync(string keyword, int count)
        {
            var param = new QueryParameter();
            var result = await _context.Search<BlogSample>(param).Match(x => x.Title, keyword).GetResultAsync();
            _logger.LogInformation(JsonHelper.ToJson(result.Data));
            Assert.Equal(count, result.TotalCount);
        }

        /// <summary>
        /// 测试 - 批量插入数据
        /// </summary>
        [Fact]
        public async Task Test_BulkInsertAsync()
        {
            var list = new List<BlogSample>();
            var timeRandom = RandomizerFactory.GetRandomizer(new DateTimeFieldOptions());
            var titleRandom = RandomizerFactory.GetRandomizer(new TextFieldOptions()
                { Min = 3, Max = 30, UseLetter = true, UseNumber = true });
            for (int i = 0; i < 10000; i++)
            {
                list.Add(new BlogSample
                {
                    Id = SnowflakeIdGenerator.Current.Create().ToString(),
                    Title = titleRandom.Generate(),
                    Time = timeRandom.Generate().SafeValue(),
                });
            }

            await _blogRepo.BulkInsertAsync(list);
        }
    }
}