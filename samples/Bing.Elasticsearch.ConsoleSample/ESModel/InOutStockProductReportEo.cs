using System;
using Bing.Elasticsearch.ConsoleSample.Enums;

namespace Bing.Elasticsearch.ConsoleSample.ESModel
{
    public class InOutStockProductReportEo
    {
        /// <summary>
        /// 标识
        /// </summary>
        public Guid InOutStockProductReportId { get; set; }

        /// <summary>
        /// 商户标识
        /// </summary>
        public Guid? MerchantId { get; set; }

        /// <summary>
        /// 商品标识
        /// </summary>
        public Guid GoodsId { get; set; }

        /// <summary>
        /// 商品编码
        /// </summary>
        public string GoodsCode { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string GoodsName { get; set; }

        /// <summary>
        /// 产品标识
        /// </summary>
        public Guid ProductId { get; set; }

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public string Barcode { get; set; }

        /// <summary>
        /// 属性名称
        /// </summary>
        public string AttributeName { get; set; }

        /// <summary>
        /// 属性值
        /// </summary>
        public string AttributeValue { get; set; }

        /// <summary>
        /// 商品单位
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// 品牌标识
        /// </summary>
        public Guid? BrandId { get; set; }

        /// <summary>
        /// 品牌名称
        /// </summary>
        public string BrandName { get; set; }

        /// <summary>
        /// 分类标识
        /// </summary>
        public Guid CategoryId { get; set; }

        /// <summary>
        /// 分类名称
        /// </summary>
        public string CategoryName { get; set; }

        /// <summary>
        /// 组织机构标识
        /// </summary>
        public Guid OrganizationId { get; set; }

        /// <summary>
        /// 组织机构名称
        /// </summary>
        public string OrganizationName { get; set; }

        /// <summary>
        /// 仓库标识
        /// </summary>
        public Guid WarehouseId { get; set; }

        /// <summary>
        /// 仓库名称
        /// </summary>
        public string WarehouseName { get; set; }

        /// <summary>
        /// 上游订单标识
        /// </summary>
        public Guid? UpSheetId { get; set; }

        /// <summary>
        /// 上游订单号
        /// </summary>
        public string UpSheet { get; set; }

        /// <summary>
        /// 上游订单时间
        /// </summary>
        public DateTime? UpSheetTime { get; set; }

        /// <summary>
        /// 原始订单号
        /// </summary>
        public string OriginalSheet { get; set; }

        /// <summary>
        /// 原始订单标识
        /// </summary>
        public Guid? OriginalSheetId { get; set; }

        /// <summary>
        /// 单据时间
        /// </summary>
        public DateTime OrderTime { get; set; }

        /// <summary>
        /// 业务类型
        /// </summary>
        public InOutStockType Type { get; set; }

        /// <summary>
        /// 业务类型描述
        /// </summary>
        public string TypeDesc { get; set; }

        /// <summary>
        /// 改变值
        /// </summary>
        public long ChangeQty { get; set; }

        /// <summary>
        /// 总库存
        /// </summary>
        public long TotalQty { get; set; }

        /// <summary>
        /// 成本价
        /// </summary>
        public decimal CostPrice { get; set; }

        /// <summary>
        /// 总成本
        /// </summary>
        public decimal TotalCostPrice { get; set; }

        /// <summary>
        /// 库存成本价
        /// </summary>
        public decimal InventoryCostPrice { get; set; }

        /// <summary>
        /// 总库存成本价
        /// </summary>
        public decimal TotalInventoryCostPrice { get; set; }

        /// <summary>
        /// 不含税成本价
        /// </summary>
        public decimal NotTaxCostPrice { get; set; }

        /// <summary>
        /// 不含税总成本
        /// </summary>
        public decimal NotTaxTotalCostPrice { get; set; }

        /// <summary>
        /// 不含税库存成本价
        /// </summary>
        public decimal NotTaxInventoryCostPrice { get; set; }

        /// <summary>
        /// 不含税总库存成本价
        /// </summary>
        public decimal NotTaxTotalInventoryCostPrice { get; set; }

        /// <summary>
        /// 源单含税价格
        /// </summary>
        public decimal OriginalPrice { get; set; }

        /// <summary>
        /// 源单含税金额
        /// </summary>
        public decimal OriginalAmount { get; set; }

        /// <summary>
        /// 源单不含税价格
        /// </summary>
        public decimal OriginalNotTaxPrice { get; set; }

        /// <summary>
        /// 源单不含税金额
        /// </summary>
        public decimal OriginalNotTaxAmount { get; set; }

        /// <summary>
        /// 成本计算方式
        /// </summary>
        public StockCostWay StockCostWay { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreationTime { get; set; }

        /// <summary>
        /// 创建人标识
        /// </summary>
        public Guid? CreatorId { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string Creator { get; set; }

        /// <summary>
        /// 往来单位名称
        /// </summary>
        public string BusinessRelatedUnitName { get; set; }

        /// <summary>
        /// 往来单位标识
        /// </summary>
        public Guid? BusinessRelatedUnitId { get; set; }

        /// <summary>
        /// 往来单位类型
        /// </summary>
        public BusinessRelatedUnitType BusinessRelatedUnitType { get; set; }

        /// <summary>
        /// 结算类型
        /// </summary>
        public OrderBillingType OrderBillingType { get; set; }
    }
}
