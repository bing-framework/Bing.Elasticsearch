using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Bing.Data;
using Bing.Data.Queries;
using Bing.Extensions;
using Nest;

namespace Bing.Elasticsearch.Model
{
    /// <summary>
    /// ES查询模型
    /// </summary>
    /// <typeparam name="TResult">查询结果类型</typeparam>
    public class EsSearch<TResult> where TResult : class
    {
        /// <summary>
        /// ES上下文
        /// </summary>
        private readonly IElasticsearchContext _context;

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
        private readonly EsQuery<TResult> _query;

        /// <summary>
        /// 初始化一个<see cref="EsSearch{TResult}"/>类型的实例
        /// </summary>
        /// <param name="context">ES上下文</param>
        /// <param name="query">查询参数</param>
        public EsSearch(IElasticsearchContext context, IQueryParameter query)
        {
            context.CheckNull(nameof(context));
            query.CheckNull(nameof(query));
            _context = context;
            _queryParam = query;
            _sorts = new List<ISort>();
            _includeFields = new List<Field>();
            _excludeFields = new List<Field>();
            _query = new EsQuery<TResult>();
        }

        /// <summary>
        /// 设置索引名称或别名
        /// </summary>
        /// <param name="index">索引名称，也可以是别名</param>
        public EsSearch<TResult> Index(string index)
        {
            _index = index;
            return this;
        }

        /// <summary>
        /// 搜索全部索引
        /// </summary>
        public EsSearch<TResult> AllIndex()
        {
            _index = "_all";
            return this;
        }

        /// <summary>
        /// 获取当前查询对象
        /// </summary>
        public EsQuery<TResult> GetQuery() => _query;

        /// <summary>
        /// 创建新的查询对象
        /// </summary>
        public EsQuery<TResult> NewQuery() => new EsQuery<TResult>();

        /// <summary>
        /// 设置起始行数，从0开始
        /// </summary>
        /// <param name="from">起始行数</param>
        public EsSearch<TResult> From(int from)
        {
            _from = from;
            return this;
        }

        /// <summary>
        /// 设置分页大小
        /// </summary>
        /// <param name="size">分页大小</param>
        public EsSearch<TResult> Size(int size)
        {
            _size = size;
            return this;
        }

        /// <summary>
        /// 排序
        /// </summary>
        /// <typeparam name="TProperty">属性类型</typeparam>
        /// <param name="expression">排序字段表达式</param>
        /// <param name="desc">是否降序排序</param>
        public EsSearch<TResult> OrderBy<TProperty>(Expression<Func<TResult, TProperty>> expression, bool desc = false)
        {
            _sorts.Add(new FieldSort { Field = new Field(expression), Order = GetOrder(desc) });
            return this;
        }

        /// <summary>
        /// 排序
        /// </summary>
        /// <typeparam name="TProperty">属性类型</typeparam>
        /// <param name="expression">排序字段表达式</param>
        /// <param name="desc">是否降序排序</param>
        /// <param name="condition">该值为true时添加查询条件，否则忽略</param>
        public EsSearch<TResult> OrderByIf<TProperty>(Expression<Func<TResult, TProperty>> expression, bool desc, bool condition)
        {
            return condition ? OrderBy(expression, desc) : this;
        }

        /// <summary>
        /// 嵌套属性排序
        /// </summary>
        /// <typeparam name="TProperty">属性类型</typeparam>
        /// <param name="path">排序路径</param>
        /// <param name="expression">排序字段表达式</param>
        /// <param name="desc">是否降序排序</param>
        public EsSearch<TResult> NestOrderBy<TProperty>(Expression<Func<TResult, TProperty>> path, Expression<Func<TResult, TProperty>> expression, bool desc = false)
        {
            _sorts.Add(new FieldSort
            {
                Field = new Field(expression),
                Order = GetOrder(desc),
                Nested = new NestedSort
                {
                    Path = new Field(path),
                    Filter = new QueryContainer()
                }
            });
            return this;
        }

        /// <summary>
        /// 嵌套属性排序
        /// </summary>
        /// <typeparam name="TProperty">属性类型</typeparam>
        /// <param name="path">排序路径</param>
        /// <param name="expression">排序字段表达式</param>
        /// <param name="desc">是否降序排序</param>
        /// <param name="condition">该值为true时添加查询条件，否则忽略</param>
        public EsSearch<TResult> NestOrderByIf<TProperty>(Expression<Func<TResult, TProperty>> path, Expression<Func<TResult, TProperty>> expression, bool desc, bool condition)
        {
            return condition ? NestOrderBy(path, expression, desc) : this;
        }

