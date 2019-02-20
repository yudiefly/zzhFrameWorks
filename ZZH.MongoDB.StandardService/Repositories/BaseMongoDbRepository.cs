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
    /// 表示仓储基类
    /// </summary>
    public abstract class BaseMongoDbRepository<TEntity, TPrimaryKey> : IMongoDbRepository<TEntity, TPrimaryKey>
         where TEntity : class, IMongoDbEntity<TPrimaryKey>
    {
        #region Query

        public abstract IQueryable<TEntity> GetAll();

        public virtual List<TEntity> GetAllList()
        {
            return GetAll().ToList();
        }

        public virtual Task<List<TEntity>> GetAllListAsync()
        {
            return Task.FromResult(GetAllList());
        }

        public virtual List<TEntity> GetAllList(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().Where(predicate).ToList();
        }

        public virtual Task<List<TEntity>> GetAllListAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return Task.FromResult(GetAllList(predicate));
        }

        public virtual TEntity Get(TPrimaryKey id)
        {
            var entity = FirstOrDefault(id);
            if (entity == null)
            {
                throw new EntityNotFoundException(typeof(TEntity), id);
            }

            return entity;
        }

        public virtual async Task<TEntity> GetAsync(TPrimaryKey id)
        {
            var entity = await FirstOrDefaultAsync(id);
            if (entity == null)
            {
                throw new EntityNotFoundException(typeof(TEntity), id);
            }

            return entity;
        }

        public virtual TEntity FirstOrDefault(TPrimaryKey id)
        {
            return GetAll().FirstOrDefault(CreateEqualityExpressionForId(id));
        }

        public virtual Task<TEntity> FirstOrDefaultAsync(TPrimaryKey id)
        {
            return Task.FromResult(FirstOrDefault(id));
        }

        public virtual TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().FirstOrDefault(predicate);
        }

        public Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return Task.FromResult(FirstOrDefault(predicate));
        }

        public virtual TEntity Single(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().Single(predicate);
        }

        public virtual Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return Task.FromResult(Single(predicate));
        }

        public virtual IEnumerable<TEntity> GetAllPaged(Expression<Func<TEntity, bool>> predicate, int pageNumber, int itemsPerPage, Expression<Func<TEntity, object>> sortingProperty, bool ascending = true)
        {
            IOrderedQueryable<TEntity> orderedQuery = null;
            if (ascending)
            {
                orderedQuery = GetAll().Where(predicate).OrderBy(sortingProperty);
            }
            else
            {
                orderedQuery = GetAll().Where(predicate).OrderByDescending(sortingProperty);
            }

            return orderedQuery.Skip((pageNumber + 1) * itemsPerPage)
                               .Take(itemsPerPage);
        }

        public virtual Task<IEnumerable<TEntity>> GetAllPagedAsync(Expression<Func<TEntity, bool>> predicate, int pageNumber, int itemsPerPage, bool ascending = true, params Expression<Func<TEntity, object>>[] sortingExpression)
        {
            IOrderedQueryable<TEntity> orderedQuery = null;
            if (ascending)
            {
                orderedQuery = GetAll().Where(predicate).OrderBy(sortingExpression[0]);
                for (int i = 1; i < sortingExpression.Length; i++)
                {
                    orderedQuery = orderedQuery.OrderBy(sortingExpression[i]);
                }
            }
            else
            {
                orderedQuery = GetAll().Where(predicate).OrderByDescending(sortingExpression[0]);
                for (int i = 1; i < sortingExpression.Length; i++)
                {
                    orderedQuery = orderedQuery.OrderByDescending(sortingExpression[i]);
                }
            }

            var entities = orderedQuery.Skip((pageNumber + 1) * itemsPerPage)
                                       .Take(itemsPerPage)
                                       .AsEnumerable();

            return Task.FromResult(entities);
        }

        public virtual int Count()
        {
            return GetAll().Count();
        }

        public virtual Task<int> CountAsync()
        {
            return Task.FromResult(Count());
        }

        public virtual int Count(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().Count(predicate);
        }

        public virtual Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return Task.FromResult(Count(predicate));
        }

        public virtual long LongCount()
        {
            return GetAll().LongCount();
        }

        public virtual Task<long> LongCountAsync()
        {
            return Task.FromResult(LongCount());
        }

        public virtual long LongCount(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().LongCount(predicate);
        }

        public virtual Task<long> LongCountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return Task.FromResult(LongCount(predicate));
        }

        #endregion

        #region Command

        public abstract TEntity Insert(TEntity entity);

        public virtual Task<TEntity> InsertAsync(TEntity entity)
        {
            return Task.FromResult(Insert(entity));
        }

        public abstract void InsertMany(IEnumerable<TEntity> entities);

        public virtual Task InsertManyAsync(IEnumerable<TEntity> entities)
        {
            InsertMany(entities);
            return Task.FromResult(0);
        }

        public abstract TEntity Update(TEntity entity);

        public virtual Task<TEntity> UpdateAsync(TEntity entity)
        {
            return Task.FromResult(Update(entity));
        }

        public abstract void Delete(TEntity entity);

        public virtual Task DeleteAsync(TEntity entity)
        {
            Delete(entity);
            return Task.FromResult(0);
        }

        public abstract void Delete(TPrimaryKey id);

        public virtual Task DeleteAsync(TPrimaryKey id)
        {
            Delete(id);
            return Task.FromResult(0);
        }

        public virtual void Delete(Expression<Func<TEntity, bool>> predicate)
        {
            foreach (var entity in GetAllList(predicate))
            {
                Delete(entity);
            }
        }

        public Task DeleteAsync(Expression<Func<TEntity, bool>> predicate)
        {
            Delete(predicate);
            return Task.FromResult(0);
        }

        #endregion

        protected virtual Expression<Func<TEntity, object>> ConvertToPropertyExpression(string propertyName)
        {
            var lambdaParam = Expression.Parameter(typeof(TEntity));
            var lambdaBody = Expression.Property(lambdaParam, propertyName);
            return Expression.Lambda<Func<TEntity, object>>(lambdaBody, lambdaParam);
        }

        protected virtual Expression<Func<TEntity, bool>> CreateEqualityExpressionForId(TPrimaryKey id)
        {
            var lambdaParam = Expression.Parameter(typeof(TEntity));

            var lambdaBody = Expression.Equal(
                Expression.PropertyOrField(lambdaParam, nameof(IMongoDbEntity<TPrimaryKey>.Id)),
                Expression.Constant(id, typeof(TPrimaryKey))
                );

            return Expression.Lambda<Func<TEntity, bool>>(lambdaBody, lambdaParam);
        }
    }
}
