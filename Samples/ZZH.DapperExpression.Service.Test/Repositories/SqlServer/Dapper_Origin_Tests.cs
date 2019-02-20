using System;
using System.Collections.Generic;
using System.Text;
using Dapper;

namespace ZZH.DapperExpression.Service.Test.Repositories.SqlServer
{
    public class Dapper_Origin_Tests
    {
        public void Db_Get_Tests()
        {
            using (var conn = SqlServerProvider.CreateSqlServerConncetion())
            {

            }
        }

     
        public void Db_Get_Insert_Tests()
        {
            using (var conn = SqlServerProvider.CreateSqlServerConncetion())
            {
                conn.Open();


                var products = conn.Query<Product>("SELECT * FROM Product");

                //Assert.NotNull(products);
                //Assert.True(products.Count() > 0);

                var product = conn.Query<Product>("SELECT * FROM Product WHERE NO = @NO", new { NO = "1" });
                //Assert.NotNull(product);

                var sql = "INSERT INTO [dbo].[Product]([ID],[NO],[Name],[Weight],[InBound],[Remark],[IsDeleted])" +
                    " VALUES(@ID, @NO, @NAME, @Weight, @InBound, @Remark, @IsDeleted)";

                var items = new List<object>
                {
                    new { ID = Guid.NewGuid(), NO = "2", NAME = "P-000002", Weight = 3.1, InBound = DateTime.Now, Remark = (string)null, IsDeleted = false },
                    new { ID = Guid.NewGuid(), NO = "3", NAME = "P-000003", Weight = 4.1, InBound = DateTime.Now, Remark = (string)null, IsDeleted = false },
                    new { ID = Guid.NewGuid(), NO = "4", NAME = "P-000004", Weight = 5.1, InBound = DateTime.Now, Remark = (string)null, IsDeleted = false },
                    new { ID = Guid.NewGuid(), NO = "5", NAME = "P-000005", Weight = 6.1, InBound = DateTime.Now, Remark = (string)null, IsDeleted = false },
                    new { ID = Guid.NewGuid(), NO = "6", NAME = "P-000006", Weight = 7.1, InBound = DateTime.Now, Remark = (string)null, IsDeleted = false },
                };

                using (var tran = conn.BeginTransaction())
                {

                    try
                    {
                        foreach (var item in items)
                        {
                            conn.Execute(sql, item, transaction: tran);
                        }

                        tran.Commit();
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();

                        //Assert.True(false, ex.Message);
                    }
                }

                var product2 = conn.Query<Product>("SELECT * FROM Product WHERE NO = @NO", new { NO = "1" });
                //Assert.NotNull(product2);

                //Assert.True(conn.State == System.Data.ConnectionState.Open);
            }
        }
    }
}
