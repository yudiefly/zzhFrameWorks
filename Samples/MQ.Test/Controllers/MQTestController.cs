using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExampleModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ZZH.RabbitMQ.Service.Model;
using ZZH.RabbitMQ.Service.Producer;
using ZZH.RabbitMQ.Service;

namespace MQ.Test.Controllers
{
    [Produces("application/json")]
    [Route("api/MQTest")]
    public class MQTestController : Controller
    {
        private ZZH.RabbitMQ.Service.Constants config;
        public MQTestController()
        {
            config = new ZZH.RabbitMQ.Service.Constants
            {
                HostName = "localhost",
                Password = "guest",
                UserName = "guest",
                Port = 5672,
                TAG = "NIO_",
                VirtualHost = "/"
            };
            //您的业务队列名列表（框架自动添加前缀）
            var queuelst = new List<string>();
            queuelst.Add("DOCTORINFO_QUEUE");
            new RabbitMQDeclarer().Declare(config, queuelst, 60000);

        }
        [Route("doctors")]
        public string UpdateDoctors()
        {

            var producter = new BusinessProducer(config);
            Random rnd = new Random(99999);

            var doctor = new DoctorInfoMQ
            {
                DoctorId = rnd.Next(),
                DoctorName = "Doc-100",
                HeadImgUrl = "",
                Introduce = "test:100",
                Level = "",
                PhoneNumber = "15900507061",
                ServiceDeptName = "NIO",
                Speciality = ""
            };
            producter.Publish("DOCTORINFO_QUEUE", doctor);

            return "Success!";
        }

    }
}