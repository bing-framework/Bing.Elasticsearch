using System;
using System.Collections.Concurrent;
using Bing.Extensions;
using Nest;

namespace Bing.Elasticsearch.Internals
{
    /// <summary>
    /// 内部帮助类
    /// </summary>
    internal static class Helper
    {
        /// <summary>
        /// 索引缓存字典
        /// </summary>
        internal static readonly ConcurrentDictionary<Type, string> IndexCacheDict = new ConcurrentDictionary<Type, string>();

        /// <summary>
        /// 安全获取索引名称
        /// </summary>
        /// <typeparam name="TDocument">文档类型</typeparam>
        /// <param name="index">索引名称。注意：必须小写</param>
        public static string SafeIndexName<TDocument>(string index = null)
        {
            if (index.IsEmpty() == false)
                return index;
            var type = typeof(TDocument);
            if (!IndexCacheDict.ContainsKey(type))
            {
                var elasticsearchTypeAttribute = type.GetAttribute<ElasticsearchTypeAttribute>();
                if (elasticsearchTypeAttribute != null)
                    IndexCacheDict[type] = elasticsearchTypeAttribute.RelationName;
                else
                    IndexCacheDict[type] = type.Name.ToLower();
            }

            return IndexCacheDict[type];
        }

        /// <summary>
        /// 获取ES标识
        /// </summary>
        /// <param name="id">标识</param>
        public static Id GetEsId(object id)
        {
            Id entityId;
            switch (id)
            {
                case long longId:
                    entityId = new Id(longId);
                    break;
                case string stringId:
                    entityId = new Id(stringId);
                    break;
                default:
                    entityId = new Id(id);
                    break;
            }

            return entityId;
        }
    }
}
