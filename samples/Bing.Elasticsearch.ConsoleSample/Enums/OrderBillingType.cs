using System;
using System.ComponentModel;

namespace Bing.Elasticsearch.ConsoleSample.Enums
{
    /// <summary>
    /// 结算类型
    /// </summary>
    [Flags]
    public enum OrderBillingType
    {
        /// <summary>
        /// 空
        /// </summary>
        [Description("")]
        None = 0x0000,
        /// <summary>
        /// 购销
        /// </summary>
        [Description("购销")]
        PurchaseSale = 0x0001,

        /// <summary>
        /// 代销
        /// </summary>
        [Description("代销")]
        Consignment = 0x0002,

        /// <summary>
        /// 购销并代销
        /// </summary>
        [Description("购销|代销")]
        All = 0x0003,

    }
}
