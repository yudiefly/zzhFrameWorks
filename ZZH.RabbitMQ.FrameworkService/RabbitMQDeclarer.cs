using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZZH.RabbitMQ.FrameworkService
{
    public class RabbitMQDeclarer
    {
        /// <summary>
        /// 重试的间隔时间
        /// </summary>
        private long TimeToLive = 1000 * 60 * 6;//6m
        //long TimeToLive = 1000 * 6;//6s
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="constants"></param>
        /// <param name="QueueNames"></param>
        public void Declare(Constants constants, List<string> QueueNames, long timeTolive)
        {
            this.TimeToLive = timeTolive;
            IConnection Connection = null;
            IModel Channel = null;
            try
            {
                Connection = RabbitMQHelper.CreateConnectFactory(constants).CreateConnection();
                Channel = Connection.CreateModel();
                Channel.BasicQos(0, 1, false);//这样RabbitMQ就会使得每个Consumer在同一个时间点最多处理一个Message。
                                              //换句话说，在接收到该Consumer的ack前，他它不会将新的Message分发给它。 
                #region 死信 Exchange Queue
                var retryArg = new Dictionary<string, object>();
                retryArg.Add("x-message-ttl", TimeToLive);
                retryArg.Add("x-dead-letter-exchange", constants.BUSINESS_EXCHANGE);
                //不指定 x -dead-letter-routing-key 参数，则使用原来的routingkey

                Channel.ExchangeDeclare(constants.DXL_RETRY_EXCHANGE, ExchangeType.Fanout, true, false, null);//ExchangeType.Fanout 来自不同消息队列的 Routing Key 不同

                Channel.QueueDeclare(constants.DXL_RETRY_QUEUE, true, false, false, retryArg);
                Channel.QueueBind(constants.DXL_RETRY_QUEUE, constants.DXL_RETRY_EXCHANGE, string.Empty, null);
                #endregion

                #region Business Exchange queueArg
                var queueArg = new Dictionary<string, object>();
                queueArg.Add("x-dead-letter-exchange", constants.DXL_RETRY_EXCHANGE);

                Channel.ExchangeDeclare(constants.BUSINESS_EXCHANGE, ExchangeType.Direct, true, false, null);
                #endregion

                #region 定义队列
                foreach (string eachQueue in QueueNames)
                {
                    Channel.QueueDeclare(constants.TAG + eachQueue, true, false, false, queueArg);
                    Channel.QueueBind(constants.TAG + eachQueue, constants.BUSINESS_EXCHANGE, constants.TAG + eachQueue);
                }
                #endregion

                //测试HelloWorld
                //Channel.QueueDeclare(Constants.HELLOWORLD_QUEUE, true, false, false, queueArg);
                //Channel.QueueBind(Constants.HELLOWORLD_QUEUE, Constants.BUSINESS_EXCHANGE, Constants.HELLOWORLD_QUEUE, null);             

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
