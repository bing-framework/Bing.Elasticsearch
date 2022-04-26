using System;

namespace Bing.Elasticsearch.Exceptions
{
    /// <summary>
    /// 重复文档异常
    /// </summary>
    public class DuplicateDocumentException : DocumentException
    {
        /// <summary>
        /// 初始化一个<see cref="DuplicateDocumentException"/>类型的实例
        /// </summary>
        public DuplicateDocumentException()
        {
        }

        /// <summary>
        /// 初始化一个<see cref="DuplicateDocumentException"/>类型的实例
        /// </summary>
        /// <param name="message">消息</param>
        public DuplicateDocumentException(string message) : base(message)
        {
        }

        /// <summary>
        /// 初始化一个<see cref="DuplicateDocumentException"/>类型的实例
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="innerException">内部异常</param>
        public DuplicateDocumentException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}