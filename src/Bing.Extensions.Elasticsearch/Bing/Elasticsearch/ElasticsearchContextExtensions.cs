using Bing.Data.Queries;
using Bing.Elasticsearch.Model;
using Bing.Extensions;

namespace Bing.Elasticsearch
{
    /// <summary>
    /// ES上下文扩展
    /// </summary>
    public static partial  class ElasticsearchContextExtensions
    {
        /// <summary>
        /// 搜索
        /// </summary>
        /// <typeparam name="TResult">结果类型</typeparam>
        /// <param name="context">服务</param>
        /// <param name="query">查询参数</param>
        public static EsSearch<TResult> Search<TResult>(this IElasticsearchContext context, IQueryParameter query) 
            where TResult : class
        {
            context.CheckNull(nameof(context));
            query.CheckNull(nameof(query));
            return new EsSearch<TResult>(context, query);
        }
    }
}
