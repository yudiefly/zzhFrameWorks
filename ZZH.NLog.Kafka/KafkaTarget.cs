using Confluent.Kafka;
using NLog;
using NLog.Config;
using NLog.Targets;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZZH.NLog.Kafka
{
    [Target("Kafka")]
    public class KafkaTarget : TargetWithLayout
    {
        internal static IProducer<Null, string> Producer;
        internal static string Topic { get; set; }

        [RequiredParameter]
        public string kafkaSection { get; set; }

        protected override void InitializeTarget()
        {
            base.InitializeTarget();
            NLogKafkaExtensions.KafkaSection = kafkaSection;
        }

        protected override void Write(LogEventInfo logEvent)
        {
            //var task = Task.Factory.StartNew(() =>
            //{
            string message = Layout.Render(logEvent);

            SendMessageToQueue(message, logEvent);
            //});


            // base.Write(logEvent);
        }

        //protected override void WriteAsyncThreadSafe(AsyncLogEventInfo logEvent)
        //{
        //    var task = Task.Factory.StartNew(() => {
        //        string message = Layout.Render(logEvent.LogEvent);

        //        SendMessageToQueue(message, logEvent.LogEvent);
        //    });
        //}


        private void SendMessageToQueue(string message, LogEventInfo logEvent)
        {
            try
            {
                if (string.IsNullOrEmpty(message))
                    return;
                #region 异步调用

                var task = Producer.ProduceAsync(Topic, new Message<Null, string> { Value = message });
                task.ContinueWith(t =>
                {
                    if (t.IsFaulted)
                    {

                        Console.WriteLine(t.Exception.Message + t.Exception.StackTrace);

                    }

                });

                #endregion

                #region 同步调用
                /*
                Action<DeliveryReport<Null, string>> handler = r =>
                {
                    if (r.Error.IsError)
                    {
                        Console.WriteLine($"Delivery Error: {r.Error.Reason}");
                    }
                    //Console.WriteLine(!r.Error.IsError
                    //    ? $"Delivered message:{r.Message.Value} to {r.TopicPartitionOffset}"
                    //    : $"Delivery Error: {r.Error.Reason}");

                };
                Producer.Produce(Topic, new Message<Null, string> { Value = message }, handler);
                */
                #endregion
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + e.StackTrace);
            }
        }

        private void CloseProducer()
        {
            if (Producer != null)
            {
                Producer?.Flush(TimeSpan.FromSeconds(60));
                Producer?.Dispose();
            }
            Producer = null;
        }


        protected override void CloseTarget()
        {
            CloseProducer();
            base.CloseTarget();
        }

    }
}
