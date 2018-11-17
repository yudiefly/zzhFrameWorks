using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZZH.Dapper.Service.IRepository;

namespace ZZH.Dapper.Service.Repository.SqlServer
{
    /// <summary>
    /// SqlServer基础仓储
    /// </summary>
    public class BaseRepository : IBaseRepository
    {
        #region 初始化

        /// <summary>
        /// 数据库链接字符串
        /// </summary>
        private string ConnectionString { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectionString">数据库链接字符串</param>
        public BaseRepository(string connectionString)
        {
            ConnectionString = connectionString;
        }

        #endregion

        #region 执行增（INSERT）语句【执行单条SQL语句，返回自增列】【同步】

        /// <summary>
        /// 执行增（INSERT）语句【执行单条SQL语句，返回自增列】【同步】
        /// </summary>
        /// <param name="sql">SQL语句（本方法会自动在SQL的结尾增加分号，并增加查询ID的语句）</param>
        /// <param name="param">参数</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <returns>自增ID</returns>
        public int Insert(string sql, object param = null, int? commandTimeout = null)
        {
            sql = sql + ";select @id= SCOPE_IDENTITY();";
            var newParam = new DynamicParameters();
            newParam.AddDynamicParams(param);
            newParam.Add("@id", dbType: DbType.Int32, direction: ParameterDirection.Output);
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Execute(sql, newParam, commandTimeout: commandTimeout);
                var id = newParam.Get<int>("@id");
                return id;
            }
        }

        #endregion

        #region 执行增（INSERT）语句【执行单条SQL语句，返回自增列】【异步】

        /// <summary>
        /// 执行增（INSERT）语句【执行单条SQL语句，返回自增列】【异步】
        /// </summary>
        /// <param name="sql">SQL语句（本方法会自动在SQL的结尾增加分号，并增加查询ID的语句）</param>
        /// <param name="param">参数</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <returns>自增ID</returns>
        public async Task<int> InsertAsync(string sql, object param = null, int? commandTimeout = null)
        {
            sql = sql + ";select @id= SCOPE_IDENTITY();";
            using (var conn = new SqlConnection(ConnectionString))
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                var result = await conn.ExecuteScalarAsync<int>(sql, param, commandTimeout: commandTimeout);
                return result;
            }
        }

        #endregion

        #region 执行增（INSERT）删（DELETE）改（UPDATE）语句或存储过程【单条SQL语句，可以获得输出参数】【同步】


        /// <summary>
        /// 执行增（INSERT）删（DELETE）改（UPDATE）语句或存储过程【单条SQL语句，可以获得输出参数】【同步】
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="param">参数</param>
        /// <param name="outParam">输出参数[参数名(@name)] </param>
        /// <param name="commandType">如何解释命令字符串</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <returns>Tuple(受影响的行数, Dictionary(输出参数名(@name), 输出参数))</returns>
        
