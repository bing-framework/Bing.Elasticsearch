using System.ComponentModel.DataAnnotations.Schema;

namespace Bing.Elasticsearch.ConsoleSample.Additionals.Products
{
    /// <summary>
    /// 产品附加
    /// </summary>
    [NotMapped]
    public class ProductAdditional
    {
        /// <summary>
        /// 属性名称
        /// </summary>
        public string AttributeName { get; set; }

        /// <summary>
        /// 属性值
        /// </summary>
        public string AttributeValue { get; set; }

        /// <summary>
        /// 产品属性附加
        /// </summary>
        public ProductAttributeAdditional AttributeAdditional { get; set; }

        /// <summary>
        /// 产品自定义字段附加
        /// </summary>
        public ProductCustomFieldAdditional CustomFieldAdditional { get; set; }
    }
}