using System;
using System.Linq.Expressions;
using Nest;

namespace Bing.Data.Queries.Conditions
{
    /// <summary>
    /// double范围过滤条件
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TProperty">属性类型</typeparam>
    public class DoubleRangeEsCondition<TEntity, TProperty> : RangeEsConditionBase<TEntity, TProperty, double, NumericRangeQuery>
        where TEntity : class
    {
        /// <summary>
        /// 初始化一个<see cref="DoubleRangeEsCondition{TEntity,TProperty}"/>类型的实例
        /// </summary>
        /// <param name="propertyExpression">属性表达式</param>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <param name="boundary">包含边界</param>
        public DoubleRangeEsCondition(Expression<Func<TEntity, TProperty>> propertyExpression, double? min, double? max, Boundary boundary = Boundary.Both)
            : base(propertyExpression, min, max, boundary)
        {
        }

        /// <summary>
        /// 最小值是否大于最大值
        /// </summary>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        protected override bool IsMinGreaterMax(double? min, double? max) => min > max;

        /// <summary>
        /// 创建查询条件
        /// </summary>
        protected override NumericRangeQuery CreateCondition() => new NumericRangeQuery();

        /// <summary>
        /// 设置字段
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <param name="field">字段</param>
        protected override void SetField(NumericRangeQuery condition, Field field) => condition.Field = field;

        /// <summary>
        /// 设置大于等于操作
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <param name="min">最小值</param>
        protected override void SetGreaterEqual(NumericRangeQuery condition, double? min) => condition.GreaterThanOrEqualTo = min;

        /// <summary>
        /// 设置大于操作
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <param name="min">最小值</param>
        protected override void SetGreater(NumericRangeQuery condition, double? min) => condition.GreaterThan = min;

        /// <summary>
        /// 设置小于等于操作
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <param name="max">最大值</param>
        protected override void SetLessEqual(NumericRangeQuery condition, double? max) => condition.LessThanOrEqualTo = max;

        /// <summary>
        /// 设置小于操作
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <param name="max">最大值</param>
        protected override void SetLess(NumericRangeQuery condition, double? max) => condition.LessThan = max;
    }
}
