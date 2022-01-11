using System;
using System.Collections.Generic;
using System.Threading;
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
        /// <param name="cancellationToken">取消令牌</param>
        Task<bool> AliasExistsAsync(string alias, CancellationToken cancellationToken = default);

        /// <summary>
        /// 是否存在指定索引名称
        /// </summary>
        /// <param name="index">索引名称。注意：必须小写</param>
        /// <param name="cancellation">取消令牌</param>
        Task<bool> IndexExistsAsync(string index, CancellationToken cancellation = default);

        /// <summary>
        /// 获取索引名称列表
        /// </summary>
        /// <param name="alias">别名名称。注意：必须小写</param>
        /// <param name="cancellationToken">取消令牌</param>
        Task<List<string>> GetIndexesByAliasAsync(string alias, CancellationToken cancellationToken = default);

        /// <summary>
        /// 创建索引
        /// </summary>
        /// <param name="index">索引名称。注意：必须小写</param>
        /// <param name="alias">别名名称。注意：必须小写</param>
        /// <param name="selector">映射操作</param>
        /// <param name="cancellationToken">取消令牌</param>
        Task<CreateIndexResponse> CreateIndexAsync(string index, string alias = null, Func<CreateIndexDescriptor, ICreateIndexRequest> selector = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// 创建索引
        /// </summary>
        /// <typeparam name="TDocument">文档类型</typeparam>
        /// <param name="index">索引名称。注意：必须小写</param>
        /// <param name="alias">别名名称。注意：必须小写</param>
        /// <param name="cancellationToken">取消令牌</param>
        Task CreateIndexAsync<TDocument>(string index, string alias = null, CancellationToken cancellationToken = default) where TDocument : class;

        /// <summary>
        /// 删除索引
        /// </summary>
        /// <typeparam name="TDocument">文档类型</typeparam>
        /// <param name="cancellationToken">取消令牌</param>
        Task<DeleteIndexResponse> DeleteIndexAsync<TDocument>(CancellationToken cancellationToken = default) where TDocument : class;

        /// <summary>
        /// 删除索引
        /// </summary>
        /// <param name="index">索引名称。注意：必须小写</param>
        /// <param name="cancellationToken">取消令牌</param>
        Task<DeleteIndexResponse> DeleteIndexAsync(string index, CancellationToken cancellationToken = default);

        /// <summary>
        /// 通过别名删除索引集合
        /// </summary>
        /// <param name="alias">别名名称。注意：必须小写</param>
        /// <param name="cancellationToken">取消令牌</param>
        Task DeleteIndexesByAliasAsync(string alias, CancellationToken cancellationToken = default);

        /// <summary>
        /// 删除全部索引
        /// </summary>
        /// <param name="cancellationToken">取消令牌</param>
        Task<DeleteIndexResponse> DeleteAllIndexAsync(CancellationToken cancellationToken = default);

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
        /// <typeparam name="TDocument">文档类型</typeparam>
        /// <param name="index">索引名称。注意：必须小写</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <remarks>说明：最多返回10000条</remarks>
        Task<List<TDocument>> GetAllAsync<TDocument>(string index = null, CancellationToken cancellationToken = default)
            where TDocument : class;

        /// <summary>
        /// 通过标识查找
        /// </summary>
        /// <typeparam name="TDocument">文档类型</typeparam>
        /// <param name="id">文档标识</param>
        /// <param name="index">索引名称。注意：必须小写</param>
        /// <param name="cancellationToken">取消令牌</param>
        Task<TDocument> FindByIdAsync<TDocument>(object id, string index = null, CancellationToken cancellationToken = default)
            where TDocument : class;

        /// <summary>
        /// 通过标识集合查找
        /// </summary>
        /// <typeparam name="TDocument">文档类型</typeparam>
        /// <param name="ids">文档标识集合</param>
        /// <param name="index">索引名称。注意：必须小写</param>
        /// <param name="cancellationToken">取消令牌</param>
        Task<IEnumerable<TDocument>> FindByIdsAsync<TDocument>(IEnumerable<string> ids, string index = null,
            CancellationToken cancellationToken = default) where TDocument : class;

        /// <summary>
        /// 通过标识集合查找
        /// </summary>
        /// <typeparam name="TDocument">文档类型</typeparam>
        /// <param name="ids">文档标识集合</param>
        /// <param name="index">索引名称。注意：必须小写</param>
        /// <param name="cancellationToken">取消令牌</param>
        Task<IEnumerable<TDocument>> FindByIdsAsync<TDocument>(IEnumerable<long> ids, string index = null,
            CancellationToken cancellationToken = default) where TDocument : class;

        /// <summary>
        /// 通过标识集合查找
        /// </summary>
        /// <typeparam name="TDocument">文档类型</typeparam>
        /// <param name="index">索引名称。注意：必须小写</param>
        /// <param name="ids">文档标识集合</param>
        Task<IEnumerable<TDocument>> FindByIdsAsync<TDocument>(string index, params string[] ids) where TDocument : class;

        /// <summary>
        /// 通过标识集合查找
        /// </summary>
        /// <typeparam name="TDocument">文档类型</typeparam>
        /// <param name="index">索引名称。注意：必须小写</param>
        /// <param name="ids">文档标识集合</param>
        Task<IEnumerable<TDocument>> FindByIdsAsync<TDocument>(string index, params long[] ids) where TDocument : class;

        /// <summary>
        /// 查询
        /// </summary>
        /// <typeparam name="TDocument">文档类型</typeparam>
        /// <param name="searchTerms">查询条件</param>
        /// <param name="index">索引名称。注意：必须小写</param>
        /// <param name="cancellationToken">取消令牌</param>
        Task<ISearchResponse<TDocument>> SearchAsync<TDocument>(Func<QueryContainerDescriptor<TDocument>, QueryContainer> searchTerms = null, string index = null, CancellationToken cancellationToken = default)
            where TDocument : class;

        /// <summary>
        /// 查询
        /// </summary>
        /// <typeparam name="TDocument">文档类型</typeparam>
        /// <param name="selector">查询表达式</param>
        /// <param name="cancellationToken">取消令牌</param>
        Task<ISearchResponse<TDocument>> SearchAsync<TDocument>(Func<SearchDescriptor<TDocument>, ISearchRequest> selector = null, CancellationToken cancellationToken = default)
            where TDocument : class;

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

        #region Document(文档操作)

        /// <summary>
        /// 添加文档
        /// </summary>
        /// <typeparam name="TDocument">文档类型</typeparam>
        /// <param name="document">文档对象</param>
        /// <param name="index">索引名称。注意：必须小写</param>
        /// <param name="id">文档标识</param>
        /// <param name="cancellationToken">取消令牌</param>
        Task<IndexResponse> AddAsync<TDocument>(TDocument document, string index = null, object id = null, CancellationToken cancellationToken = default)
            where TDocument : class;

        /// <summary>
        /// 批量保存文档
        /// </summary>
        /// <typeparam name="TDocument">文档类型</typeparam>
        /// <param name="documents">文档对象列表</param>
        /// <param name="index">索引名称。注意：必须小写</param>
        /// <param name="timeout">超时时间间隔。单位：毫秒，默认值：300000，即5分钟</param>
        /// <param name="cancellationToken">取消令牌</param>
        Task<BulkResponse> BulkSaveAsync<TDocument>(IEnumerable<TDocument> documents, string index = null,
            double timeout = 300000, CancellationToken cancellationToken = default) where TDocument : class;

        /// <summary>
        /// 删除文档
        /// </summary>
        /// <typeparam name="TDocument">文档类型</typeparam>
        /// <param name="id">文档标识</param>
        /// <param name="index">索引名称。注意：必须小写</param>
        /// <param name="cancellationToken">取消令牌</param>
        Task<DeleteResponse> DeleteAsync<TDocument>(object id, string index = null,
            CancellationToken cancellationToken = default) where TDocument : class;

        /// <summary>
        /// 删除文档
        /// </summary>
        /// <typeparam name="TDocument">文档类型</typeparam>
        /// <param name="document">文档</param>
        /// <param name="index">索引名称。注意：必须小写</param>
        /// <param name="cancellationToken">取消令牌</param>
        Task<DeleteResponse> DeleteAsync<TDocument>(TDocument document, string index = null,
            CancellationToken cancellationToken = default) where TDocument : class;

        /// <summary>
        /// 按查询条件删除文档
        /// </summary>
        /// <typeparam name="TDocument">文档类型</typeparam>
        /// <param name="selector">删除表达式</param>
        /// <param name="cancellationToken">取消令牌</param>
        Task<DeleteByQueryResponse> DeleteByQueryAsync<TDocument>(
            Func<DeleteByQueryDescriptor<TDocument>, IDeleteByQueryRequest> selector,
            CancellationToken cancellationToken = default) where TDocument : class;

        /// <summary>
        /// 更新文档
        /// </summary>
        /// <typeparam name="TDocument">文档类型</typeparam>
        /// <param name="document">文档</param>
        /// <param name="index">索引名称。注意：必须小写</param>
        /// <param name="cancellationToken">取消令牌</param>
        Task<UpdateResponse<TDocument>> UpdateAsync<TDocument>(TDocument document, string index = null,
            CancellationToken cancellationToken = default) where TDocument : class;

        /// <summary>
        /// 更新文档
        /// </summary>
        /// <typeparam name="TDocument">文档类型</typeparam>
        /// <param name="id">文档标识</param>
        /// <param name="document">文档</param>
        /// <param name="index">索引名称。注意：必须小写</param>
        /// <param name="cancellationToken">取消令牌</param>
        Task<UpdateResponse<TDocument>> UpdateAsync<TDocument>(object id, TDocument document, string index = null,
            CancellationToken cancellationToken = default) where TDocument : class;

        /// <summary>
        /// 按查询条件更新文档
        /// </summary>
        /// <typeparam name="TDocument">文档类型</typeparam>
        /// <param name="selector">更新表达式</param>
        /// <param name="cancellationToken">取消令牌</param>
        Task<UpdateByQueryResponse> UpdateByQueryAsync<TDocument>(Func<UpdateByQueryDescriptor<TDocument>, IUpdateByQueryRequest> selector,
            CancellationToken cancellationToken = default)
            where TDocument : class;

        /// <summary>
        /// 是否存在指定文档标识
        /// </summary>
        /// <typeparam name="TDocument">文档类型</typeparam>
        /// <param name="id">文档标识</param>
        /// <param name="index">索引名称。注意：必须小写</param>
        /// <param name="cancellationToken">取消令牌</param>
        Task<bool> ExistsAsync<TDocument>(object id, string index = null, CancellationToken cancellationToken = default)
            where TDocument : class;

        /// <summary>
        /// 获取文档计数
        /// </summary>
        /// <typeparam name="TDocument">文档类型</typeparam>
        /// <param name="index">索引名称。注意：必须小写</param>
        /// <param name="cancellationToken">取消令牌</param>
        Task<long> GetTotalCountAsync<TDocument>(string index = null, CancellationToken cancellationToken = default)
            where TDocument : class;

        #endregion

        #region 辅助操作

        /// <summary>
        /// 获取索引名称
        /// </summary>
        /// <typeparam name="TDocument">文档类型</typeparam>
        /// <param name="index">索引名称。注意：必须小写</param>
        string GetIndexName<TDocument>(string index = null);

        /// <summary>
        /// 获取索引名称
        /// </summary>
        /// <param name="index">索引名称。注意：必须小写</param>
        string GetIndexName(string index);

        #endregion
    }
}
