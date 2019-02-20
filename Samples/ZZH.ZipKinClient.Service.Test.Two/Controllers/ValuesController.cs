using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ExampleModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using zipkin4net.Transport.Http;

namespace ZZH.ZipKinClient.Service.Test.Two.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
         private IHttpContextAccessor _accessor;
        public ValuesController(IHttpContextAccessor accessor)
        {
            //var request = MyHttpContext.Current.Request;           
            _accessor = accessor;
            var content = accessor.HttpContext;
            var request = content.Request;
            ZipKinClientHelper.RegisterService();
        }

        [HttpGet]
        [Route("entity")]
        public ResponseData<MyEntity> getEntity()
        {

            using (var httpclient = new HttpClient(new TracingHandler(ZipKinClientHelper.ServiceName)))
            {
                var callServiceUrl = "http://localhost:34318/api/values/entity";
                var response = httpclient.GetStringAsync(callServiceUrl);
                var rst = JSONHelper.DeserializeObject<ResponseData<MyEntity>>(response.Result);
                return rst;
            }          
        }

        /// <summary>
        /// Login Test
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("Login")]
        [HttpPost]
        public ResponseData<UserOutPut> Login([FromBody]UserInput input)
        {
            #region login
            if (input == null)
            {
                return new ResponseData<UserOutPut> { Code="-100", Messages="参数错误" };
            }
            else
            {
                if (input.UserName == "yudiefly" || input.UserName == "NIO" || input.UserName == "zzh203" || input.UserName == "zhuzonghai")
                {
                    if (input.Password == "NIO" + DateTime.Now.ToString("MMddHH"))
                    {
                        return new ResponseData<UserOutPut>
                        {
                            Code = "200",
                            Data = new UserOutPut
                            {
                                LiveTime = DateTime.Now.AddHours(10).ToString("yyyy-MM-dd HH:mm:ss"),
                                LoginTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                                Token = "NIO" + Guid.NewGuid().ToString().Replace("-", ""),
                                UserName = input.UserName
                            },
                            Messages = "Success"
                        };
                    }
                    return new ResponseData<UserOutPut> {
                         Code="999", Messages="用户名或密码错误"
                    };
                }
                else
                {
                    return new ResponseData<UserOutPut>
                    {
                        Code = "999",
                        Messages = "用户名或密码错误"
                    };
                }
            }
            #endregion
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
