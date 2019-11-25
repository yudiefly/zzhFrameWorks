using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ZZH.Kafka.Service
{
    public class KafkaProducer<T> where T : class
    {
        #region 自定义事件 
        public class KafkaProducerArgs
        {
            public string Key { set; get; }
            public T Messages { set; get; }
            public ProduceException<string, string> KafkaProducerExecption { set; get; }
            public KafkaProducerArgs(T messages, string key = "", ProduceException<string, string> KafkaProducerExecption = null)
            {
                this.Messages = messages;
                this.Key = key;
                this.KafkaProducerExecption = KafkaProducerExecption;
            }
        }
        public delegate void KafkaProducerHandle(KafkaProducerArgs e);
        /// <summary>
        /// 生产者报错事件
        /// </summary>
        public event KafkaProducerHandle KakfaProducerException;
        private KafkaProducerArgs OnKakfaProducerException(T messages, string key = "", ProduceException<string, string> kafkaProducerExecption = null)
        {
            var e = new KafkaProducerArgs(messages, key, kafkaProducerExecption);
            KakfaProducerException?.Invoke(e);
            return e;
        }
        /// <summary>
        /// 消息发布成功的事件
        /// </summary>
        public event KafkaProducerHandle KakfaProducerSucces;
        private KafkaProducerArgs OnKakfaProducerSucces(T message, string key = "")
        {
            var e = new KafkaProducerArgs(message, key, null);
            KakfaProducerSucces?.Invoke(e);
            return e;
        }
        #endregion

        private string TopicName = "";
        private ProducerConfig producerConfig;
        private IProducer<string, T> _producer;

        /// <summary>
        /// 普通初始化
        /// </summary>
        /// <param name="_config">如果有用户名和密码,请放在KafkaConsumerConfig的ClientConfig属性中</param>
        public KafkaProducer(KafkaProducerConfig _config_, ISerializer<T> serializer = null)
        {
            this.TopicName = _config_.TopicName;
            if (_config_.ClientConfig?.Count > 0)
            {
                producerConfig = new ProducerConfig(_config_.ClientConfig) { BootstrapServers = _config_.BrokerServers };
            }
            else
            {
                producerConfig = new ProducerConfig { BootstrapServers = _config_.BrokerServers };
            }
            var producerBuilder = new ProducerBuilder<string, T>(producerConfig);
            if (serializer != null)
            {
                _producer = producerBuilder.SetValueSerializer(serializer).Build();
            }
            _producer = producerBuilder.Build();
        }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="_config_">服务器、Topic、用户名和密码</param>
        public KafkaProducer(KafkaProducerConfigForCredit _config_, ISerializer<T> serializer = null)
        {
            this.TopicName = _config_.TopicName;
            producerConfig = new ProducerConfig
            {
                BootstrapServers = _config_.BrokerServers,
                SaslUsername = _config_.SaslUsername,
                SaslPassword = _config_.SaslPassword,
                SaslMechanism = _config_.SaslMechanism,
                SecurityProtocol = _config_.SecurityProtocol
            };
            var producerBuilder = new ProducerBuilder<string, T>(producerConfig);
            if (serializer != null)
            {
                producerBuilder = producerBuilder.SetValueSerializer(serializer);
            }
            _producer = producerBuilder.Build();
        }
        /// <summary>
        ///  高级初始化的方法
        /// </summary>
        /// <param name="clientConfig">通常只需要设置：clientConfig的BootstrapServers、SaslUsername、SaslPassword、SaslMechanism、SecurityProtocol等属性即可</param>
        /// <param name="TopicName"></param>
        public KafkaProducer(KafkaClientConfig clientConfig, string TopicName, ISerializer<T> serializer = null)
        {
            this.TopicName = TopicName;
            producerConfig = new ProducerConfig
            {
                #region 常规选项（服务器地址、用户名和密码）
                BootstrapServers = clientConfig.BootstrapServers,
                SaslUsername = clientConfig.SaslUsername,
                SaslPassword = clientConfig.SaslPassword,
                SaslMechanism = clientConfig.SaslMechanism,
                SecurityProtocol = clientConfig.SecurityProtocol,
                #endregion
                #region 高级选项设置（你可以不用管）
                Acks = clientConfig.Acks,
                ApiVersionFallbackMs = clientConfig.ApiVersionFallbackMs,
                ApiVersionRequest = clientConfig.ApiVersionRequest,
                ApiVersionRequestTimeoutMs = clientConfig.ApiVersionRequestTimeoutMs,
                BrokerAddressFamily = clientConfig.BrokerAddressFamily,
                BrokerAddressTtl = clientConfig.BrokerAddressTtl,
                BrokerVersionFallback = clientConfig.BrokerVersionFallback,
                ClientId = clientConfig.ClientId,
                Debug = clientConfig.Debug,
                MaxInFlight = clientConfig.MaxInFlight,
                LogConnectionClose = clientConfig.LogConnectionClose,
                MessageCopyMaxBytes = clientConfig.MessageCopyMaxBytes,
                EnableSaslOauthbearerUnsecureJwt = clientConfig.EnableSaslOauthbearerUnsecureJwt,
                LogQueue = clientConfig.LogQueue,
                InternalTerminationSignal = clientConfig.InternalTerminationSignal,
                EnableSslCertificateVerification = clientConfig.EnableSslCertificateVerification,
                LogThreadName = clientConfig.LogThreadName,
                MessageMaxBytes = clientConfig.MessageMaxBytes,
                MetadataMaxAgeMs = clientConfig.MetadataMaxAgeMs,
                MetadataRequestTimeoutMs = clientConfig.MetadataRequestTimeoutMs,
                PluginLibraryPaths = clientConfig.PluginLibraryPaths,
                ReceiveMessageMaxBytes = clientConfig.ReceiveMessageMaxBytes,
                ReconnectBackoffMaxMs = clientConfig.ReconnectBackoffMaxMs,
                ReconnectBackoffMs = clientConfig.ReconnectBackoffMs,
                SaslKerberosKeytab = clientConfig.SaslKerberosKeytab,
                SaslKerberosKinitCmd = clientConfig.SaslKerberosKinitCmd,
                SaslKerberosMinTimeBeforeRelogin = clientConfig.SaslKerberosMinTimeBeforeRelogin,
                SaslKerberosPrincipal = clientConfig.SaslKerberosPrincipal,
                SaslKerberosServiceName = clientConfig.SaslKerberosServiceName,
                SaslOauthbearerConfig = clientConfig.SaslOauthbearerConfig,
                SocketKeepaliveEnable = clientConfig.SocketKeepaliveEnable,
                SocketMaxFails = clientConfig.SocketMaxFails,
                SocketNagleDisable = clientConfig.SocketNagleDisable,
                SocketReceiveBufferBytes = clientConfig.SocketReceiveBufferBytes,
                SocketSendBufferBytes = clientConfig.SocketSendBufferBytes,
                SocketTimeoutMs = clientConfig.SocketTimeoutMs,
                SslCaLocation = clientConfig.SslCaLocation,
                SslCertificateLocation = clientConfig.SslCertificateLocation,
                SslCertificatePem = clientConfig.SslCertificatePem,
                SslCipherSuites = clientConfig.SslCipherSuites,
                SslCrlLocation = clientConfig.SslCrlLocation,
                SslCurvesList = clientConfig.SslCurvesList,
                SslEndpointIdentificationAlgorithm = clientConfig.SslEndpointIdentificationAlgorithm,
                SslKeyLocation = clientConfig.SslKeyLocation,
                SslKeyPassword = clientConfig.SslKeyPassword,
                SslKeyPem = clientConfig.SslKeyPem,
                SslKeystoreLocation = clientConfig.SslKeystoreLocation,
                SslKeystorePassword = clientConfig.SslKeystorePassword,
                SslSigalgsList = clientConfig.SslSigalgsList,
                StatisticsIntervalMs = clientConfig.StatisticsIntervalMs,
                TopicMetadataRefreshSparse = clientConfig.TopicMetadataRefreshSparse,
                TopicBlacklist = clientConfig.TopicBlacklist,
                TopicMetadataRefreshFastIntervalMs = clientConfig.TopicMetadataRefreshFastIntervalMs,
                TopicMetadataRefreshIntervalMs = clientConfig.TopicMetadataRefreshIntervalMs
                #endregion
            };
            var producerBuilder = new ProducerBuilder<string, T>(producerConfig);
            if (serializer != null)
            {
                producerBuilder = producerBuilder.SetValueSerializer(serializer);
            }
            _producer = producerBuilder.Build();
        }
        /// <summary>
        /// 生产者发布消息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task ProduceAsync(T message, string key = "")
        {
            var valuekey = (string.IsNullOrEmpty(key)) ? Guid.NewGuid().ToString() : key;
            try
            {
                var deliveryReport = await _producer.ProduceAsync(this.TopicName, new Message<string, T> { Key = valuekey, Value = message });
                OnKakfaProducerSucces(message, valuekey);
            }
            catch (ProduceException<string, string> e)
            {
                OnKakfaProducerException(message, valuekey, e);
            }
        }
        /// <summary>
        /// 生产者发布消息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="key"></param>
        public void Produce(T message, string key = "")
        {
            var valuekey = (string.IsNullOrEmpty(key)) ? Guid.NewGuid().ToString() : key;
            try
            {
                Action<DeliveryReport<string, T>> handler = r =>
                {
                    if (!r.Error.IsError)
                    {
                        OnKakfaProducerSucces(message, valuekey);
                    }
                    else
                    {
                        OnKakfaProducerException(message, valuekey, new ProduceException<string, string>(r.Error, null));
                    }
                };
                _producer.Produce(this.TopicName, new Message<string, T> { Key = valuekey, Value = message }, handler);
            }
            catch (ProduceException<string, string> e)
            {
                OnKakfaProducerException(message, valuekey, e);
            }

        }
        /// <summary>
        /// 关闭生产者
        /// </summary>
        public void Close()
        {
            _producer?.Flush(TimeSpan.FromSeconds(60));
            _producer?.Dispose();
        }
    }
}
