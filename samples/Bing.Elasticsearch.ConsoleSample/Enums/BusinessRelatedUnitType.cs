using System.ComponentModel;

namespace Bing.Elasticsearch.ConsoleSample.Enums
{
    /// <summary>
    /// 单位类型
    /// </summary>
    public enum BusinessRelatedUnitType
    {
        /// <summary>
        /// 供应商
        /// </summary>
        [Description("供应商")]
        Supplier = 1,

        /// <summary>
        /// 门店
        /// </summary>
        [Description("门店")]
        Shop = 2,

        /// <summary>
        /// 客户
        /// </summary>
        [Description("客户")]
        Client = 3,

        /// <summary>
        /// 仓库
        /// </summary>
        [Description("仓库")]
        Warehouse = 4
    }
}
