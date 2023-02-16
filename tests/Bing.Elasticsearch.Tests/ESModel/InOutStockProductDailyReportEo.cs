using System;
using Nest;

namespace Bing.Elasticsearch.Tests.ESModel
{
    /// <summary>
    /// 出入库产品日报表 ES对象
    /// </summary>
    [ElasticsearchType(RelationName = "in_out_stock_product_daily_report", IdProperty = "InOutStockProductDailyReportId")]
    public class InOutStockProductDailyReportEo
    {
        /// <summary>
        /// 出入库产品日报表标识
        /// </summary>
        public Guid InOutStockProductDailyReportId { get; set; }

        /// <summary>
        /// 报表日期
        /// </summary>
        public DateTime ReportDate { get; set; }

        /// <summary>
        /// 商户标识
        /// </summary>
        public Guid MerchantId { get; set; }

        /// <summary>
        /// 仓库标识
        /// </summary>
        public Guid WarehouseId { get; set; }

        /// <summary>
        /// 仓库名称
        /// </summary>
        public string WarehouseName { get; set; }

        /// <summary>
        /// 产品标识
        /// </summary>
        public Guid ProductId { get; set; }

        /// <summary>
        /// 期初数量
        /// </summary>
        public decimal OpeningQty { get; set; }

        /// <summary>
        /// 期初含税成本
        /// </summary>
        public decimal OpeningCostPrice { get; set; }

        /// <summary>
        /// 期初不含税成本
        /// </summary>
        public decimal OpeningNotTaxCost { get; set; }

        /// <summary>
        /// 调拨入库数量
        /// </summary>
        public decimal TransferInQty { get; set; }

        /// <summary>
        /// 调拨入库含税成本
        /// </summary>
        public decimal TransferInCostPrice { get; set; }

        /// <summary>
        /// 调拨入库不含税成本
        /// </summary>
        public decimal TransferInNotTaxCost { get; set; }

        /// <summary>
        /// 调拨回货入库数量
        /// </summary>
        public decimal TransferRegainInQty { get; set; }

        /// <summary>
        /// 调拨回货入库成本
        /// </summary>
        public decimal TransferRegainInCostPrice { get; set; }

        /// <summary>
        /// 调拨回货入库不含税成本
        /// </summary>
        public decimal TransferRegainInNotTaxCost { get; set; }

        /// <summary>
        /// 采购入库数量
        /// </summary>
        public decimal PurchaseQty { get; set; }

        /// <summary>
        /// 采购入库含税成本
        /// </summary>
        public decimal PurchaseCostPrice { get; set; }

        /// <summary>
        /// 采购入库不含税成本
        /// </summary>
        public decimal PurchaseNotTaxCost { get; set; }

        /// <summary>
        /// 销售退货入库数量
        /// </summary>
        public decimal SaleReturnQty { get; set; }

        /// <summary>
        /// 销售退货入库含税成本
        /// </summary>
        public decimal SaleReturnCostPrice { get; set; }

        /// <summary>
        /// 销售退货入库不含税成本
        /// </summary>
        public decimal SaleReturnNotTaxCost { get; set; }

        /// <summary>
        /// 盘盈入库数量
        /// </summary>
        public decimal InventoryInQty { get; set; }

        /// <summary>
        /// 盘盈入库含税成本
        /// </summary>
        public decimal InventoryInCostPrice { get; set; }

        /// <summary>
        /// 盘盈入库不含税成本
        /// </summary>
        public decimal InventoryInNotTaxCost { get; set; }

        /// <summary>
        /// 校正入库数量
        /// </summary>
        public decimal AdjustInQty { get; set; }

        /// <summary>
        /// 调整入库成本
        /// </summary>
        public decimal AdjustInCost { get; set; }

        /// <summary>
        /// 调整入库不含税成本
        /// </summary>
        public decimal AdjustInNotTaxCost { get; set; }

        /// <summary>
        /// 调整入库数量
        /// </summary>
        public decimal AlterInQty { get; set; }

        /// <summary>
        /// 调整入库成本
        /// </summary>
        public decimal AlterInCost { get; set; }

        /// <summary>
        /// 调整入库不含税成本
        /// </summary>
        public decimal AlterInNotTaxCost { get; set; }

        /// <summary>
        /// 成本调整入库数量
        /// </summary>
        public decimal InventoryCostInQty { get; set; }

        /// <summary>
        /// 成本调整入库成本
        /// </summary>
        public decimal InventoryCostInCost { get; set; }

