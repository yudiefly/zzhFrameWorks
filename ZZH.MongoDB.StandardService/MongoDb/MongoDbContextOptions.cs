using System;
using System.Collections.Generic;
using System.Text;

namespace ZZH.MongoDB.StandardService.MongoDb
{
    public class MongoDbContextOptions
    {
        /// <summary>
        /// MongoDB 的连接字符串
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// 要连接的数据库名称
        /// </summary>
        public string DatabaseName { get; set; }
    }
}
