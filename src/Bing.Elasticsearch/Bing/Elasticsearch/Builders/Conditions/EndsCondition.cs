using System;
using Bing.Elasticsearch.Common.Constants;
using Nest;

namespace Bing.Elasticsearch.Builders.Conditions;

/// <summary>
/// 尾匹配查询条件
/// </summary>
public class EndsCondition : LikeConditionBase
{
    /// <summary>
    /// 初始化一个<see cref="EndsCondition"/>类型的实例
    /// </summary>
    /// <param name="column">列名</param>
    /// <param name="value">值</param>
    /// <exception cref="ArgumentNullException"></exception>
    public EndsCondition(Field column, object value) : base(column, value)
    {
    }

    /// <summary>
    /// 获取值
    /// </summary>
    protected override object GetValue() => $"{BaseEsConst.WILDCARD_SIGN}{Value}";
}