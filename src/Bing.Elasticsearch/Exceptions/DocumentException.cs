using System;

namespace Bing.Elasticsearch.Exceptions;

/// <summary>
/// 文档异常
/// </summary>
public class DocumentException : Exception
{
    /// <summary>
    /// 初始化一个<see cref="DocumentException"/>类型的实例
    /// </summary>
    public DocumentException()
    {
    }

    /// <summary>
    /// 初始化一个<see cref="DocumentException"/>类型的实例
    /// </summary>
    /// <param name="message">消息</param>
    public DocumentException(string message) : base(message)
    {
    }

    /// <summary>
    /// 初始化一个<see cref="DocumentException"/>类型的实例
    /// </summary>
    /// <param name="message">消息</param>
    /// <param name="innerException">内部异常</param>
    public DocumentException(string message, Exception innerException) : base(message, innerException)
    {
    }
}