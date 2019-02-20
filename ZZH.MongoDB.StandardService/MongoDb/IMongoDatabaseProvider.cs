using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZZH.MongoDB.StandardService.MongoDb
{
    /// <summary>
    /// 定义 MongoDB 提供者对象
    /// </summary>
    public interface IMongoDatabaseProvider
    {
        /// <summary>
        /// 获取 <see cref="MongoDatabase"/>.
        /// </summary>
        IMongoDatabase Database { get; }
    }
}
