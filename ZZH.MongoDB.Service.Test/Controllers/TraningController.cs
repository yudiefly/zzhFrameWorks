using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using ZZH.MongoDB.Service.Test.MongoConfigs;
using ZZH.MongoDB.Service.Test.Services;
using ZZH.MongoDB.Service.Test.Model;

namespace ZZH.MongoDB.Service.Test.Controllers
{
    [Produces("application/json")]
    [Route("api/Traning")]
    public class TraningController : Controller
    {
        private readonly IConfiguration Configuration;
        private readonly IOptions<MongoConfig> _MongoLogConfig, _MongoTwoConfig;
        public TraningController(IOptions<MongoLogConfig> settings1, IOptions<MongoTwoConfig> settings2, IConfiguration _Configuration)
        {
            _MongoLogConfig = settings1;
            _MongoTwoConfig = settings2;
            Configuration = _Configuration;
        }
        /// <summary>
        /// 一个测试接口，写入数据到MongoDB服务器中
        /// </summary>
        /// <param name="rows"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("traning")]
        public string Tranings(int rows = 5)
        {
            Random rnd = new Random(999999);
            var service = new HelloWorldService((MongoConfig)_MongoLogConfig.Value);
            for (int i = 0; i < rows; i++)
            {
                service.AddOrReplacePendingDataL(new HelloWorldEntity
                {
                    Id = Guid.NewGuid().ToString(),
                    CommitOn = DateTime.Now.AddHours(-1),
                    CTitle = "myTitle:" + i.ToString(),
                    CustId = rnd.Next(),
                    DctId = rnd.Next(),
                    Mobile = "15900507061",
                    Nickname = Guid.NewGuid().ToString(),
                    Sex = Convert.ToString(rnd.Next() % 2),
                    OrderByTime = DateTime.Now,
                    PhotoUrl = ""
                });
            }
            return "Success for add data rows into mongodb servers";
        }
    }
}