using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using Orleans.Statistics;

namespace OrleansBasics
{
    public class Program
    {
        public static int Main(string[] args)
        {
            return RunMainAsync().Result;
        }

        private static async Task<int> RunMainAsync()
        {
            try
            {
                var host = await StartSilo();
                Console.WriteLine("\n\n Press Enter to terminate...\n\n");
                Console.ReadLine();

                await host.StopAsync();

                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return 1;
            }
        }

        private static async Task<ISiloHost> StartSilo()
        {
            var builder = new SiloHostBuilder()
                .UseLocalhostClustering()
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = "dev";
                    options.ServiceId = "OrleansBasics";
                })
            #region Soli配置
                 //// 端口设置（用于Silo到Silo通信端口为:11111，将网关的端口设置为:30000。)此方法将检测要监听的端口
                 //.ConfigureEndpoints(siloPort:11111,gatewayPort:30000)
                 //.Configure<EndpointOptions>(options =>
                 //{
                 //    // 用于 Silo-to-Silo 的端口
                 //    options.SiloPort = 11111;
                 //    //  gateway 的端口
                 //    options.GatewayPort = 30000;
                 //    // 在集群中进行注册的IP地址
                 //    options.AdvertisedIPAddress = IPAddress.Parse("172.16.0.42");
                 //    // 监听的silo 远程连接点
                 //    options.GatewayListeningEndpoint = new IPEndPoint(IPAddress.Any, 40000);
                 //    // 监听的silo 远程端口连接点
                 //    options.SiloListeningEndpoint = new IPEndPoint(IPAddress.Any, 50000);

                 //})
                 ////监听的主silo 远程连接点 为空则创建一个主silo连接点
                 //.UseDevelopmentClustering(new IPEndPoint(IPAddress.Parse("192.168.8.1"), 11111))
            #endregion
                 .Configure<EndpointOptions>(options => options.AdvertisedIPAddress = IPAddress.Loopback)//配置Silo的端口
                .ConfigureApplicationParts(parts =>parts.AddFromApplicationBaseDirectory()
                    .AddApplicationPart(typeof(HelloGrain).Assembly).WithReferences()
                    .AddApplicationPart(typeof(Student).Assembly).WithReferences()
                    .AddApplicationPart(typeof(Classroom).Assembly).WithReferences()
                )
                //注册Dashboard，具体信息见：https://github.com/OrleansContrib/OrleansDashboard
                .UseDashboard(options =>
                {
                    options.Username = "admin";
                    options.Password = "123456";
                    options.Host = "*";
                    options.Port = 9999;
                    options.HostSelf = true;
                    options.CounterUpdateIntervalMs = 1000;
                })
                .UsePerfCounterEnvironmentStatistics()
                .ConfigureLogging(logging => logging.AddConsole());

            var host = builder.Build();
            await host.StartAsync();
            return host;
        }
    }
}
