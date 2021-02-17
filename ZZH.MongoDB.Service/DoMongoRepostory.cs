using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.IO;
using MongoDB.Bson;

namespace ZZH.MongoDB.Service
{
    public class DoMongoRepostory<TAggregate> : MongoBase
               where TAggregate : AggregateBase
    {
        #region 初始化及字段属性设置
        /// <summary>
        /// MongoDB数据库
        /// </summary>
        private IMongoDatabase MongoDatabase = null;
        /// <summary>
        /// 获取集合
        /// </summary>
        public IMongoCollection<TAggregate> Collection;
        /// <summary>
        /// 创建Mongo仓储对象
        /// </summary>
        /// <param name="dbSelectionKey">仓储键名</param>
        /// <param name="collectionName">表名</param>
        /// <param name="mongoConfig">MongoDB服务器配置类</param>
        public DoMongoRepostory(string dbSelectionKey, string collectionName, MongoConfig mongoConfig)
        {
            this.MongoDatabase = ShareMongoDb(dbSelectionKey, mongoConfig);
            this.Collection = ShareMongoDb(dbSelectionKey, mongoConfig).GetCollection<TAggregate>(collectionName);
        }
        /// <summary>
        /// 创建Mongo仓储对象
        /// </summary>
        /// <param name="dbSelectionKey">仓储键名</param>
        /// <param name="dbName">数据库名</param>
        /// <param name="collectionName">表名</param>
        /// <param name="mongoConnectionStrings">连接字符串</param>
        public DoMongoRepostory(string dbSelectionKey, string dbName, string collectionName, string mongoConnectionStrings)
        {
            this.MongoDatabase = ShareMongoDb(dbSelectionKey, mongoConnectionStrings, dbName);
            this.Collection = ShareMongoDb(dbSelectionKey, mongoConnectionStrings, dbName).GetCollection<TAggregate>(collectionName);
        }

        private List<WriteModel<TAggregate>> writers = new List<WriteModel<TAggregate>>();//写入模型

        /// <summary>
        /// 指示是否起用事务,默认true
        /// </summary>
        public bool IsUseTransaction { get; set; }

        private List<TAggregate> beforeChange = new List<TAggregate>();//记录更新前的数据
        private List<string> beforeAdd = new List<string>();   //记录添加前的数据ID
        private List<TAggregate> beforeDelete = new List<TAggregate>();//记录数据删除前的数据


        private bool isRollback = false;//回滚控制 
        #endregion

        #region 添加
        /// <summary>
        /// 添加一条数据
        /// </summary>
        /// <param name="entity"></param>
        public void Add(TAggregate entity)
        {
            if (entity == null)
                return;
            if (IsUseTransaction)
            {
                try
                {
                    beforeAdd.Add(entity.Id);//记录添加之前的数据的ID
                    writers.Add(new InsertOneModel<TAggregate>(entity));
                    isRollback = false;//控制是否回滚
                    return;
                }
                catch (Exception ex)
                {
                    isRollback = true;
                    throw new Exception(ex.Message);
                }
            }
            try
            {
                Collection.InsertOne(entity);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 添加数据集合
        /// </summary>
        /// <param name="entities"></param>
        public void Add(IEnumerable<TAggregate> entities)
        {
            if (entities.Count() <= 0)
                return;
            if (IsUseTransaction)
            {
                try
                {
                    entities.ToList().ForEach(o =>
                    {
                        beforeAdd.Add(o.Id);
                        writers.Add(new InsertOneModel<TAggregate>(o));
                    });
                    isRollback = false;
                    return;
                }
                catch (Exception ex)
                {
                    isRollback = true;
                    throw new Exception(ex.Message);
                }
            }
            try
            {
                Collection.InsertMany(entities);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region 替换
        /// <summary>
        /// 替换一条过滤的数据(请确保此方法Id属性是不能变)
        /// </summary>
        /// <param name="filter">过滤条件</param>
        /// <param name="enity">目标数据(目标数据的Id值必为源数据的Id)</param>
        public void ReplaceOne(Expression<Func<TAggregate, bool>> filter, TAggregate enity)
        {
            if (enity == null)
                return;
            if (IsUseTransaction)
            {
                try
                {
                    //先记录修改之前的数据
                    beforeChange.Add(Collection.Find(Builders<TAggregate>.Filter.Where(filter)).FirstOrDefault());
                    writers.Add(new ReplaceOneModel<TAggregate>(Builders<TAggregate>.Filter.Where(filter), enity));
                    isRollback = false;
                    return;
                }
                catch (Exception ex)
                {
                    isRollback = true;
                    throw new Exception(ex.Message);
                }
            }

            try
            {
                Collection.ReplaceOne(filter, enity);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public void Replace(TAggregate enity)
        {
            ReplaceById(enity.Id, enity);
        }
        /// <summary>
        /// 替换一条数据(请确保此方法Id属性是不能变)
        /// </summary>
        /// <param name="id">目标id</param>
        /// <param name="enity">目标数据(目标数据的Id值必为源数据的Id)</param>
        public void ReplaceById(string id, TAggregate enity)
        {
            if (enity == null)
                return;
            if (enity.Id != id)
            {
                isRollback = true;
                throw new Exception("the id can not change");
            }
            if (IsUseTransaction)
            {
                try
                {
                    beforeChange.Add(Collection.Find(Builders<TAggregate>.Filter.Eq(o => o.Id, id)).FirstOrDefault());
                    writers.Add(new ReplaceOneModel<TAggregate>(Builders<TAggregate>.Filter.Eq(o => o.Id, id), enity));
                    isRollback = false;
                    return;
                }
                catch (Exception ex)
                {
                    isRollback = true;
                    throw new Exception(ex.Message);
                }
            }
            try
            {
                Collection.ReplaceOne(o => o.Id == id, enity);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 查找一条数据并且替换
        /// </summary>
        /// <param name="id">目标数据的id</param>
        /// <param name="enity">更改后的数据</param>
        /// <returns>更改前的数据</returns>
        public TAggregate FindOneAndReplace(string id, TAggregate enity)
        {
            if (enity == null)
                return null;
            if (enity.Id != id)
            {
                throw new Exception("the id can not change");
            }

            return Collection.FindOneAndReplace(o => o.Id == id, enity);
        }
        /// <summary>
        /// 查找一条数据并且替换
        /// </summary>
        /// <param name="filter">条件</param>
        /// <param name="enity">更改后的数据</param>
        /// <returns>更改前的数据</returns>
        public TAggregate FindOneAndReplace(Expression<Func<TAggregate, bool>> filter, TAggregate enity)
        {
            if (enity == null)
                return null;
            return Collection.FindOneAndReplace(filter, enity);
        }

        #endregion

        #region 移除
        /// <summary>
        /// 根据过滤删除数据
        /// </summary>
        /// <param name="filter"></param>
        public void Remove(Expression<Func<TAggregate, bool>> filter)
        {
            if (IsUseTransaction)
            {
                try
                {
                    if (Collection.Find(filter).FirstOrDefault() == null)//如果要删除的数据不存在数据库中
                    {
                        throw new Exception("要删除的数据不存在数据库中");
                    }
                    beforeDelete.Add(Collection.Find(filter).FirstOrDefault());
                    writers.Add(new DeleteOneModel<TAggregate>(Builders<TAggregate>.Filter.Where(filter)));
                    isRollback = false;
                    return;
                }
                catch (Exception ex)
                {
                    isRollback = true;
                    throw new Exception(ex.Message);
                }
            }
            try
            {
                Collection.DeleteMany(filter);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public void RemoveById(string id)
        {
            if (IsUseTransaction)
            {
                try
                {
                    beforeDelete.Add(Collection.Find(Builders<TAggregate>.Filter.Eq(o => o.Id, id)).FirstOrDefault());
                    writers.Add(new DeleteOneModel<TAggregate>(Builders<TAggregate>.Filter.Eq(o => o.Id, id)));
                    isRollback = false;
                    return;
                }
                catch (Exception ex)
                {
                    isRollback = true;
                    throw new Exception(ex.Message);
                }
            }
            try
            {
                Collection.DeleteOne(o => o.Id == id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region 更新
        /// <summary>
        /// 过滤数据，执行更新操作(如不便使用，请用Replace相关的方法代替)
        /// 
        /// 一般用replace来代替这个方法。其实这个功能还算强大的，可以很自由修改多个属性
        /// 关健是set参数比较不好配置，并且如果用此方法，调用端必须引用相关的DLL，set举例如下
        /// set = Builders<TAggregate>.Update.Update.Set(o => o.Number, 1).Set(o => o.Description, "002.thml");
        /// set作用：将指定TAggregate类型的实例对象的Number属性值更改为1，Description属性值改为"002.thml"
        /// 说明：Builders<TAggregate>.Update返回类型为UpdateDefinitionBuilder<TAggregate>，这个类有很多静态
        /// 方法，Set()是其中一个，要求传入一个func的表达示，以指示当前要修改的，TAggregate类型中的属性类型，
        /// 另一个参数就是这个属性的值。
        /// 
        /// Builders<TAggregate>类有很多属性，返回很多如UpdateDefinitionBuilder<TAggregate>的很有用帮助类型
        /// 可以能参CSharpDriver-2.2.3.chm文件 下载MongoDB-CSharpDriver时带有些文件 
        /// 或从官网https://docs.mongodb.com/ecosystem/drivers/csharp/看看
        /// 
        /// </summary>
        /// <param name="filter">过滤条件</param>
        /// <param name="set">修改设置</param>
        public void Update(Expression<Func<TAggregate, bool>> filter, UpdateDefinition<TAggregate> set)
        {
            if (set == null)
                return;
            if (IsUseTransaction)//如果启用事务
            {
                try
                {
                    beforeChange.Add(Collection.Find(filter).FirstOrDefault());
                    writers.Add(new UpdateManyModel<TAggregate>(Builders<TAggregate>.Filter.Where(filter), set));
                    isRollback = false;//不回滚
                    return;//不执行后继操作
                }
                catch (Exception ex)
                {
                    isRollback = true;
                    throw new Exception(ex.Message);
                }
            }
            try
            {
                Collection.UpdateMany(filter, set);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 过滤数据，执行更新操作(如不便使用，请用Replace相关的方法代替)
        /// 
        /// 一般用replace来代替这个方法。其实这个功能还算强大的，可以很自由修改多个属性
        /// 关健是set参数比较不好配置，并且如果用此方法，调用端必须引用相关的DLL，set举例如下
        /// set = Builders<TAggregate>.Update.Update.Set(o => o.Number, 1).Set(o => o.Description, "002.thml");
        /// set作用：将指定TAggregate类型的实例对象的Number属性值更改为1，Description属性值改为"002.thml"
        /// 说明：Builders<TAggregate>.Update返回类型为UpdateDefinitionBuilder<TAggregate>，这个类有很多静态
        /// 方法，Set()是其中一个，要求传入一个func的表达示，以指示当前要修改的，TAggregate类型中的属性类型，
        /// 另一个参数就是这个属性的值。
        /// 
        /// Builders<TAggregate>类有很多属性，返回很多如UpdateDefinitionBuilder<TAggregate>的很有用帮助类型
        /// 可以能参CSharpDriver-2.2.3.chm文件 下载MongoDB-CSharpDriver时带有些文件 
        /// 或从官网https://docs.mongodb.com/ecosystem/drivers/csharp/看看
        /// 
        /// </summary>
        /// <param name="id">找出指定的id数据</param>
        /// <param name="set">修改设置</param>
        public void Update(string id, UpdateDefinition<TAggregate> set)
        {
            if (set == null)
                return;
            if (IsUseTransaction)//如果启用事务
            {
                try
                {
                    beforeChange.Add(Collection.Find(Builders<TAggregate>.Filter.Eq(o => o.Id, id)).FirstOrDefault());
                    writers.Add(new UpdateManyModel<TAggregate>(Builders<TAggregate>.Filter.Eq(o => o.Id, id), set));
                    isRollback = false;//不回滚
                    return;//不执行后继操作
                }
                catch (Exception ex)
                {
                    isRollback = true;
                    throw new Exception(ex.Message);
                }
            }
            try
            {
                Collection.UpdateMany(o => o.Id == id, set);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region 事务控制
        public void Commit()
        {
            if (!isRollback && writers.Count > 0)//如果不回滚，并且writers有数据
            {
                BulkWriteResult<TAggregate> result;
                try
                {
                    result = Collection.BulkWrite(writers);
                }
                catch (Exception)
                {
                    Rollback();//若BulkWriteResult发生异常
                    throw;
                }
                if (result.ProcessedRequests.Count != writers.Count)//检查完成写入的数量，如果有误，回滚
                {
                    Rollback();
                }
                writers.Clear();//此时说明已成功提交，清空writers数据
                return;
            }
            Rollback();
        }
        public void Rollback()
        {
            writers.Clear();//清空writers
            //执行反操作
            beforeDelete.ForEach(o =>
            {
                Collection.InsertOne(o);
            });
            beforeChange.ForEach(o =>
            {
                Collection.ReplaceOne(c => c.Id == o.Id, o);
            });
            beforeAdd.ForEach(o =>
            {
                Collection.DeleteOne(d => d.Id == o);
            });

        }
        #endregion

        #region 查询
        /// <summary>
        /// 查找所有数据集合
        /// </summary>
        /// <returns></returns>
        public IQueryable<TAggregate> FindAll()
        {
            return Collection.AsQueryable();
        }

        /// <summary>
        /// 根据Id查找一条数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TAggregate FindById(string id)
        {
            var find = Collection.Find(o => o.Id == id);
            if (!find.Any())
                return null;
            return find.FirstOrDefault();
        }

        /// <summary>
        /// 根据过滤条件找出符合条件的集合
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public List<TAggregate> FindByFilter(Expression<Func<TAggregate, bool>> filter)
        {
            var find = Collection.Find(filter);
            if (!find.Any())
                return new List<TAggregate>();
            return find.ToList();
        }

        /// <summary>
        /// 根据过滤条件找出一条数据
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public TAggregate FindOne(Expression<Func<TAggregate, bool>> filter)
        {
            return Collection.Find(filter).FirstOrDefault();
        }

        /// <summary>
        /// 根据过滤条件分页获取数据
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="pageIndex">从1开始记页</param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<TAggregate> FindPagerList(Expression<Func<TAggregate, bool>> filter, SortDefinition<TAggregate> sortFilter, int pageIndex, int pageSize)
        {
            var find = Collection.Find(filter).Sort(sortFilter).Skip((pageIndex - 1) * pageSize).Limit(pageSize);
            if (!find.Any())
                return new List<TAggregate>();
            return find.ToList();
        }

        public int Count(Expression<Func<TAggregate, bool>> filter)
        {
            var count = Collection.Count(filter);
            return (int)count;
        }

        #endregion

        #region GridFS
        /// <summary>
        /// 上传一个文件
        /// </summary>
        /// <param name="filePath">文件名（包含路径）</param>
        /// <param name="collectionName">集合名字(数据库表名)</param>
        /// <returns>返回一个ObjectId（字符串）</returns>
        public string UploadFile(string filePath, string collectionName)
        {
            if (File.Exists(filePath))
            {
                var fi = new FileInfo(filePath);
                FileStream fileStream = new FileStream(filePath, FileMode.Open);
                var bucket = new GridFSBucket(this.MongoDatabase, new GridFSBucketOptions
                {
                    BucketName = collectionName,
                    ChunkSizeBytes = 358400,
                    WriteConcern = WriteConcern.WMajority,
                    ReadPreference = ReadPreference.Secondary
                });
                var options = new GridFSUploadOptions
                {
                    ChunkSizeBytes = 358400,
                    Metadata = new BsonDocument { { "type", fi.Extension }, { "name", fi.Name }, { "copyright", true } }
                };
                var fieldid = bucket.UploadFromStream(fi.Name, fileStream, options);
                fileStream.Close();
                return fieldid.ToString();
            }
            else
            {
                return "error";
            }
        }

        #endregion

    }
}
