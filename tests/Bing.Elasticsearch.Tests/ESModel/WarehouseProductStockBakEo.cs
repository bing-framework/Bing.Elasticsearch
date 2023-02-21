using System;
using Nest;

namespace Bing.Elasticsearch.Tests.ESModel
{
    /// <summary>
    /// 仓库产品库存 ES对象
    /// </summary>
    [ElasticsearchType(RelationName = "backups_warehouse_product_stock", IdProperty = "WarehouseProductStockBakId")]
    public class WarehouseProductStockBakEo
    {
        /// <summary>
        /// 仓库库存标识
        /// </summary>
        public Guid WarehouseProductStockBakId { get; set; }

        /// <summary>
        /// 备份时间
        /// </summary>
        public DateTime BakTime { get; set; }

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
        [Keyword]
        public string GoodsName { get; set; }

        /// <summary>
        /// 商品类别名称
        /// </summary>
        public string CategoryName { get; set; }

        /// <summary>
        /// 商品品牌名称
        /// </summary>
        public string BrandName { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// 副单位
        /// </summary>
        public string UnitAdditional { get; set; }

        /// <summary>
        /// 库存副单位
        /// </summary>
        public string StockUnitAdditional { get; set; }

        /// <summary>
        /// 产品标识
        /// </summary>
        public Guid ProductId { get; set; }

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode { get; set; }

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
        /// 现货库存
        /// </summary>
        public decimal CurrentQty { get; set; }

        /// <summary>
        /// 可用库存
        /// </summary>
        public decimal UsableQty { get; set; }

        /// <summary>
        /// 预占库存
        /// </summary>
        public decimal OrderQty { get; set; }

        /// <summary>
        /// 锁定库存
        /// </summary>
        public decimal LockQty { get; set; }

        /// <summary>
        /// 在途库存
        /// </summary>
        public decimal InTransitQty { get; set; }

        /// <summary>
        /// 成本价
        /// </summary>
        public decimal CostPrice { get; set; }

        /// <summary>
        /// 总成本
        /// </summary>
        public decimal TotalCostPrice { get; set; }

        /// <summary>
        /// 不含税成本价
        /// </summary>
        public decimal NotTaxCostPrice { get; set; }

        /// <summary>
        /// 不含税总成本
        /// </summary>
        public decimal NotTaxTotalCostPrice { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreationTime { get; set; }

        /// <summary>
        /// 最后修改时间
        /// </summary>
        public DateTime? LastModificationTime { get; set; }
    }
}