        public Tuple<int, Dictionary<string, string>> Execute(string sql, DynamicParameters param, List<string> outParam, CommandType? commandType = null, int? commandTimeout = null)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                var rows = conn.Execute(sql, param, commandTimeout: commandTimeout, commandType: commandType);
                var outDic = new Dictionary<string, string>();
                foreach (var item in outParam)
                {
                    outDic.Add(item, param.Get<string>(item));
                }
                return new Tuple<int, Dictionary<string, string>>(rows, outDic);
            }
        }

        #endregion

        #region 执行增（INSERT）删（DELETE）改（UPDATE）语句或存储过程【单条SQL语句，可以获得输出参数】【异步】

        /// <summary>
        /// 执行增（INSERT）删（DELETE）改（UPDATE）语句或存储过程【单条SQL语句，可以获得输出参数】【异步】
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="param">参数</param>
        /// <param name="outParam">输出参数[参数名(@name)] </param>
        /// <param name="commandType">如何解释命令字符串</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <returns>Tuple(受影响的行数, Dictionary(输出参数名(@name), 输出参数))</returns>
        public async Task<Tuple<int, Dictionary<string, string>>> ExecuteAsync(string sql, DynamicParameters param, List<string> outParam, CommandType? commandType = null, int? commandTimeout = null)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                var rows = await conn.ExecuteAsync(sql, param, commandTimeout: commandTimeout, commandType: commandType);
                var outDic = new Dictionary<string, string>();
                foreach (var item in outParam)
                {
                    outDic.Add(item, param.Get<string>(item));
                }
                return new Tuple<int, Dictionary<string, string>>(rows, outDic);
            }
        }

        #endregion

        #region 执行增（INSERT）删（DELETE）改（UPDATE）语句【执行单条SQL语句】【同步】

        /// <returns></returns>
        /// <summary>
        /// 执行增（INSERT）删（DELETE）改（UPDATE）语句【执行单条SQL语句】【同步】
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="param">参数</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <returns>受影响的行数</returns>
        public int Execute(string sql, object param = null, int? commandTimeout = null)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                return conn.Execute(sql, param, commandTimeout: commandTimeout);
            }
        }

        #endregion

        #region 执行增（INSERT）删（DELETE）改（UPDATE）语句【执行单条SQL语句】【异步】

        /// <summary>
        /// 执行增（INSERT）删（DELETE）改（UPDATE）语句【执行单条SQL语句】【异步】
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="param">参数</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <returns>受影响的行数</returns>
        public async Task<int> ExecuteAsync(string sql, object param = null, int? commandTimeout = null)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                var result = await conn.ExecuteAsync(sql, param, commandTimeout: commandTimeout);
                return result;
            }
        }

        #endregion

        #region 执行增（INSERT）删（DELETE）改（UPDATE）语句【带事务，可以同时执行多条SQL】【同步】

        /// <summary>
        /// 执行增（INSERT）删（DELETE）改（UPDATE）语句【带事务，可以同时执行多条SQL】【同步】
        /// </summary>
        /// <param name="sqlDic">SQL + 参数【key：sql语句 value：参数】</param>ConnectionString
        /// <param name="commandTimeout">超时时间</param>
        /// <returns></returns>
        public void ExecuteToTransaction(Dictionary<string, object> sqlDic, int? commandTimeout = null)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                var trans = conn.BeginTransaction();
                try
                {
                    foreach (var item in sqlDic)
                    {
                        conn.Execute(item.Key, item.Value, transaction: trans, commandTimeout: commandTimeout);
                    }
                    trans.Commit();
                }
                catch (Exception)
                {
                    trans.Rollback();
                    throw;
                }
            }
        }

        #endregion

        #region 执行增（INSERT）删（DELETE）改（UPDATE）语句【带事务，可以同时执行多条SQL】【异步】【待测试】

        /// <summary>
        /// 执行增（INSERT）删（DELETE）改（UPDATE）语句【带事务，可以同时执行多条SQL】【异步】【待测试】
        /// </summary>
        /// <param name="sqlDic">SQL + 参数【key：sql语句 value：参数】</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <returns></returns>
        public async Task ExecuteToTransactionAsync(Dictionary<string, object> sqlDic, int? commandTimeout = null)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                var trans = conn.BeginTransaction();
                try
                {
                    foreach (var item in sqlDic)
                    {
                        await conn.ExecuteAsync(item.Key, item.Value, transaction: trans, commandTimeout: commandTimeout);
                    }
                    trans.Commit();
                }
                catch (Exception)
                {
                    trans.Rollback();
                    throw;
                }
            }
        }

        #endregion

        #region 执行分页存储过程（Procdeure）【同步】

        /// <summary>
        /// 执行分页存储过程（Procdeure）【同步】
        /// </summary>
        /// <typeparam name="T">返回结果的类型</typeparam>
        /// <param name="param">参数</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <returns></returns>
        public Tuple<IEnumerable<T>, int> ExecuteToPaginationProcdeure<T>(DynamicParameters param = null, int? commandTimeout = null)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                var dataList = conn.Query<T>("proc_sql_Paging", param: param, commandTimeout: commandTimeout, commandType: CommandType.StoredProcedure);
                var count = param.Get<int>("@recordCount");
                return new Tuple<IEnumerable<T>, int>(dataList, count);
            }
        }

        #endregion

        #region 执行分页存储过程（Procdeure）【异步】

        /// <summary>
        /// 执行分页存储过程（Procdeure）【异步】
        /// </summary>
        /// <typeparam name="T">返回结果的类型</typeparam>
        /// <param name="param">参数</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <returns></returns>
        public async Task<Tuple<IEnumerable<T>, int>> ExecuteToPaginationProcdeureAsync<T>(DynamicParameters param = null, int? commandTimeout = null)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                var dataList = await conn.QueryAsync<T>("proc_sql_Paging", param: param, commandTimeout: commandTimeout, commandType: CommandType.StoredProcedure);
                var count = param.Get<int>("@recordCount");
                return new Tuple<IEnumerable<T>, int>(dataList, count);
            }
        }

        #endregion

        #region 执行存储过程（Procdeure）【同步】

        /// <summary>
        /// 执行存储过程（Procdeure）【同步】
        /// </summary>
        /// <typeparam name="T">返回结果的类型</typeparam>
        /// <param name="porcdeureName">存储过程名称</param>
        /// <param name="param">参数</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <returns></returns>
        public IEnumerable<T> ExecuteToProcdeure<T>(string porcdeureName, object param = null, int? commandTimeout = null)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                return conn.Query<T>(porcdeureName, commandTimeout: commandTimeout, commandType: CommandType.StoredProcedure, param: param);
            }
        }

        #endregion

        #region 执行存储过程（Procdeure）【异步】

        /// <summary>
        /// 执行存储过程（Procdeure）【异步】
        /// </summary>
        /// <typeparam name="T">返回结果的类型</typeparam>
        /// <param name="porcdeureName">存储过程名称</param>
        /// <param name="param">参数</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> ExecuteToProcdeureAsync<T>(string porcdeureName, object param = null, int? commandTimeout = null)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                var result = await conn.QueryAsync<T>(porcdeureName, commandTimeout: commandTimeout, commandType: CommandType.StoredProcedure, param: param);
                return result;
            }
        }

        #endregion

        #region 执行查询【同步】

        /// <summary>
        /// 执行查询【同步】
        /// </summary>
        /// <typeparam name="T">返回结果的类型</typeparam>
        /// <param name="sql">SQL语句</param>
        /// <param name="param">参数</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <returns></returns>
        public IEnumerable<T> Query<T>(string sql, object param = null, int? commandTimeout = null)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                return conn.Query<T>(sql, param: param, commandTimeout: commandTimeout);
            }
        }

        #endregion

        #region 执行查询【异步】

        /// <summary>
        /// 执行查询【异步】
        /// </summary>
        /// <typeparam name="T">返回结果的类型</typeparam>
        /// <param name="sql">SQL语句</param>
        /// <param name="param">参数</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object param = null, int? commandTimeout = null)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                var result = await conn.QueryAsync<T>(sql, param: param, commandTimeout: commandTimeout);
                return result;
            }
        }

        #endregion
    }
}
