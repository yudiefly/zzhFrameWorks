using System;
using System.Threading.Tasks;

namespace ZZH.Kafka.Service.Test
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string BootstrapServers = "10.132.12.11:9092,10.132.12.7:9092,10.132.12.8:9092";// "10.125.234.185:9092,10.125.233.31:9092,10.125.232.186:9092";
            //定义一个生产者
            var config = new KafkaProducerConfigForCredit
            {
                BrokerServers = BootstrapServers,
                SaslUsername = "9740d9b27d5a",
                SaslPassword = "1efd388171b8",
                SecurityProtocol = Confluent.Kafka.SecurityProtocol.SaslPlaintext,
                SaslMechanism = Confluent.Kafka.SaslMechanism.Plain,
                TopicName = "yudiefly-home-app"
            };

            var producer = new KafkaProducer<string>(config);
            producer.KakfaProducerSucces += Producer_KakfaProducerSucces;
            producer.KakfaProducerException += Producer_KakfaProducerException;
            for (int i = 0; i < 50; i++)
            {
                //await producer.ProduceAsync("i=" + i.ToString(), "");// "key_" + i.ToString()
                producer.Produce("TT-i:" + i.ToString(), "my-key:" + i.ToString());
            }

            //定义一个消费者
            var consumer = new KafkaConsumer<string>(new KafkaConsumerConfigForCredit
            {
                #region 这几个参数可以不指定（取默认也可以）
                //GroupId = Guid.NewGuid().ToString(),
                //SessionTimeoutMs = 6000,
                //StatisticsIntervalMs = 6000,
                #endregion

                BrokerServers = BootstrapServers,
                SaslUsername = "9740d9b27d5a",//NPtaqalu
                SaslPassword = "1efd388171b8",//Y8gutwQQuPNmUYWC
                SecurityProtocol = Confluent.Kafka.SecurityProtocol.SaslPlaintext,
                SaslMechanism = Confluent.Kafka.SaslMechanism.Plain,
                Topics = new System.Collections.Generic.List<string>() { "yudiefly-home-app" } 
            });
            consumer.OnMessage += Consumer_OnMessage;
            consumer.KakfaConsumerException += Consumer_KakfaConsumerException;
            consumer.Consume();

            Console.ReadLine();
        }

        private static void Consumer_KakfaConsumerException(KafkaConsumer<string>.KafkaConsumerMessageArgs e)
        {
            Console.WriteLine($"failed to get message: {e.ExceptionOfMessages} [{e.KafkaCousmerExecption.StackTrace}]");
        }

        private static void Producer_KakfaProducerSucces(KafkaProducer<string>.KafkaProducerArgs e)
        {
            Console.WriteLine($"Success to deliver message: {e.Key} [{e.Messages}]");
        }

   
        private static void Consumer_OnKakfaConsumerException(KafkaConsumer<string>.KafkaConsumerMessageArgs e)
        {
            Console.WriteLine($"failed to deliver message: {e.KafkaCousmerExecption.Message} [{e.KafkaCousmerExecption.Error.Code}]");
        }

        private static void Consumer_OnMessage(KafkaConsumer<string>.KafkaConsumerMessageArgs e)
        {
            Console.WriteLine($"we get the message is: {e.Messages} 【success】");
        }

        private static void Producer_KakfaProducerException(KafkaProducer<string>.KafkaProducerArgs e)
        {
            Console.WriteLine($"failed to deliver message:{e.KafkaProducerExecption.Message} [{e.KafkaProducerExecption.Error.Code}]");

        }
    }
}
