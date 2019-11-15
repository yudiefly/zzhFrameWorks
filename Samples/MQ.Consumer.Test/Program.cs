using System;
using ZZH.RabbitMQ.Service.Consumer;
using MQ.Consumer.Test.Consumer;

namespace MQ.Consumer.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            IConsumer DoctorInfoConsumer = new DoctorInfoConsumer(new ZZH.RabbitMQ.Service.Constants
            {

                HostName = "localhost",
                Password = "guest",
                Port = 5672,
                TAG = "YNIO_",
                UserName = "guest",
                VirtualHost = "/"
            });
            DoctorInfoConsumer.Subscribe();
            Console.ReadLine();
            //Console.WriteLine("Hello World!");
        }
    }
}
