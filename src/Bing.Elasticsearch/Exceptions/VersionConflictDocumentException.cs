using System;

namespace Bing.Elasticsearch.Exceptions;

/// <summary>
/// 版本冲突文档异常
/// </summary>
public class VersionConflictDocumentException : DocumentException
{
    /// <summary>
    /// 初始化一个<see cref="VersionConflictDocumentException"/>类型的实例
    /// </summary>
    public VersionConflictDocumentException()
    {
    }

    /// <summary>
    /// 初始化一个<see cref="VersionConflictDocumentException"/>类型的实例
    /// </summary>
    /// <param name="message">消息</param>
    public VersionConflictDocumentException(string message) : base(message)
    {
    }

    /// <summary>
    /// 初始化一个<see cref="VersionConflictDocumentException"/>类型的实例
    /// </summary>
    /// <param name="message">消息</param>
    /// <param name="innerException">内部异常</param>
    public VersionConflictDocumentException(string message, Exception innerException) : base(message, innerException)
    {
    }
}