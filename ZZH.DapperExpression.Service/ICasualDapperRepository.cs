using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ZZH.DapperExpression.Service.Data;

namespace ZZH.DapperExpression.Service
{
    /// <summary>
    /// 表示一种基于 Dapper 的自由行很强的仓储接口
    /// </summary>
    public interface ICasualDapperRepository
    {
        /// <summary>
        /// 获取工作单元
        /// </summary>
        IUnitOfWork UnitOfWork { get; }

        #region Query

        /// <summary>
        /// 获取实体对象
        /// </summary>
        /// <param name="id">主键 ID, 以 ID 或 ID 结尾的默认为主键，选取最先匹配的那个</param>
        /// <returns></returns>
        [CanBeNull]
        TEntity Get<TEntity>([NotNull] object id) where TEntity : class;

        /// <summary>
        /// 获取实体对象
        /// </summary>
        /// <param name="id">主键 ID, 以 ID 或 ID 结尾的默认为主键，选取最先匹配的那个</param>
        /// <returns></returns>
        [CanBeNull]
        Task<TEntity> GetAsync<TEntity>([NotNull] object id) where TEntity : class;

        [CanBeNull]
        TEntity GetFirstOrDefault<TEntity>([NotNull] Expression<Func<TEntity, bool>> predicate) where TEntity : class;

        [CanBeNull]
        Task<TEntity> GetFirstOrDefaultAsync<TEntity>([NotNull] Expression<Func<TEntity, bool>> predicate) where TEntity : class;

        [CanBeNull]
        TEntity GetSingleOrDefault<TEntity>([NotNull] Expression<Func<TEntity, bool>> predicate) where TEntity : class;

        [CanBeNull]
        Task<TEntity> GetSingleOrDefaultAsync<TEntity>([NotNull] Expression<Func<TEntity, bool>> predicate) where TEntity : class;

        int Count<TEntity>([NotNull] Expression<Func<TEntity, bool>> predicate) where TEntity : class;

        Task<int> CountAsync<TEntity>([NotNull] Expression<Func<TEntity, bool>> predicate) where TEntity : class;

        [NotNull]
        IEnumerable<TEntity> GetAll<TEntity>() where TEntity : class;

        [NotNull]
        Task<IEnumerable<TEntity>> GetAllAsync<TEntity>() where TEntity : class;

        [NotNull]
        IEnumerable<TEntity> GetAll<TEntity>([NotNull] Expression<Func<TEntity, bool>> predicate) where TEntity : class;

        [NotNull]
        Task<IEnumerable<TEntity>> GetAllAsync<TEntity>([NotNull] Expression<Func<TEntity, bool>> predicate) where TEntity : class;

        /// <summary>
        /// 分页
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="predicate">筛选条件</param>
        /// <param name="pageNumber">起始页，从 0 开始</param>
        /// <param name="itemsPerPage">每页数目</param>
        /// <param name="sortingProperty">排序的字段</param>
        /// <param name="ascending">是否是 ACS 排序，默认为 true</param>
        /// <returns></returns>
        [NotNull]
        IEnumerable<TEntity> GetAllPaged<TEntity>([NotNull] Expression<Func<TEntity, bool>> predicate, int pageNumber, int itemsPerPage, [NotNull] string sortingProperty, bool ascending = true) where TEntity : class;

        /// <summary>
        /// 分页
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="predicate">筛选条件</param>
        /// <param name="pageNumber">起始页，从 0 开始</param>
        /// <param name="itemsPerPage">每页数目</param>
        /// <param name="sortingProperty">排序的字段</param>
        /// <param name="ascending">是否是 ACS 排序，默认为 true</param>
        /// <returns></returns>
        [NotNull]
        Task<IEnumerable<TEntity>> GetAllPagedAsync<TEntity>([NotNull] Expression<Func<TEntity, bool>> predicate, int pageNumber, int itemsPerPage, [NotNull] string sortingProperty, bool ascending = true) where TEntity : class;

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
        IEnumerable<TEntity> GetAllPaged<TEntity>([NotNull] Expression<Func<TEntity, bool>> predicate, int pageNumber, int itemsPerPage, bool ascending = true, params Expression<Func<TEntity, object>>[] sortingExpression) where TEntity : class;

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
        Task<IEnumerable<TEntity>> GetAllPagedAsync<TEntity>([NotNull] Expression<Func<TEntity, bool>> predicate, int pageNumber, int itemsPerPage, bool ascending = true, params Expression<Func<TEntity, object>>[] sortingExpression) where TEntity : class;


