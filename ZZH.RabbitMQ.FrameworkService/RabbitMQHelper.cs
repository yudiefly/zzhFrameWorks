using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZZH.RabbitMQ.FrameworkService
{
    public class RabbitMQHelper
    {
        /// <summary>
        /// 创建连接
        /// </summary>
        /// <returns></returns>
        public static ConnectionFactory CreateConnectFactory(Constants configs)
        {
            var connectionFactory = new ConnectionFactory
            {
                VirtualHost = configs.VirtualHost,
                HostName = configs.HostName,
                Port = configs.Port,
                UserName = configs.UserName,
                Password = configs.Password,
                Protocol = Protocols.AMQP_0_9_1,
                RequestedFrameMax = UInt32.MaxValue,
                RequestedHeartbeat = UInt16.MaxValue,
                AutomaticRecoveryEnabled = true //MQ断开重连
            };

            return connectionFactory;
        }
    }
}
