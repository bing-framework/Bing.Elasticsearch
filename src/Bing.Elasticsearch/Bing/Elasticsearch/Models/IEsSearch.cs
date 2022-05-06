using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Bing.Data;
using Bing.Data.Queries;
using Nest;

namespace Bing.Elasticsearch.Models
{
    /// <summary>
    /// ES查询对象
    /// </summary>
    /// <typeparam name="TSearch">ES查询对象类型</typeparam>
    /// <typeparam name="TResult">查询结果类型</typeparam>
    public interface IEsSearch<out TSearch, TResult>
        where TSearch : IEsSearch<TSearch, TResult>
        where TResult : class
    {
        /// <summary>
        /// 获取当前查询对象
        /// </summary>
        EsQuery<TResult> GetQuery();

        /// <summary>
        /// 创建新的查询对象
        /// </summary>
        EsQuery<TResult> NewQuery();

        /// <summary>
        /// 设置索引名称或别名
        /// </summary>
        /// <param name="index">索引名称，也可以是别名</param>
        TSearch Index(string index);

        /// <summary>
        /// 搜索全部索引
        /// </summary>
        TSearch AllIndex();

        /// <summary>
        /// 设置起始行数，从0开始
        /// </summary>
        /// <param name="from">起始行数</param>
        TSearch From(int from);

        /// <summary>
        /// 设置分页大小
        /// </summary>
        /// <param name="size">分页大小</param>
        TSearch Size(int size);

        /// <summary>
        /// 排序
        /// </summary>
        /// <typeparam name="TProperty">属性类型</typeparam>
        /// <param name="expression">排序字段表达式</param>
        /// <param name="desc">是否降序排序</param>
        TSearch OrderBy<TProperty>(Expression<Func<TResult, TProperty>> expression, bool desc = false);

        /// <summary>
        /// 排序
        /// </summary>
        /// <typeparam name="TProperty">属性类型</typeparam>
        /// <param name="expression">排序字段表达式</param>
        /// <param name="desc">是否降序排序</param>
        /// <param name="condition">该值为true时添加查询条件，否则忽略</param>
        TSearch OrderByIf<TProperty>(Expression<Func<TResult, TProperty>> expression, bool desc, bool condition);

        /// <summary>
        /// 嵌套属性排序
        /// </summary>
        /// <typeparam name="TProperty">属性类型</typeparam>
        /// <param name="path">排序路径</param>
        /// <param name="expression">排序字段表达式</param>
        /// <param name="desc">是否降序排序</param>
        TSearch NestOrderBy<TProperty>(Expression<Func<TResult, TProperty>> path, Expression<Func<TResult, TProperty>> expression, bool desc = false);

        /// <summary>
        /// 嵌套属性排序
        /// </summary>
        /// <typeparam name="TProperty">属性类型</typeparam>
        /// <param name="path">排序路径</param>
        /// <param name="expression">排序字段表达式</param>
        /// <param name="desc">是否降序排序</param>
        /// <param name="condition">该值为true时添加查询条件，否则忽略</param>
        TSearch NestOrderByIf<TProperty>(Expression<Func<TResult, TProperty>> path, Expression<Func<TResult, TProperty>> expression, bool desc, bool condition);

        /// <summary>
        /// 设置包含字段列表
        /// </summary>
        /// <param name="fields">字段列表</param>
        TSearch IncludeFields(string fields);

        /// <summary>
        /// 设置包含字段
        /// </summary>
        /// <typeparam name="TProperty">属性类型</typeparam>
        /// <param name="expression">字段表达式</param>
        TSearch IncludeField<TProperty>(Expression<Func<TResult, TProperty>> expression);

        /// <summary>
        /// 设置排除字段列表
        /// </summary>
        /// <param name="fields">字段列表</param>
        TSearch ExcludeFields(string fields);

        /// <summary>
        /// 设置排除字段
        /// </summary>
        /// <typeparam name="TProperty">属性类型</typeparam>
        /// <param name="expression">字段表达式</param>
        TSearch ExcludeField<TProperty>(Expression<Func<TResult, TProperty>> expression);

        /// <summary>
        /// 设置折叠字段
        /// </summary>
        /// <typeparam name="TProperty">属性类型</typeparam>
        /// <param name="expression">字段表达式</param>
        TSearch Collapse<TProperty>(Expression<Func<TResult, TProperty>> expression);

        /// <summary>
        /// 嵌套查询
        /// </summary>
        /// <typeparam name="TProperty">属性类型</typeparam>
        /// <param name="path">嵌套字段</param>
        /// <param name="condition">查询条件</param>
        TSearch Nest<TProperty>(Expression<Func<TResult, TProperty>> path, IEsCondition condition);

        /// <summary>
        /// 嵌套查询
        /// </summary>
        /// <typeparam name="TProperty">属性类型</typeparam>
        /// <param name="path">嵌套字段</param>
        /// <param name="condition">查询条件</param>
        TSearch Nest<TProperty>(Expression<Func<TResult, TProperty>> path, QueryContainer condition);

        /// <summary>
        /// 嵌套查询
        /// </summary>
        /// <typeparam name="TProperty">属性类型</typeparam>
        /// <param name="path">嵌套字段</param>
        /// <param name="action">查询操作</param>
        TSearch Nest<TProperty>(Expression<Func<TResult, TProperty>> path, Action<EsQuery<TResult>> action);

        /// <summary>
        /// 精确匹配词条
        /// </summary>
        /// <typeparam name="TProperty">属性类型</typeparam>
        /// <param name="expression">字段表达式</param>
        /// <param name="value">值</param>
        TSearch Term<TProperty>(Expression<Func<TResult, TProperty>> expression, object value);

        /// <summary>
        /// 精确匹配词条
        /// </summary>
        /// <typeparam name="TProperty">属性类型</typeparam>
        /// <param name="expression">字段表达式</param>
        /// <param name="value">值</param>
        /// <param name="condition">该值为true时添加查询条件，否则忽略</param>
        TSearch TermIf<TProperty>(Expression<Func<TResult, TProperty>> expression, object value, bool condition);

        /// <summary>
        /// 精确匹配词条，当值为空时忽略条件
        /// </summary>
        /// <typeparam name="TProperty">属性类型</typeparam>
        /// <param name="expression">字段表达式</param>
        /// <param name="value">值</param>
        TSearch TermIfNotEmpty<TProperty>(Expression<Func<TResult, TProperty>> expression, object value);

        /// <summary>
        /// 精确匹配词条列表
        /// </summary>
        /// <typeparam name="TProperty">属性类型</typeparam>
        /// <param name="expression">字段表达式</param>
        /// <param name="values">值</param>
        TSearch Terms<TProperty>(Expression<Func<TResult, TProperty>> expression, IEnumerable<object> values);

        /// <summary>
        /// 精确匹配词条列表
        /// </summary>
        /// <typeparam name="TProperty">属性类型</typeparam>
        /// <param name="expression">字段表达式</param>
        /// <param name="values">值</param>
        /// <param name="condition">该值为true时添加查询条件，否则忽略</param>
        TSearch TermsIf<TProperty>(Expression<Func<TResult, TProperty>> expression, IEnumerable<object> values, bool condition);

        /// <summary>
        /// 精确匹配词条列表，当值为空时忽略条件
        /// </summary>
        /// <typeparam name="TProperty">属性类型</typeparam>
        /// <param name="expression">字段表达式</param>
        /// <param name="values">值</param>
        TSearch TermsIfNotEmpty<TProperty>(Expression<Func<TResult, TProperty>> expression, IEnumerable<object> values);

        /// <summary>
        /// 匹配词条
        /// </summary>
        /// <typeparam name="TProperty">属性类型</typeparam>
        /// <param name="expression">字段表达式</param>
        /// <param name="value">值</param>
        TSearch Match<TProperty>(Expression<Func<TResult, TProperty>> expression, string value);

        /// <summary>
        /// 匹配词条
        /// </summary>
        /// <typeparam name="TProperty">属性类型</typeparam>
        /// <param name="expression">字段表达式</param>
        /// <param name="value">值</param>
        /// <param name="condition">该值为true时添加查询条件，否则忽略</param>
        TSearch MatchIf<TProperty>(Expression<Func<TResult, TProperty>> expression, string value, bool condition);

        /// <summary>
        /// 匹配词条，当值为空时忽略条件
        /// </summary>
        /// <typeparam name="TProperty">属性类型</typeparam>
        /// <param name="expression">字段表达式</param>
        /// <param name="value">值</param>
        TSearch MatchIfNotEmpty<TProperty>(Expression<Func<TResult, TProperty>> expression, string value);

        /// <summary>
        /// 多字段匹配词条
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="expressions">字段表达式列表</param>
        TSearch MultiMatch(string value, params Expression<Func<TResult, object>>[] expressions);

        /// <summary>
        /// 多字段匹配词条，当值为空时忽略条件
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="expressions">字段表达式列表</param>
        TSearch MultiMatchIfNotEmpty(string value, params Expression<Func<TResult, object>>[] expressions);

        /// <summary>
        /// 匹配范围
        /// </summary>
        /// <typeparam name="TProperty">属性类型</typeparam>
        /// <param name="propertyExpression">属性表达式。范围：t = > t.Age</param>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <param name="boundary">包含边界</param>
        TSearch Between<TProperty>(Expression<Func<TResult, TProperty>> propertyExpression, int? min, int? max, Boundary boundary = Boundary.Both);

        /// <summary>
        /// 匹配范围
        /// </summary>
        /// <typeparam name="TProperty">属性类型</typeparam>
        /// <param name="propertyExpression">属性表达式。范围：t = > t.Age</param>
        /// <param name="condition">该值为true时添加查询条件，否则忽略</param>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <param name="boundary">包含边界</param>
        TSearch BetweenIf<TProperty>(Expression<Func<TResult, TProperty>> propertyExpression, bool condition, int? min, int? max = null, Boundary boundary = Boundary.Both);

        /// <summary>
        /// 匹配范围
        /// </summary>
        /// <typeparam name="TProperty">属性类型</typeparam>
        /// <param name="propertyExpression">属性表达式。范围：t = > t.Age</param>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <param name="boundary">包含边界</param>
        TSearch Between<TProperty>(Expression<Func<TResult, TProperty>> propertyExpression, double? min, double? max, Boundary boundary = Boundary.Both);

        /// <summary>
        /// 匹配范围
        /// </summary>
        /// <typeparam name="TProperty">属性类型</typeparam>
        /// <param name="propertyExpression">属性表达式。范围：t = > t.Age</param>
        /// <param name="condition">该值为true时添加查询条件，否则忽略</param>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <param name="boundary">包含边界</param>
        TSearch BetweenIf<TProperty>(Expression<Func<TResult, TProperty>> propertyExpression, bool condition, double? min, double? max = null, Boundary boundary = Boundary.Both);

        /// <summary>
        /// 匹配范围
        /// </summary>
        /// <typeparam name="TProperty">属性类型</typeparam>
        /// <param name="propertyExpression">属性表达式。范围：t = > t.Time</param>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <param name="includeTime">是否包含时间</param>
        /// <param name="boundary">包含边界</param>
        TSearch Between<TProperty>(Expression<Func<TResult, TProperty>> propertyExpression, DateTime? min, DateTime? max, bool includeTime = true, Boundary? boundary = null);

        /// <summary>
        /// 获取ES结果
        /// </summary>
        Task<ISearchResponse<TResult>> GetEsResultAsync();

        /// <summary>
        /// 获取结果
        /// </summary>
        Task<List<TResult>> GetResultAsync();
    }
}
