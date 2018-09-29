using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZZH.RabbitMQ.FrameworkService
{
    /// <summary>
    /// 基本配置信息类
    /// </summary>
    public class Constants
    {
        /// <summary>
        /// 自定义前缀
        /// </summary>
        private string _tag = "ZZH_";
        /// <summary>
        /// MQ虚机
        /// </summary>
        public string VirtualHost { set; get; }
        /// <summary>
        /// 主机名
        /// </summary>
        public string HostName { set; get; }
        /// <summary>
        /// 端口号
        /// </summary>
        public int Port { set; get; }
        /// <summary>
        /// 用户名 
        /// </summary>
        public string UserName { set; get; }
        /// <summary>
        /// 密码 
        /// </summary>
        public string Password { set; get; }
        /// <summary>
        /// 前缀 
        /// </summary>
        public string TAG
        {
            get { return _tag; }
            set { _tag = value; }
        }
        /// <summary>
        /// 初始化
        /// </summary>
        public Constants()
        {

        }
        /// <summary>
        /// 自定义前缀
        /// </summary>
        /// <param name="_mytag"></param>
        public Constants(string _mytag)
        {
            _tag = _mytag;
        }

        /// <summary>
        /// 重试的Exchange
        /// </summary>
        public string DXL_RETRY_EXCHANGE
        {
            get { return _tag + "DXL_RETRY_EXCHANGE"; }
        }
        /// <summary>
        /// 重试的Queue
        /// </summary>
        public string DXL_RETRY_QUEUE
        {
            get { return _tag + "DXL_RETRY_QUEUE"; }
        }
        /// <summary>
        /// 业务Exchange
        /// </summary>
        public string BUSINESS_EXCHANGE
        {
            get { return _tag + "BUSINESS_EXCHANGE"; }
        }
    }
}
