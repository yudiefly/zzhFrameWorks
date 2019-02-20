using System;
using System.Collections.Generic;
using System.Text;

namespace ExampleModel
{
    public class MyEntity
    {
        public string EName { set; get; }
        public DateTime timeSpan { set; get; }
    }
    public class UserInput
    {
        public string UserName { set; get; }

        public string Password { set; get; }
    }

    public class UserOutPut
    {
        public string UserName { set; get; }
        public string LoginTime { set; get; }
        /// <summary>
        /// Token
        /// </summary>
        public string Token { set; get; }
        /// <summary>
        /// 有效期
        /// </summary>
        public string LiveTime { set; get; }
    }
}
