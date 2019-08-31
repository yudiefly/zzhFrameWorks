using NLog.Config;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZZH.NLog.Kafka
{
    [NLogConfigurationItem]
    public class ProducerConfig
    {
        [RequiredParameter]
        public string Key { get; set; }

        [RequiredParameter]
        public string value { set; get; }
    }
}
