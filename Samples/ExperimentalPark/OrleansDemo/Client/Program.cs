using System;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using System.Threading.Tasks;

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
                using(var client=await ConnectClient())
                {
                    await DoClientWork(client);
                    await DoClientWork_Student(client);
                    Console.ReadKey();
                }
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nException while trying to run client:{ex.Message}");
                Console.WriteLine($"Make sure the silo the client is trying to connect to running.");
                Console.WriteLine($"\nPress any key to exit.");
                Console.ReadKey();
                return 1;
            }
        }
        private static async Task<IClusterClient> ConnectClient()
        {
            IClusterClient client;
            client = new ClientBuilder()
                .UseLocalhostClustering()
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = "dev";
                    options.ServiceId = "OrleansBasics";
                })
                .ConfigureLogging(logging => logging.AddConsole())
                .Build();

            await client.Connect();
            Console.WriteLine("Client successfully connected to silo host \n");
            return client;
        }
        private static async Task DoClientWork(IClusterClient client)
        {
            var friend = client.GetGrain<IHello>(0);
            var response = await friend.SayHello("Good morning,HelloGrain!");
            Console.WriteLine("\n\n{0}\n\n", response);
        }

        private static async Task DoClientWork_Student(IClusterClient client)
        {
            //从客户端调用Grain的示例
            var student = client.GetGrain<IStudent>(321);
            var response = await student.SayHello();
            Console.WriteLine("\n\n{0}\n\n", response);
        }
    }
}
