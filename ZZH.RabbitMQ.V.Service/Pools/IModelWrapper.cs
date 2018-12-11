using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZZH.RabbitMQ.V.Service
{
    public class IModelWrapper : IDisposable
    {
        public IModel Model { get; set; }
        public bool IsBusy { get; set; }
        public DateTime IdleTime { get; set; }

        public IModelWrapper(IModel model)
        {
            if (model == null)
                new ArgumentNullException("参数model不能为空，请检查参数");         
            Model = model;
            IsBusy = true;
        }

        public void Dispose()
        {
            Model.Dispose();
        }

        public void SetNotBusy()
        {
            IsBusy = false;
            IdleTime = DateTime.UtcNow;
        }
    }
}
