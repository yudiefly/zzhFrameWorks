using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZZH.Metrics.ServiceCollection.Extensions
{
    /// <summary>
    /// YNios metrics application builder extensions.
    /// </summary>
    public static class MetricsApplicationBuilderExtensions
    {
        /// <summary>
        /// Uses the ynios metrics.
        /// </summary>
        /// <returns>The nio metrics.</returns>
        /// <param name="app">App.</param>
        public static IApplicationBuilder UseZzhMetrics(this IApplicationBuilder app)
        {
            app.UseMetricsAllMiddleware();
            return app;
        }

    }
}
