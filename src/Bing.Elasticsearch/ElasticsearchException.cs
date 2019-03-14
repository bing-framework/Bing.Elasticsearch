using System;
using System.Runtime.Serialization;

namespace Bing.Elasticsearch
{
    /// <summary>
    /// Elasticsearch 异常
    /// </summary>
    [Serializable]
    public class ElasticsearchException: Exception
    {
        /// <summary>
        /// 初始化一个<see cref="ElasticsearchException"/>类型的实例
        /// </summary>
        public ElasticsearchException() { }

        /// <summary>
        /// 初始化一个<see cref="ElasticsearchException"/>类型的实例
        /// </summary>
        /// <param name="serializationInfo">序列号信息</param>
        /// <param name="context">流上下文</param>
        public ElasticsearchException(SerializationInfo serializationInfo, StreamingContext context) : base(
            serializationInfo, context)
        {
        }

        /// <summary>
        /// 初始化一个<see cref="ElasticsearchException"/>类型的实例
        /// </summary>
        /// <param name="message">错误消息</param>
        public ElasticsearchException(string message) : base(message) { }

        /// <summary>
        /// 初始化一个<see cref="ElasticsearchException"/>类型的实例
        /// </summary>
        /// <param name="message">错误消息</param>
        /// <param name="innerException">内部异常</param>
        public ElasticsearchException(string message, Exception innerException) : base(message, innerException) { }
    }
}
