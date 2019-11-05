using System;
using Bumblebee;
using Bumblebee.Caching;
using Bumblebee.ConcurrentLimits;
using Bumblebee.Configuration;
using Bumblebee.Jwt;
using Bumblebee.Logs;
using Bumblebee.UrlRewrite;

namespace ZZH.Bumblebee.GateWay
{
    class Program
    {
        private static Gateway gateway;
        static void Main(string[] args)
        {
            gateway = new Gateway();
            gateway.HttpOptions(o => {
                o.Port = 8080;
                o.LogToConsole = true;
                o.LogLevel = BeetleX.EventArgs.LogType.Error;
            });
            gateway.Open();            
            gateway.LoadPlugin(
                //管理插件提供UI配置
                typeof(Management).Assembly,
                //统一验证(jwt)
                typeof(JwtPlugin).Assembly,
                //并发控制
                typeof(UrlConcurrentLimits).Assembly,
                //日志处理
                typeof(FileLog).Assembly,
                //URL重写
                typeof(RewriteLoader).Assembly,
                //输出缓存
                typeof(default_request_cached_reader).Assembly
                );
            
            Console.Read();
        }
    }
}
