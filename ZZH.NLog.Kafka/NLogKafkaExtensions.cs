using Confluent.Kafka;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NLog;
using NLog.Web;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZZH.NLog.Kafka
{
    public static class NLogKafkaExtensions
    {
        /// <summary>
        /// 约定参数常量
        /// </summary>
        private const string Topic = "topic";
        /// <summary>
        /// 约定参数常量
        /// </summary>
        private const string ProducerConfig = "producerConfig";
        public static IWebHostBuilder UseNLog(this IWebHostBuilder builder, string configFileName)
        {
            builder.ConfigureServices(ConfigureKafka);
            LogManager.LoadConfiguration(configFileName);
            return builder.UseNLog();
        }

        internal static string KafkaSection { get; set; }
        private class ChangeValue
        {
            public string topic { get; set; }
            public Dictionary<string, string> producerConfig { get; set; }
        }

        private static void ConfigureKafka(IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            var configuration = serviceProvider.GetService<IConfiguration>();
            try
            {
                var sectionConfig = configuration.GetSection(KafkaSection);
                services.Configure<ChangeValue>(sectionConfig);
                KafkaTarget.Topic = sectionConfig.GetValue<string>(Topic);
                var producerConfig = sectionConfig.GetSection(ProducerConfig).Get<Dictionary<string, string>>();
                KafkaTarget.Producer = new ProducerBuilder<Null, string>(producerConfig).Build();
                var optionsMonitor = services.BuildServiceProvider().GetService<IOptionsMonitor<ChangeValue>>();
                optionsMonitor.OnChange(OnChanged);
            }
            catch (Exception ex)
            {
                // ignored
            }
        }
        private static void OnChanged(ChangeValue value, string name)
        {
            KafkaTarget.Topic = value.topic;
            KafkaTarget.Producer?.Flush(TimeSpan.FromSeconds(60));
            KafkaTarget.Producer?.Dispose();
            KafkaTarget.Producer = new ProducerBuilder<Null, string>(value.producerConfig).Build();
        }
    }
}
