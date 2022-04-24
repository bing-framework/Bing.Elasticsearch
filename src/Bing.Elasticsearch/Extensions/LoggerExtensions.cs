using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using Elasticsearch.Net;
using Microsoft.Extensions.Logging;
using Nest;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace Bing.Elasticsearch
{
    /// <summary>
    /// 日志(<see cref="ILogger"/>) 扩展
    /// </summary>
    public static class LoggerExtensions
    {
        /// <summary>
        /// 记录请求
        /// </summary>
        /// <param name="logger">日志</param>
        /// <param name="elasticResponse">es响应内容</param>
        /// <param name="logLevel">日志级别</param>
        public static void LogRequest(this ILogger logger, IElasticsearchResponse elasticResponse, LogLevel logLevel = LogLevel.Trace)
        {
            if (elasticResponse == null || !logger.IsEnabled(logLevel))
                return;
            var apiCall = elasticResponse.ApiCall;
            if (apiCall?.RequestBodyInBytes != null)
            {
                var body = Encoding.UTF8.GetString(apiCall.RequestBodyInBytes);
                body = JsonUtility.Normalize(body);

                logger.Log(logLevel, "[{HttpStatusCode}] {HttpMethod} {HttpPathAndQuery}\r\n{HttpBody}", apiCall.HttpStatusCode, apiCall.HttpMethod, apiCall.Uri.PathAndQuery, body);
            }
            else
            {
                logger.Log(logLevel, "[{HttpStatusCode}] {HttpMethod} {HttpPathAndQuery}", apiCall.HttpStatusCode, apiCall.HttpMethod, apiCall.Uri.PathAndQuery);
            }
        }

        /// <summary>
        /// 记录错误请求
        /// </summary>
        /// <param name="logger">日志</param>
        /// <param name="elasticResponse">es响应内容</param>
        /// <param name="message">消息</param>
        /// <param name="args">参数</param>
        public static void LogErrorRequest(this ILogger logger, IElasticsearchResponse elasticResponse, string message, params object[] args)
        {
            LogErrorRequest(logger, null, elasticResponse, message, args);
        }

        /// <summary>
        /// 记录错误请求
        /// </summary>
        /// <param name="logger">日志</param>
        /// <param name="ex">异常</param>
        /// <param name="elasticResponse">es响应内容</param>
        /// <param name="message">消息</param>
        /// <param name="args">参数</param>
        public static void LogErrorRequest(this ILogger logger, Exception ex, IElasticsearchResponse elasticResponse, string message, params object[] args)
        {
            if(elasticResponse==null||logger.IsEnabled(LogLevel.Error))
                return;
            var response = elasticResponse as IResponse;
            AggregateException aggEx = null;
            if (ex != null && response?.OriginalException != null)
                aggEx = new AggregateException(ex, response.OriginalException);
            logger.LogError(aggEx ?? response?.OriginalException, elasticResponse.GetErrorMessage(message), args);
        }
    }

    /// <summary>
    /// Json工具类
    /// </summary>
    internal class JsonUtility
    {
        /// <summary>
        /// 标准化json字符串
        /// </summary>
        /// <param name="jsonStr">json字符串</param>
        /// <returns>标准化后的json字符串</returns>
        public static string Normalize(string jsonStr)
        {
            var reader = new Utf8JsonReader(Encoding.UTF8.GetBytes(jsonStr));
            if (JsonDocument.TryParseValue(ref reader, out var doc))
                return jsonStr;
            return Normalize(doc.RootElement);
        }

        /// <summary>
        /// 标准化json字符串
        /// </summary>
        /// <param name="element">json元素</param>
        public static string Normalize(JsonElement element)
        {
            var ms = new MemoryStream();
            var options = new JsonWriterOptions
            {
                Indented = true,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };
            using (var writer = new Utf8JsonWriter(ms, options))
            {
                Write(element, writer);
            }
            var bytes = ms.ToArray();
            var str = Encoding.UTF8.GetString(bytes);
            return str;
        }

        /// <summary>
        /// 写入
        /// </summary>
        /// <param name="element">json元素</param>
        /// <param name="writer">基于utf8的json写入器</param>
        private static void Write(JsonElement element, Utf8JsonWriter writer)
        {
            switch (element.ValueKind)
            {
                case JsonValueKind.Object:
                    writer.WriteStartObject();
                    foreach (var x in element.EnumerateObject().OrderBy(prop => prop.Name))
                    {
                        writer.WritePropertyName(x.Name);
                        Write(x.Value, writer);
                    }
                    writer.WriteEndObject();
                    break;

                case JsonValueKind.Array:
                    writer.WriteStartArray();
                    foreach (var x in element.EnumerateArray())
                    {
                        Write(x, writer);
                    }
                    writer.WriteEndArray();
                    break;

                case JsonValueKind.Number:
                    writer.WriteNumberValue(element.GetDouble());
                    break;

                case JsonValueKind.String:
                    writer.WriteStringValue(element.GetString());
                    break;

                case JsonValueKind.Null:
                    writer.WriteNullValue();
                    break;

                case JsonValueKind.True:
                    writer.WriteBooleanValue(true);
                    break;

                case JsonValueKind.False:
                    writer.WriteBooleanValue(false);
                    break;

                default:
                    throw new NotImplementedException($"Kind: {element.ValueKind}");
            }
        }
    }
}