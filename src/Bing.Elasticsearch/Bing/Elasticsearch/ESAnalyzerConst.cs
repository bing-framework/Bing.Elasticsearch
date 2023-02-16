namespace Bing.Elasticsearch;

/// <summary>
/// ES分词常量
/// </summary>
public static class ESAnalyzerConst
{
    /// <summary>
    /// IK分词
    /// </summary>
    public const string Ik = "ik";

    /// <summary>
    /// IK分词 - 将需要分词的文本做最大粒度的拆分。
    /// </summary>
    public const string IkSmart = "ik_smart";

    /// <summary>
    /// IK分词 - 将需要分词的文本做最小粒度的拆分，尽量分更多的词。
    /// </summary>
    public const string IkMaxWord = "ik_max_word";
}