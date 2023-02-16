using System;

namespace Bing.Elasticsearch.Exceptions;

/// <summary>
/// ES仓储异常
/// </summary>
public class EsRepositoryException : Exception
{
    /// <summary>
    /// 初始化一个<see cref="EsRepositoryException"/>类型的实例
    /// </summary>
    public EsRepositoryException()
    {
    }

    /// <summary>
    /// 初始化一个<see cref="EsRepositoryException"/>类型的实例
    /// </summary>
    /// <param name="message">消息</param>
    public EsRepositoryException(string message) : base(message)
    {
    }

    /// <summary>
    /// 初始化一个<see cref="EsRepositoryException"/>类型的实例
    /// </summary>
    /// <param name="message">消息</param>
    /// <param name="innerException">内部异常</param>
    public EsRepositoryException(string message, Exception innerException) : base(message, innerException)
    {
    }
}