using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bing.Elasticsearch.Builders;
using Bing.Elasticsearch.Repositories;
using Bing.Elasticsearch.Tests.ESModel;
using Bing.Utils.Json;
using Microsoft.Extensions.Logging;
using Xunit;

namespace Bing.Elasticsearch.Tests
{
    /// <summary>
    /// ES Lambda查询测试
    /// </summary>
    public class EsLambdaQueryTest
    {
        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger<EsLambdaQueryTest> _logger;

        /// <summary>
        /// es context
        /// </summary>
        private readonly IElasticsearchContext _context;

        /// <summary>
        /// es repo
        /// </summary>
        private readonly IEsRepository<WarehouseProductStockBakEo> _repository;

        public EsLambdaQueryTest(ILogger<EsLambdaQueryTest> logger
            , IElasticsearchContext context
            , IEsRepository<WarehouseProductStockBakEo> repository)
        {
            _logger = logger;
            _context = context;
            _repository = repository;
        }

        [Fact]
        public async Task Test_Equal_Async()
        {
            var id = "0259d57d-796d-11ed-bc6a-0242ac120002";
            var notId = "02662dfe-796d-11ed-bc6a-0242ac120002";
            var ids = new List<string> { "02662dfe-796d-11ed-bc6a-0242ac120002", "02663e3f-796d-11ed-bc6a-0242ac120002" };
            var bakTime = DateTime.Parse("2022-12-11");
            var builder = _context.CreateBuilder<WarehouseProductStockBakEo>();
            builder.SelectAll()
                //.From<WarehouseProductStockBakEo>()
                //.Where<WarehouseProductStockBakEo>(x => x.WarehouseProductStockBakId, id, Operator.Equal)
                //.Where<WarehouseProductStockBakEo>(x => x.WarehouseProductStockBakId, notId, Operator.NotEqual)
                .NotEqual<WarehouseProductStockBakEo>(x => x.WarehouseProductStockBakId, notId)
                //.Where<WarehouseProductStockBakEo>(x => x.WarehouseProductStockBakId, ids, Operator.In)
                //.Where<WarehouseProductStockBakEo>(x => x.WarehouseProductStockBakId, ids, Operator.NotIn)
                .Where<WarehouseProductStockBakEo>(x => x.BakTime, bakTime)
                //.Where<WarehouseProductStockBakEo>(x => x.CurrentQty, 100, Operator.GreaterEqual)
                //.Between<WarehouseProductStockBakEo>(x => x.CurrentQty, 90, 100)
                //.Where<WarehouseProductStockBakEo>(x => x.UsableQty, 100, Operator.Less)
                //.Where<WarehouseProductStockBakEo>(x => x.GoodsName, "新增商品", Operator.Starts)
                //.Where<WarehouseProductStockBakEo>(x => x.GoodsName, "新增商品", Operator.Contains, true)
                //.Where<WarehouseProductStockBakEo>(x => x.GoodsName, "新增商品", Operator.Ends)
                //.OrderBy<WarehouseProductStockBakEo>(x => x.WarehouseName, appendKeyword: true)
                //.OrderBy<WarehouseProductStockBakEo>(x => x.WarehouseName)
                //.OrderBy<WarehouseProductStockBakEo>(x => x.UsableQty)
                .Take(10);
            var result = await _context
                .SearchAsync<WarehouseProductStockBakEo>(x => builder.GetSearchRequest());
            foreach (var item in result.Documents)
            {
                _logger.LogInformation(item.ToJson());
            }
            _logger.LogInformation($"count: {result.Total}, {result.Documents.Count}");
        }

        [Fact]
        public async Task Test_ScrollAllAsync()
        {
            var id = "0259d57d-796d-11ed-bc6a-0242ac120002";
            var notId = "02662dfe-796d-11ed-bc6a-0242ac120002";
            var ids = new List<string> { "02662dfe-796d-11ed-bc6a-0242ac120002", "02663e3f-796d-11ed-bc6a-0242ac120002" };
            var bakTime = DateTime.Parse("2022-12-11");
            var builder = _context.CreateBuilder<WarehouseProductStockBakEo>();
            builder.SelectAll()
                //.Where<WarehouseProductStockBakEo>(x => x.WarehouseProductStockBakId, id, Operator.Equal)
                //.Where<WarehouseProductStockBakEo>(x => x.WarehouseProductStockBakId, notId, Operator.NotEqual)
                .NotEqual<WarehouseProductStockBakEo>(x => x.WarehouseProductStockBakId, notId)
                //.Where<WarehouseProductStockBakEo>(x => x.WarehouseProductStockBakId, ids, Operator.In)
                //.Where<WarehouseProductStockBakEo>(x => x.WarehouseProductStockBakId, ids, Operator.NotIn)
                .Where<WarehouseProductStockBakEo>(x => x.BakTime, bakTime)
                .Where<WarehouseProductStockBakEo>(x => x.CurrentQty, 100, Operator.GreaterEqual)
                //.Between<WarehouseProductStockBakEo>(x => x.CurrentQty, 90, 100)
                .Where<WarehouseProductStockBakEo>(x => x.UsableQty, 100, Operator.Less)
                //.Where<WarehouseProductStockBakEo>(x => x.GoodsName, "新增商品", Operator.Starts)
                .Where<WarehouseProductStockBakEo>(x => x.GoodsName, "新增商品", Operator.Contains, true)
                //.Where<WarehouseProductStockBakEo>(x => x.GoodsName, "新增商品", Operator.Ends)
                .OrderBy<WarehouseProductStockBakEo>(x => x.WarehouseName, appendKeyword: true)
                //.OrderBy<WarehouseProductStockBakEo>(x => x.WarehouseName)
                //.OrderBy<WarehouseProductStockBakEo>(x => x.UsableQty)
                .Take(10);
            var result = await _context
                .ScrollAllAsync<WarehouseProductStockBakEo>(builder);
            foreach (var item in result)
            {
                _logger.LogInformation(item.ToJson());
            }
            _logger.LogInformation($"count: {result.Count}");
        }

        [Fact]
        public async Task Test_ScrollAllAsync_1()
        {
            var merchantId = Guid.Parse("3a02e707-3265-3213-a676-b043d8208460");
            var merchantIdStr = "3a02e707-3265-3213-a676-b043d8208460";
            var bakTime = DateTime.Parse("2022-12-14");
            var warehouseId = Guid.Parse("3a02e70a-7723-b9cc-d0a9-0a438498732d");
            var builder = _repository.CreateBuilder<WarehouseProductStockBakEo>();
            builder.SelectAll()
                .Where<WarehouseProductStockBakEo>(x => x.MerchantId, merchantId)
                //.Where<WarehouseProductStockBakEo>(x => x.MerchantId.Suffix("keyword"), merchantId)
                .Where<WarehouseProductStockBakEo>(x => x.BakTime, bakTime)
                //.Where<WarehouseProductStockBakEo>(x => x.CurrentQty, 0, Operator.Greater)
                //.Where<WarehouseProductStockBakEo>(x => x.WarehouseId, warehouseId, appendKeyword: true)
                .Take(100);
            var result = await _context.ScrollAllAsync<WarehouseProductStockBakEo>(builder);
            //foreach (var item in result)
            //{
            //    _logger.LogInformation(item.ToJson());
            //}
            _logger.LogInformation($"count: {result.Count}");
        }
    }
}