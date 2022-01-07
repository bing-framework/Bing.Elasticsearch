using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Nest;

namespace Bing.Elasticsearch
{
    /// <summary>
    /// ES上下文
    /// </summary>
    public interface IElasticsearchContext
    {
        #region Index(索引)

        /// <summary>
        /// 是否存在指定索引别名
        /// </summary>
        /// <param name="alias">别名名称。注意：必须小写</param>
        Task<bool> AliasExistsAsync(string alias);

        /// <summary>
        /// 获取索引名称列表
        /// </summary>
        /// <param name="alias">别名名称。注意：必须小写</param>
        Task<List<string>> GetIndexesByAliasAsync(string alias);

        /// <summary>
        /// 创建索引
        /// </summary>
        /// <param name="index">索引名称。注意：必须小写</param>
        /// <param name="alias">别名名称。注意：必须小写</param>
        /// <param name="selector">映射操作</param>
        Task<CreateIndexResponse> CreateIndexAsync(string index, string alias = null, Func<CreateIndexDescriptor, ICreateIndexRequest> selector = null);

        /// <summary>
        /// 删除索引
        /// </summary>
        /// <typeparam name="TDocument">文档类型</typeparam>
        Task<DeleteIndexResponse> DeleteIndexAsync<TDocument>() where TDocument : class;

        /// <summary>
        /// 删除索引
        /// </summary>
        /// <param name="index">索引名称。注意：必须小写</param>
        Task<DeleteIndexResponse> DeleteIndexAsync(string index);

        /// <summary>
        /// 通过别名删除索引集合
        /// </summary>
        /// <param name="alias">别名名称。注意：必须小写</param>
        Task DeleteIndexesByAliasAsync(string alias);

        /// <summary>
        /// 删除全部索引
        /// </summary>
        Task<DeleteIndexResponse> DeleteAllIndexAsync();

        /// <summary>
        /// 添加索引列表到别名
        /// </summary>
        /// <param name="alias">别名名称。注意：必须小写</param>
        /// <param name="indexes">索引名称列表。注意：必须小写</param>
        Task AddIndexesToAliasAsync(string alias, params string[] indexes);

        /// <summary>
        /// 从别名中移除索引列表
        /// </summary>
        /// <param name="alias">别名名称。注意：必须小写</param>
        /// <param name="indexes">索引名称列表。注意：必须小写</param>
        Task RemoveIndexesFromAliasAsync(string alias, params string[] indexes);

        #endregion

        #region Search(查询)

        /// <summary>
        /// 获取全部数据。
        /// </summary>
        /// <typeparam name="TResult">结果类型</typeparam>
        /// <param name="index">索引名称。注意：必须小写</param>
        /// <remarks>说明：最多返回10000条</remarks>
        Task<List<TResult>> GetAllAsync<TResult>(string index = null) where TResult : class;

        #endregion

        #region Client(客户端)

        /// <summary>
        /// 获取客户端
        /// </summary>
        IElasticClient GetClient();

        /// <summary>
        /// 获取客户端
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        IElasticClient GetClient<T>();

        #endregion
    }
}