        /// <summary>
        /// 获取排序方向
        /// </summary>
        /// <param name="desc">是否降序排序</param>
        private SortOrder GetOrder(bool desc) => desc ? SortOrder.Descending : SortOrder.Ascending;

        /// <summary>
        /// 设置包含字段列表
        /// </summary>
        /// <param name="fields">字段列表</param>
        public EsSearch<TResult> IncludeFields(string fields)
        {
            Fields fieldList = fields;
            _includeFields.AddRange(fieldList.ToArray());
            return this;
        }

        /// <summary>
        /// 设置包含字段
        /// </summary>
        /// <typeparam name="TProperty">属性类型</typeparam>
        /// <param name="expression">字段表达式</param>
        public EsSearch<TResult> IncludeField<TProperty>(Expression<Func<TResult, TProperty>> expression)
        {
            var field = new Field(expression);
            _includeFields.Add(field);
            return this;
        }

        /// <summary>
        /// 设置排除字段列表
        /// </summary>
        /// <param name="fields">字段列表</param>
        public EsSearch<TResult> ExcludeFields(string fields)
        {
            Fields fieldList = fields;
            _excludeFields.AddRange(fieldList.ToArray());
            return this;
        }

        /// <summary>
        /// 设置排除字段
        /// </summary>
        /// <typeparam name="TProperty">属性类型</typeparam>
        /// <param name="expression">字段表达式</param>
        public EsSearch<TResult> ExcludeField<TProperty>(Expression<Func<TResult, TProperty>> expression)
        {
            var field = new Field(expression);
            _excludeFields.Add(field);
            return this;
        }

        /// <summary>
        /// 设置折叠字段
        /// </summary>
        /// <typeparam name="TProperty">属性类型</typeparam>
        /// <param name="expression">字段表达式</param>
        public EsSearch<TResult> Collapse<TProperty>(Expression<Func<TResult, TProperty>> expression)
        {
            _collapseField = new Field(expression);
            return this;
        }

        /// <summary>
        /// 嵌套查询
        /// </summary>
        /// <typeparam name="TProperty">属性类型</typeparam>
        /// <param name="path">嵌套字段</param>
        /// <param name="condition">查询条件</param>
        public EsSearch<TResult> Nest<TProperty>(Expression<Func<TResult, TProperty>> path, IEsCondition condition)
        {
            _query.Nest(path, condition);
            return this;
        }

        /// <summary>
        /// 嵌套查询
        /// </summary>
        /// <typeparam name="TProperty">属性类型</typeparam>
        /// <param name="path">嵌套字段</param>
        /// <param name="condition">查询条件</param>
        public EsSearch<TResult> Nest<TProperty>(Expression<Func<TResult, TProperty>> path, QueryContainer condition)
        {
            _query.Nest(path, condition);
            return this;
        }

        /// <summary>
        /// 嵌套查询
        /// </summary>
        /// <typeparam name="TProperty">属性类型</typeparam>
        /// <param name="path">嵌套字段</param>
        /// <param name="action">查询操作</param>
        public EsSearch<TResult> Nest<TProperty>(Expression<Func<TResult, TProperty>> path, Action<EsQuery<TResult>> action)
        {
            _query.Nest(path, action);
            return this;
        }

        /// <summary>
        /// 精确匹配词条
        /// </summary>
        /// <typeparam name="TProperty">属性类型</typeparam>
        /// <param name="expression">字段表达式</param>
        /// <param name="value">值</param>
        public EsSearch<TResult> Term<TProperty>(Expression<Func<TResult, TProperty>> expression, object value)
        {
            _query.Term(expression, value);
            return this;
        }

        /// <summary>
        /// 精确匹配词条
        /// </summary>
        /// <typeparam name="TProperty">属性类型</typeparam>
        /// <param name="expression">字段表达式</param>
        /// <param name="value">值</param>
        /// <param name="condition">该值为true时添加查询条件，否则忽略</param>
        public EsSearch<TResult> TermIf<TProperty>(Expression<Func<TResult, TProperty>> expression, object value, bool condition)
        {
            _query.TermIf(expression, value, condition);
            return this;
        }

        /// <summary>
        /// 精确匹配词条，当值为空时忽略条件
        /// </summary>
        /// <typeparam name="TProperty">属性类型</typeparam>
        /// <param name="expression">字段表达式</param>
        /// <param name="value">值</param>
        public EsSearch<TResult> TermIfNotEmpty<TProperty>(Expression<Func<TResult, TProperty>> expression, object value)
        {
            _query.TermIfNotEmpty(expression, value);
            return this;
        }

