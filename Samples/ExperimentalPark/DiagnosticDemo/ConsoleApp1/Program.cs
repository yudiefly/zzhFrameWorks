using Microsoft.Diagnostics.NETCore.Client;
using Microsoft.Diagnostics.Tracing;
using Microsoft.Diagnostics.Tracing.Parsers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Any())
            {
                switch (args[0])
                {
                    case "ps":
                        PrintProcessStatus();
                        break;
                    case "runtime":
                        PrintRuntime(int.Parse(args[1]));
                        break;
                    case "dump":
                        Dump(int.Parse(args[1]));
                        break;
                }
            }

        }
        static void PrintProcessStatus()
        {
            var processes = DiagnosticsClient.GetPublishedProcesses().Select(Process.GetProcessById).Where(process => process != null);
            foreach (var process in processes)
            {
                Console.WriteLine($"ProcessId: {process.Id}");
                Console.WriteLine($"ProcessName: {process.ProcessName}");
                Console.WriteLine($"StartTime: {process.StartTime}");
                Console.WriteLine($"Threads: {process.Threads.Count}");
                Console.WriteLine();
                Console.WriteLine();
            }
        }

        static void PrintRuntime(int processId)
        {
            var providers = new List<EventPipeProvider>()
            {
                new EventPipeProvider("Microsoft-Windows-DotNETRuntime",EventLevel.Informational, (long)ClrTraceEventParser.Keywords.GC)
            };
            var client = new DiagnosticsClient(processId);
            using (var session = client.StartEventPipeSession(providers, false))
            {
                var source = new EventPipeEventSource(session.EventStream);
                source.Clr.All += (TraceEvent obj) =>
                {
                    Console.WriteLine(obj.EventName);
                };
                try
                {
                    source.Process();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
        }
        /// <summary>
        /// 生成dump文件
        /// </summary>
        /// <param name="processId"></param>
        static void Dump(int processId)
        {
            var client = new DiagnosticsClient(processId);
            client.WriteDump(DumpType.Normal, @"mydump.dmp", false);
        }
        /// <summary>
        /// 生成Trace文件，可以用VS
        /// </summary>
        /// <param name="processId"></param>
        static void Trace(int processId)
        {
            var cpuProviders = new List<EventPipeProvider>()
            {
                new EventPipeProvider("Microsoft-Windows-DotNETRuntime", EventLevel.Informational, (long)ClrTraceEventParser.Keywords.Default),
                new EventPipeProvider("Microsoft-DotNETCore-SampleProfiler", EventLevel.Informational, (long)ClrTraceEventParser.Keywords.None)
            };
            var client = new DiagnosticsClient(processId);
            using (var traceSession = client.StartEventPipeSession(cpuProviders))
            {
                Task.Run(async () =>
                {
                    using (FileStream fs = new FileStream(@"mytrace.nettrace", FileMode.Create, FileAccess.Write))
                    {
                        await traceSession.EventStream.CopyToAsync(fs);
                    }

                }).Wait(10 * 1000);

                traceSession.Stop();
            }
        }
        /// <summary>
        /// 对应上面的(TraceEvent obj)=> { 代码块};
        /// </summary>
        /// <param name="obj"></param>
        private static void Clr_All(TraceEvent obj)
        {
            Console.WriteLine(obj.EventName);
        }
    }
}
