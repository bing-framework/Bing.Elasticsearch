using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bing.Elasticsearch.ConsoleSample.Additionals.Products
{
    /// <summary>
    /// 商品自定义字段附加
    /// </summary>
    [NotMapped]
    public class GoodsCustomFieldAdditional
    {
        /// <summary>
        /// 明细
        /// </summary>
        public List<GoodsCustomFieldAdditionalItem> Items { get; set; }

        /// <summary>
        /// 自定义字段附加
        /// </summary>
        public class GoodsCustomFieldAdditionalItem
        {
            /// <summary>
            /// 名称
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// 值
            /// </summary>
            public string Value { get; set; }
        }
    }
}