        /// <summary>
        /// 精确匹配词条列表
        /// </summary>
        /// <typeparam name="TProperty">属性类型</typeparam>
        /// <param name="expression">字段表达式</param>
        /// <param name="values">值</param>
        public EsSearch<TResult> Terms<TProperty>(Expression<Func<TResult, TProperty>> expression, IEnumerable<object> values)
        {
            _query.Terms(expression, values);
            return this;
        }

        /// <summary>
        /// 精确匹配词条列表
        /// </summary>
        /// <typeparam name="TProperty">属性类型</typeparam>
        /// <param name="expression">字段表达式</param>
        /// <param name="values">值</param>
        /// <param name="condition">该值为true时添加查询条件，否则忽略</param>
        public EsSearch<TResult> TermsIf<TProperty>(Expression<Func<TResult, TProperty>> expression, IEnumerable<object> values, bool condition)
        {
            _query.TermsIf(expression, values, condition);
            return this;
        }

        /// <summary>
        /// 精确匹配词条列表，当值为空时忽略条件
        /// </summary>
        /// <typeparam name="TProperty">属性类型</typeparam>
        /// <param name="expression">字段表达式</param>
        /// <param name="values">值</param>
        public EsSearch<TResult> TermsIfNotEmpty<TProperty>(Expression<Func<TResult, TProperty>> expression, IEnumerable<object> values)
        {
            _query.TermsIfNotEmpty(expression, values);
            return this;
        }

        /// <summary>
        /// 匹配词条
        /// </summary>
        /// <typeparam name="TProperty">属性类型</typeparam>
        /// <param name="expression">字段表达式</param>
        /// <param name="value">值</param>
        public EsSearch<TResult> Match<TProperty>(Expression<Func<TResult, TProperty>> expression, string value)
        {
            _query.Match(expression, value);
            return this;
        }

        /// <summary>
        /// 匹配词条
        /// </summary>
        /// <typeparam name="TProperty">属性类型</typeparam>
        /// <param name="expression">字段表达式</param>
        /// <param name="value">值</param>
        /// <param name="condition">该值为true时添加查询条件，否则忽略</param>
        public EsSearch<TResult> MatchIf<TProperty>(Expression<Func<TResult, TProperty>> expression, string value, bool condition)
        {
            _query.MatchIf(expression, value, condition);
            return this;
        }

        /// <summary>
        /// 匹配词条，当值为空时忽略条件
        /// </summary>
        /// <typeparam name="TProperty">属性类型</typeparam>
        /// <param name="expression">字段表达式</param>
        /// <param name="value">值</param>
        public EsSearch<TResult> MatchIfNotEmpty<TProperty>(Expression<Func<TResult, TProperty>> expression, string value)
        {
            _query.MatchIfNotEmpty(expression, value);
            return this;
        }

        /// <summary>
        /// 多字段匹配词条
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="expressions">字段表达式列表</param>
        public EsSearch<TResult> MultiMatch(string value, params Expression<Func<TResult, object>>[] expressions)
        {
            _query.MultiMatch(value, expressions);
            return this;
        }

        /// <summary>
        /// 多字段匹配词条，当值为空时忽略条件
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="expressions">字段表达式列表</param>
        public EsSearch<TResult> MultiMatchIfNotEmpty(string value, params Expression<Func<TResult, object>>[] expressions)
        {
            _query.MultiMatchIfNotEmpty(value, expressions);
            return this;
        }

        /// <summary>
        /// 匹配范围
        /// </summary>
        /// <typeparam name="TProperty">属性类型</typeparam>
        /// <param name="propertyExpression">属性表达式。范围：t = > t.Age</param>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <param name="boundary">包含边界</param>
        public EsSearch<TResult> Between<TProperty>(Expression<Func<TResult, TProperty>> propertyExpression, int? min, int? max, Boundary boundary = Boundary.Both)
        {
            _query.Between(propertyExpression, min, max, boundary);
            return this;
        }

        /// <summary>
        /// 匹配范围
        /// </summary>
        /// <typeparam name="TProperty">属性类型</typeparam>
        /// <param name="propertyExpression">属性表达式。范围：t = > t.Age</param>
        /// <param name="condition">该值为true时添加查询条件，否则忽略</param>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <param name="boundary">包含边界</param>
        public EsSearch<TResult> BetweenIf<TProperty>(Expression<Func<TResult, TProperty>> propertyExpression, bool condition, int? min, int? max = null, Boundary boundary = Boundary.Both)
        {
            _query.BetweenIf(propertyExpression, condition, min, max, boundary);
            return this;
        }

