using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZZH.RabbitMQ.FrameworkService.Model
{
    [Serializable]
    public class BaseMessage<T>
    {
        //public int MsgType { get; set; }
        public T Data { get; set; }

        public BaseMessage(T t)
        {
            Data = t;
            //MsgType = msgType;
        }
    }
}
