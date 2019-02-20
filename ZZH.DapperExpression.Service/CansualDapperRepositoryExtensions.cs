using Dapper;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using ZZH.DapperExpression.Service.Data;

namespace ZZH.DapperExpression.Service
{
    /// <summary>
    /// <see cref="CasualDapperRepository"/> 扩展
    /// </summary>
    public static class CansualDapperRepositoryExtensions
    {
        /// <summary>
        /// 执行带返回值的存储过程
        /// </summary>
        /// <param name="procedureName">存储过程名称</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        [NotNull]
        public static IEnumerable<TEntity> ExecuteProcedure<TDbContext, TEntity>([NotNull] this CasualDapperRepository<TDbContext> repository, [NotNull] string procedureName, [CanBeNull] object parameters = null)
            where TDbContext : ActiveDbContext
            where TEntity : class
        {
            return repository.Connection.Query<TEntity>(procedureName, parameters, transaction: repository.ActiveTransaction, commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// 执行带返回值的存储过程
        /// </summary>
        /// <param name="procedureName">存储过程名称</param>
        /// <param name="parameters">参数</param>
        [CanBeNull]
        public static Task<IEnumerable<TEntity>> ExecuteProcedureAsync<TDbContext, TEntity>([NotNull] this CasualDapperRepository<TDbContext> repository, [NotNull] string procedureName, [CanBeNull] object parameters = null)
            where TDbContext : ActiveDbContext
            where TEntity : class
        {
            return repository.Connection.QueryAsync<TEntity>(procedureName, parameters, transaction: repository.ActiveTransaction, commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// 执行没有返回值的存储过程
        /// </summary>
        /// <param name="procedureName">存储过程名称</param>
        /// <param name="parameters">参数</param>
        public static void ExecuteProcedure<TDbContext>([NotNull] this CasualDapperRepository<TDbContext> repository, [NotNull] string procedureName, [CanBeNull] object parameters = null)
             where TDbContext : ActiveDbContext
        {
            repository.Connection.Execute(procedureName, parameters, transaction: repository.ActiveTransaction, commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// 执行没有返回值的存储过程
        /// </summary>
        /// <param name="procedureName">存储过程名称</param>
        /// <param name="parameters">参数</param>
        public static Task ExecuteProcedureAsync<TDbContext>([NotNull] this CasualDapperRepository<TDbContext> repository, [NotNull] string procedureName, [CanBeNull] object parameters = null)
             where TDbContext : ActiveDbContext
        {
            return repository.Connection.ExecuteAsync(procedureName, parameters, transaction: repository.ActiveTransaction, commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// 查询对象
        /// </summary>
        /// <typeparam name="TDbContext">DB 上下文</typeparam>
        /// <typeparam name="TFirst">映射的第一个对象类型</typeparam>
        /// <typeparam name="TSecond">映射的第二个对象类型</typeparam>
        /// <typeparam name="TReturn">最终返回的对象类型</typeparam>
        /// <param name="repository"></param>
        /// <param name="sql">查询语句</param>
        /// <param name="map">映射关系</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        public static IEnumerable<TReturn> Query<TDbContext, TFirst, TSecond, TReturn>([NotNull] this CasualDapperRepository<TDbContext> repository, [NotNull] string sql, [NotNull] Func<TFirst, TSecond, TReturn> map, [CanBeNull] object parameters = null)
            where TDbContext : ActiveDbContext
        {
            return repository.Connection.Query(sql, map, parameters, transaction: repository.ActiveTransaction);
        }

        /// <summary>
        /// 查询对象
        /// </summary>
        /// <typeparam name="TDbContext">DB 上下文</typeparam>
        /// <typeparam name="TFirst">映射的第一个对象类型</typeparam>
        /// <typeparam name="TSecond">映射的第二个对象类型</typeparam>
        /// <typeparam name="TReturn">最终返回的对象类型</typeparam>
        /// <param name="repository"></param>
        /// <param name="sql">查询语句</param>
        /// <param name="map">映射关系</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        public static Task<IEnumerable<TReturn>> QueryAsync<TDbContext, TFirst, TSecond, TReturn>([NotNull] this CasualDapperRepository<TDbContext> repository, [NotNull] string sql, [NotNull] Func<TFirst, TSecond, TReturn> map, [CanBeNull] object parameters = null)
         where TDbContext : ActiveDbContext
        {
            return repository.Connection.QueryAsync(sql, map, parameters, transaction: repository.ActiveTransaction);
        }

        /// <summary>
        /// 查询对象
        /// </summary>
        /// <typeparam name="TDbContext">DB 上下文</typeparam>
        /// <typeparam name="TFirst">映射的第一个对象类型</typeparam>
        /// <typeparam name="TSecond">映射的第二个对象类型</typeparam>
        /// <typeparam name="TThird">映射的第三个对象类型</typeparam>
        /// <typeparam name="TReturn">最终返回的对象类型</typeparam>
        /// <param name="repository"></param>
        /// <param name="sql">查询语句</param>
        /// <param name="map">映射关系</param>
        /// <param name="parameters">参数</param>
        public static IEnumerable<TReturn> Query<TDbContext, TFirst, TSecond, TThird, TReturn>([NotNull] this CasualDapperRepository<TDbContext> repository, [NotNull] string sql, [NotNull] Func<TFirst, TSecond, TThird, TReturn> map, [CanBeNull] object parameters = null)
            where TDbContext : ActiveDbContext
        {
            return repository.Connection.Query(sql, map, parameters, transaction: repository.ActiveTransaction);
        }

        /// <summary>
        /// 查询对象
        /// </summary>
        /// <typeparam name="TDbContext">DB 上下文</typeparam>
        /// <typeparam name="TFirst">映射的第一个对象类型</typeparam>
        /// <typeparam name="TSecond">映射的第二个对象类型</typeparam>
        /// <typeparam name="TThird">映射的第三个对象类型</typeparam>
        /// <typeparam name="TReturn">最终返回的对象类型</typeparam>
        /// <param name="repository"></param>
        /// <param name="sql">查询语句</param>
        /// <param name="map">映射关系</param>
        /// <param name="parameters">参数</param>
        public static Task<IEnumerable<TReturn>> QueryAsync<TDbContext, TFirst, TSecond, TThird, TReturn>([NotNull] this CasualDapperRepository<TDbContext> repository, [NotNull] string sql, [NotNull] Func<TFirst, TSecond, TThird, TReturn> map, [CanBeNull] object parameters = null)
           where TDbContext : ActiveDbContext
        {
            return repository.Connection.QueryAsync(sql, map, parameters, transaction: repository.ActiveTransaction);
        }
    }
}
