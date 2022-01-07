using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bing.Elasticsearch.Configs;
using Bing.Elasticsearch.Model;
using Nest;

namespace Bing.Elasticsearch
{
    /// <summary>
    /// ES客户端
    /// </summary>
    public class ElasticsearchClient : IElasticsearchClient
    {
        /// <summary>
        /// ES客户端生成器
        /// </summary>
        private ElasticsearchClientBuilder _builder;

        /// <summary>
        /// 初始化一个<see cref="ElasticsearchClient"/>类型的实例
        /// </summary>
        /// <param name="configProvider">配置提供程序</param>
        public ElasticsearchClient(IElasticsearchConfigProvider configProvider)
        {
            _builder = new ElasticsearchClientBuilder(configProvider);
        }

        /// <summary>
        /// 是否存在指定索引
        /// </summary>
        /// <param name="indexName">索引名</param>
        /// <returns></returns>
        public async Task<bool> ExistsAsync(string indexName)
        {
            var client = await _builder.GetClientAsync();
            var result = await client.Indices.ExistsAsync(indexName);
            return result.Exists;
        }

        /// <summary>
        /// 添加索引。不映射
        /// </summary>
        /// <param name="indexName">索引名</param>
        public async Task AddAsync(string indexName)
        {
            var client = await _builder.GetClientAsync();
            if (await ExistsAsync(indexName))
                return;
            await client.InitializeIndexMapAsync(indexName);
        }

        /// <summary>
        /// 添加索引。自动映射实体属性
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="indexName">索引名</param>
        public async Task AddAsync<T>(string indexName) where T : class
        {
            var client = await _builder.GetClientAsync();
            if (await ExistsAsync(indexName))
                return;
            await client.InitializeIndexMapAsync<T>(indexName);
        }

        /// <summary>
        /// 添加索引。自动映射实体属性并赋值
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="indexName">索引名</param>
        /// <param name="entity">实体</param>
        public async Task AddAsync<T>(string indexName, T entity) where T : class
        {
            var client = await _builder.GetClientAsync();
            if (!await ExistsAsync(indexName))
                await client.InitializeIndexMapAsync<T>(indexName);
            var response = await client.IndexAsync(entity, x => x.Index(indexName));
            if (!response.IsValid)
            {
                throw new ElasticsearchException($"新增数据[{indexName}]失败 : {response.ServerError.Error.Reason}");
            }
        }

        /// <summary>
        /// 更新索引。
        /// 由于是普通的简单更新，当ID已经存在时，则会更新文档，所以这里直接调用index方法（复杂方法待研究）
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="indexName">索引名</param>
        /// <param name="entity">实体</param>
        public async Task UpdateAsync<T>(string indexName, T entity) where T : class
        {
            await AddAsync<T>(indexName, entity);
        }

        /// <summary>
        /// 删除索引
        /// </summary>
        /// <param name="indexName">索引名</param>
        public async Task DeleteAsync(string indexName)
        {
            var client = await _builder.GetClientAsync();
            var response = await client.Indices.DeleteAsync(indexName);
            if (response.Acknowledged)
                return;
            throw new ElasticsearchException($"删除索引[{indexName}]失败 : {response.ServerError.Error.Reason}");
        }

        /// <summary>
        /// 删除索引
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="indexName">索引名</param>
        /// <param name="entity">实体</param>
        public async Task DeleteAsync<T>(string indexName, T entity) where T : class
        {
            var client = await _builder.GetClientAsync();
            var response = await client.DeleteAsync(new DeleteRequest(indexName, new Id(entity)));
            if (response.ServerError == null)
                return;
            throw new ElasticsearchException($"删除索引[{indexName}]失败 : {response.ServerError.Error.Reason}");
        }

