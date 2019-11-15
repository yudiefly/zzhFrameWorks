using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZZH.Kafka.Service
{
    /// <summary>
    /// 客户端链接配置类--该类为Kafka的高级玩家所配备，普通玩家只需要关注以下属性：
    /// BootstrapServers、 SaslUsername、SaslPassword
    /// </summary>
    public class KafkaClientConfig
    {
        #region 常规选项配置（服务器地址、用户名和密码）
        /// <summary>
        /// Initial list of brokers as a CSV list of broker host or host:port. The application
        /// may also use `rd_kafka_brokers_add()` to add brokers during runtime. default:'' importance: high
        /// </summary>
        public string BootstrapServers { get; set; }
        /// <summary>
        ///  SASL username for use with the PLAIN and SASL-SCRAM-.. mechanisms default: ''
        ///  importance: high
        /// </summary>
        public string SaslUsername { get; set; }
        /// <summary>
        /// SASL password for use with the PLAIN and SASL-SCRAM-.. mechanism default: ''
        /// importance: high
        /// </summary>
        public string SaslPassword { get; set; }

        #endregion
        #region 高级选项配置
        /// <summary>
        /// Timeout for broker API version requests. default: 10000 importance: low
        /// </summary>
        public int? ApiVersionRequestTimeoutMs { get; set; }

        /// <summary>
        ///       Dictates how long the `broker.version.fallback` fallback is used in the case
        ///     the ApiVersionRequest fails. **NOTE**: The ApiVersionRequest is only issued when
        ///     a new connection to the broker is made (such as after an upgrade). default: 0
        ///     importance: medium
        /// </summary>
        public int? ApiVersionFallbackMs { get; set; }

        /// <summary>
        /// Older broker versions (before 0.10.0) provide no way for a client to query for
        ///     supported protocol features (ApiVersionRequest, see `api.version.request`) making
        ///     it impossible for the client to know what features it may use. As a workaround
        ///     a user may set this property to the expected broker version and the client will
        ///    automatically adjust its feature set accordingly if the ApiVersionRequest fails
        ///     (or is disabled). The fallback broker version will be used for `api.version.fallback.ms`.
        ///     Valid values are: 0.9.0, 0.8.2, 0.8.1, 0.8.0. Any other value >= 0.10, such as
        ///     0.10.2.1, enables ApiVersionRequests. default: 0.10.0 importance: medium
        /// </summary>
        public string BrokerVersionFallback { get; set; }

        /// <summary>
        /// Protocol used to communicate with brokers. default: plaintext importance: high
        /// </summary>
        public SecurityProtocol? SecurityProtocol { get; set; }

        /// <summary>
        /// A cipher suite is a named combination of authentication, encryption, MAC and
        /// key exchange algorithm used to negotiate the security settings for a network
        ///  connection using TLS or SSL network protocol. See manual page for `ciphers(1)`
        /// and `SSL_CTX_set_cipher_list(3). default: '' importance: low
        /// </summary>
        public string SslCipherSuites { get; set; }

        /// <summary>
        /// The supported-curves extension in the TLS ClientHello message specifies the curves
        /// (standard/named, or 'explicit' GF(2^k) or GF(p)) the client is willing to have
        ///  the server use. See manual page for `SSL_CTX_set1_curves_list(3)`. OpenSSL >=
        ///  1.0.2 required. default: '' importance: low        
        /// </summary>
        public string SslCurvesList { get; set; }

        /// <summary>
        /// The client uses the TLS ClientHello signature_algorithms extension to indicate
        ///  to the server which signature/hash algorithm pairs may be used in digital signatures.
        /// See manual page for `SSL_CTX_set1_sigalgs_list(3)`. OpenSSL >= 1.0.2 required.
        /// default: '' importance: low
        /// </summary>
        public string SslSigalgsList { get; set; }

        /// <summary>
        ///  Path to client's private key (PEM) used for authentication. default: '' importance:low
        /// </summary>
        public string SslKeyLocation { get; set; }

        /// <summary>
        /// Private key passphrase (for use with `ssl.key.location` and `set_ssl_cert()`) default: '' importance: low
        /// </summary>
        public string SslKeyPassword { get; set; }

        /// <summary>
        ///  Client's private key string (PEM format) used for authentication. default: '' importance: low
        /// </summary>
        public string SslKeyPem { get; set; }

        /// <summary>
        /// Path to client's public key (PEM) used for authentication. default: '' importance:low
        /// </summary>
        public string SslCertificateLocation { get; set; }

        /// <summary>
        /// Client's public key string (PEM format) used for authentication. default: ''importance: low
        /// </summary>
        public string SslCertificatePem { get; set; }

        /// <summary>
        /// File or directory path to CA certificate(s) for verifying the broker's key. default:'' importance: low
        /// </summary>
        public string SslCaLocation { get; set; }

        /// <summary>
        /// Path to CRL for verifying broker's certificate validity. default: '' importance:low
        /// </summary>
        public string SslCrlLocation { get; set; }

        /// <summary>
        ///  Path to client's keystore (PKCS#12) used for authentication. default: '' importance:low
        /// </summary>
        public string SslKeystoreLocation { get; set; }

        /// <summary>
        /// Client's keystore (PKCS#12) password. default: '' importance: low
        /// </summary>
        public string SslKeystorePassword { get; set; }

        /// <summary>
        /// Enable OpenSSL's builtin broker (server) certificate verification. This verification
        /// can be extended by the application by implementing a certificate_verify_cb. default:
        /// true importance: low
        /// </summary>
        public bool? EnableSslCertificateVerification { get; set; }

        /// <summary>
        ///  Endpoint identification algorithm to validate broker hostname using broker certificate.
        ///  https - Server (broker) hostname verification as specified in RFC2818. none -
        ///  No endpoint verification. OpenSSL >= 1.0.2 required. default: none importance:low
        /// </summary>
        public SslEndpointIdentificationAlgorithm? SslEndpointIdentificationAlgorithm { get; set; }

        /// <summary>
        ///  Kerberos principal name that Kafka runs as, not including /hostname@REALM default:
        ///   kafka importance: low
        /// </summary>
        public string SaslKerberosServiceName { get; set; }

        /// <summary>
        /// This client's Kerberos principal name. (Not supported on Windows, will use the 
        /// logon user's principal). default: kafkaclient importance: low
        /// </summary>
        public string SaslKerberosPrincipal { get; set; }

        /// <summary>
        /// Shell command to refresh or acquire the client's Kerberos ticket. This command
        /// is executed on client creation and every sasl.kerberos.min.time.before.relogin 
        /// (0=disable). %{config.prop.name} is replaced by corresponding config object value.
        /// default: kinit -R -t "%{sasl.kerberos.keytab}" -k %{sasl.kerberos.principal}
        /// || kinit -t "%{sasl.kerberos.keytab}" -k %{sasl.kerberos.principal} importance:low
        /// </summary>
        public string SaslKerberosKinitCmd { get; set; }

        /// <summary>
        /// Path to Kerberos keytab file. This configuration property is only used as a variable
        /// in `sasl.kerberos.kinit.cmd` as ` ... -t "%{sasl.kerberos.keytab}"`. default:
        /// '' importance: low
        /// </summary>
        public string SaslKerberosKeytab { get; set; }

        /// <summary>
        /// Minimum time in milliseconds between key refresh attempts. Disable automatic
        /// key refresh by setting this property to 0. default: 60000 importance: low
        /// </summary>
        public int? SaslKerberosMinTimeBeforeRelogin { get; set; }


        public string SaslOauthbearerConfig { get; set; }

        /// <summary>
        /// Request broker's supported API versions to adjust functionality to available
        /// protocol features. If set to false, or the ApiVersionRequest fails, the fallback
        ///  version `broker.version.fallback` will be used. **NOTE**: Depends on broker version
        /// >=0.10.0. If the request is not supported by (an older) broker the `broker.version.fallback`
        /// fallback is used. default: true importance: high
        /// </summary>
        public bool? ApiVersionRequest { get; set; }

        /// <summary>
        /// Signal that librdkafka will use to quickly terminate on rd_kafka_destroy(). If
        /// this signal is not set then there will be a delay before rd_kafka_wait_destroyed()
        /// returns true as internal threads are timing out their system calls. If this signal
        /// is set however the delay will be minimal. The application should mask this signal
        /// as an internal signal handler is installed. default: 0 importance: low
        /// </summary>
        public int? InternalTerminationSignal { get; set; }

        /// <summary>
        /// Print internal thread name in log messages (useful for debugging librdkafka internals)
        /// default: true importance: low
        /// </summary>
        public bool? LogThreadName { get; set; }

        /// <summary>
        /// Enable the builtin unsecure JWT OAUTHBEARER token handler if no oauthbearer_refresh_cb
        /// has been set. This builtin handler should only be used for development or testing,
        /// and not in production. default: false importance: low
        /// </summary>
        public bool? EnableSaslOauthbearerUnsecureJwt { get; set; }

        /// <summary>
        /// SASL mechanism to use for authentication. Supported: GSSAPI, PLAIN, SCRAM-SHA-256,
        /// SCRAM-SHA-512. **NOTE**: Despite the name, you may not configure more than one
        /// mechanism.
        /// </summary>
        public SaslMechanism? SaslMechanism { get; set; }

        /// <summary>
        /// This field indicates the number of acknowledgements the leader broker must receive
        /// from ISR brokers before responding to the request: Zero=Broker does not send
        /// any response/ack to client, One=The leader will write the record to its local
        /// log but will respond without awaiting full acknowledgement from all followers.
        /// All=Broker will block until message is committed by all in sync replicas (ISRs).
        /// If there are less than min.insync.replicas (broker configuration) in the ISR
        /// set the produce request will fail.
        /// </summary>
        public Acks? Acks { get; set; }

        /// <summary>
        /// Client identifier. default: rdkafka importance: low
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Maximum Kafka protocol request message size. default: 1000000 importance: medium
        /// </summary>
        public int? MessageMaxBytes { get; set; }

        /// <summary>
        ///  Maximum size for message to be copied to buffer. Messages larger than this will
        /// be passed by reference (zero-copy) at the expense of larger iovecs. default:
        /// 65535 importance: low
        /// </summary>
        public int? MessageCopyMaxBytes { get; set; }

        /// <summary>
        /// Maximum Kafka protocol response message size. This serves as a safety precaution
        /// to avoid memory exhaustion in case of protocol hickups. This value must be at
        /// least `fetch.max.bytes` + 512 to allow for protocol overhead; the value is adjusted
        /// automatically unless the configuration property is explicitly set. default: 100000000
        /// importance: medium
        /// </summary>
        public int? ReceiveMessageMaxBytes { get; set; }

        /// <summary>
        /// Maximum number of in-flight requests per broker connection. This is a generic
        /// property applied to all broker communication, however it is primarily relevant
        /// to produce requests. In particular, note that other mechanisms limit the number
        /// of outstanding consumer fetch request per broker to one. default: 1000000 importance:low
        /// </summary>
        public int? MaxInFlight { get; set; }

        /// <summary>
        /// Non-topic request timeout in milliseconds. This is for metadata requests, etc.
        ///  default: 60000 importance: low
        /// </summary>
        public int? MetadataRequestTimeoutMs { get; set; }

        /// <summary>
        ///  Period of time in milliseconds at which topic and broker metadata is refreshed
        /// in order to proactively discover any new brokers, topics, partitions or partition
        /// leader changes. Use -1 to disable the intervalled refresh (not recommended).
        /// If there are no locally referenced topics (no topic objects created, no messages
        /// produced, no subscription or no assignment) then only the broker list will be
        /// refreshed every interval but no more often than every 10s. default: 300000 importance:low
        /// </summary>
        public int? TopicMetadataRefreshIntervalMs { get; set; }

        /// <summary>
        /// Metadata cache max age. Defaults to topic.metadata.refresh.interval.ms * 3 default:
        /// 900000 importance: low
        /// </summary>
        public int? MetadataMaxAgeMs { get; set; }

        /// <summary>
        /// When a topic loses its leader a new metadata request will be enqueued with this
        /// initial interval, exponentially increasing until the topic metadata has been
        /// refreshed. This is used to recover quickly from transitioning leader brokers.
        /// default: 250 importance: low
        /// </summary>
        public int? TopicMetadataRefreshFastIntervalMs { get; set; }

        /// <summary>
        /// Sparse metadata requests (consumes less network bandwidth) default: true importance:low
        /// </summary>
        public bool? TopicMetadataRefreshSparse { get; set; }

        /// <summary>
        ///  Topic blacklist, a comma-separated list of regular expressions for matching topic
        ///  names that should be ignored in broker metadata information as if the topics
        ///  did not exist. default: '' importance: low
        /// </summary>
        public string TopicBlacklist { get; set; }

        /// <summary>
        /// A comma-separated list of debug contexts to enable. Detailed Producer debugging:
        ///  broker,topic,msg. Consumer: consumer,cgrp,topic,fetch default: '' importance: medium
        /// </summary>
        public string Debug { get; set; }

        /// <summary>
        /// Default timeout for network requests. Producer: ProduceRequests will use the
        /// lesser value of `socket.timeout.ms` and remaining `message.timeout.ms` for the
        ///  first message in the batch. Consumer: FetchRequests will use `fetch.wait.max.ms`
        /// + `socket.timeout.ms`. Admin: Admin requests will use `socket.timeout.ms` or
        /// explicitly set `rd_kafka_AdminOptions_set_operation_timeout()` value. default:60000 importance: low
        /// </summary>
        public int? SocketTimeoutMs { get; set; }

        /// <summary>
        ///  Broker socket send buffer size. System default is used if 0. default: 0 importance:low
        /// </summary>
        public int? SocketSendBufferBytes { get; set; }

        /// <summary>
        ///  Broker socket receive buffer size. System default is used if 0. default: 0 importance:low
        /// </summary>
        public int? SocketReceiveBufferBytes { get; set; }

        /// <summary>
        /// Enable TCP keep-alives (SO_KEEPALIVE) on broker sockets default: false importance:low
        /// </summary>
        public bool? SocketKeepaliveEnable { get; set; }

        /// <summary>
        ///  Disable the Nagle algorithm (TCP_NODELAY) on broker sockets. default: false importance:low
        /// </summary>
        public bool? SocketNagleDisable { get; set; }

        /// <summary>
        /// Disconnect from broker when this number of send failures (e.g., timed out requests)
        ///  is reached. Disable with 0. WARNING: It is highly recommended to leave this setting
        ///  at its default value of 1 to avoid the client and broker to become desynchronized
        ///  in case of request timeouts. NOTE: The connection is automatically re-established.
        ///  default: 1 importance: low 
        /// </summary>       
        public int? SocketMaxFails { get; set; }

        /// <summary>
        /// How long to cache the broker address resolving results (milliseconds). default: 1000 importance: low
        /// </summary>
        public int? BrokerAddressTtl { get; set; }

        /// <summary>
        /// Allowed broker IP address families: any, v4, v6 default: any importance: low
        /// </summary>
        public BrokerAddressFamily? BrokerAddressFamily { get; set; }

        /// <summary>
        /// The initial time to wait before reconnecting to a broker after the connection
        /// has been closed. The time is increased exponentially until `reconnect.backoff.max.ms`
        ///  is reached. -25% to +50% jitter is applied to each reconnect backoff. A value
        ///  of 0 disables the backoff and reconnects immediately. default: 100 importance:  medium
        /// </summary>
        public int? ReconnectBackoffMs { get; set; }

        /// <summary>
        /// The maximum time to wait before reconnecting to a broker after the connection 
        /// has been closed. default: 10000 importance: medium
        /// </summary>
        public int? ReconnectBackoffMaxMs { get; set; }

        /// <summary>
        /// librdkafka statistics emit interval. The application also needs to register a
        /// stats callback using `rd_kafka_conf_set_stats_cb()`. The granularity is 1000ms.
        ///  A value of 0 disables statistics. default: 0 importance: high
        /// </summary>
        public int? StatisticsIntervalMs { get; set; }

        /// <summary>
        /// Disable spontaneous log_cb from internal librdkafka threads, instead enqueue
        /// log messages on queue set with `rd_kafka_set_log_queue()` and serve log callbacks
        /// or events through the standard poll APIs. **NOTE**: Log messages will linger
        ///in a temporary queue until the log queue has been set. default: false importance:low
        /// </summary>
        public bool? LogQueue { get; set; }

        /// <summary>
        /// Log broker disconnects. It might be useful to turn this off when interacting
        /// with 0.9 brokers with an aggressive `connection.max.idle.ms` value. default:
        /// true importance: low
        /// </summary>
        public bool? LogConnectionClose { get; set; }

        /// <summary>
        ///  List of plugin libraries to load (; separated). The library search path is platform
        ///  dependent (see dlopen(3) for Unix and LoadLibrary() for Windows). If no filename
        ///   extension is specified the platform-specific extension (such as .dll or .so)
        /// will be appended automatically. default: '' importance: low
        /// </summary>
        public string PluginLibraryPaths { get; set; }
        #endregion
    }
}
