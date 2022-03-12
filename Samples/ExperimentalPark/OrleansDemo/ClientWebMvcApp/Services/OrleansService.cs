using Orleans;
using Orleans.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientWebMvcApp.Services
{
    public class OrleansService: IOrleansService
    {
        private readonly IClusterClient clusterClient;
        public OrleansService()
        {
            clusterClient = ConnectClient().Result;
        }

        public T GetGrain<T>(long integerKey) where T : IGrainWithIntegerKey
        {
            return clusterClient.GetGrain<T>(integerKey);
        }
        /// <summary>
        /// 使用本地配置连接服务
        /// </summary>
        /// <returns></returns>
        private async Task<IClusterClient> ConnectClient()
        {
            IClusterClient client=new ClientBuilder()
                .UseLocalhostClustering()
                .Configure<ClusterOptions>(options=>
                {
                    options.ClusterId = "dev";
                    options.ServiceId = "OrleansBasics";
                })
                .Build();
            await client.Connect();
            return client;
        }
    }
    public interface IOrleansService
    {
    }
}
