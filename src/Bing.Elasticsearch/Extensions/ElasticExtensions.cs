using System.Linq;
using System.Text;
using Elasticsearch.Net;
using Nest;

namespace Bing.Elasticsearch
{
    /// <summary>
    /// Elastic 相关扩展
    /// </summary>
    public static class ElasticExtensions
    {
        /// <summary>
        /// 获取错误消息
        /// </summary>
        /// <param name="elasticResponse">es响应内容</param>
        /// <param name="message">消息</param>
        /// <param name="normalize">是否标准化输出</param>
        /// <param name="includeResponse">是否包含响应内容</param>
        /// <param name="includeDebugInformation">是否包含调试信息</param>
        /// <returns>处理后的错误消息</returns>
        public static string GetErrorMessage(this IElasticsearchResponse elasticResponse, string message = null, bool normalize = false, bool includeResponse = false, bool includeDebugInformation = false)
        {
            if (elasticResponse == null)
                return string.Empty;

            var sb = new StringBuilder();
            if (string.IsNullOrEmpty(message))
                sb.AppendLine(message);

            var response = elasticResponse as IResponse;
            if (includeDebugInformation && response?.DebugInformation != null)
                sb.AppendLine(response.DebugInformation);

            if (response?.OriginalException != null)
                sb.AppendLine($"Original: [{response.OriginalException.GetType().Name}] {response.OriginalException.Message}");

            if (response?.ServerError?.Error != null)
                sb.AppendLine($"Server Error (Index={response.ServerError.Error?.Index}): {response.ServerError.Error.Reason}");

            if (elasticResponse is BulkResponse bulkResponse)
                sb.AppendLine($"Bulk: {string.Join("\r\n", bulkResponse.ItemsWithErrors.Select(i => i.Error))}");

            if (elasticResponse.ApiCall != null)
                sb.AppendLine($"[{elasticResponse.ApiCall.HttpStatusCode}] {elasticResponse.ApiCall.HttpMethod} {elasticResponse.ApiCall.Uri?.PathAndQuery}");

            if (elasticResponse.ApiCall?.RequestBodyInBytes != null)
            {
                var body = Encoding.UTF8.GetString(elasticResponse.ApiCall?.RequestBodyInBytes);
                if (normalize)
                    body = JsonUtility.Normalize(body);
                sb.AppendLine(body);
            }

            var apiCall = response.ApiCall;
            if (includeResponse && apiCall.ResponseBodyInBytes != null && apiCall.ResponseBodyInBytes.Length > 0 && apiCall.ResponseBodyInBytes.Length < 20000)
            {
                var body = Encoding.UTF8.GetString(apiCall?.ResponseBodyInBytes);
                if (normalize)
                    body = JsonUtility.Normalize(body);
                if (string.IsNullOrWhiteSpace(body))
                {
                    sb.AppendLine("##### Response #####");
                    sb.AppendLine(body);
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// 获取请求
        /// </summary>
        /// <param name="elasticResponse">es响应内容</param>
        /// <param name="normalize">是否标准化输出</param>
        /// <param name="includeResponse">是否包含响应内容</param>
        /// <param name="includeDebugInformation">是否包含调试信息</param>
        /// <returns>处理后请求内容</returns>
        public static string GetRequest(this IElasticsearchResponse elasticResponse, bool normalize = false, bool includeResponse = false, bool includeDebugInformation = false)
        {
            return GetErrorMessage(elasticResponse, null, normalize, includeResponse, includeDebugInformation);
        }
    }
}