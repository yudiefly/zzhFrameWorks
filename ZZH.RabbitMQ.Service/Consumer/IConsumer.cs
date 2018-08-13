using System;
using System.Collections.Generic;
using System.Text;

namespace ZZH.RabbitMQ.Service.Consumer
{
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
        /// <summary>
        /// 判断是否运行
        /// </summary>
        /// <returns></returns>
        bool IsRunning();
    }
}
