using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bing.Elasticsearch.ConsoleSample.Additionals.Products
{
    /// <summary>
    /// 产品属性附加
    /// </summary>
    [NotMapped]
    public class ProductAttributeAdditional
    {
        /// <summary>
        /// 商品所选属性
        /// </summary>
        public List<Item> Items { get; set; }
    }
}