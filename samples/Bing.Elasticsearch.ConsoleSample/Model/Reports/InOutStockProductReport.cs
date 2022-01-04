using System;
using System.ComponentModel.DataAnnotations;
using Bing.Elasticsearch.ConsoleSample.Enums;
using FreeSql.DataAnnotations;

namespace Bing.Elasticsearch.ConsoleSample.Model.Reports
{
    /// <summary>
    /// 出入库产品报表
    /// </summary>
    [Display(Name = "出入库产品报表")]
    [Table(Name = "`Reports.InOutStockProductReport`")]
    public partial class InOutStockProductReport
    {

        /// <summary>
        /// 出入库产品报表标识
        /// </summary>
        [Column(IsPrimary = true)]
        [Display(Name = "出入库产品报表标识")]
        [Required(ErrorMessage = "出入库产品报表标识不能为空")]
        public Guid InOutStockProductReportId { get; set; }

        /// <summary>
        /// 商户标识
        /// </summary>
        [Display(Name = "商户标识")]
        public Guid? MerchantId { get; set; }

        /// <summary>
        /// 商品标识
        /// </summary>
        [Display(Name = "商品标识")]
        [Required(ErrorMessage = "商品标识不能为空")]
        public Guid GoodsId { get; set; }

        /// <summary>
        /// 商品编码
        /// </summary>
        [Display(Name = "商品编码")]
        [Required(ErrorMessage = "商品编码不能为空")]
        [StringLength(50, ErrorMessage = "商品编码输入过长，不能超过50位")]
        public string GoodsCode { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        [Display(Name = "商品名称")]
        [Required(ErrorMessage = "商品名称不能为空")]
        [StringLength(200, ErrorMessage = "商品名称输入过长，不能超过200位")]
        public string GoodsName { get; set; }

        /// <summary>
        /// 产品标识
        /// </summary>
        [Display(Name = "产品标识")]
        [Required(ErrorMessage = "产品标识不能为空")]
        public Guid ProductId { get; set; }

        /// <summary>
        /// 产品编码
        /// </summary>
        [Display(Name = "产品编码")]
        [Required(ErrorMessage = "产品编码不能为空")]
        [StringLength(50, ErrorMessage = "产品编码输入过长，不能超过50位")]
        public string ProductCode { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        [Display(Name = "产品名称")]
        [StringLength(200, ErrorMessage = "产品名称输入过长，不能超过200位")]
        public string ProductName { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        [Display(Name = "条码")]
        [StringLength(50, ErrorMessage = "条码输入过长，不能超过50位")]
        public string Barcode { get; set; }

        /// <summary>
        /// 商品单位
        /// </summary>
        [Display(Name = "商品单位")]
        [Required(ErrorMessage = "商品单位不能为空")]
        [StringLength(20, ErrorMessage = "商品单位输入过长，不能超过20位")]
        public string Unit { get; set; }

        /// <summary>
        /// 品牌标识
        /// </summary>
        [Display(Name = "品牌标识")]
        public Guid? BrandId { get; set; }

        /// <summary>
        /// 品牌名称
        /// </summary>
        [Display(Name = "品牌名称")]
        [StringLength(200, ErrorMessage = "品牌名称输入过长，不能超过200位")]
        public string BrandName { get; set; }

        /// <summary>
        /// 分类标识
        /// </summary>
        [Display(Name = "分类标识")]
        [Required(ErrorMessage = "分类标识不能为空")]
        public Guid CategoryId { get; set; }

        /// <summary>
        /// 分类名称
        /// </summary>
        [Display(Name = "分类名称")]
        [Required(ErrorMessage = "分类名称不能为空")]
        [StringLength(200, ErrorMessage = "分类名称输入过长，不能超过200位")]
        public string CategoryName { get; set; }

        /// <summary>
        /// 组织机构标识
        /// </summary>
        [Display(Name = "组织机构标识")]
        [Required(ErrorMessage = "组织机构标识不能为空")]
        public Guid OrganizationId { get; set; }

        /// <summary>
        /// 组织机构名称
        /// </summary>
        [Display(Name = "组织机构名称")]
        [Required(ErrorMessage = "组织机构名称不能为空")]
        [StringLength(200, ErrorMessage = "组织机构名称输入过长，不能超过200位")]
        public string OrganizationName { get; set; }

        /// <summary>
        /// 仓库标识
        /// </summary>
        [Display(Name = "仓库标识")]
        [Required(ErrorMessage = "仓库标识不能为空")]
        public Guid WarehouseId { get; set; }

        /// <summary>
        /// 仓库名称
        /// </summary>
        [Display(Name = "仓库名称")]
        [Required(ErrorMessage = "仓库名称不能为空")]
        [StringLength(200, ErrorMessage = "仓库名称输入过长，不能超过200位")]
        public string WarehouseName { get; set; }

        /// <summary>
        /// 上游订单标识
        /// </summary>
        [Display(Name = "上游订单标识")]
        public Guid? UpSheetId { get; set; }

        /// <summary>
        /// 上游订单号
        /// </summary>
        [Display(Name = "上游订单号")]
        [StringLength(50, ErrorMessage = "上游订单号输入过长，不能超过50位")]
        public string UpSheet { get; set; }

        /// <summary>
        /// 上游订单时间
        /// </summary>
        [Display(Name = "上游订单时间")]
        public DateTime? UpSheetTime { get; set; }

        /// <summary>
        /// 原始订单号
        /// </summary>
        [Display(Name = "原始订单号")]
        [StringLength(50, ErrorMessage = "原始订单号输入过长，不能超过50位")]
        public string OriginalSheet { get; set; }
        /// <summary>
        /// 原始订单标识
        /// </summary>
        [Display(Name = "原始订单标识")]
        public Guid? OriginalSheetId { get; set; }
        /// <summary>
        /// 单据时间
        /// </summary>
        [Display(Name = "单据时间")]
        public DateTime OrderTime { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        [Display(Name = "类型")]
        [Required(ErrorMessage = "类型不能为空")]
        public InOutStockType Type { get; set; }

        /// <summary>
        /// 改变值
        /// </summary>
        [Display(Name = "改变值")]
        [Required(ErrorMessage = "改变值不能为空")]
        public long ChangeQty { get; set; }

        /// <summary>
        /// 总库存
        /// </summary>
        [Display(Name = "总库存")]
        [Required(ErrorMessage = "总库存不能为空")]
        public long TotalQty { get; set; }

        /// <summary>
        /// 成本价
        /// </summary>
        [Display(Name = "成本价")]
        [Required(ErrorMessage = "成本价不能为空")]
        public decimal CostPrice { get; set; }

        /// <summary>
        /// 总成本
        /// </summary>
        [Display(Name = "总成本")]
        [Required(ErrorMessage = "总成本不能为空")]
        public decimal TotalCostPrice { get; set; }

        /// <summary>
        /// 库存成本价
        /// </summary>
        [Display(Name = "库存成本价")]
        [Required(ErrorMessage = "库存成本价不能为空")]
        public decimal InventoryCostPrice { get; set; }

        /// <summary>
        /// 总库存成本价
        /// </summary>
        [Display(Name = "总库存成本价")]
        [Required(ErrorMessage = "总库存成本价不能为空")]
        public decimal TotalInventoryCostPrice { get; set; }

        /// <summary>
        /// 不含税成本价
        /// </summary>
        [Display(Name = "不含税成本价")]
        [Required(ErrorMessage = "不含税成本价不能为空")]
        public decimal NotTaxCostPrice { get; set; }

        /// <summary>
        /// 不含税总成本
        /// </summary>
        [Display(Name = "不含税总成本")]
        [Required(ErrorMessage = "不含税总成本不能为空")]
        public decimal NotTaxTotalCostPrice { get; set; }

        /// <summary>
        /// 不含税库存成本价
        /// </summary>
        [Display(Name = "不含税库存成本价")]
        [Required(ErrorMessage = "不含税库存成本价不能为空")]
        public decimal NotTaxInventoryCostPrice { get; set; }

        /// <summary>
        /// 不含税总库存成本价
        /// </summary>
        [Display(Name = "不含税总库存成本价")]
        [Required(ErrorMessage = "不含税总库存成本价不能为空")]
        public decimal NotTaxTotalInventoryCostPrice { get; set; }

        /// <summary>
        /// 源单含税价格
        /// </summary>
        [Display(Name = "源单含税价格")]
        [Required(ErrorMessage = "源单含税价格")]
        public decimal OriginalPrice { get; set; }

        /// <summary>
        /// 源单含税金额
        /// </summary>
        [Display(Name = "源单含税金额")]
        [Required(ErrorMessage = "源单含税金额")]
        public decimal OriginalAmount { get; set; }

        /// <summary>
        /// 源单不含税价格
        /// </summary>
        [Display(Name = "源单不含税价格")]
        [Required(ErrorMessage = "源单不含税价格")]
        public decimal OriginalNotTaxPrice { get; set; }

        /// <summary>
        /// 源单不含税金额
        /// </summary>
        [Display(Name = "源单不含税金额")]
        [Required(ErrorMessage = "源单不含税金额")]
        public decimal OriginalNotTaxAmount { get; set; }

        /// <summary>
        /// 成本计算方式
        /// </summary>
        [Display(Name = "成本计算方式")]
        [Required(ErrorMessage = "成本计算方式")]
        public StockCostWay StockCostWay { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Display(Name = "创建时间")]
        public DateTime? CreationTime { get; set; }

        /// <summary>
        /// 创建人标识
        /// </summary>
        [Display(Name = "创建人标识")]
        public Guid? CreatorId { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        [Display(Name = "创建人")]
        [StringLength(200, ErrorMessage = "创建人输入过长，不能超过200位")]
        public string Creator { get; set; }

        /// <summary>
        /// 往来单位名称
        /// </summary>
        [Display(Name = "往来单位名称")]
        public string BusinessRelatedUnitName { get; set; }

        /// <summary>
        /// 往来单位标识
        /// </summary>
        [Display(Name = "往来单位名称")]
        public Guid? BusinessRelatedUnitId { get; set; }

        /// <summary>
        /// 往来单位类型
        /// </summary>
        [Display(Name = "往来单位类型")]
        public BusinessRelatedUnitType BusinessRelatedUnitType { get; set; }

        /// <summary>
        /// 结算类型
        /// </summary>
        [Display(Name = "结算类型")]
        public OrderBillingType OrderBillingType { get; set; }

    }
}