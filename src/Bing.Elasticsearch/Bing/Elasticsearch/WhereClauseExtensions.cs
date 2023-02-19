using System;
using System.Linq.Expressions;
using Bing.Elasticsearch.Builders;
using Bing.Elasticsearch.Models;

namespace Bing.Elasticsearch;

/// <summary>
/// Where子句操作扩展
/// </summary>
public static class WhereClauseExtensions
{
    public static IEsSearch Where<TEntity>(this IEsSearch source, Expression<Func<TEntity, object>> expression, object value, Operator @operator = Operator.Equal)
        where TEntity : class
    {
        if(source==null) 
            throw new ArgumentNullException("source");
        if(source is IEsPartAccessor accessor)
            accessor.WhereClause.Where(expression,value,@operator);
        return source;
    }
}