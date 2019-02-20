using System;
using System.Collections.Generic;
using System.Text;

namespace ZZH.DapperExpression.Service.Linq.Builder
{
    public static class QueryFunctions
    {
        /// <summary>
        /// For reflection only.
        /// </summary>
        /// <param name="pattern"></param>
        /// <param name="member"></param>
        /// <returns></returns>
        public static bool Like(string pattern, object member)
        {
            throw new InvalidOperationException("For reflection only!");
        }
    }
}
