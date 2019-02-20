using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ZZH.MongoDB.StandardService.Entities;

namespace ZZH.MongoDB.StandardService.Repositories
{
    /// <summary>
    /// MongoDb Repository 扩展类
    /// </summary>
    public static class MongoDbRepositoryExtensions
    {
        /// <summary>
        /// 批量插入数据
        /// </summary>
        /// <param name="repository">仓储对象</param>
        /// <param name="entities">要插入的实体集合</param>
        public static void InsertMany<TEntity, TPrimaryKey>(this IMongoDbRepository<TEntity, TPrimaryKey> repository, IEnumerable<TEntity> entities)
            where TEntity : class, IMongoDbEntity<TPrimaryKey>
        {
            if (repository is MongoDbRepositoryBase<TEntity, TPrimaryKey> rep)
            {
                rep.Collection.InsertMany(entities);
                return;
            }

            throw new InvalidOperationException("IMongoDbRepository could not assignable from MongoDbRepositoryBase.");
        }

        /// <summary>
        /// 批量插入数据
        /// </summary>
        /// <param name="repository">仓储对象</param>
        /// <param name="entities">要插入的实体集合</param>
        public static Task InsertManyAsync<TEntity, TPrimaryKey>(this IMongoDbRepository<TEntity, TPrimaryKey> repository, IEnumerable<TEntity> entities)
            where TEntity : class, IMongoDbEntity<TPrimaryKey>
        {
            if (repository is MongoDbRepositoryBase<TEntity, TPrimaryKey> rep)
            {
                return rep.Collection.InsertManyAsync(entities);
            }

            throw new InvalidOperationException("IMongoDbRepository could not assignable from MongoDbRepositoryBase.");
        }
    }
}
