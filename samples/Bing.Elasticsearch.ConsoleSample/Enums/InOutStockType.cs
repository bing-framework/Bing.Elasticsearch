using System.ComponentModel;

namespace Bing.Elasticsearch.ConsoleSample.Enums
{
    /// <summary>
    /// 出入库单类型
    /// </summary>
    public enum InOutStockType
    {
        #region 出库

        /// <summary>
        /// 采购退货出库
        /// </summary>
        [Description("采购退货出库")]
        PurchaseReturn = 1,

        /// <summary>
        /// 销售出库
        /// </summary>
        [Description("销售出库")]
        Sale = 11,

        /// <summary>
        /// 盘亏出库
        /// </summary>
        [Description("盘亏出库")]
        InventoryOut = 21,

        /// <summary>
        /// 调拨出库
        /// </summary>
        [Description("调拨出库")]
        TransferOut = 31,

        /// <summary>
        /// 报损出库
        /// </summary>
        [Description("报损出库")]
        Loss = 41,

        /// <summary>
        /// 赠品出库
        /// </summary>
        [Description("赠品出库")]
        Gift = 61,

        /// <summary>
        /// 库存调整出库
        /// </summary>
        [Description("库存调整出库")]
        AdjustOut = 51,

        /// <summary>
        /// 销售换货出库
        /// </summary>
        [Description("销售换货出库")]
        ExchangeOut = 71,

        /// <summary>
        /// 出库单出库
        /// </summary>
        [Description("出库单出库")]
        OutStockOut = 81,

        /// <summary>
        /// 其它出库
        /// </summary>
        [Description("其它出库")]
        OtherOut = 91,

        #endregion

        #region  入库

        /// <summary>
        /// 采购入库
        /// </summary>
        [Description("采购入库")]
        Purchase = 0,

        /// <summary>
        /// 销售退货入库
        /// </summary>
        [Description("销售退货入库")]
        SaleReturn = 10,

        /// <summary>
        /// 盘盈入库
        /// </summary>
        [Description("盘盈入库")]
        InventoryIn = 20,

        /// <summary>
        /// 调拨入库
        /// </summary>
        [Description("调拨入库")]
        TransferIn = 30,

        /// <summary>
        /// 调拨回货入库
        /// </summary>
        [Description("调拨回货入库")]
        TransferRegainIn = 40,

        /// <summary>
        /// 库存调整入库
        /// </summary>
        [Description("库存调整入库")]
        AdjustIn = 50,

        /// <summary>
        /// 销售换货入库
        /// </summary>
        [Description("销售换货入库")]
        ExchangeIn = 70,

        /// <summary>
        /// 入库单入库
        /// </summary>
        [Description("入库单入库")]
        InStockIn = 80,

        /// <summary>
        /// 其它入库
        /// </summary>
        [Description("其它入库")]
        OtherIn = 90,
        #endregion
    }
}
