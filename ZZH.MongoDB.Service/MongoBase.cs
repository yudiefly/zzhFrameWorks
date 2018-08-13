using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZZH.MongoDB.Service
{
    public class MongoBase
    {
        public static Dictionary<string, IMongoDatabase> dbDic = new Dictionary<string, IMongoDatabase>();
        private bool _disposed = false;
        private static object lockObj = new object();

        /// <summary>
        /// 静态构造方法，初始化链接池管理对象
        /// </summary>
        protected IMongoDatabase ShareMongoDb(string dbConfigSelectionKey, MongoConfig _mongoConfig)
        {
            if (!dbDic.ContainsKey(dbConfigSelectionKey))
            {
                lock (lockObj)
                {
                    if (!dbDic.ContainsKey(dbConfigSelectionKey) && _mongoConfig != null)
                    {
                        var logSetting = new MongoClientSettings
                        {
                            Server = new MongoServerAddress(_mongoConfig.ServerConStr, _mongoConfig.ServerPort),
                            MaxConnectionPoolSize = _mongoConfig.MaxConnectionPoolSize,
                            MaxConnectionIdleTime = TimeSpan.FromSeconds(_mongoConfig.MaxConnectionIdleTime),
                            MaxConnectionLifeTime = TimeSpan.FromSeconds(_mongoConfig.MaxConnectionLifeTime),
                            ConnectTimeout = TimeSpan.FromSeconds(_mongoConfig.ConnectTimeout),
                            WaitQueueSize = _mongoConfig.WaitQueueSize,
                            SocketTimeout = TimeSpan.FromSeconds(_mongoConfig.SocketTimeout),
                            WaitQueueTimeout = TimeSpan.FromSeconds(_mongoConfig.WaitQueueTimeout)
                        };
                        if (!string.IsNullOrEmpty(_mongoConfig.UserName) && !string.IsNullOrEmpty(_mongoConfig.PassWord))//设置MongoDb用户密码
                        {
                            string authDB = _mongoConfig.AuthDb;
                            if (string.IsNullOrEmpty(authDB))
                            {
                                authDB = _mongoConfig.DefaultDb;
                            }
                            MongoCredential mongoCredential = MongoCredential.CreateCredential(authDB, _mongoConfig.UserName, _mongoConfig.PassWord);
                            List<MongoCredential> mongoCredentialList = new List<MongoCredential>();
                            mongoCredentialList.Add(mongoCredential);
                            logSetting.Credentials = mongoCredentialList;
                        }
                        var logClient = new MongoClient(logSetting);
                        IMongoDatabase db = logClient.GetDatabase(_mongoConfig.DefaultDb);
                        dbDic.Add(dbConfigSelectionKey, db);
                    }
                }
            }
            return dbDic[dbConfigSelectionKey];
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    // 释放链接
                    // Core.Disconnect();
                    dbDic = null;
                }
            }
            this._disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }

}
