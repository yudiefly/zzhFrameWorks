using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ZZH.MongoDB.StandardService.Entities;

namespace ZZH.MongoDB.StandardService.Repositories
{
    /// <summary>
    /// 实体仓储接口, 主键类型为 <see cref="Guid"/>
    /// </summary>
    public interface IMongoDbRepository<TEntity> : IMongoDbRepository<TEntity, Guid>
        where TEntity : class, IMongoDbEntity
    {

    }

    /// <summary>
    /// 实体仓储接口
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TPrimaryKey">实体主键类型</typeparam>
    public interface IMongoDbRepository<TEntity, TPrimaryKey> where TEntity : class, IMongoDbEntity<TPrimaryKey>
    {
        #region Query

        /// <summary>
        /// 获取用于从 DB 检索的 IQueryable 对象
        /// </summary>
        /// <returns>IQueryable</returns>
        IQueryable<TEntity> GetAll();

        /// <summary>
        /// 获取全部的实体对象
        /// </summary>
        /// <returns>全部实体集合</returns>
        [NotNull]
        List<TEntity> GetAllList();

        /// <summary>
        /// 获取全部的实体对象
        /// </summary>
        /// <returns>全部实体对象集合</returns>
        [NotNull]
        Task<List<TEntity>> GetAllListAsync();

