using System;
using System.Collections.Generic;
using System.Text;
using Bing.Data.Queries;
using Nest;

namespace Bing.Elasticsearch.Model
{
    public class Search<TResult> where TResult : class
    {
        /// <summary>
        /// 查询参数
        /// </summary>
        private readonly IQueryParameter _queryParam;

        /// <summary>
        /// 索引
        /// </summary>
        private string _index;

        /// <summary>
        /// 分页大小
        /// </summary>
        private int? _size;

        /// <summary>
        /// 起始行数
        /// </summary>
        private int? _from;

        /// <summary>
        /// 排序列表
        /// </summary>
        private readonly List<ISort> _sorts;

        /// <summary>
        /// 包含字段集合
        /// </summary>
        private readonly List<Field> _includeFields;

        /// <summary>
        /// 排除字段集合
        /// </summary>
        private readonly List<Field> _excludeFields;

        /// <summary>
        /// 折叠字段
        /// </summary>
        private Field _collapseField;

        /// <summary>
        /// 搜索条件
        /// </summary>
        private readonly Data.Queries.Query<TResult> _query;

        public Search<TResult> Index(string index)
        {
        }
    }
}