        /// <summary>
        /// 删除索引
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="indexName">索引名</param>
        /// <param name="id">主键ID</param>
        public async Task DeleteAsync<T>(string indexName, long id) where T : class
        {
            var client = await _builder.GetClientAsync();
            var response = await client.DeleteAsync(DocumentPath<T>.Id(new Id(id)), x => x.Index(indexName));
            if (response.ServerError == null)
                return;
            throw new ElasticsearchException($"删除索引[{indexName}]失败 : {response.ServerError.Error.Reason}");
        }

        /// <summary>
        /// 查询实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="indexName">索引名</param>
        /// <param name="id">主键ID</param>
        /// <returns></returns>
        public async Task<T> FindAsync<T>(string indexName, long id) where T : class
        {
            var client = await _builder.GetClientAsync();
            var response = await client.GetAsync<T>(id, x => x.Index(indexName));
            return response?.Source;
        }

        /// <summary>
        /// 查询。单一条件查询，一般是精确查询
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="indexName">索引名</param>
        /// <param name="field">字段名</param>
        /// <param name="value">查询值</param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> QueryAsync<T>(string indexName, string field, object value) where T : class
        {
            if (string.IsNullOrWhiteSpace(field))
            {
                return null;
            }
            var client = await _builder.GetClientAsync();
            var searchRequest = new SearchDescriptor<T>()
                .Index(indexName)
                .PostFilter(f => f.Term(x => x.Field(field).Value(value)));
            var response = await client.SearchAsync<T>(searchRequest);
            return response.Documents;
        }

        /// <summary>
        /// 查找实体列表
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="indexName">索引名</param>
        /// <param name="ids">主键值</param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> FindByIdsAsync<T>(string indexName, params long[] ids) where T : class
        {
            var client = await _builder.GetClientAsync();
            var searchRequest = new SearchDescriptor<T>().Index(indexName).Query(q => q.Ids(x => x.Values(ids)));
            var response = await client.SearchAsync<T>(searchRequest);
            return response.Documents;
        }

        /// <summary>
        /// 查找实体列表
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="indexName">索引名</param>
        /// <param name="ids">主键值</param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> FindByIdsAsync<T>(string indexName, params string[] ids) where T : class
        {
            var client = await _builder.GetClientAsync();
            var searchRequest = new SearchDescriptor<T>().Index(indexName).Query(q => q.Ids(x => x.Values(ids)));
            var response = await client.SearchAsync<T>(searchRequest);
            return response.Documents;
        }

