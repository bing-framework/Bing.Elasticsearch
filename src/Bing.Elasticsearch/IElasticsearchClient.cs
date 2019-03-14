using System;
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
        /// 是否存在指定索引
        /// </summary>
        /// <param name="indexName">索引名</param>
        /// <returns></returns>
        Task<bool> ExistsAsync(string indexName);

        /// <summary>
        /// 添加索引。不映射
        /// </summary>
        /// <param name="indexName">索引名</param>
        Task AddAsync(string indexName);

        /// <summary>
        /// 添加索引。自动映射实体属性
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="indexName">索引名</param>
        Task AddAsync<T>(string indexName) where T : class;

        /// <summary>
        /// 添加索引。自动映射实体属性并赋值
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="indexName">索引名</param>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        Task AddAsync<T>(string indexName, T entity) where T : class;

        /// <summary>
        /// 更新索引。
        /// 由于是普通的简单更新，当ID已经存在时，则会更新文档，所以这里直接调用index方法（复杂方法待研究）
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="indexName">索引名</param>
        /// <param name="entity">实体</param>
        Task UpdateAsync<T>(string indexName, T entity) where T : class;

        /// <summary>
        /// 删除索引
        /// </summary>
        /// <param name="indexName">索引名</param>
        Task DeleteAsync(string indexName);

        /// <summary>
        /// 删除索引
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="indexName">索引名</param>
        /// <param name="entity">实体</param>
        Task DeleteAsync<T>(string indexName, T entity) where T : class;

        /// <summary>
        /// 删除索引
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="indexName">索引名</param>
        /// <param name="id">主键ID</param>
        Task DeleteAsync<T>(string indexName, long id) where T : class;

        /// <summary>
        /// 查询实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="indexName">索引名</param>
        /// <param name="id">主键ID</param>
        /// <returns></returns>
        Task<T> FindAsync<T>(string indexName, long id) where T : class;

        /// <summary>
        /// 查询。单一条件查询，一般是精确查询
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="indexName">索引名</param>
        /// <param name="field">字段名</param>
        /// <param name="value">查询值</param>
        /// <returns></returns>
        Task<IEnumerable<T>> QueryAsync<T>(string indexName, string field, object value) where T : class;

        /// <summary>
        /// 查找实体列表
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="indexName">索引名</param>
        /// <param name="ids">主键值</param>
        /// <returns></returns>
        Task<IEnumerable<T>> FindByIdsAsync<T>(string indexName, params long[] ids) where T : class;

        /// <summary>
        /// 查找实体列表
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="indexName">索引名</param>
        /// <param name="ids">主键值</param>
        /// <returns></returns>
        Task<IEnumerable<T>> FindByIdsAsync<T>(string indexName, params string[] ids) where T : class;

        /// <summary>
        /// 查找实体列表
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="indexName">索引名</param>
        /// <param name="ids">主键值</param>
        /// <returns></returns>
        Task<IEnumerable<T>> FindByIdsAsync<T>(string indexName, params Guid[] ids) where T : class;

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="param">分页参数</param>
        /// <param name="indexName">索引名</param>
        /// <returns></returns>
        Task<IQueryResult<T>> PageQueryAsync<T>(IPageParam param, string indexName) where T : class;

        /// <summary>
        /// 批量保存
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="indexName">索引名</param>
        /// <param name="entities">实体列表</param>
        Task BulkSaveAsync<T>(string indexName, IEnumerable<T> entities) where T : class;
    }
}