        /// <summary>
        /// 通过指定的筛选条件获取所有的实体对象
        /// </summary>
        /// <param name="predicate">筛选条件</param>
        /// <returns>全部实体对象集合</returns>
        [NotNull]
        List<TEntity> GetAllList([NotNull] Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 通过指定的筛选条件获取所有的实体对象
        /// </summary>
        /// <param name="predicate">筛选条件</param>
        /// <returns>全部实体对象集合</returns>
        [NotNull]
        Task<List<TEntity>> GetAllListAsync([NotNull] Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 通过主键获取实体
        /// </summary>
        /// <param name="id">用户获取实体的主键</param>
        /// <returns>Entity</returns>
        /// <exception cref="EntityNotFoundException"></exception>
        [NotNull]
        TEntity Get([NotNull] TPrimaryKey id);

        /// <summary>
        /// 通过主键获取实体
        /// </summary>
        /// <param name="id">用户获取实体的主键</param>
        /// <returns>Entity</returns>
        /// <exception cref="EntityNotFoundException"></exception>
        [NotNull]
        Task<TEntity> GetAsync([NotNull] TPrimaryKey id);

        /// <summary>
        /// 通过指定的筛选条件获取唯一的实体。
        /// 若没有实体或实体多与一个会抛出异常
        /// </summary>
        /// <param name="predicate">Entity</param>
        [NotNull]
        TEntity Single([NotNull] Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 通过指定的筛选条件获取唯一的实体。
        /// 若没有实体或实体多与一个会抛出异常
        /// </summary>
        /// <param name="predicate">Entity</param>
        [NotNull]
        Task<TEntity> SingleAsync([NotNull] Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 通过指定的主键获取实体。
        /// </summary>
        /// <param name="id">主键 Id</param>
        /// <returns>Entity or null</returns>
        [CanBeNull]
        TEntity FirstOrDefault([NotNull] TPrimaryKey id);

        /// <summary>
        /// 通过指定的主键获取实体。
        /// </summary>
        /// <param name="id">主键 Id</param>
        /// <returns>Entity or null</returns>
        [NotNull]
        Task<TEntity> FirstOrDefaultAsync([NotNull] TPrimaryKey id);

        /// <summary>
        /// 通过指定的筛选条件获取实体。
        /// </summary>
        /// <param name="predicate">筛选条件</param>
        [CanBeNull]
        TEntity FirstOrDefault([NotNull] Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 通过指定的筛选条件获取实体。
        /// </summary>
        /// <param name="predicate">筛选条件</param>
        [CanBeNull]
        Task<TEntity> FirstOrDefaultAsync([NotNull] Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="predicate">筛选条件</param>
        /// <param name="pageNumber">起始页，从 0 开始</param>
        /// <param name="itemsPerPage">每页数目</param>
        /// <param name="sortingProperty">排序的字段</param>
        /// <param name="ascending">是否是 ACS 排序，默认为 true</param>
        /// <returns></returns>
        [NotNull]
        IEnumerable<TEntity> GetAllPaged([NotNull] Expression<Func<TEntity, bool>> predicate, int pageNumber, int itemsPerPage, [NotNull] Expression<Func<TEntity, object>> sortingProperty, bool ascending = true);

        /// <summary>
        /// 分页
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="predicate">筛选条件</param>
        /// <param name="pageNumber">起始页，从 0 开始</param>
        /// <param name="itemsPerPage">每页数目</param>
        /// <param name="ascending">是否是 ACS 排序，默认为 true</param>
        /// <param name="sortingExpression">要排序的字段集合</param>
        /// <returns></returns>
        [NotNull]
        Task<IEnumerable<TEntity>> GetAllPagedAsync([NotNull] Expression<Func<TEntity, bool>> predicate, int pageNumber, int itemsPerPage, bool ascending = true, [NotNull] params Expression<Func<TEntity, object>>[] sortingExpression);

        /// <summary>
        /// 获取所有的实体数量
        /// </summary>
        /// <returns>实体数量</returns>
        int Count();

        /// <summary>
        /// 获取所有的实体数量
        /// </summary>
        /// <returns>实体数量</returns>
        Task<int> CountAsync();

        /// <summary>
        /// 通过指定的筛选条件获取实体数量
        /// </summary>
        /// <param name="predicate">筛选条件</param>
        /// <returns>实体数量</returns>
        int Count([NotNull] Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 通过指定的筛选条件获取实体数量
        /// </summary>
        /// <param name="predicate">筛选条件</param>
        /// <returns>实体数量</returns>
        Task<int> CountAsync([NotNull] Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 通过指定的筛选条件获取实体数量。
        /// 若数量大于 <see cref="int.MaxValue"/> 使用这个。
        /// </summary>
        /// <returns>实体数量</returns>
        long LongCount();

        /// <summary>
        /// 通过指定的筛选条件获取实体数量。
        /// 若数量大于 <see cref="int.MaxValue"/> 使用这个。
        /// </summary>
        /// <returns>实体数量</returns>
        Task<long> LongCountAsync();

        /// <summary>
        /// 通过指定的筛选条件获取实体数量。
        /// 若数量大于 <see cref="int.MaxValue"/> 使用这个。
        /// </summary>
        /// <param name="predicate">筛选条件</param>
        /// <returns>实体数量</returns>
        long LongCount([NotNull] Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 通过指定的筛选条件获取实体数量。
        /// 若数量大于 <see cref="int.MaxValue"/> 使用这个。
        /// </summary>
        /// <param name="predicate">筛选条件</param>
        /// <returns>实体数量</returns>
        Task<long> LongCountAsync([NotNull] Expression<Func<TEntity, bool>> predicate);

        #endregion

        #region Command

        /// <summary>
        /// 插入一个新的实体
        /// </summary>
        /// <param name="entity">要插入的实体</param>
        [NotNull]
        TEntity Insert([NotNull] TEntity entity);

        /// <summary>
        /// 插入一个新的实体
        /// </summary>
        /// <param name="entity">要插入的实体</param>
        [NotNull]
        Task<TEntity> InsertAsync([NotNull] TEntity entity);

        /// <summary>
        /// 插入集合
        /// </summary>
        /// <param name="entities">要插入的集合</param>
        void InsertMany([NotNull] IEnumerable<TEntity> entities);

        /// <summary>
        /// 插入集合
        /// </summary>
        /// <param name="entities">要插入的集合</param>
        /// <returns></returns>
        Task InsertManyAsync([NotNull] IEnumerable<TEntity> entities);

        /// <summary>
        /// 更新一个已存在的实体
        /// </summary>
        /// <param name="entity">Entity</param>
        [NotNull]
        TEntity Update([NotNull] TEntity entity);

        /// <summary>
        /// 更新一个已存在的实体
        /// </summary>
        /// <param name="entity">Entity</param>
        [NotNull]
        Task<TEntity> UpdateAsync([NotNull] TEntity entity);

        /// <summary>
        /// 删除一个实体
        /// </summary>
        /// <param name="entity">要删除的实体</param>
        void Delete([NotNull] TEntity entity);

        /// <summary>
        /// 删除一个实体
        /// </summary>
        /// <param name="entity">要删除的实体</param>
        Task DeleteAsync([NotNull] TEntity entity);

        /// <summary>
        /// 通过主键删除一个实体
        /// </summary>
        /// <param name="id">实体主键</param>
        void Delete([NotNull] TPrimaryKey id);

        /// <summary>
        /// 通过主键删除一个实体
        /// </summary>
        /// <param name="id">实体主键</param>
        Task DeleteAsync([NotNull] TPrimaryKey id);

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="predicate">筛选实体的条件</param>
        void Delete([NotNull] Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="predicate">筛选实体的条件</param>
        Task DeleteAsync([NotNull] Expression<Func<TEntity, bool>> predicate);

        #endregion
    }
}