        /// <summary>
        /// 查找实体列表
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="indexName">索引名</param>
        /// <param name="ids">主键值</param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> FindByIdsAsync<T>(string indexName, params Guid[] ids) where T : class
        {
            var client = await _builder.GetClientAsync();
            var searchRequest = new SearchDescriptor<T>().Index(indexName).Query(q => q.Ids(x => x.Values(ids)));
            var response = await client.SearchAsync<T>(searchRequest);
            return response.Documents;
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="param">分页参数</param>
        /// <param name="indexName">索引名</param>
        /// <returns></returns>
        public async Task<IQueryResult<T>> PageQueryAsync<T>(IPageParam param, string indexName) where T : class
        {
            if (param == null)
            {
                param = new PageParam
                {
                    Page = 1,
                    PageSize = 20
                };
            }

            var searchRequest = new SearchDescriptor<T>()
                .Index(indexName)
                .From(param.GetSkipCount())
                .Size(param.PageSize);
            if (param is PageParamWithSearch pageSearch)
            {
                ConfigPageRequest(pageSearch, ref searchRequest);
            }
            else if (param is PageParam pageParam)
            {
                ConfigPageRequest(pageParam, ref searchRequest);
            }

            // 是否需要高亮
            bool hasHighlight = param.Highlight?.Keys?.Length > 0;
            if (hasHighlight)
            {
                BuildHighLightQuery(param, ref searchRequest);
            }

            var client = await _builder.GetClientAsync();
            var response = await client.SearchAsync<T>(x => searchRequest);
            //if (hasHighlight)
            //{
            //    var listWithHightlight = new List<T>();
            //    response.Hits.ToList().ForEach(x =>
            //    {
            //        if (x.Highlights?.Count > 0)
            //        {
            //            PropertyInfo[] properties = typeof(T).GetProperties();
            //            foreach (string key in pageParams.Highlight?.Keys)
            //            {
            //                //先得到要替换的内容
            //                if (x.Highlights.ContainsKey(key))
            //                {
            //                    string value = string.Join("", x.Highlights[key]?.Highlights);
            //                    PropertyInfo info = properties.FirstOrDefault(p => p.Name == pageParams.Highlight.PrefixOfKey + key);
            //                    //没找到带前缀的属性，则替换之前的
            //                    if (info == null && pageParams.Highlight.ReplaceAuto)
            //                    {
            //                        info = properties.FirstOrDefault(p => p.Name == key);
            //                    }
            //                    if (info?.CanWrite == true)
            //                    {
            //                        if (!string.IsNullOrEmpty(value))
            //                        {
            //                            //如果高亮字段不为空，才赋值，否则就赋值成空
            //                            info.SetValue(x.Source, value);
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //        listWithHightlight.Add(x.Source);
            //    });
            //}
            return new CustomQueryResult<T>()
            {
                Data = response.Documents,
                Took = response.Took,
                TotalCount = response.Total
            };
        }

        /// <summary>
        /// 配置指定字段的分页请求
        /// </summary>
        private void ConfigPageRequest<T>(PageParamWithSearch param, ref SearchDescriptor<T> searchRequest) where T : class
        {
            searchRequest = searchRequest.Query(q =>
                q.QueryString(qs =>
                    qs.Fields(param.SearchKeys)
                        .Query(param.Keyword)
                        .DefaultOperator(param.Operator)));
        }

        /// <summary>
        /// 配置分页请求
        /// </summary>
        private void ConfigPageRequest<T>(PageParam param, ref SearchDescriptor<T> searchRequest) where T : class
        {
            searchRequest = searchRequest.Query(q =>
                q.QueryString(qs =>
                    qs.Query(param.Keyword)
                        .DefaultOperator(param.Operator)));
        }

        /// <summary>
        /// 构造高亮查询
        /// </summary>
        private void BuildHighLightQuery<T>(IPageParam param, ref SearchDescriptor<T> searchRequest) where T : class
        {
            var keysLength = param.Highlight?.Keys?.Length ?? 0;
            var fieldDescriptor = new Func<HighlightFieldDescriptor<T>, IHighlightField>[keysLength];
            var keysIndex = 0;
            foreach (var key in param.Highlight?.Keys)
            {
                fieldDescriptor[keysIndex] = hf => hf.Field(key)
                    .HighlightQuery(q => q.Match(m => m.Field(key).Query(param.Keyword)));
                keysIndex++;
            }

            IHighlight highlight = new HighlightDescriptor<T>()
                .PreTags(param.Highlight.PreTags)
                .PostTags(param.Highlight.PostTags)
                .Fields(fieldDescriptor);
            searchRequest = searchRequest.Highlight(s => highlight);
        }

        /// <summary>
        /// 批量保存
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="indexName">索引名</param>
        /// <param name="entities">实体列表</param>
        public async Task BulkSaveAsync<T>(string indexName, IEnumerable<T> entities) where T : class
        {
            var client = await _builder.GetClientAsync();

            if (!await ExistsAsync(indexName))
            {
                await client.InitializeIndexMapAsync<T>(indexName);
            }
            var bulk = new BulkRequest(indexName)
            {
                Operations = new List<IBulkOperation>()
            };
            foreach (var entity in entities)
            {
                bulk.Operations.Add(new BulkIndexOperation<T>(entity));
            }

            var response = await client.BulkAsync(bulk);
            if (response.Errors)
                throw new ElasticsearchException($"批量保存文档在索引 {indexName} 失败：{response.ServerError.Error.Reason}");
        }
    }
}
