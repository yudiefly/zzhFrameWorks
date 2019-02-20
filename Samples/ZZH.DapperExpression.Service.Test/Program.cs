using System;
using ZZH.DapperExpression.Service.Test.Linq.Builder;
using ZZH.DapperExpression.Service.Test.Repositories.SqlServer;

namespace ZZH.DapperExpression.Service.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var test1 = new QueryBuilderTests();
            test1.ConvertToSql_From_Expression_Test();

            var test2 = new Dapper_Origin_Tests();
            test2.Db_Get_Insert_Tests();
            test2.Db_Get_Tests();


            Console.ReadLine();
        }
    }
}
