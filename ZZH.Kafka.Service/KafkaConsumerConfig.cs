using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZZH.Kafka.Service
{

    public class KafkaConsumerConfig
    {
        public string BootstrapServers { set; get; }
        public List<string> Topics { set; get; }
        /// <summary>
        /// 客户端配置字典
        /// SaslUsername、SaslPassword 等信息
        /// </summary>
        public IDictionary<string, string> ClientConfig { set; get; }
        /// <summary>
        /// 分组ID
        /// </summary>
        public string GroupId { set; get; }
        /// <summary>
        /// 是否自动提交，默认：false
        /// </summary>
        public bool? EnableAutoCommit { set; get; }
        /// <summary>
        /// 默认：5000
        /// </summary>
        public int? StatisticsIntervalMs { set; get; }
        /// <summary>
        /// 默认：6000
        /// </summary>
        public int? SessionTimeoutMs { set; get; }
        public AutoOffsetReset? AutoOffsetReset { set; get; }
        /// <summary>
        /// 默认：true
        /// </summary>
        public bool? EnablePartitionEof { set; get; }
    }
    /// <summary>
    /// Kafka配置类（含用户名和密码）
    /// </summary>
    public class KafkaConsumerConfigForCredit
    {
        public List<string> Topics { set; get; }
        public string BrokerServers { set; get; }
        public string SaslUsername { set; get; }
        public string SaslPassword { set; get; }
        public SaslMechanism? SaslMechanism { set; get; }
        public SecurityProtocol? SecurityProtocol { get; set; }
        public string GroupId { set; get; }
        public bool? EnableAutoCommit { set; get; }
        public int? StatisticsIntervalMs { set; get; }
        public int? SessionTimeoutMs { set; get; }
        public AutoOffsetReset? AutoOffsetReset { set; get; }// AutoOffsetReset.Earliest,
        public bool? EnablePartitionEof { set; get; }
    }
}