        /// <summary>
        /// 匹配范围
        /// </summary>
        /// <typeparam name="TProperty">属性类型</typeparam>
        /// <param name="propertyExpression">属性表达式。范围：t = > t.Age</param>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <param name="boundary">包含边界</param>
        public EsSearch<TResult> Between<TProperty>(Expression<Func<TResult, TProperty>> propertyExpression, double? min, double? max, Boundary boundary = Boundary.Both)
        {
            _query.Between(propertyExpression, min, max, boundary);
            return this;
        }

        /// <summary>
        /// 匹配范围
        /// </summary>
        /// <typeparam name="TProperty">属性类型</typeparam>
        /// <param name="propertyExpression">属性表达式。范围：t = > t.Age</param>
        /// <param name="condition">该值为true时添加查询条件，否则忽略</param>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <param name="boundary">包含边界</param>
        public EsSearch<TResult> BetweenIf<TProperty>(Expression<Func<TResult, TProperty>> propertyExpression, bool condition, double? min, double? max = null, Boundary boundary = Boundary.Both)
        {
            _query.BetweenIf(propertyExpression, condition, min, max, boundary);
            return this;
        }

        /// <summary>
        /// 匹配范围
        /// </summary>
        /// <typeparam name="TProperty">属性类型</typeparam>
        /// <param name="propertyExpression">属性表达式。范围：t = > t.Time</param>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <param name="includeTime">是否包含时间</param>
        /// <param name="boundary">包含边界</param>
        public EsSearch<TResult> Between<TProperty>(Expression<Func<TResult, TProperty>> propertyExpression, DateTime? min, DateTime? max, bool includeTime = true, Boundary? boundary = null)
        {
            _query.Between(propertyExpression, min, max, includeTime, boundary);
            return this;
        }

        /// <summary>
        /// 获取结果
        /// </summary>
        public async Task<PagerList<TResult>> GetResultAsync()
        {
            var result = await GetEsResultAsync();
            return CreateResult(result);
        }

        /// <summary>
        /// 获取ES结果
        /// </summary>
        public async Task<ISearchResponse<TResult>> GetEsResultAsync()
        {
            var request = new SearchRequest<TResult>(GetIndex())
            {
                From = GetFrom(),
                Size = GetSize(),
                Sort = _sorts,
                Source = GetSource(),
                Collapse = GetCollapse(),
                Query = GetCondition()
            };
            Func<SearchDescriptor<TResult>, ISearchRequest> selector = x => request;
            return await _context.SearchAsync<TResult>(selector);
        }

        /// <summary>
        /// 获取索引名称
        /// </summary>
        private string GetIndex() => _context.GetIndexName<TResult>(_index);

        /// <summary>
        /// 获取起始行数
        /// </summary>
        private int GetFrom() => _from > 0 ? _from.SafeValue() : _queryParam.GetStartNumber() - 1;

        /// <summary>
        /// 获取分页大小
        /// </summary>
        private int GetSize() => _size > 0 ? _size.SafeValue() : _queryParam.PageSize;

        /// <summary>
        /// 获取源过滤器
        /// </summary>
        private SourceFilter GetSource() =>
            new SourceFilter
            {
                Includes = GetIncludeFields(),
                Excludes = GetExcludeFields()
            };

        /// <summary>
        /// 获取包含字段
        /// </summary>
        private Fields GetIncludeFields() => _includeFields.Count == 0 ? "*" : _includeFields.ToArray();

        /// <summary>
        /// 获取排除字段
        /// </summary>
        private Fields GetExcludeFields() => _excludeFields.ToArray();

        /// <summary>
        /// 获取折叠
        /// </summary>
        private IFieldCollapse GetCollapse() =>
            _collapseField == null
                ? null
                : new FieldCollapse {Field = _collapseField};

        /// <summary>
        /// 获取查询条件
        /// </summary>
        private QueryContainer GetCondition() => _query.GetCondition();

        /// <summary>
        /// 创建分页结果
        /// </summary>
        /// <param name="result">查询响应结果</param>
        private PagerList<TResult> CreateResult(ISearchResponse<TResult> result)
        {
            _queryParam.TotalCount = Convert.ToInt32(result.Total);
            return new PagerList<TResult>(_queryParam, result.Documents.ToList());
        }
    }
}
