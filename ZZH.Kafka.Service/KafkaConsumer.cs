using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ZZH.Kafka.Service
{
    public class KafkaConsumer<T> where T : class
    {
        #region 定义事件
        public class KafkaConsumerMessageArgs
        {
            public T Messages { set; get; }
            public ConsumeException KafkaCousmerExecption { set; get; }
            public Exception sysExceptions { set; get; }
            public string ExceptionOfMessages { set; get; }
            public KafkaConsumerMessageArgs(T messages, string _ExceptionMessages_ = null, ConsumeException kafkaConsumeException = null, Exception sysExceptions = null)
            {
                this.Messages = messages;
                this.ExceptionOfMessages = _ExceptionMessages_;
                this.KafkaCousmerExecption = KafkaCousmerExecption;
                this.sysExceptions = sysExceptions;
            }
        }
        public delegate void KafkaConsumerHandle(KafkaConsumerMessageArgs e);
        /// <summary>
        /// 消费者获取成功消息事件
        /// </summary>
        public event KafkaConsumerHandle OnMessage;
        private KafkaConsumerMessageArgs onOnMessage(T _message_)
        {
            var e = new KafkaConsumerMessageArgs(_message_, null, null);
            OnMessage?.Invoke(e);//等同于 if(OnMessage!=null){ OnMessage(e); } 
            return e;
        }
        /// <summary>
        /// 消费者出错事件
        /// </summary>
        public event KafkaConsumerHandle KakfaConsumerException;
        private KafkaConsumerMessageArgs onKakfaConsumerException(ConsumeException _exception_, string _exception_messages_ = "")
        {
            var e = new KafkaConsumerMessageArgs(default(T), _exception_messages_, _exception_, null);
            KakfaConsumerException?.Invoke(e);
            return e;
        }
        /// <summary>
        /// 操作出错或取消事件
        /// </summary>
        public event KafkaConsumerHandle KafkaSystemExcepiton;
        private KafkaConsumerMessageArgs onKafkaSystemExcepiton(Exception _exception_, string _exception_messages_ = "")
        {
            var e = new KafkaConsumerMessageArgs(default(T), "", null, _exception_);
            KafkaSystemExcepiton?.Invoke(e);
            return e;
        }
        #endregion

        private List<string> Topics = new List<string>();
        private ConsumerConfig consumerConfig;
        private IConsumer<string, T> consumer;
        /// <summary>
        /// 初始化
        /// 如果有用户名和密码,请放在KafkaConsumerConfig的ClientConfig属性中
        /// </summary>
        /// <param name="_config"></param>
        /// <param name="_deserializer_">自定义序列化器</param>
        public KafkaConsumer(KafkaConsumerConfig _config_, IDeserializer<T> _deserializer_ = null)
        {
            this.Topics = _config_.Topics;
            if (_config_.ClientConfig.Count > 0)
                consumerConfig = new ConsumerConfig(_config_.ClientConfig) { BootstrapServers = _config_.BootstrapServers };
            else
                consumerConfig = new ConsumerConfig { BootstrapServers = _config_.BootstrapServers };
            #region config（更多选项配置）
            if (_config_.AutoOffsetReset.HasValue)
            {
                consumerConfig.AutoOffsetReset = _config_.AutoOffsetReset;
            }
            if (_config_.EnableAutoCommit.HasValue)
            {
                consumerConfig.EnableAutoCommit = _config_.EnableAutoCommit;
            }
            if (_config_.EnablePartitionEof.HasValue)
            {
                consumerConfig.EnablePartitionEof = _config_.EnablePartitionEof;
            }
            if (_config_.SessionTimeoutMs.HasValue)
            {
                consumerConfig.SessionTimeoutMs = _config_.SessionTimeoutMs;
            }
            if (_config_.StatisticsIntervalMs.HasValue)
            {
                consumerConfig.StatisticsIntervalMs = _config_.StatisticsIntervalMs;
            }
            #endregion
            var consumerBuilder = new ConsumerBuilder<string, T>(consumerConfig);
            if (_deserializer_ != null)
            {
                consumer = consumerBuilder.SetValueDeserializer(_deserializer_).Build();
            }
            consumer = consumerBuilder.Build();
        }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="_config_">服务器、Topic、用户名和密码</param>
        public KafkaConsumer(KafkaConsumerConfigForCredit _config_, IDeserializer<T> _deserializer_ = null)
        {
            this.Topics = _config_.Topics;
            consumerConfig = new ConsumerConfig
            {
                BootstrapServers = _config_.BrokerServers,
                SaslUsername = _config_.SaslUsername,
                SaslPassword = _config_.SaslPassword,
                SaslMechanism = _config_.SaslMechanism,
                SecurityProtocol = _config_.SecurityProtocol,
                GroupId = (string.IsNullOrEmpty(_config_.GroupId)) ? Guid.NewGuid().ToString() : _config_.GroupId,
                EnableAutoCommit = _config_.EnableAutoCommit,
                StatisticsIntervalMs = (_config_.StatisticsIntervalMs == 0) ? 6000 : _config_.StatisticsIntervalMs,
                SessionTimeoutMs = (_config_.SessionTimeoutMs == 0) ? 6000 : _config_.SessionTimeoutMs,
                AutoOffsetReset = _config_.AutoOffsetReset,
                EnablePartitionEof = _config_.EnablePartitionEof
            };
            var consumerBuilder = new ConsumerBuilder<string, T>(consumerConfig);
            if (_deserializer_ != null)
            {
                consumer = consumerBuilder.SetValueDeserializer(_deserializer_).Build();
            }
            consumer = consumerBuilder.Build();
        }

        /// <summary>
        /// 高级初始化的方法
        /// 注意：别忘记了Topics哦！
        /// </summary>
        /// <param name="Topics">Topics List</param>
        /// <param name="clientConfig">通常只需要设置：clientConfig的BootstrapServers、SaslUsername、SaslPassword属性即可</param>
        public KafkaConsumer(KafkaClientConfig clientConfig, List<string> Topics, IDeserializer<T> _deserializer_ = null)
        {
            this.Topics = Topics;
            consumerConfig = new ConsumerConfig(new ClientConfig
            {
                #region 常规选项（服务器地址、用户名和密码）
                BootstrapServers = clientConfig.BootstrapServers,
                SaslUsername = clientConfig.SaslUsername,
                SaslPassword = clientConfig.SaslPassword,
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
                SaslMechanism = clientConfig.SaslMechanism,
                SaslOauthbearerConfig = clientConfig.SaslOauthbearerConfig,
                SecurityProtocol = clientConfig.SecurityProtocol,
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
            });
            var consumerBuilder = new ConsumerBuilder<string, T>(consumerConfig);
            if (_deserializer_ != null)
            {
                consumer = consumerBuilder.SetValueDeserializer(_deserializer_).Build();
            }
            consumer = consumerBuilder.Build();
        }
        /// <summary>
        /// 开启消费者
        /// </summary>
        public void Consume(JsonDeserializer<T> deserializer = null)
        {
            consumer.Subscribe(this.Topics);
            CancellationTokenSource cts = new CancellationTokenSource();
            try
            {
                while (true)
                {
                    try
                    {
                        var cr = consumer.Consume(cts.Token);
                        onOnMessage(cr.Value);
                    }
                    catch (ConsumeException e)
                    {
                        onKakfaConsumerException(e, e.Message);
                    }
                }
            }
            catch (Exception opException)
            {
                consumer.Close();
                onKafkaSystemExcepiton(opException, opException.Message);
            }
        }
    }
}
