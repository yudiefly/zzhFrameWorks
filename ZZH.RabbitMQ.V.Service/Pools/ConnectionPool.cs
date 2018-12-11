using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace ZZH.RabbitMQ.V.Service
{
    public static class ConnectionPool
    {
        private static readonly List<ConnectionWrapper> ConnList = new List<ConnectionWrapper>();///连接池里的链接
        /// <summary>                                                                                                 
        /// 读写锁                                                                                                 
        /// </summary>
        private static ReaderWriterLockSlim GetConnlockObj = new ReaderWriterLockSlim();
        /// <summary>
        /// 扫描周期，单位（秒），默认60秒
        /// </summary>
        private static readonly int ScanvageInterval = 60 * 1000;
        /// <summary>
        /// 最大闲置时间，单位（秒）,默认120秒
        /// </summary>
        internal static int MaxIdleTime = 120;

        private static readonly System.Timers.Timer ScanTimer;

        static ConnectionPool()
        {
            //后台清理线程，定期清理整个应用域中每一个SocketQueue中的每一个Socket项
            ScanTimer = new System.Timers.Timer(ScanvageInterval);
            ScanTimer.Elapsed += new System.Timers.ElapsedEventHandler(DoScavenging);
            ScanTimer.AutoReset = true;
            ScanTimer.Enabled = true;

        }

        /// <summary>
        /// 清理方法，清理本CacheQueue中过期的cache项
        /// </summary>
        private static void DoScavenging(object sender, ElapsedEventArgs e)
        {
            List<ConnectionWrapper> keysToRemove = new List<ConnectionWrapper>();

            GetConnlockObj.EnterUpgradeableReadLock();
            try
            {
                foreach (var item in ConnList)
                {
                    if (item.HasSessionConn() == false)
                    {
                        if (item.IdleTime == default(DateTime))
                            item.IdleTime = DateTime.UtcNow;
                        else
                        {
                            var timeDur = TimeHelper.GetTimeDurationTotalSeconds(item.IdleTime, DateTime.UtcNow);
                            if (timeDur > MaxIdleTime)
                            {
                                keysToRemove.Add(item);
                            }
                        }
                    }
                }
                foreach (var item in keysToRemove)
                    InnerRemove(item);
            }
            finally
            {
                GetConnlockObj.ExitUpgradeableReadLock();
            }
        }

        internal static void InnerRemove(ConnectionWrapper item)
        {
            GetConnlockObj.EnterWriteLock();
            try
            {
                ConnList.Remove(item);
                item.Dispose();
                item = null;
            }
            finally
            {
                GetConnlockObj.ExitWriteLock();
            }
        }

        public static IModelWrapper GetOrCreateChannel(ConnectionFactory connFactory)
        {
            if (connFactory == null)
                throw new ArgumentNullException("参数connFactory不能为空，请检查参数");

            GetConnlockObj.EnterUpgradeableReadLock();///若线程用光了，得写一个连接线程，所以这里用可升级锁
            IModelWrapper model = null;
            try
            {
                foreach (var item in ConnList)
                {
                    model = item.GetOrCreateChannel();
                    if (model != null)
                        return model;
                }
                if (model == null)
                {
                    //创建connection对象
                    ConnectionWrapper conn = CreateConnection(connFactory);
                    return conn.GetOrCreateChannel();
                }
            }
            finally
            {
                GetConnlockObj.ExitUpgradeableReadLock();
            }
            return null;
        }

        private static ConnectionWrapper CreateConnection(ConnectionFactory connFactory)
        {
            GetConnlockObj.EnterWriteLock();
            try
            {
                var newConn = connFactory.CreateConnection();
                ConnectionWrapper connWrapper = new ConnectionWrapper(connFactory, newConn);
                ConnList.Add(connWrapper);
                return connWrapper;
            }
            finally
            {
                GetConnlockObj.ExitWriteLock();
            }
        }
    }
}
