using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZZH.RabbitMQ.FrameworkService.Consumer
{
    /// <summary>
    /// 消费者接口
    /// </summary>
    public interface IConsumer
    {
        /// <summary>
        /// 订阅
        /// </summary>
        void Subscribe();
        /// <summary>
        /// 退订
        /// </summary>
        void UnSubscribe();

        bool IsRunning();
    }
}
