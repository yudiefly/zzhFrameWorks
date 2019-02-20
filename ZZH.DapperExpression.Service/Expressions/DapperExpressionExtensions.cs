using DapperExtensions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using ZZH.DapperExpression.Service.Utils;

namespace ZZH.DapperExpression.Service.Expressions
{
    public static class DapperExpressionExtensions
    {
        /// <summary>
        /// 将表达式转换为 <see cref="IPredicate"/>
        /// </summary>
        /// <param name="expression">要转换的表达式</param>
        /// <returns></returns>
        public static IPredicate ToPredicateGroup<TEntity, TPrimaryKey>(this Expression<Func<TEntity, bool>> expression)
            where TEntity : class
        {
            Check.NotNull(expression, nameof(expression));

            var dev = new DapperExpressionVisitor<TEntity, TPrimaryKey>();
            return dev.Process(expression);
        }

        /// <summary>
        /// 将表达式转换为 <see cref="IPredicate"/>
        /// </summary>
        /// <param name="expression">要转换的表达式</param>
        public static IPredicate ToPredicateGroup<TEntity>(this Expression<Func<TEntity, bool>> expression)
            where TEntity : class
        {
            Check.NotNull(expression, nameof(expression));

            var dev = new DapperExpressionVisitor<TEntity, string>();
            return dev.Process(expression);
        }
    }
}
