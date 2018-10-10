using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZZH.RabbitMQ.V.Service
{
    public class RabbitMQHelper
    {
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
    }
}
