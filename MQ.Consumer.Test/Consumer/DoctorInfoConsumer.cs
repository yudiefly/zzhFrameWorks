using ExampleModel;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using ZZH.RabbitMQ.Service;
using ZZH.RabbitMQ.Service.Consumer;
using ZZH.RabbitMQ.Service.Model;

namespace MQ.Consumer.Test.Consumer
{
    public class DoctorInfoConsumer : BaseConsumer
    {
        private Constants constants;
        public DoctorInfoConsumer(Constants _constants)
        {
            constants = _constants;
        }

        public override void Subscribe()
        {
            try
            {
                Console.WriteLine("Subscribe start:");
                Connection = RabbitMQHelper.CreateConnectFactory(constants).CreateConnection();
                Channel = Connection.CreateModel();
                Channel.BasicQos(0, 1, false);
                Consumer = new EventingBasicConsumer(Channel);
                Consumer.Received += (model, eventArgs) =>
                {
                    try
                    {
                        var bytes = eventArgs.Body;
                        var doctor = Deserialize<BaseMessage<DoctorInfoMQ>>(bytes).Data;

                        var proxy = new ProxyDemo();
                        var result=proxy.SendDoctorInfo(doctor.DoctorId, doctor.DoctorName, doctor.Introduce, doctor.Level, doctor.PhoneNumber, doctor.ServiceDeptName, doctor.Speciality, doctor.HeadImgUrl);
                        if(result.Item1)
                        {
                            Channel.BasicAck(eventArgs.DeliveryTag, false);
                        }
                        else
                        {
                            RejectInvoke(Channel, eventArgs);
                        }                     
                        
                    }
                    catch (Exception ex)
                    {
                        Channel.BasicAck(eventArgs.DeliveryTag, false);                      
                    }
                };
                //消费者 订阅 消息队列
                Channel.BasicConsume(constants.TAG+ "DOCTORINFO_QUEUE", false, Consumer);//需要接受方发送ack回执,删除消息
            }
            catch (Exception ex)
            {
                Console.WriteLine("Execptions:{0}", ex.Message);
            }
        }
    }
}
