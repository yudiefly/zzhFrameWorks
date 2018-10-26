
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZZH.Metrics.ServiceCollection.Extensions
{
    /// <summary>
    /// The metrics mvc core mvc builder extensions.
    /// </summary>
    public static class MetricsMvcCoreMvcBuilderExtensions
    {
        /// <summary>
        /// Adds the nio mertrics.
        /// </summary>
        /// <returns>The nio mertrics.</returns>
        /// <param name="builder">Builder.</param>
        public static IMvcBuilder AddZzhMertrics(this IMvcBuilder builder)
        {
            builder.AddZzhMertrics();
            return builder;
        }
    }
}