        /// <summary>
        /// 成本调整入库不含税成本
        /// </summary>
        public decimal InventoryCostInNotTaxCost { get; set; }

        /// <summary>
        /// 入库合计数量
        /// </summary>
        public decimal InStockQty { get; set; }

        /// <summary>
        /// 入库合计含税成本
        /// </summary>
        public decimal InStockCostPrice { get; set; }

        /// <summary>
        /// 入库合计不含税成本
        /// </summary>
        public decimal InStockNotTaxCost { get; set; }

        /// <summary>
        /// 销售出库数量
        /// </summary>
        public decimal SaleQty { get; set; }

        /// <summary>
        /// 销售出库含税成本
        /// </summary>
        public decimal SaleCostPrice { get; set; }

        /// <summary>
        /// 销售出库不含税成本
        /// </summary>
        public decimal SaleNotTaxCost { get; set; }

        /// <summary>
        /// 采购退货数量
        /// </summary>
        public decimal PurchaseReturnQty { get; set; }

        /// <summary>
        /// 采购退货含税成本
        /// </summary>
        public decimal PurchaseReturnCostPrice { get; set; }

        /// <summary>
        /// 采购退货不含税成本
        /// </summary>
        public decimal PurchaseReturnNotTaxCost { get; set; }

        /// <summary>
        /// 调拨出库数量
        /// </summary>
        public decimal TransferOutQty { get; set; }

        /// <summary>
        /// 调拨出库含税成本
        /// </summary>
        public decimal TransferOutCostPrice { get; set; }

        /// <summary>
        /// 调拨出库不含税成本
        /// </summary>
        public decimal TransferOutNotTaxCost { get; set; }

        /// <summary>
        /// 盘亏出库数量
        /// </summary>
        public decimal InventoryOutQty { get; set; }

        /// <summary>
        /// 盘亏出库含税成本
        /// </summary>
        public decimal InventoryOutCostPrice { get; set; }

        /// <summary>
        /// 盘亏出库不含税成本
        /// </summary>
        public decimal InventoryOutNotTaxCost { get; set; }

        /// <summary>
        /// 报损出库数量
        /// </summary>
        public decimal LossOutQty { get; set; }

        /// <summary>
        /// 报损出库含税成本
        /// </summary>
        public decimal LossOutCostPrice { get; set; }

        /// <summary>
        /// 报损出库不含税成本
        /// </summary>
        public decimal LossOutNotTaxCost { get; set; }

        /// <summary>
        /// 校正出库数量
        /// </summary>
        public decimal AdjustOutQty { get; set; }

        /// <summary>
        /// 校正出库成本
        /// </summary>
        public decimal AdjustOutCost { get; set; }

        /// <summary>
        /// 校正出库不含税成本
        /// </summary>
        public decimal AdjustOutNotTaxCost { get; set; }

        /// <summary>
        /// 调整出库数量
        /// </summary>
        public decimal AlterOutQty { get; set; }

        /// <summary>
        /// 调整出库成本
        /// </summary>
        public decimal AlterOutCost { get; set; }

        /// <summary>
        /// 调整出库不含税成本
        /// </summary>
        public decimal AlterOutNotTaxCost { get; set; }

        /// <summary>
        /// 成本调整出库数量
        /// </summary>
        public decimal InventoryCostOutQty { get; set; }
        /// <summary>
        /// 成本调整出库成本
        /// </summary>
        public decimal InventoryCostOutCost { get; set; }
        /// <summary>
        /// 成本调整出库不含税成本
        /// </summary>
        public decimal InventoryCostOutNotTaxCost { get; set; }

        /// <summary>
        /// 赠品出库数量
        /// </summary>
        public decimal GiftOutQty { get; set; }

        /// <summary>
        /// 赠品出库含税成本
        /// </summary>
        public decimal GiftOutCostPrice { get; set; }

        /// <summary>
        /// 赠品出库不含税成本
        /// </summary>
        public decimal GiftOutNotTaxCost { get; set; }

        /// <summary>
        /// 出库合计数量
        /// </summary>
        public decimal OutStockQty { get; set; }

        /// <summary>
        /// 出库合计含税成本
        /// </summary>
        public decimal OutStockCostPrice { get; set; }

        /// <summary>
        /// 出库合计不含税成本
        /// </summary>
        public decimal OutStockNotTaxCost { get; set; }

        /// <summary>
        /// 结算类型
        /// </summary>
        public int OrderBillingType { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreationTime { get; set; }
    }
}