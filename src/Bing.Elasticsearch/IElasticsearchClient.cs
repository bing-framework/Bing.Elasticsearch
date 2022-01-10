using System.Collections.Generic;
using System.Threading.Tasks;
using Bing.Elasticsearch.Model;

namespace Bing.Elasticsearch
{
    /// <summary>
    /// ES客户端
    /// </summary>
    public interface IElasticsearchClient
    {
        /// <summary>
        /// 查询。单一条件查询，一般是精确查询
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="indexName">索引名</param>
        /// <param name="field">字段名</param>
        /// <param name="value">查询值</param>
        Task<IEnumerable<T>> QueryAsync<T>(string indexName, string field, object value) where T : class;

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="param">分页参数</param>
        /// <param name="indexName">索引名</param>
        Task<IQueryResult<T>> PageQueryAsync<T>(IPageParam param, string indexName) where T : class;
    }
}
