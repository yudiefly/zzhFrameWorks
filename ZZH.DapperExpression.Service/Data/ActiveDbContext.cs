using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;

namespace ZZH.DapperExpression.Service.Data
{
    /// <summary>
    /// Active DB 上下文
    /// </summary>
    public abstract class ActiveDbContext : IUnitOfWork, IDisposable
    {
        private readonly Lazy<DbConnection> _dbConnection;
        private DbTransaction _activeTransaction;
        private volatile bool wasOpenTransaction = false;

        public DbConnection ActiveConnection => _dbConnection.Value;

        public DbTransaction ActiveTransaction
        {
            get
            {
                if (!wasOpenTransaction)
                {
                    if (ActiveConnection.State == System.Data.ConnectionState.Closed)
                    {
                        ActiveConnection.Open();
                    }

                    _activeTransaction = ActiveConnection.BeginTransaction();

                    wasOpenTransaction = true;
                }

                return _activeTransaction;
            }
        }

        /// <summary>
        /// 获取或设置连接字符串
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// 初始化一个新的<see cref="ActiveDbContext"/>对象
        /// </summary>
        protected ActiveDbContext()
        {
            _dbConnection = new Lazy<DbConnection>(() =>
            {
                return CreateDbConnection(ConnectionString);
            });
        }

        /// <summary>
        /// 创建数据库连接对象
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        /// <returns></returns>
        public abstract DbConnection CreateDbConnection(string connectionString);

        /// <summary>
        /// 提交事务
        /// </summary>
        public void Commit()
        {
            if (wasOpenTransaction)
            {
                try
                {
                    ActiveTransaction.Commit();
                }
                catch (Exception)
                {
                    ActiveTransaction.Rollback();
                    throw;
                }
                finally
                {
                    wasOpenTransaction = false;
                }
            }
        }

        /// <summary>
        /// 提交事务
        /// </summary>
        /// <returns></returns>
        public Task CommitAsync()
        {
            Commit();
            return Task.FromResult(0);
        }

        public void Dispose()
        {
            if (wasOpenTransaction)
            {
                _activeTransaction.Rollback();
                _activeTransaction.Dispose();
                wasOpenTransaction = false;
            }

            if (ActiveConnection != null)
            {
                ActiveConnection.Close();
                ActiveConnection.Dispose();
            }
        }
    }
}
