using System;
using System.Linq.Expressions;
using Bing.Extensions;

namespace Bing.Data.Queries.Conditions
{
    /// <summary>
    /// 日期范围过滤条件 - 不包含时间
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TProperty">属性类型</typeparam>
    public class DateRangeEsCondition<TEntity, TProperty> : DateTimeRangeEsCondition<TEntity, TProperty> where TEntity : class
    {
        /// <summary>
        /// 初始化一个<see cref="DateRangeEsCondition{TEntity,TProperty}"/>类型的实例
        /// </summary>
        /// <param name="propertyExpression">属性表达式</param>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <param name="boundary">包含边界</param>
        public DateRangeEsCondition(Expression<Func<TEntity, TProperty>> propertyExpression, DateTime? min, DateTime? max, Boundary boundary = Boundary.Both)
            : base(propertyExpression, min, max, boundary)
        {
        }

        /// <summary>
        /// 获取最小值
        /// </summary>
        protected override DateTime? GetMinValue() => base.GetMinValue().SafeValue().Date;

        /// <summary>
        /// 获取最大值
        /// </summary>
        protected override DateTime? GetMaxValue() => base.GetMaxValue().SafeValue().Date.AddDays(1);
    }
}
