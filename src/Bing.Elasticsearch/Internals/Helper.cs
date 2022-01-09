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
        /// 安全获取索引名称
        /// </summary>
        /// <typeparam name="TDocument">文档类型</typeparam>
        /// <param name="index">索引名称。注意：必须小写</param>
        public static string SafeIndexName<TDocument>(string index = null)
        {
            if (index.IsEmpty() == false)
                return index;
            return typeof(TDocument).Name.ToLower();
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
