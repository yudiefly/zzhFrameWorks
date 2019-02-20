using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace ZZH.DapperExpression.Service.Test.Repositories.SqlServer
{
    public class CasualDapperRepository_SqlServer_Tests
    {
        public void Should_Get_Test()
        {
            using (var dbContext = new SqlServerActiveDbContext())
            {
                var repository = (ICasualDapperRepository)new SqlServerCasualDapperRepository(dbContext);
                var product = repository.Get<Product>("7BA398DF-1C60-41FB-99CB-A9C400E17A37");
                //Assert.NotNull(product);
            }
        }

        public void Should_Get_Via_Lambda_Test()
        {
            using (var dbContext = new SqlServerActiveDbContext())
            {
                var repository = (ICasualDapperRepository)new SqlServerCasualDapperRepository(dbContext);
                var no = "001";
                var product = repository.GetFirstOrDefault<Product>(s => s.NO == no && !s.IsDeleted);
                //Assert.NotNull(product);
            }
        }

        public void Should_Get_Paged_Test()
        {
            using (var dbContext = new SqlServerActiveDbContext())
            {
                var repository = (ICasualDapperRepository)new SqlServerCasualDapperRepository(dbContext);
                var products = repository.GetAllPaged<Product>(s => s.IsDeleted == false, 1, 2, true, s => s.InBound);
                //Assert.NotNull(products);
                //Assert.True(products.Count() == 2, products.Count().ToString());
            }
        }

        public void Should_Add_Test()
        {
            using (var dbContext = new SqlServerActiveDbContext())
            {
                var repository = (ICasualDapperRepository)new SqlServerCasualDapperRepository(dbContext);

                var product = new Product
                {
                    ID = Guid.NewGuid(),
                    NO = "11",
                    Name = "P-000011",
                    Weight = 2.1,
                    InBound = DateTime.Now,
                };
                repository.Insert(product);

                repository.UnitOfWork.Commit();
            }
        }
    }
}
