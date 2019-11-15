using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZZH.Kafka.Service
{
    public class KafkaProducerConfig
    {
        public string TopicName { set; get; }
        public string BrokerServers { set; get; }
        /// <summary>
        /// 客户端配置字典
        /// SaslUsername、SaslPassword、SaslMechanism等信息
        /// </summary>
        public IDictionary<string, string> ClientConfig { set; get; }
    }
    /// <summary>
    /// Kafka配置类（含用户名和密码）
    /// </summary>
    public class KafkaProducerConfigForCredit
    {
        public string TopicName { set; get; }
        public string BrokerServers { set; get; }
        public string SaslUsername { set; get; }
        public string SaslPassword { set; get; }
        public SaslMechanism? SaslMechanism { set; get; }
        public SecurityProtocol? SecurityProtocol { get; set; }
    }
}
