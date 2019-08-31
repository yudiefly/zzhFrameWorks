using NLog;
using NLog.LayoutRenderers;
using System;
using System.Net;
using System.Text;

namespace ZZH.NLog.Kafka
{
    [LayoutRenderer("HostName")]
    public class HostNameLayoutRenderer : LayoutRenderer
    {
        protected override void Append(StringBuilder buffer, LogEventInfo logEvent)
        {
            buffer.Append(Dns.GetHostName());
        }
    }
}
