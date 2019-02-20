using Dapper;
using DapperExtensions;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ZZH.DapperExpression.Service.Data;
using ZZH.DapperExpression.Service.Extensions;
using ZZH.DapperExpression.Service.Linq.Builder;

namespace ZZH.DapperExpression.Service
{
    /// <summary>
    /// 基于 Dapper 的自由性很强的仓储基类。
    /// 注：使用 this 可调用相关扩展方法。
    /// </summary>
    public abstract class CasualDapperRepository<TDbContext> : ICasualDapperRepository
        where TDbContext : ActiveDbContext
    {
        private readonly bool _useTransaction;

        /// <summary>
        /// DB 上下文对象
        /// </summary>
        public TDbContext Context { get; }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="dbContext">DB 上下文</param>
        /// <param name="useTransaction">是否使用事务，默认为 false</param>
        protected CasualDapperRepository(TDbContext dbContext, bool useTransaction = false)
        {
            Context = dbContext;
            _useTransaction = useTransaction;
        }

        // use for internal extension
        internal DbConnection Connection => Context.ActiveConnection;

        // use for internal extension
        internal DbTransaction ActiveTransaction => _useTransaction ? Context.ActiveTransaction : null;

        public IUnitOfWork UnitOfWork => Context;

        #region Query

        public TEntity Get<TEntity>(object id) where TEntity : class
        {
            return Connection.Get<TEntity>(id, transaction: ActiveTransaction);
        }

        public Task<TEntity> GetAsync<TEntity>(object id) where TEntity : class
        {
            var entity = Get<TEntity>(id);
            return Task.FromResult(entity);
        }

        public TEntity GetFirstOrDefault<TEntity>(Expression<Func<TEntity, bool>> predicate)
            where TEntity : class
        {
            var pg = predicate.ToPredicateGroup();
            return Connection.GetList<TEntity>(pg, transaction: ActiveTransaction).FirstOrDefault();
        }

        public Task<TEntity> GetFirstOrDefaultAsync<TEntity>(Expression<Func<TEntity, bool>> predicate)
            where TEntity : class
        {
            var entity = GetFirstOrDefault(predicate);
            return Task.FromResult(entity);
        }

        public TEntity GetSingleOrDefault<TEntity>(Expression<Func<TEntity, bool>> predicate)
            where TEntity : class
        {
            var pg = predicate.ToPredicateGroup();
            return Connection.GetList<TEntity>(pg, transaction: ActiveTransaction).SingleOrDefault();
        }

        public Task<TEntity> GetSingleOrDefaultAsync<TEntity>(Expression<Func<TEntity, bool>> predicate)
            where TEntity : class
        {
            var entity = GetSingleOrDefault(predicate);
            return Task.FromResult(entity);
        }

        public int Count<TEntity>(Expression<Func<TEntity, bool>> predicate)
            where TEntity : class
        {
            var pg = predicate.ToPredicateGroup();
            return Connection.Count<TEntity>(pg, transaction: ActiveTransaction);
        }

        public Task<int> CountAsync<TEntity>(Expression<Func<TEntity, bool>> predicate)
            where TEntity : class
        {
            var count = Count(predicate);
            return Task.FromResult(count);
        }

        public IEnumerable<TEntity> GetAll<TEntity>()
            where TEntity : class
        {
            return Connection.GetList<TEntity>(transaction: ActiveTransaction);
        }

        public Task<IEnumerable<TEntity>> GetAllAsync<TEntity>()
            where TEntity : class
        {
            var entities = GetAll<TEntity>();
            return Task.FromResult(entities);
        }

        public IEnumerable<TEntity> GetAll<TEntity>(Expression<Func<TEntity, bool>> predicate)
            where TEntity : class
        {
            var pg = predicate.ToPredicateGroup();
            return Connection.GetList<TEntity>(pg, transaction: ActiveTransaction);
        }

        public Task<IEnumerable<TEntity>> GetAllAsync<TEntity>(Expression<Func<TEntity, bool>> predicate)
            where TEntity : class
        {
            var entities = GetAll(predicate);
            return Task.FromResult(entities);
        }

        public IEnumerable<TEntity> GetAllPaged<TEntity>(Expression<Func<TEntity, bool>> predicate, int pageNumber, int itemsPerPage, string sortingProperty, bool ascending = true)
            where TEntity : class
        {
            var pg = predicate.ToPredicateGroup();
            return Connection.GetPage<TEntity>(
                pg,
                new List<ISort> { new Sort { Ascending = ascending, PropertyName = sortingProperty } },
                pageNumber,
                itemsPerPage,
                transaction: ActiveTransaction);
        }

        public Task<IEnumerable<TEntity>> GetAllPagedAsync<TEntity>(Expression<Func<TEntity, bool>> predicate, int pageNumber, int itemsPerPage, string sortingProperty, bool ascending = true)
            where TEntity : class
        {
            var entities = GetAllPaged(predicate, pageNumber, itemsPerPage, sortingProperty, ascending);
            return Task.FromResult(entities);
        }

        public IEnumerable<TEntity> GetAllPaged<TEntity>(Expression<Func<TEntity, bool>> predicate, int pageNumber, int itemsPerPage, bool ascending = true, params Expression<Func<TEntity, object>>[] sortingExpression)
            where TEntity : class
        {
            var pg = predicate.ToPredicateGroup();
            return Connection.GetPage<TEntity>(pg, sortingExpression.ToSortable(ascending), pageNumber, itemsPerPage, transaction: ActiveTransaction);
        }

        public Task<IEnumerable<TEntity>> GetAllPagedAsync<TEntity>(Expression<Func<TEntity, bool>> predicate, int pageNumber, int itemsPerPage, bool ascending = true, params Expression<Func<TEntity, object>>[] sortingExpression)
            where TEntity : class
        {
            var entities = GetAllPaged(predicate, pageNumber, itemsPerPage, ascending, sortingExpression);
            return Task.FromResult(entities);
        }


        public IEnumerable<TEntity> Query<TEntity>(string sql, object parameters = null)
            where TEntity : class
        {
            return Connection.Query<TEntity>(sql, parameters, transaction: ActiveTransaction);
        }

        public Task<IEnumerable<TEntity>> QueryAsync<TEntity>(string sql, object parameters = null)
            where TEntity : class
        {
            return Connection.QueryAsync<TEntity>(sql, parameters, transaction: ActiveTransaction);
        }

        #endregion

        #region Command

        public TEntity Insert<TEntity>(TEntity entity)
            where TEntity : class
        {
            Connection.Insert(entity, ActiveTransaction);
            return entity;
        }

        public Task<TEntity> InsertAsync<TEntity>(TEntity entity)
            where TEntity : class
        {
            var e = Insert(entity);
            return Task.FromResult(e);
        }

        public void Insert<TEntity>(IEnumerable<TEntity> entities)
            where TEntity : class
        {
            Connection.Insert(entities, ActiveTransaction);
        }

        public Task InsertAsync<TEntity>(IEnumerable<TEntity> entities)
            where TEntity : class
        {
            Insert(entities);
            return Task.FromResult(0);
        }

        /// <summary>
        /// 更新实体对象
        /// 注：会更新实体的所有字段
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity">要更新的实体</param>
        /// <returns></returns>
        public TEntity Update<TEntity>(TEntity entity)
            where TEntity : class
        {
            Connection.Update(entity, ActiveTransaction);
            return entity;
        }

        /// <summary>
        /// 更新实体对象
        /// 注：会更新实体的所有字段
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity">要更新的实体</param>
        /// <returns></returns>
        public Task<TEntity> UpdateAsync<TEntity>(TEntity entity)
            where TEntity : class
        {
            var e = Update(entity);
            return Task.FromResult(e);
        }

        public void Delete<TEntity>(TEntity entity)
            where TEntity : class
        {
            Connection.Delete(entity, ActiveTransaction);
        }

        public Task DeleteAsync<TEntity>(TEntity entity)
            where TEntity : class
        {
            Delete(entity);
            return Task.FromResult(0);
        }

        public void Delete<TEntity>(object id)
            where TEntity : class
        {
            var entity = Get<TEntity>(id);
            if (entity != null)
            {
                Delete(entity);
            }
        }

        public Task DeleteAsync<TEntity>(object id)
            where TEntity : class
        {
            Delete<TEntity>(id);
            return Task.FromResult(0);
        }


        public int Execute(string sql, object parameters = null)
        {
            return Connection.Execute(sql, parameters, ActiveTransaction);
        }

        public Task<int> ExecuteAsync(string sql, object parameters = null)
        {
            return Connection.ExecuteAsync(sql, parameters, ActiveTransaction);
        }

        #endregion
    }
}
