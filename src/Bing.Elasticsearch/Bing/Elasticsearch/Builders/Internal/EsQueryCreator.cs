using System;
using System.Collections.Generic;
using Nest;

namespace Bing.Elasticsearch.Builders.Internal;

/// <summary>
/// ES查询创建器
/// </summary>
public static class EsQueryCreator
{
    /// <summary>
    /// 创建ID查询。仅仅过滤与所提供的ID相匹配的文档
    /// </summary>
    /// <param name="values">数值</param>
    public static QueryContainer CreateIdsQuery(IEnumerable<Id> values)
    {
        return new IdsQuery
        {
            Values = values
        };
    }

    /// <summary>
    /// 创建通配符查询。匹配与通配符表达式具有匹配字段的文档。
    /// </summary>
    /// <param name="field">字段</param>
    /// <param name="value">值</param>
    public static QueryContainer CreateWildcardQuery(string field, object value)
    {
        return new WildcardQuery
        {
            Field = field,
            Value = value
        };
    }

    /// <summary>
    /// 创建项查询。查找包含在反向索引中指定的确切项的文档。
    /// </summary>
    /// <param name="field">字段</param>
    /// <param name="value">值</param>
    public static QueryContainer CreateTermQuery(string field, object value)
    {
        return new TermQuery
        {
            Field = field,
            Value = value
        };
    }

    public static QueryContainer CreateTermsQuery(string field, IEnumerable<object> value)
    {
        return new TermsQuery
        {
            Field = field,
            Terms = value
        };
    }

    public static QueryContainer CreateFuzzyQuery(string field, object value)
    {
        return new FuzzyQuery
        {
            Field = field,
            Fuzziness = Fuzziness.Auto,
            Value = value.ToString(),
            Boost = 100
        };
    }

    public static QueryContainer CreateMatchQuery(string field, string value, Nest.Operator @operator = Nest.Operator.And)
    {
        return new MatchQuery
        {
            Field = field,
            Query = value,
            Operator = @operator
        };
    }

    public static QueryContainer CreateMatchPhraseQuery(string field, string value, int slop = 0)
    {
        return new MatchPhraseQuery
        {
            Field = field,
            Query = value,
            Slop = slop
        };
    }

    public static QueryContainer CreateEqualQuery(string field, object value)
    {
        if (value == null)
            throw new ArgumentNullException(nameof(value));
        if (value is string)
            return CreateMatchPhraseQuery(field, value.ToString());
        return CreateTermQuery(field, value);
    }

    public static QueryContainer CreateMultiMatchQuery(string[] fields, string value, Nest.Operator @operator, TextQueryType textQueryType, int slop = 0)
    {
        return new MultiMatchQuery
        {
            Fields = fields,
            Query = value,
            Operator = @operator,
            Type = textQueryType,
            Slop = slop
        };
    }
}