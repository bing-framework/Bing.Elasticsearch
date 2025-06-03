using System;
using Nest;

namespace Bing.Elasticsearch.Tests.Models;

public class TestModel1
{
    /// <summary>
    /// 系统编号
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// keyword 不分词
    /// </summary>
    [Keyword(Name = "Name", Index = true)]
    public string Name { get; set; }

    /// <summary>
    /// text 分词，Analyzer = "ik_max_word"
    /// </summary>
    [Text(Name = "Test_Dic",Analyzer = "ik_max_word")]
    public string Dic { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    public int State { get; set; }

    /// <summary>
    /// 逻辑删除
    /// </summary>
    public bool Deleted { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// float 值
    /// </summary>
    public float Fvalue { get; set; }

    /// <summary>
    /// double 值
    /// </summary>
    public double Dvalue { get; set; }
}