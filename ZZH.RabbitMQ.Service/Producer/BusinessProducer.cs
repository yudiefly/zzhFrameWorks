using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using ZZH.RabbitMQ.Service.Model;

namespace ZZH.RabbitMQ.Service.Producer
{
    public class BusinessProducer
    {
        /// <summary>
        /// MQ的基本配置信息
        /// </summary>
        private Constants constants { set; get; }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="_constants"></param>
        public BusinessProducer(Constants _constants)
        {
            constants = _constants;
        }
        public byte[] Serialize(object obj)
        {

            using (var stream = new MemoryStream())
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, obj);
                return stream.ToArray();
            }
        }
        /// <summary>
        /// Publish
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queue"></param>
        /// <param name="msg"></param>
        /// <param name="type"></param>
        public void Publish<T>(string queue, T msg)
        {
            IConnection Connection = null;
            IModel Channel = null;
            try
            {
                Connection = RabbitMQHelper.CreateConnectFactory(constants).CreateConnection();
                Channel = Connection.CreateModel();
                //Channel.BasicQos(0, 1, false);
                var properties = Channel.CreateBasicProperties();
                properties.DeliveryMode = 2;//数据持久化
                //properties.Headers = queueArg;
                properties.Headers = new Dictionary<string, object>();
                //var msgBytes = Serialize(new BaseMessage<T>(msg, 0));
                var msgBytes = Serialize(new BaseMessage<T>(msg));
                //var msgBytes = Encoding.UTF8.GetBytes(msg);
                Channel.BasicPublish(constants.BUSINESS_EXCHANGE,constants.TAG+queue, properties, msgBytes);
            }
            catch (Exception ex)
            {
                string str = ex.Message;
            }
            finally
            {
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
        }
        /// <summary>
        /// 异步方式发布消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queue"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public async Task PublishAsync<T>(string queue,T msg)
        {
            IConnection Connection = null;
            IModel Channel = null;
            try
            {
                Connection = RabbitMQHelper.CreateConnectFactory(constants).CreateConnection();
                Channel = Connection.CreateModel();
                //Channel.BasicQos(0, 1, false);
                var properties = Channel.CreateBasicProperties();
                properties.DeliveryMode = 2;//数据持久化
                //properties.Headers = queueArg;
                properties.Headers = new Dictionary<string, object>();
                //var msgBytes = Serialize(new BaseMessage<T>(msg, 0));
                var msgBytes = Serialize(new BaseMessage<T>(msg));
                //var msgBytes = Encoding.UTF8.GetBytes(msg);
                Channel.BasicPublish(constants.BUSINESS_EXCHANGE, constants.TAG + queue, properties, msgBytes);
            }
            catch (Exception ex)
            {
                string str = ex.Message;
            }
            finally
            {
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
        }
    }
}
