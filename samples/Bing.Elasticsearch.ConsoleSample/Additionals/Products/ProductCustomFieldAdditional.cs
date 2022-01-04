using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bing.Elasticsearch.ConsoleSample.Additionals.Products
{
    /// <summary>
    /// 产品自定义字段附加
    /// </summary>
    [NotMapped]
    public class ProductCustomFieldAdditional
    {
        /// <summary>
        /// 明细
        /// </summary>
        public List<GoodsCustomFieldAdditional.GoodsCustomFieldAdditionalItem> Items { get; set; }
    }
}