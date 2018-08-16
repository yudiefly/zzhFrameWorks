using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ExampleModel;
using Microsoft.AspNetCore.Mvc;
using zipkin4net.Transport.Http;

namespace ZZH.ZipKinClient.Service.Test.One.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        public ValuesController()
        {
            ZipKinClientHelper.RegisterService();
        }
        [HttpGet]
        [Route("entity")]
        public ResponseData<MyEntity> getEntity()
        {
            using (var httpclient = new HttpClient(new TracingHandler(ZipKinClientHelper.ServiceName)))
            {
                var callServiceUrl = "http://localhost:40179/api/values/entity";
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
            using (var httpclient = new HttpClient(new TracingHandler(ZipKinClientHelper.ServiceName)))
            {
                var callServiceUrl = "http://localhost:40179/api/values/Login";
                var strBody = JSONHelper.SerializeObject(input);
                HttpContent content = new StringContent(strBody);
                var response = httpclient.PostAsync(callServiceUrl,content).Result;
                var strResult = response.Content.ReadAsStringAsync().Result;
                var rst = JSONHelper.DeserializeObject<ResponseData<UserOutPut>>(strResult);
                return rst;
            }
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
