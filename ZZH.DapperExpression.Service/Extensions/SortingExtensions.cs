using DapperExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using ZZH.DapperExpression.Service.Utils;

namespace ZZH.DapperExpression.Service.Extensions
{
    internal static class SortingExtensions
    {
        /// <summary>
        /// 将表达式树转换为可排序的集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sortingExpression">表达式数据集合</param>
        /// <param name="ascending">是否是正序排列,默认为 true</param>
        /// <returns></returns>
        public static List<ISort> ToSortable<T>(this Expression<Func<T, object>>[] sortingExpression, bool ascending = true)
        {
            Check.NotNullOrEmpty(sortingExpression, nameof(sortingExpression));

            var sortList = new List<ISort>();
            sortingExpression.ToList().ForEach(sortExpression =>
            {
                MemberInfo sortProperty = ReflectionHelper.GetProperty(sortExpression);
                sortList.Add(new Sort { Ascending = ascending, PropertyName = sortProperty.Name });
            });

            return sortList;
        }
    }
}
