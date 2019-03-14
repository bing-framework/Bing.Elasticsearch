using System;
using Nest;

namespace Bing.Elasticsearch.Tests
{
    /// <summary>
    /// 5.x特性
    /// </summary>
    [ElasticsearchType(Name = "TestModel5", IdProperty = "Id")]// name=文档类型,idProperty=文档的唯一键字段名
    public class TestModel5
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        [Number(NumberType.Long,Name = "Id")]
        public long Id { get; set; }

        /// <summary>
        /// keyword 不分词
        /// </summary>
        [Keyword(Name = "Name",Index = true)] // 不需要分词的字符串，name=名称,index=是否建立索引
        public string Name { get; set; }

        /// <summary>
        /// text 分词，Analyzer = "ik_max_word"
        /// </summary>
        [Text(Name = "Dic",Index = true)] // 需要分词的字符串，name=名称,index=是否建立索引,Analyzer=分词器
        public string Dic { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [Number(NumberType.Integer,Name = "State")]// 数字类型+名称
        public int State { get; set; }

        /// <summary>
        /// 逻辑删除
        /// </summary>
        [Boolean(Name = "Deleted")]
        public bool Deleted { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Date(Name = "CreateTime")]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// float 值
        /// </summary>
        [Number(NumberType.Float,Name = "Fvalue")]
        public float Fvalue { get; set; }

        /// <summary>
        /// double 值
        /// </summary>
        [Number(NumberType.Double,Name = "Dvalue")]
        public double Dvalue { get; set; }
    }
}
