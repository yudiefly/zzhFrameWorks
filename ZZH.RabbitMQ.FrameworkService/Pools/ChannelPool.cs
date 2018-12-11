using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace ZZH.RabbitMQ.FrameworkService
{
    public class ChannelPool
    {
        private List<IModelWrapper> ListChannel = new List<IModelWrapper>();

        private IConnection Conn;
        /// <summary>
        /// 扫描周期，单位（秒），默认60秒
        /// </summary>
        private static readonly int ScanvageInterval = 60 * 1000;
        /// <summary>
        /// 最大闲置时间，单位（秒）,默认120秒
        /// </summary>
        internal static int MaxIdleTime = 120;
      
        private readonly System.Timers.Timer ScanTimer;
        /// <summary>
        /// 读写锁
        /// </summary>
        private ReaderWriterLockSlim GetChannellockObj = new ReaderWriterLockSlim();

        public ChannelPool(IConnection conn)
        {
            if (conn == null)
                throw new ArgumentNullException("参数conn不能为空，请检查参数");

            Conn = conn;
            //后台清理线程，定期清理整个应用域中每一个Channel中的每一个Channel项
            ScanTimer = new System.Timers.Timer(ScanvageInterval);
            ScanTimer.Elapsed += new System.Timers.ElapsedEventHandler(DoScavenging);
            ScanTimer.AutoReset = true;
            ScanTimer.Enabled = true;
        }
        private void DoScavenging(object sender, ElapsedEventArgs e)
        {
            GetChannellockObj.EnterUpgradeableReadLock();
            try
            {
                List<IModelWrapper> keysToRemove = new List<IModelWrapper>();
                foreach (var item in ListChannel)
                {
                    var timeDur = TimeHelper.GetTimeDurationTotalSeconds(item.IdleTime, DateTime.UtcNow);
                    if (item.IsBusy == false && timeDur >= MaxIdleTime)
                    {
                        keysToRemove.Add(item);
                    }
                }
                foreach (var item in keysToRemove)
                    InnerRemove(item);
            }
            finally
            {
                GetChannellockObj.ExitUpgradeableReadLock();
            }
        }

        internal void InnerRemove(IModelWrapper item)
        {
            GetChannellockObj.EnterWriteLock();
            try
            {
                ListChannel.Remove(item);

                item.Dispose();              
            }
            finally
            {
                GetChannellockObj.ExitWriteLock();
            }
        }
        private IModelWrapper CreateChannel()
        {
            GetChannellockObj.EnterWriteLock();
            try
            {
                IModel channel = Conn.CreateModel();   

                IModelWrapper ModelWrapper = new IModelWrapper(channel);

                ListChannel.Add(ModelWrapper);

                return ModelWrapper;
            }
            finally
            {
                GetChannellockObj.ExitWriteLock();
            }
        }
        public IModelWrapper GetOrCreateChannel()
        {
            GetChannellockObj.EnterUpgradeableReadLock();///若线程用光了，得写一个连接线程，所以这里用可升级锁
            try
            {
                IModelWrapper Channel = ListChannel.Find(p =>p.IsBusy == false);

                if (Channel != null)
                {
                    Channel.IsBusy = true;
                    return Channel;
                }
                else if (Channel == null && ListChannel.Count < Conn.ChannelMax)
                {
                    return CreateChannel();
                }
                else
                {
                    return null;
                }
            }
            finally
            {
                GetChannellockObj.ExitUpgradeableReadLock();
            }
        }

        public int GetSessionManager()
        {
            return ListChannel.Count();
        }
    }
}
