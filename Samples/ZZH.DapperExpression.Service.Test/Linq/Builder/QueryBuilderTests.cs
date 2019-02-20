using DapperExtensions;
using DapperExtensions.Sql;
using System;
using System.Collections.Generic;
using System.Text;
using ZZH.DapperExpression.Service.Linq.Builder;

namespace ZZH.DapperExpression.Service.Test.Linq.Builder
{
    public class QueryBuilderTests
    {
        public void ConvertToSql_From_Expression_Test()
        {
            var name = "P-001";
            QueryBuilder<QueryModelTest>.FromExpression(s => s.Name == name && !s.IsDeleted && s.IsActive);
            var pg = QueryBuilder<QueryModelTest>.FromExpression(s => s.Name == name && !s.IsDeleted && s.IsActive && s.Name == null || (s.Name == "123" && s.Name == "abc"));

            var config = new DapperExtensionsConfiguration();
            var generator = new SqlGeneratorImpl(config);
            var classMapper = generator.Configuration.GetMap<QueryModelTest>();
            var updateSql = generator.Update(classMapper, pg, new Dictionary<string, object> { { "Name", "aaa" } });
            var delSql = generator.Delete(classMapper, pg, new Dictionary<string, object> { { "Name", "aaa" } });

            Console.WriteLine(updateSql);
            Console.WriteLine(delSql);
        }
    }

    class QueryModelTest
    {
        public string Name { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }
    }
}
