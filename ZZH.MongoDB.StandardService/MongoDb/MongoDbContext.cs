using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZZH.MongoDB.StandardService.MongoDb
{
    /// <summary>
    /// MongoDb 上下文对象。
    /// Note：官方建议将将 <see cref="MongoClient"/> 对象以单例的方式储存，详细参考文档 
    /// http://mongodb.github.io/mongo-csharp-driver/2.7/reference/driver/connecting/#mongo-client
    /// </summary>
    public class MongoDbContext
    {
        private readonly Lazy<IMongoDatabase> _mongoDatabase;

        /// <summary>
        /// 初始化一个新的<see cref="MongoDbContext"/>对象。
        /// </summary>
        /// <param name="options">参数选项</param>
        public MongoDbContext(MongoDbContextOptions options)
        {
            _mongoDatabase = new Lazy<IMongoDatabase>(() =>
            {
                var client = new MongoClient(options.ConnectionString);
                return client.GetDatabase(options.DatabaseName);
            });
        }

        /// <summary>
        /// 获取 MongoDB 的 DataBase 。
        /// Note：当给定的 Database 不存在时，会自动创建一个。
        /// </summary>
        public IMongoDatabase DataBase => _mongoDatabase.Value;
    }
}
