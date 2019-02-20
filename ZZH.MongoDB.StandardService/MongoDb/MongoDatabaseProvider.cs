using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZZH.MongoDB.StandardService.MongoDb
{
    public class MongoDatabaseProvider : IMongoDatabaseProvider
    {
        private readonly MongoDbContext _mongoDbContext;

        public MongoDatabaseProvider(MongoDbContext mongoDbContext)
        {
            _mongoDbContext = mongoDbContext;
        }

        public IMongoDatabase Database => _mongoDbContext.DataBase;
    }
}
