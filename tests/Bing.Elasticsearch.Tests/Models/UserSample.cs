using Nest;

namespace Bing.Elasticsearch.Tests.Models;

[ElasticsearchType(RelationName = "test_user", IdProperty = "UserId")]
public class UserSample
{
    /// <summary>
    /// 用户标识
    /// </summary>
    public long UserId { get; set; }

    /// <summary>
    /// 用户名
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// float 值
    /// </summary>
    public float FloatValue { get; set; }

    /// <summary>
    /// double 值
    /// </summary>
    public double DoubleValue { get; set; }
}