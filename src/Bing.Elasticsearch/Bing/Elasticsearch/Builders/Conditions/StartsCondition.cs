using System;
using Bing.Elasticsearch.Common.Constants;
using Nest;

namespace Bing.Elasticsearch.Builders.Conditions;

/// <summary>
/// 头匹配查询条件
/// </summary>
public class StartsCondition : LikeConditionBase
{
    /// <summary>
    /// 初始化一个<see cref="StartsCondition"/>类型的实例
    /// </summary>
    /// <param name="column">列名</param>
    /// <param name="value">值</param>
    /// <exception cref="ArgumentNullException"></exception>
    public StartsCondition(Field column, object value) : base(column, value)
    {
    }

    /// <summary>
    /// 获取值
    /// </summary>
    protected override object GetValue() => $"{Value}{BaseEsConst.WILDCARD_SIGN}";
}