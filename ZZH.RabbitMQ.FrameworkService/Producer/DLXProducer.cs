using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZZH.RabbitMQ.FrameworkService.Producer
{
    public class DLXProducer
    {
        //long TimeToLive = 1000 * 60 * 6;//6m
        private long TimeToLive = 1000 * 6;//6s
        /// <summary>
        /// 定义业务Exchange与死信Exchange并绑定相关队列
        /// </summary>
        /// <param name="constants">配置</param>
        /// <param name="timeTolive">TTL时间（毫秒）</param>
        public void Declare(Constants constants, long timeTolive=6000)
        {
            this.TimeToLive = timeTolive;
            IConnection Connection = null;
            IModel Channel = null;
            try
            {
                var queueArg = new Dictionary<string, object>();
                queueArg.Add("x-message-ttl", TimeToLive);
                queueArg.Add("x-dead-letter-exchange", constants.BUSINESS_EXCHANGE);
                //queueArg.Add("x-dead-letter-routing-key", Constants.HELLOWORLD_QUEUE);
                //不指定 x -dead-letter-routing-key 参数，则使用原来的routingkey

                Connection = RabbitMQHelper.CreateConnectFactory(constants).CreateConnection();
                Channel = Connection.CreateModel();
                Channel.BasicQos(0, 1, false);//这样RabbitMQ就会使得每个Consumer在同一个时间点最多处理一个Message。
                //换句话说，在接收到该Consumer的ack前，他它不会将新的Message分发给它。 
                Channel.ExchangeDeclare(constants.DXL_RETRY_EXCHANGE, ExchangeType.Fanout, true, false, null);//ExchangeType.Fanout 来自不同消息队列的 Routing Key 不同
                Channel.QueueDeclare(constants.DXL_RETRY_QUEUE, true, false, false, queueArg);
                Channel.QueueBind(constants.DXL_RETRY_QUEUE, constants.DXL_RETRY_EXCHANGE, string.Empty, null);
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
