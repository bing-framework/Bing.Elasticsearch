﻿using Bing.Data;
using Nest;

namespace Bing.Elasticsearch.Builders.Conditions;

/// <summary>
/// And连接条件
/// </summary>
public class AndCondition : IEsCondition
{
    /// <summary>
    /// ES条件1
    /// </summary>
    private readonly QueryContainer _condition1;

    /// <summary>
    /// ES条件2
    /// </summary>
    private readonly QueryContainer _condition2;

    /// <summary>
    /// 初始化一个<see cref="AndCondition"/>
    /// </summary>
    /// <param name="condition1">ES条件1</param>
    /// <param name="condition2">ES条件2</param>
    public AndCondition(IEsCondition condition1, IEsCondition condition2)
    {
        _condition1 = condition1?.GetCondition();
        _condition2 = condition2?.GetCondition();
    }

    /// <summary>
    /// 获取查询条件
    /// </summary>
    public QueryContainer GetCondition()
    {
        if (_condition1 == null)
            return _condition2;
        if (_condition2 == null)
            return _condition1;
        return _condition1 && _condition2;
    }
}