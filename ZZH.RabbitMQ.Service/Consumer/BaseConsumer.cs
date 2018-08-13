using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace ZZH.RabbitMQ.Service.Consumer
{
    public abstract class BaseConsumer : IConsumer
    {
        protected IConnection Connection;
        protected IModel Channel;
        protected EventingBasicConsumer Consumer;

        public void UnSubscribe()
        {
            if (Consumer != null)
            {
                Consumer = null;
            }
            if (Channel != null && Channel.IsOpen)
            {
                Channel.Close();
                Channel.Dispose();
            }
            if (Connection != null && Connection.IsOpen)
            {
                Connection.Close();
                Connection.Dispose();
            }
        }

        public bool IsRunning()
        {
            return Consumer != null && Consumer.IsRunning;
        }

        public abstract void Subscribe();

        protected T Deserialize<T>(byte[] byteData)
        {
            using (var stream = new MemoryStream(byteData, false))
            {
                IFormatter formatter = new BinaryFormatter();
                return (T)formatter.Deserialize(stream);
            }
        }
        protected const int MAX_RETRY_COUNT = 20;
        public void RejectInvoke(IModel channel, BasicDeliverEventArgs eventArgs)
        {
            var list = new List<object>();
            if (eventArgs.BasicProperties.Headers.Keys.Contains("x-death"))
            {
                list = (List<object>)eventArgs.BasicProperties.Headers["x-death"];
            }
            if (list.Count < MAX_RETRY_COUNT)
            {
                channel.BasicReject(eventArgs.DeliveryTag, false);
                //channel.BasicNack(eventArgs.DeliveryTag, false,false);
            }
            else
            {
                channel.BasicAck(eventArgs.DeliveryTag, false);
            }
        }
    }
}
