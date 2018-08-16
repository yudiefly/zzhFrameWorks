using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExampleModel;
using Microsoft.AspNetCore.Mvc;

namespace ZZH.ZipKinClient.Service.Test.Three.Controllers
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
            return new ResponseData<MyEntity>
            {
                Code = "200",
                Data = new MyEntity
                {
                    EName = "yudiefly",
                    timeSpan = DateTime.Now.AddMinutes(-10)
                },
                Messages = "Success"
            };
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
