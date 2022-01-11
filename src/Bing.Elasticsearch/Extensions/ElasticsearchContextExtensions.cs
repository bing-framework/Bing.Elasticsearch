using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Bing.Elasticsearch.Internals;
using Bing.Extensions;
using Nest;

// ReSharper disable once CheckNamespace
namespace Bing.Elasticsearch
{
    /// <summary>
    /// ES上下文扩展
    /// </summary>
    public static partial class ElasticsearchContextExtensions
    {
        /// <summary>
        /// 查询。单一条件查询，一般是精确查询
        /// </summary>
        /// <typeparam name="TDocument">文档类型</typeparam>
        /// <param name="context">ES上下恩</param>
        /// <param name="field">字段名</param>
        /// <param name="value">值</param>
        /// <param name="index">索引名称。注意：必须小写</param>
        /// <param name="cancellationToken">取消令牌</param>
        public static async Task<IEnumerable<TDocument>> SearchAsync<TDocument>(this IElasticsearchContext context, string field, object value, string index = null, CancellationToken cancellationToken = default) 
            where TDocument : class
        {
            if (field.IsEmpty())
                throw new ArgumentNullException(nameof(field));
            index = context.GetIndexName(Helper.SafeIndexName<TDocument>(index));
            var descriptor = new SearchDescriptor<TDocument>();
            descriptor.Index(index)
                .PostFilter(f => f.Term(x => x.Field(field).Value(value)));
            Func<SearchDescriptor<TDocument>, ISearchRequest> selector = x => descriptor;
            var response = await context.SearchAsync(selector, cancellationToken);
            return response.Documents;
        }
    }
}
