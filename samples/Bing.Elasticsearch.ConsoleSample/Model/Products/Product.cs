using System;
using System.ComponentModel.DataAnnotations;
using FreeSql.DataAnnotations;

namespace Bing.Elasticsearch.ConsoleSample.Model.Products
{
    /// <summary>
    /// 产品
    /// </summary>
    [Display(Name = "产品")]
    [Table(Name = "`Products.Product`")]
    public partial class Product
    {

        /// <summary>
        /// 产品标识
        /// </summary>
        [Column(IsPrimary = true)]
        [Display(Name = "产品标识")]
        [Required(ErrorMessage = "产品标识不能为空")]
        public Guid ProductId { get; set; }
    
        /// <summary>
        /// 商品标识
        /// </summary>
        [Display(Name = "商品标识")]
        [Required(ErrorMessage = "商品标识不能为空")]
        public Guid GoodsId { get; set; }
    
        /// <summary>
        /// 商户标识
        /// </summary>
        [Display(Name = "商户标识")]
        public Guid? MerchantId { get; set; }
    
        /// <summary>
        /// 编码
        /// </summary>
        [Display(Name = "编码")]
        [Required(ErrorMessage = "编码不能为空")]
        [StringLength( 50, ErrorMessage = "编码输入过长，不能超过50位" )]
        public string Code { get; set; }
    
        /// <summary>
        /// 条码
        /// </summary>
        [Display(Name = "条码")]
        [Required(ErrorMessage = "条码不能为空")]
        [StringLength( 50, ErrorMessage = "条码输入过长，不能超过50位" )]
        public string Barcode { get; set; }
    
        /// <summary>
        /// 国标码
        /// </summary>
        [Display(Name = "国标码")]
        [StringLength( 50, ErrorMessage = "国标码输入过长，不能超过50位" )]
        public string NationalCode { get; set; }
    
        /// <summary>
        /// 单位
        /// </summary>
        [Display(Name = "单位")]
        [Required(ErrorMessage = "单位不能为空")]
        [StringLength( 20, ErrorMessage = "单位输入过长，不能超过20位" )]
        public string Unit { get; set; }
    
        /// <summary>
        /// 建议零售价
        /// </summary>
        [Display(Name = "建议零售价")]
        public decimal? SuggestedRetailPrice { get; set; }
    
        /// <summary>
        /// 参考采购价
        /// </summary>
        [Display(Name = "参考采购价")]
        public decimal? ReferPurchasePrice { get; set; }
    
        /// <summary>
        /// 重量
        /// </summary>
        [Display(Name = "重量")]
        public long? Weight { get; set; }
    
        /// <summary>
        /// 共享条码
        /// </summary>
        [Display(Name = "共享条码")]
        [StringLength( 50, ErrorMessage = "共享条码输入过长，不能超过50位" )]
        public string ShareBarcode { get; set; }
    
        /// <summary>
        /// 是否业务占用
        /// </summary>
        [Display(Name = "是否业务占用")]
        public bool IsBusinessOccupy { get; set; }
    
        /// <summary>
        /// 是否删除
        /// </summary>
        [Display(Name = "是否删除")]
        public bool IsDeleted { get; set; }
    
        /// <summary>
        /// 版本号
        /// </summary>
        [Display(Name = "版本号")]
        public byte[] Version { get; set; }
    
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
        [StringLength( 200, ErrorMessage = "创建人输入过长，不能超过200位" )]
        public string Creator { get; set; }
    
        /// <summary>
        /// 最后修改时间
        /// </summary>
        [Display(Name = "最后修改时间")]
        public DateTime? LastModificationTime { get; set; }
    
        /// <summary>
        /// 最后修改人标识
        /// </summary>
        [Display(Name = "最后修改人标识")]
        public Guid? LastModifierId { get; set; }
    
        /// <summary>
        /// 最后修改人
        /// </summary>
        [Display(Name = "最后修改人")]
        [StringLength( 200, ErrorMessage = "最后修改人输入过长，不能超过200位" )]
        public string LastModifier { get; set; }
    
        /// <summary>
        /// 扩展
        /// </summary>
        [Display(Name = "扩展")]
        public string Extend { get; set; }
    
    }
}