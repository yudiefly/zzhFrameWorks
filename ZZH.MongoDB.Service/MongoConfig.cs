using System;
using System.Collections.Generic;
using System.Text;

namespace ZZH.MongoDB.Service
{
    public class MongoConfig
    {
        /// <summary>
        /// 可写的Mongo链接地址
        /// </summary>       
        public string ServerConStr
        {
            get;
            set;
        }

        public int ServerPort
        {
            get;
            set;
        }

        /// <summary>
        /// 默认的连接数据库
        /// </summary>        
        public string DefaultDb
        {
            get;
            set;
        }
        /// <summary>
        /// 身份验证的数据库
        /// </summary>        
        public string AuthDb
        {
            get;
            set;
        }
        public string UserName
        {
            get;
            set;
        }
        public string PassWord
        {
            get;
            set;
        }

        /// <summary>
        /// 最大连接池
        /// </summary>    
        public int MaxConnectionPoolSize
        {
            get;
            set;
        }

        /// <summary>
        /// 最大闲置时间
        /// </summary>
        public int MaxConnectionIdleTime
        {
            get;
            set;
        }

        /// <summary>
        /// 最大存活时间
        /// </summary>      
        public int MaxConnectionLifeTime
        {
            get;
            set;
        }

        /// <summary>
        /// 链接时间
        /// </summary>
        public int ConnectTimeout
        {
            get;
            set;
        }


        /// <summary>
        /// 等待队列大小
        /// </summary>   
        public int WaitQueueSize
        {
            get;
            set;
        }

        /// <summary>
        /// socket超时时间
        /// </summary>
        public int SocketTimeout
        {
            get;
            set;
        }

        /// <summary>
        /// 队列等待时间
        /// </summary>
        public int WaitQueueTimeout
        {
            get;
            set;
        }

    }
}
