using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ZZH.Dapper.Service.Test.Model;
using ZZH.Dapper.Service.Test.Respository;

namespace ZZH.Dapper.Service.Test.Controllers
{
    [Route("api/[controller]")]
    public class InfoController : Controller
    {
        private MembersRespository _memberService_ = new MembersRespository();
        [Route("members")]
        [HttpPost, HttpGet]
        public List<Members> GetMembers(string Name="")
        {
            var list = _memberService_.GetMembers(Name);
            return list;
        }
    }
}
