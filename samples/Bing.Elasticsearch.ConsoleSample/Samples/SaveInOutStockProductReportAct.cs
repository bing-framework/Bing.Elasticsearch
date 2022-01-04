﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bing.Elasticsearch.ConsoleSample.Additionals.Products;
using Bing.Elasticsearch.ConsoleSample.ESModel;
using Bing.Elasticsearch.ConsoleSample.Model.Products;
using Bing.Elasticsearch.ConsoleSample.Model.Reports;
using Bing.Extensions;
using Bing.Utils.Json;

namespace Bing.Elasticsearch.ConsoleSample.Samples
{
    /// <summary>
    /// 保存 - 出入库产品报表
    /// </summary>
    public static class SaveInOutStockProductReportAct
    {
        /// <summary>
        /// 执行
        /// </summary>
        public static async Task ExecuteAsync(SampleContext context)
        {
            var beginTime = DateTime.Parse("2021-11-01 00:00:00");
            var endTime = DateTime.Parse("2021-11-02 00:00:00");

            var indexName = $"test_{nameof(InOutStockProductReport).ToSnakeCase()}_{beginTime:yyyy.MM.dd}";

            var inOutStockProductReports = await context.Orm.Select<InOutStockProductReport>()
                .Where(x => x.OrderTime >= beginTime)
                .Where(x => x.OrderTime < endTime)
                //.Limit(10)
                .ToListAsync();

            var productIds = inOutStockProductReports.Select(x => x.ProductId).Distinct().ToList();
            var products = await context.Orm.Select<Product>()
                .Where(x => productIds.Contains(x.ProductId))
                .ToListAsync();

            var result = new List<InOutStockProductReportEo>();

            foreach (var item in inOutStockProductReports)
            {
                var product = products.FirstOrDefault(x => x.ProductId == item.ProductId);
                if (product == null)
                    continue;
                var extend = JsonHelper.ToObject<ProductAdditional>(product.Extend);
                var eoItem = new InOutStockProductReportEo
                {
                    InOutStockProductReportId = item.InOutStockProductReportId,
                    MerchantId = item.MerchantId,
                    GoodsId = item.GoodsId,
                    GoodsCode = item.GoodsCode,
                    GoodsName = item.GoodsName,
                    ProductId = item.ProductId,
                    ProductCode = item.ProductCode,
                    ProductName = item.ProductName,
                    Barcode = item.Barcode,
                    AttributeName = extend.AttributeName,
                    AttributeValue = extend.AttributeValue,
                    Unit = item.Unit,
                    BrandId = item.BrandId,
                    BrandName = item.BrandName,
                    CategoryId = item.CategoryId,
                    CategoryName = item.CategoryName,
                    OrganizationId = item.OrganizationId,
                    OrganizationName = item.OrganizationName,
                    WarehouseId = item.WarehouseId,
                    WarehouseName = item.WarehouseName,
                    UpSheetId = item.UpSheetId,
                    UpSheet = item.UpSheet,
                    UpSheetTime = item.UpSheetTime,
                    OriginalSheet = item.OriginalSheet,
                    OriginalSheetId = item.OriginalSheetId,
                    OrderTime = item.OrderTime,
                    Type = item.Type,
                    TypeDesc = item.Type.Description(),
                    ChangeQty = item.ChangeQty,
                    TotalQty = item.TotalQty,
                    CostPrice = item.CostPrice,
                    TotalCostPrice = item.TotalCostPrice,
                    InventoryCostPrice = item.InventoryCostPrice,
                    TotalInventoryCostPrice = item.TotalInventoryCostPrice,
                    NotTaxCostPrice = item.NotTaxCostPrice,
                    NotTaxTotalCostPrice = item.NotTaxTotalCostPrice,
                    NotTaxInventoryCostPrice = item.NotTaxInventoryCostPrice,
                    NotTaxTotalInventoryCostPrice = item.NotTaxTotalInventoryCostPrice,
                    OriginalPrice = item.OriginalPrice,
                    OriginalAmount = item.OriginalAmount,
                    OriginalNotTaxPrice = item.OriginalNotTaxPrice,
                    OriginalNotTaxAmount = item.OriginalNotTaxAmount,
                    StockCostWay = item.StockCostWay,
                    CreationTime = item.CreationTime,
                    CreatorId = item.CreatorId,
                    Creator = item.Creator,
                    BusinessRelatedUnitName = item.BusinessRelatedUnitName,
                    BusinessRelatedUnitId = item.BusinessRelatedUnitId,
                    BusinessRelatedUnitType = item.BusinessRelatedUnitType,
                    OrderBillingType = item.OrderBillingType
                };
                result.Add(eoItem);
            }

            if (!await context.ESClient.ExistsAsync(indexName))
                await context.ESClient.AddAsync<InOutStockProductReportEo>(indexName);
            await context.ESClient.BulkSaveAsync(indexName, result);
        }
    }
}
