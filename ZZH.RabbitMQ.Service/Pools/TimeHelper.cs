using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZZH.RabbitMQ.Service
{
    public static class TimeHelper
    {
        public static double GetTimeDurationTotalSeconds(DateTime startTime, DateTime endTime)
        {
            TimeSpan ts1 = new TimeSpan(startTime.Ticks);
            TimeSpan ts2 = new TimeSpan(endTime.Ticks);
            TimeSpan ts = ts1.Subtract(ts2).Duration();
            return ts.TotalSeconds;
        }
    }
}
