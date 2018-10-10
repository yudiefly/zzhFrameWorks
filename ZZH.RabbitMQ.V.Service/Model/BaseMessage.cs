using System;
using System.Collections.Generic;
using System.Text;

namespace ZZH.RabbitMQ.V.Service.Model
{
    [Serializable]
    public class BaseMessage<T>
    {
        public T Data { get; set; }

        public BaseMessage(T t)
        {
            Data = t;
        }
    }
}
