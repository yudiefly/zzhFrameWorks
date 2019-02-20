using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using ZZH.DapperExpression.Service.Data;

namespace ZZH.DapperExpression.Service.Test.Repositories.SqlServer
{
    public class SqlServerCasualDapperRepository : CasualDapperRepository<SqlServerActiveDbContext>
    {
        public SqlServerCasualDapperRepository(SqlServerActiveDbContext context) : base(context)
        {

        }
    }

    public class SqlServerActiveDbContext : ActiveDbContext
    {
        public override DbConnection CreateDbConnection(string connectionString)
        {
            return SqlServerProvider.CreateSqlServerConncetion();
        }
    }
}
