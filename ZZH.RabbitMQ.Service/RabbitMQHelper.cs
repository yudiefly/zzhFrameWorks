using RabbitMQ.Client;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ZZH.RabbitMQ.Service
{
    public class RabbitMQHelper
    {
        private readonly static ConcurrentQueue<IConnection> FreeConnectionQueue;//空闲连接对象队列
        private readonly static ConcurrentDictionary<IConnection, bool> BusyConnectionDic;//使用中（忙）连接对象集合
        private readonly static ConcurrentDictionary<IConnection, int> MQConnectionPoolUsingDicNew;//连接池使用率
        private readonly static Semaphore MQConnectionPoolSemaphore;
        private readonly static object freeConnLock = new object(), addConnLock = new object();
        private static int connCount = 0;

        private static int _DefaultMaxConnectionCount_ = 30;//默认最大保持可用连接数
        private static int _DefaultMaxConnectionUsingCount_ = 10000;//默认最大连接可访问次数
        /// <summary>
        /// 初始化
        /// </summary>
        static RabbitMQHelper()
        {
            FreeConnectionQueue = new ConcurrentQueue<IConnection>();
            BusyConnectionDic = new ConcurrentDictionary<IConnection, bool>();
            MQConnectionPoolUsingDicNew = new ConcurrentDictionary<IConnection, int>();//连接池使用率
            MQConnectionPoolSemaphore = new Semaphore(MaxConnectionCount, MaxConnectionCount, "MQConnectionPoolSemaphore");//信号量，控制同时并发可用线程数
        }
        /// <summary>
        /// 最大可用连接数（默认为:30)
        /// </summary>
        public static int MaxConnectionCount
        {
            get
            {
                return _DefaultMaxConnectionCount_;
            }
            set
            {
                _DefaultMaxConnectionCount_ = value;
            }
        }
        /// <summary>
        /// 最大连接可访问次数（默认为：10000）
        /// </summary>
        public static int MaxConnectionUsingCount
        {
            set { _DefaultMaxConnectionUsingCount_ = value; }
            get { return _DefaultMaxConnectionUsingCount_; }
        }

        /// <summary>
        /// 创建连接
        /// </summary>
        /// <returns></returns>
        public static ConnectionFactory CreateConnectFactory(Constants _constants)
        {
            var connectionFactory = new ConnectionFactory
            {
                VirtualHost = _constants.VirtualHost,
                HostName = _constants.HostName,
                Port = _constants.Port,
                UserName = _constants.UserName,
                Password = _constants.Password,
                Protocol = Protocols.AMQP_0_9_1,
                RequestedFrameMax = UInt32.MaxValue,
                RequestedHeartbeat = UInt16.MaxValue,
                AutomaticRecoveryEnabled = true //MQ断开重连
            };
            return connectionFactory;
        }
        /// <summary>
        /// 创建连接池并获取一个连接对象
        /// </summary>
        /// <param name="_constants"></param>
        /// <returns></returns>
        public static IConnection CreateMQConnectionInPoolNew(Constants _constants)
        {

        SelectMQConnectionLine:

            MQConnectionPoolSemaphore.WaitOne();//当<MaxConnectionCount时，会直接进入，否则会等待直到空闲连接出现

            IConnection mqConnection = null;
            if (FreeConnectionQueue.Count + BusyConnectionDic.Count < MaxConnectionCount)//如果已有连接数小于最大可用连接数，则直接创建新连接
            {
                lock (addConnLock)
                {
                    if (FreeConnectionQueue.Count + BusyConnectionDic.Count < MaxConnectionCount)
                    {
                        mqConnection = CreateMQConnection(_constants);
                        BusyConnectionDic[mqConnection] = true;//加入到忙连接集合中
                        MQConnectionPoolUsingDicNew[mqConnection] = 1;
                        //  BaseUtil.Logger.DebugFormat("Create a MQConnection:{0},FreeConnectionCount:{1}, BusyConnectionCount:{2}", mqConnection.GetHashCode().ToString(), FreeConnectionQueue.Count, BusyConnectionDic.Count);
                        return mqConnection;
                    }
                }
            }
            if (!FreeConnectionQueue.TryDequeue(out mqConnection)) //如果没有可用空闲连接，则重新进入等待排队
            {
                // BaseUtil.Logger.DebugFormat("no FreeConnection,FreeConnectionCount:{0}, BusyConnectionCount:{1}", FreeConnectionQueue.Count, BusyConnectionDic.Count);
                goto SelectMQConnectionLine;
            }
            else if (MQConnectionPoolUsingDicNew[mqConnection] + 1 > MaxConnectionUsingCount || !mqConnection.IsOpen) //如果取到空闲连接，判断是否使用次数是否超过最大限制,超过则释放连接并重新创建
            {
                mqConnection.Close();
                mqConnection.Dispose();
                // BaseUtil.Logger.DebugFormat("close > DefaultMaxConnectionUsingCount mqConnection,FreeConnectionCount:{0}, BusyConnectionCount:{1}", FreeConnectionQueue.Count, BusyConnectionDic.Count);

                mqConnection = CreateMQConnection(_constants);
                MQConnectionPoolUsingDicNew[mqConnection] = 0;
                // BaseUtil.Logger.DebugFormat("create new mqConnection,FreeConnectionCount:{0}, BusyConnectionCount:{1}", FreeConnectionQueue.Count, BusyConnectionDic.Count);
            }

            BusyConnectionDic[mqConnection] = true;//加入到忙连接集合中
            MQConnectionPoolUsingDicNew[mqConnection] = MQConnectionPoolUsingDicNew[mqConnection] + 1;//使用次数加1

            // BaseUtil.Logger.DebugFormat("set BusyConnectionDic:{0},FreeConnectionCount:{1}, BusyConnectionCount:{2}", mqConnection.GetHashCode().ToString(), FreeConnectionQueue.Count, BusyConnectionDic.Count);

            return mqConnection;
        }

        /// <summary>
        /// 获取队列中的消息数
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="QueueName"></param>
        /// <returns></returns>
        public static int GetMessageCount(IConnection connection, string QueueName)
        {
            int msgCount = 0;
            try
            {

                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(QueueName, true, false, false, null); //获取队列 
                    msgCount = (int)channel.MessageCount(QueueName);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                ResetMQConnectionToFree(connection);
            }

            return msgCount;
        }
        private static void ResetMQConnectionToFree(IConnection connection)
        {
            lock (freeConnLock)
            {
                bool result = false;
                if (BusyConnectionDic.TryRemove(connection, out result)) //从忙队列中取出
                {
                    //  BaseUtil.Logger.DebugFormat("set FreeConnectionQueue:{0},FreeConnectionCount:{1}, BusyConnectionCount:{2}", connection.GetHashCode().ToString(), FreeConnectionQueue.Count, BusyConnectionDic.Count);
                }
                else
                {
                    // BaseUtil.Logger.DebugFormat("failed TryRemove BusyConnectionDic:{0},FreeConnectionCount:{1}, BusyConnectionCount:{2}", connection.GetHashCode().ToString(), FreeConnectionQueue.Count, BusyConnectionDic.Count);
                }

                if (FreeConnectionQueue.Count + BusyConnectionDic.Count > MaxConnectionCount)//如果因为高并发出现极少概率的>MaxConnectionCount，则直接释放该连接
                {
                    connection.Close();
                    connection.Dispose();
                }
                else
                {
                    FreeConnectionQueue.Enqueue(connection);//加入到空闲队列，以便持续提供连接服务
                }

                MQConnectionPoolSemaphore.Release();//释放一个空闲连接信号

                //Interlocked.Decrement(ref connCount);
                //BaseUtil.Logger.DebugFormat("Enqueue FreeConnectionQueue:{0},FreeConnectionCount:{1}, BusyConnectionCount:{2},thread count:{3}", connection.GetHashCode().ToString(), FreeConnectionQueue.Count, BusyConnectionDic.Count,connCount);
            }
        }
        private static IConnection CreateMQConnection(Constants _constants)
        {
            var factory = CreateConnectFactory(_constants);
            factory.AutomaticRecoveryEnabled = true;//自动重连
            var connection = factory.CreateConnection();
            connection.AutoClose = false;
            return connection;
        }

    }

    public enum ConsumeAction
    {
        ACCEPT,  // 消费成功
        RETRY,   // 消费失败，可以放回队列重新消费
        REJECT,  // 消费失败，直接丢弃
    }
}
