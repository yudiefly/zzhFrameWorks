using DapperExtensions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using ZZH.DapperExpression.Service.Utils;

namespace ZZH.DapperExpression.Service.Linq.Builder
{
    public static class QueryBuilderExtensions
    {
        /// <summary>
        /// 将表达式转换为 <see cref="IPredicate"/>
        /// </summary>
        /// <param name="expression">要转换的表达式</param>
        public static IPredicate ToPredicateGroup<TEntity>(this Expression<Func<TEntity, bool>> expression)
            where TEntity : class
        {
            Check.NotNull(expression, nameof(expression));

            return QueryBuilder<TEntity>.FromExpression(expression);
        }
    }
}
