using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZZH.Dapper.Service.Repository.MySql;

namespace ZZH.Dapper.Service.Test.Respository
{
    public class TestAppBaseRepository: BaseRepository
    {
        private const string strConnections = "server=localhost;port=3306;database=yudiefly;User Id=root;password=1234567890;SslMode = none;";
        public TestAppBaseRepository() : base(strConnections)
        {

        }
    }
}
