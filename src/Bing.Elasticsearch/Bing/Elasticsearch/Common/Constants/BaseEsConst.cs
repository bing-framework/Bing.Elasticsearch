namespace Bing.Elasticsearch.Common.Constants;

/// <summary>
/// Es的常量
/// </summary>
public class BaseEsConst
{
    /// <summary>
    /// 通配符
    /// </summary>
    public const string WILDCARD_SIGN = "*";

    /// <summary>
    /// 关键词
    /// </summary>
    public const string KEYWORD = "keyword";

    /// <summary>
    /// IK分词
    /// </summary>
    public const string IK = "ik";

    /// <summary>
    /// IK分词 - 将需要分词的文本做最大粒度的拆分。
    /// </summary>
    public const string IK_SMART = "ik_smart";

    /// <summary>
    /// IK分词 - 将需要分词的文本做最小粒度的拆分，尽量分更多的词。
    /// </summary>
    public const string IK_MAX_WORD = "ik_max_word";
}