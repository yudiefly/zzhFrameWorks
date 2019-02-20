using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;
using ZZH.DapperExpression.Service.Data;

namespace ZZH.DapperExpression.Service.DbContext
{
    /// <summary>
    /// 基于 SqlServer 上下文对象
    /// </summary>
    public sealed class SqlServerDbContext : ActiveDbContext
    {
        public override DbConnection CreateDbConnection(string connectionString)
        {
            return new SqlConnection(connectionString);
        }
    }
}
