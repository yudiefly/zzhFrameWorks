using System;
using System.Collections.Generic;
using System.Text;

namespace ExampleModel
{
    public class ResponseData<T>
    {
        public T Data { set; get; }

        public string Code { set; get; }

        public string Messages { set; get; }
    }
}
