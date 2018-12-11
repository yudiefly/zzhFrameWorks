using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZZH.RabbitMQ.FrameworkService
{
    public class ConnectionWrapper : IDisposable
    {
        public ConnectionFactory ConnFactory { get; set; }      

        public DateTime IdleTime { get; set; }

        protected IConnection Conn { get; set; }

        protected ChannelPool ChannelPool { get; set; }


        public IModelWrapper GetOrCreateChannel()
        {
            IModelWrapper result = this.ChannelPool.GetOrCreateChannel();

            if (result != null)
                IdleTime = default(DateTime);

            return result;
        }

        public void Dispose()
        {
            ((IConnection)Conn).Dispose();
            ChannelPool = null;
        }

        public bool HasSessionConn()
        {
            return this.ChannelPool.GetSessionManager() > 0;
        }

        public ConnectionWrapper(ConnectionFactory connFactory, IConnection conn)
        {
            if (conn == null)
                throw new ArgumentNullException("参数conn不能为null，请检查参数");          
            Conn = conn;
            ConnFactory = connFactory;
            ChannelPool = new ChannelPool(conn);
        }


    }
}
