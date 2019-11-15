using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace ZZH.DapperExpression.Service.Test.Repositories.SqlServer
{
    internal class SqlServerProvider
    {
        public static DbConnection CreateSqlServerConncetion()
        {
            return new System.Data.SqlClient.SqlConnection("Server=10.125.253.83;Database=Shadow_Develop;User Id=YNioappadmin;Password=1qaz2wsX;MultipleActiveResultSets=true");
        }
    }
}