        /// <summary>
        /// 原生态 SQL 查询方式
        /// </summary>
        /// <param name="sql">要查询的 sql 语句</param>
        /// <param name="parameters">参数</param>
        [NotNull]
        IEnumerable<TEntity> Query<TEntity>([NotNull] string query, [CanBeNull] object parameters = null) where TEntity : class;

        /// <summary>
        /// 原生态 SQL 查询方式
        /// </summary>
        /// <param name="sql">要查询的 sql 语句</param>
        /// <param name="parameters">参数</param>
        [NotNull]
        Task<IEnumerable<TEntity>> QueryAsync<TEntity>([NotNull] string query, [CanBeNull] object parameters = null) where TEntity : class;

        #endregion

        #region Command

        /// <summary>
        /// 插入实体对象
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        TEntity Insert<TEntity>([NotNull] TEntity entity) where TEntity : class;

        /// <summary>
        /// 插入实体对象
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        Task<TEntity> InsertAsync<TEntity>([NotNull] TEntity entity) where TEntity : class;

        /// <summary>
        /// 插入实体对象集合
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entities">实体对象集合</param>
        /// <returns></returns>
        void Insert<TEntity>([NotNull] IEnumerable<TEntity> entities) where TEntity : class;

        /// <summary>
        /// 插入实体对象集合
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entities">实体对象结合 </param>
        /// <returns></returns>
        Task InsertAsync<TEntity>([NotNull] IEnumerable<TEntity> entities) where TEntity : class;

        /// <summary>
        /// 更新实体对象
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity">要更新的实体对象</param>
        /// <returns></returns>
        TEntity Update<TEntity>([NotNull] TEntity entity) where TEntity : class;

        /// <summary>
        /// 更新实体对象
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity">要更新的实体对象</param>
        /// <returns></returns>
        Task<TEntity> UpdateAsync<TEntity>([NotNull] TEntity entity) where TEntity : class;

        /// <summary>
        /// 删除实体对象
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity">要删除的实体对象</param>
        void Delete<TEntity>([NotNull] TEntity entity) where TEntity : class;

        /// <summary>
        /// 删除实体对象
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity">要删除的实体对象</param>
        /// <returns></returns>
        Task DeleteAsync<TEntity>([NotNull] TEntity entity) where TEntity : class;

        /// <summary>
        /// 删除实体对象
        /// </summary>
        /// <param name="id">主键 ID, 以 ID 或 ID 结尾的默认为主键，选取最先匹配的那个</param>
        /// <returns></returns>
        void Delete<TEntity>([NotNull] object id) where TEntity : class;

        /// <summary>
        /// 删除实体对象
        /// </summary>
        /// <param name="id">主键 ID, 以 ID 或 ID 结尾的默认为主键，选取最先匹配的那个</param>
        /// <returns></returns>
        Task DeleteAsync<TEntity>([NotNull] object id) where TEntity : class;


        /// <summary>
        /// 原生态 SQL 执行方式
        /// </summary>
        /// <param name="sql">要执行的 sql 语句</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        int Execute([NotNull] string sql, [CanBeNull] object parameters = null);

        /// <summary>
        /// 原生态 SQL 执行方式
        /// </summary>
        /// <param name="sql">要执行的 sql 语句</param>
        /// <param name="parameters">参数</param>
        Task<int> ExecuteAsync([NotNull] string sql, [CanBeNull] object parameters = null);

        #endregion
    }
}
