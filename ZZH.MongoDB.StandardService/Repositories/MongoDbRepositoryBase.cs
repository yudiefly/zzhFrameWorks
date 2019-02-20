using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZZH.MongoDB.StandardService.Entities;
using ZZH.MongoDB.StandardService.MongoDb;

namespace ZZH.MongoDB.StandardService.Repositories
{
    /// <summary>
    /// MongoDB 仓储基类。主键类型为<see cref="Guid"/>。
    /// 对于 <see cref="Guid"/> 类型的主键，在插入数据时会自动赋予一个唯一的 Guid。
    /// </summary>
    public abstract class MongoDbRepositoryBase<TEntity> : MongoDbRepositoryBase<TEntity, Guid>
       where TEntity : class, IMongoDbEntity<Guid>
    {
        protected MongoDbRepositoryBase(IMongoDatabaseProvider databaseProvider) : base(databaseProvider)
        {

        }
    }

    /// <summary>
    /// MongoDB 仓储基类。
    /// 注：Entity 的 Id 属性会自动映射到中 MongoDB 的 "_id" 属性。
    /// 其中，将 Entity 的 Id 属性设置为 <see cref="Guid"/> 类型时，在插入数据时会自动赋予一个唯一的 Guid。
    /// 而对于 <see cref="int"/> 和 <see cref="string"/> 类型的主键，在插入数据时需要手动设置唯一值。
    /// </summary>
    public abstract class MongoDbRepositoryBase<TEntity, TPrimaryKey> : BaseMongoDbRepository<TEntity, TPrimaryKey>
        where TEntity : class, IMongoDbEntity<TPrimaryKey>
    {
        private readonly IMongoDatabaseProvider _databaseProvider;

        public virtual IMongoDatabase Database
        {
            get { return _databaseProvider.Database; }
        }

        protected MongoDbRepositoryBase(IMongoDatabaseProvider databaseProvider)
        {
            _databaseProvider = databaseProvider;
        }

        public virtual IMongoCollection<TEntity> Collection
        {
            get
            {
                return _databaseProvider.Database.GetCollection<TEntity>(typeof(TEntity).Name);
            }
        }

        public override IQueryable<TEntity> GetAll()
        {
            return Collection.AsQueryable();
        }

        public override TEntity Insert(TEntity entity)
        {
            Collection.InsertOne(entity);
            return entity;
        }

        public override async Task<TEntity> InsertAsync(TEntity entity)
        {
            await Collection.InsertOneAsync(entity);
            return entity;
        }

        public override void InsertMany(IEnumerable<TEntity> entities)
        {
            Collection.InsertMany(entities);
        }

        public override Task InsertManyAsync(IEnumerable<TEntity> entities)
        {
            return Collection.InsertManyAsync(entities);
        }

        /// <summary>
        /// 注：实体会全量更新, 返回值为更改前的实体对象
        /// </summary>
        public override TEntity Update(TEntity entity)
        {
            var filter = Builders<TEntity>.Filter.Eq(e => e.Id, entity.Id);
            return Collection.FindOneAndReplace(filter, entity);
        }

        /// <summary>
        /// 注：实体会全量更新, 返回值为更改前的实体对象
        /// </summary>
        public override async Task<TEntity> UpdateAsync(TEntity entity)
        {
            var filter = Builders<TEntity>.Filter.Eq(e => e.Id, entity.Id);
            return await Collection.FindOneAndReplaceAsync(filter, entity);
        }

        public override void Delete(TEntity entity)
        {
            var filter = Builders<TEntity>.Filter.Eq(e => e.Id, entity.Id);
            Collection.FindOneAndDelete(filter);
        }

        public override Task DeleteAsync(TEntity entity)
        {
            var filter = Builders<TEntity>.Filter.Eq(e => e.Id, entity.Id);
            return Collection.FindOneAndDeleteAsync(filter);
        }

        public override void Delete(TPrimaryKey id)
        {
            var filter = Builders<TEntity>.Filter.Eq(e => e.Id, id);
            Collection.FindOneAndDelete(filter);
        }

        public override Task DeleteAsync(TPrimaryKey id)
        {
            var filter = Builders<TEntity>.Filter.Eq(e => e.Id, id);
            return Collection.FindOneAndDeleteAsync(filter);
        }
    }
}
