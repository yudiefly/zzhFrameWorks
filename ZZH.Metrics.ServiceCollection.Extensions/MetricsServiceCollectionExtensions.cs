using App.Metrics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ZZH.Metrics.ServiceCollection.Extensions
{
 
    public static class MetricsServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the metrics.
        /// </summary>
        /// <returns>The metrics.</returns>
        /// <param name="services">Services.</param>
        /// <param name="builder">Options.</param>
        public static IServiceCollection AddZzhMetrics(this IServiceCollection services, System.Action<IMetricsBuilder> builder)
        {
            services.AddMetrics(builder);
            services.AddMetricsReportingHostedService();
            services.AddMetricsTrackingMiddleware();
            return services;
        }
        /// <summary>
        /// Adds the metrics to IServiceCollection.
        /// </summary>
        /// <returns>The metrics.</returns>
        /// <param name="services">Services.</param>
        /// <param name="metricsConfiguration">Metrics configuration.</param>
        public static IServiceCollection AddZzhMetrics(this IServiceCollection services, MetricsConfiguration metricsConfiguration)
        {
            return AddZzhMetrics(services, builder => {
                builder.Configuration.Configure(config =>
                {
                    config.AddAppTag(metricsConfiguration.AppTag);
                    config.AddEnvTag(metricsConfiguration.EnvTag);
                    config.AddServerTag(metricsConfiguration.ServerTag);
                    config.ReportingEnabled = metricsConfiguration.ReportingEnabled;
                });
                if (metricsConfiguration.IsReportToConsole)
                {
                    builder.Report.ToConsole(TimeSpan.FromSeconds(metricsConfiguration.ReportToConsoleTimeInterval));
                }
                if (metricsConfiguration.InfluxdbConfiguration != null)
                {
                    if (!String.IsNullOrEmpty(metricsConfiguration.InfluxdbConfiguration.BaseUri) &&
                       !String.IsNullOrEmpty(metricsConfiguration.InfluxdbConfiguration.Database))
                    {
                        builder.Report.ToInfluxDb(options => {
                            options.InfluxDb.BaseUri = new Uri(metricsConfiguration.InfluxdbConfiguration.BaseUri);
                            options.InfluxDb.Database = metricsConfiguration.InfluxdbConfiguration.Database;
                            options.InfluxDb.CreateDataBaseIfNotExists = metricsConfiguration.InfluxdbConfiguration.CreateDataBaseIfNotExists;
                        });
                    }
                }


            });
        }
        /// <summary>
        /// Adds the metrics.
        /// </summary>
        /// <returns>The metrics.</returns>
        /// <param name="services">Services.</param>
        /// <param name="configuration">Configuration.</param>
        public static IServiceCollection AddZzhMetrics(this IServiceCollection services, IConfiguration configuration)
        {

            return AddZzhMetrics(services, configuration.GetSection(nameof(MetricsConfiguration)).Get<MetricsConfiguration>());
        }
    }
}
