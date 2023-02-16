using System;
using System.Threading.Tasks;
using Bing.Data.Queries;
using Bing.Elasticsearch.Mapping;
using Bing.Elasticsearch.Repositories;
using Bing.Elasticsearch.Tests.ESModel;
using Bing.Utils.Json;
using Microsoft.Extensions.Logging;
using Xunit;

namespace Bing.Elasticsearch.Tests
{
    /// <summary>
    /// ES索引别名测试
    /// </summary>
    public class EsIndexAliasTest
    {
        /// <summary>
        /// es mapping factory
        /// </summary>
        private readonly IElasticMappingFactory _factory;

        /// <summary>
        /// es context
        /// </summary>
        private readonly IElasticsearchContext _context;

        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger<EsIndexAliasTest> _logger;

        /// <summary>
        /// repo
        /// </summary>
        private readonly IEsRepository<InOutStockProductDailyReportEo> _repository;

        public EsIndexAliasTest(IElasticMappingFactory factory
            , IElasticsearchContext context
            , ILogger<EsIndexAliasTest> logger
            , IEsRepository<InOutStockProductDailyReportEo> repository)
        {
            _factory = factory;
            _context = context;
            _logger = logger;
            _repository = repository;
        }

        /// <summary>
        /// 测试 - 通过别名进行分页查询
        /// </summary>
        [Fact]
        public async Task Test_PageSearch_With_Alias_Async()
        {
            var beginTime = DateTime.Parse("2022-12-01");
            var endTime = DateTime.Parse("2023-01-01");
            var param = new QueryParameter();
            var result = await _context
                .PageSearch<InOutStockProductDailyReportEo>(param)
                .Between(x => x.ReportDate, beginTime, endTime, true, Boundary.Left)
                .Index("in_out_stock_product_daily_report").GetPageResultAsync();
            _logger.LogInformation(JsonHelper.ToJson(result.Data));
        }

        /// <summary>
        /// 测试 - 通过别名进行查询
        /// </summary>
        [Fact]
        public async Task Test_Search_With_Alias_Async()
        {
            var beginTime = DateTime.Parse("2022-12-01");
            var endTime = DateTime.Parse("2023-01-01");
            var result = await _context
                .Search<InOutStockProductDailyReportEo>()
                .Between(x => x.ReportDate, beginTime, endTime, true, Boundary.Left)
                .Index("in_out_stock_product_daily_report").GetResultAsync();
            _logger.LogInformation($"返回数量：{result.Count}");
        }

        /// <summary>
        /// 测试 - 通过别名进行查询
        /// </summary>
        [Fact]
        public async Task Test_ScrollAll_With_Alias_Async()
        {
            var beginTime = DateTime.Parse("2022-12-01");
            var endTime = DateTime.Parse("2022-12-02");
            var result = await _repository
                .ScrollAllSearch<InOutStockProductDailyReportEo>()
                .Size(10000)
                .Between(x => x.ReportDate, beginTime, endTime, true, Boundary.Left)
                .Index("in_out_stock_product_daily_report")
                .GetLargeListAsync(300, maxParallelism: 10);
            _logger.LogInformation($"返回数量：{result.Count}");
        }
    }
}