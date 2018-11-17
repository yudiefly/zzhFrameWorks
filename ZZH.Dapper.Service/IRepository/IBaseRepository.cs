using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZZH.Dapper.Service.IRepository
{
    /// <summary>
    /// 基础仓储服务接口
    /// </summary>
    public interface IBaseRepository
    {
        /// <summary>
        /// 执行增（INSERT）语句【执行单条SQL语句，返回自增列】【同步】
        /// </summary>
        /// <param name="sql">SQL语句（本方法会自动在SQL的结尾增加分号，并增加查询ID的语句）</param>
        /// <param name="param">参数</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <returns>自增ID</returns>
        int Insert(string sql, object param = null, int? commandTimeout = null);

        /// <summary>
        /// 执行增（INSERT）语句【执行单条SQL语句，返回自增列】【异步】
        /// </summary>
        /// <param name="sql">SQL语句（本方法会自动在SQL的结尾增加分号，并增加查询ID的语句）</param>
        /// <param name="param">参数</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <returns>自增ID</returns>
        Task<int> InsertAsync(string sql, object param = null, int? commandTimeout = null);

        /// <summary>
        /// 执行增（INSERT）删（DELETE）改（UPDATE）语句或存储过程【单条SQL语句，可以获得输出参数】【同步】
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="param">参数</param>
        /// <param name="outParam">输出参数[参数名(@name)] </param>
        /// <param name="commandType">如何解释命令字符串</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <returns>Tuple(受影响的行数, Dictionary(输出参数名(@name), 输出参数))</returns>
        Tuple<int, Dictionary<string, string>> Execute(string sql, DynamicParameters param, List<string> outParam, CommandType? commandType = null, int? commandTimeout = null);

        /// <summary>
        /// 执行增（INSERT）删（DELETE）改（UPDATE）语句或存储过程【单条SQL语句，可以获得输出参数】【异步】
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="param">参数</param>
        /// <param name="outParam">输出参数[参数名(@name)] </param>
        /// <param name="commandType">如何解释命令字符串</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <returns>Tuple(受影响的行数, Dictionary(输出参数名(@name), 输出参数))</returns>
        Task<Tuple<int, Dictionary<string, string>>> ExecuteAsync(string sql, DynamicParameters param, List<string> outParam, CommandType? commandType = null, int? commandTimeout = null);

        /// <returns></returns>
        /// <summary>
        /// 执行增（INSERT）删（DELETE）改（UPDATE）语句【执行单条SQL语句】【同步】
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="param">参数</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <returns>受影响的行数</returns>
        int Execute(string sql, object param = null, int? commandTimeout = null);

        /// <summary>
        /// 执行增（INSERT）删（DELETE）改（UPDATE）语句【执行单条SQL语句】【异步】
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="param">参数</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <returns>受影响的行数</returns>
        Task<int> ExecuteAsync(string sql, object param = null, int? commandTimeout = null);

        /// <summary>
        /// 执行增（INSERT）删（DELETE）改（UPDATE）语句【带事务，可以同时执行多条SQL】【同步】
        /// </summary>
        /// <param name="sqlDic">SQL + 参数【key：sql语句 value：参数】</param>ConnectionString
        /// <param name="commandTimeout">超时时间</param>
        /// <returns></returns>
        void ExecuteToTransaction(Dictionary<string, object> sqlDic, int? commandTimeout = null);

        /// <summary>
        /// 执行增（INSERT）删（DELETE）改（UPDATE）语句【带事务，可以同时执行多条SQL】【异步】【待测试】
        /// </summary>
        /// <param name="sqlDic">SQL + 参数【key：sql语句 value：参数】</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <returns></returns>
        Task ExecuteToTransactionAsync(Dictionary<string, object> sqlDic, int? commandTimeout = null);

        /// <summary>
        /// 执行分页存储过程（Procdeure）【同步】
        /// </summary>
        /// <typeparam name="T">返回结果的类型</typeparam>
        /// <param name="param">参数</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <returns></returns>
        Tuple<IEnumerable<T>, int> ExecuteToPaginationProcdeure<T>(DynamicParameters param = null, int? commandTimeout = null);

        /// <summary>
        /// 执行分页存储过程（Procdeure）【异步】
        /// </summary>
        /// <typeparam name="T">返回结果的类型</typeparam>
        /// <param name="param">参数</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <returns></returns>
        Task<Tuple<IEnumerable<T>, int>> ExecuteToPaginationProcdeureAsync<T>(DynamicParameters param = null, int? commandTimeout = null);

        /// <summary>
        /// 执行存储过程（Procdeure）【同步】
        /// </summary>
        /// <typeparam name="T">返回结果的类型</typeparam>
        /// <param name="porcdeureName">存储过程名称</param>
        /// <param name="param">参数</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <returns></returns>
        IEnumerable<T> ExecuteToProcdeure<T>(string porcdeureName, object param = null, int? commandTimeout = null);

        /// <summary>
        /// 执行存储过程（Procdeure）【异步】
        /// </summary>
        /// <typeparam name="T">返回结果的类型</typeparam>
        /// <param name="porcdeureName">存储过程名称</param>
        /// <param name="param">参数</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <returns></returns>
        Task<IEnumerable<T>> ExecuteToProcdeureAsync<T>(string porcdeureName, object param = null, int? commandTimeout = null);

        /// <summary>
        /// 执行查询【同步】
        /// </summary>
        /// <typeparam name="T">返回结果的类型</typeparam>
        /// <param name="sql">SQL语句</param>
        /// <param name="param">参数</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <returns></returns>
        IEnumerable<T> Query<T>(string sql, object param = null, int? commandTimeout = null);

        /// <summary>
        /// 执行查询【异步】
        /// </summary>
        /// <typeparam name="T">返回结果的类型</typeparam>
        /// <param name="sql">SQL语句</param>
        /// <param name="param">参数</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <returns></returns>
        Task<IEnumerable<T>> QueryAsync<T>(string sql, object param = null, int? commandTimeout = null);
    }
}
