using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZZH.Dapper.Service.Test.Model
{
    /// <summary>
    /// Table Name is Members
    /// </summary>
    public class Members
    {
        public int Id { set; get; }
        public string Name { set; get; }
        public string BirthDay { set; get; }
        public string Sex { set; get; }
        public string Memos { set; get; }
    }
}
