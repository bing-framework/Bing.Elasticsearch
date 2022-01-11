//using System;
//using System.Threading.Tasks;
//using Bing.Elasticsearch.Model;
//using Nest;

//namespace Bing.Elasticsearch
//{
//    /// <summary>
//    /// ES客户端
//    /// </summary>
//    public class ElasticsearchClient : IElasticsearchClient
//    {
//        /// <summary>
//        /// 分页查询
//        /// </summary>
//        /// <typeparam name="T">实体类型</typeparam>
//        /// <param name="param">分页参数</param>
//        /// <param name="indexName">索引名</param>
//        /// <returns></returns>
//        public async Task<IQueryResult<T>> PageQueryAsync<T>(IPageParam param, string indexName) where T : class
//        {
//            if (param == null)
//            {
//                param = new PageParam
//                {
//                    Page = 1,
//                    PageSize = 20
//                };
//            }

//            var searchRequest = new SearchDescriptor<T>()
//                .Index(indexName)
//                .From(param.GetSkipCount())
//                .Size(param.PageSize);
//            if (param is PageParamWithSearch pageSearch)
//            {
//                ConfigPageRequest(pageSearch, ref searchRequest);
//            }
//            else if (param is PageParam pageParam)
//            {
//                ConfigPageRequest(pageParam, ref searchRequest);
//            }

//            // 是否需要高亮
//            bool hasHighlight = param.Highlight?.Keys?.Length > 0;
//            if (hasHighlight)
//            {
//                BuildHighLightQuery(param, ref searchRequest);
//            }

//            var client = await _builder.GetClientAsync();
//            var response = await client.SearchAsync<T>(x => searchRequest);
//            //if (hasHighlight)
//            //{
//            //    var listWithHightlight = new List<T>();
//            //    response.Hits.ToList().ForEach(x =>
//            //    {
//            //        if (x.Highlights?.Count > 0)
//            //        {
//            //            PropertyInfo[] properties = typeof(T).GetProperties();
//            //            foreach (string key in pageParams.Highlight?.Keys)
//            //            {
//            //                //先得到要替换的内容
//            //                if (x.Highlights.ContainsKey(key))
//            //                {
//            //                    string value = string.Join("", x.Highlights[key]?.Highlights);
//            //                    PropertyInfo info = properties.FirstOrDefault(p => p.Name == pageParams.Highlight.PrefixOfKey + key);
//            //                    //没找到带前缀的属性，则替换之前的
//            //                    if (info == null && pageParams.Highlight.ReplaceAuto)
//            //                    {
//            //                        info = properties.FirstOrDefault(p => p.Name == key);
//            //                    }
//            //                    if (info?.CanWrite == true)
//            //                    {
//            //                        if (!string.IsNullOrEmpty(value))
//            //                        {
//            //                            //如果高亮字段不为空，才赋值，否则就赋值成空
//            //                            info.SetValue(x.Source, value);
//            //                        }
//            //                    }
//            //                }
//            //            }
//            //        }
//            //        listWithHightlight.Add(x.Source);
//            //    });
//            //}
//            return new CustomQueryResult<T>()
//            {
//                Data = response.Documents,
//                Took = response.Took,
//                TotalCount = response.Total
//            };
//        }

//        /// <summary>
//        /// 配置指定字段的分页请求
//        /// </summary>
//        private void ConfigPageRequest<T>(PageParamWithSearch param, ref SearchDescriptor<T> searchRequest) where T : class
//        {
//            searchRequest = searchRequest.Query(q =>
//                q.QueryString(qs =>
//                    qs.Fields(param.SearchKeys)
//                        .Query(param.Keyword)
//                        .DefaultOperator(param.Operator)));
//        }

//        /// <summary>
//        /// 配置分页请求
//        /// </summary>
//        private void ConfigPageRequest<T>(PageParam param, ref SearchDescriptor<T> searchRequest) where T : class
//        {
//            searchRequest = searchRequest.Query(q =>
//                q.QueryString(qs =>
//                    qs.Query(param.Keyword)
//                        .DefaultOperator(param.Operator)));
//        }

//        /// <summary>
//        /// 构造高亮查询
//        /// </summary>
//        private void BuildHighLightQuery<T>(IPageParam param, ref SearchDescriptor<T> searchRequest) where T : class
//        {
//            var keysLength = param.Highlight?.Keys?.Length ?? 0;
//            var fieldDescriptor = new Func<HighlightFieldDescriptor<T>, IHighlightField>[keysLength];
//            var keysIndex = 0;
//            foreach (var key in param.Highlight?.Keys)
//            {
//                fieldDescriptor[keysIndex] = hf => hf.Field(key)
//                    .HighlightQuery(q => q.Match(m => m.Field(key).Query(param.Keyword)));
//                keysIndex++;
//            }

//            IHighlight highlight = new HighlightDescriptor<T>()
//                .PreTags(param.Highlight.PreTags)
//                .PostTags(param.Highlight.PostTags)
//                .Fields(fieldDescriptor);
//            searchRequest = searchRequest.Highlight(s => highlight);
//        }
//    }
//}
