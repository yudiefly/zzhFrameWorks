using System;
using System.Collections.Generic;
using System.Text;

namespace ZZH.Metrics.ServiceCollection.Extensions
{
    /// <summary>
    /// Nio metrics configuration.
    /// </summary>
    public class MetricsConfiguration
    {
        /// <summary>
        /// Gets or sets the app tag.
        /// </summary>
        /// <value>The app tag.</value>
        public string AppTag { get; set; }
        /// <summary>
        /// Gets or sets the env tag.
        /// </summary>
        /// <value>The env tag.</value>
        public string EnvTag { get; set; }
        /// <summary>
        /// Gets or sets the server tag.
        /// </summary>
        /// <value>The server tag.</value>
        public string ServerTag { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:NIOCoreBase.Metrics.NioMetricsConfiguration"/>
        /// reporting enabled.
        /// </summary>
        /// <value><c>true</c> if reporting enabled; otherwise, <c>false</c>.</value>
        public bool ReportingEnabled { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:NIOCoreBase.Metrics.NioMetricsConfiguration"/> is
        /// report to console.
        /// </summary>
        /// <value><c>true</c> if is report to console; otherwise, <c>false</c>.</value>
        public bool IsReportToConsole { get; set; }
        /// <summary>
        /// Gets or sets the report to console time interval.
        /// </summary>
        /// <value>The report to console time interval.</value>
        public double ReportToConsoleTimeInterval { get; set; }
        /// <summary>
        /// Gets or sets the influxdb configuration.
        /// </summary>
        /// <value>The influxdb configuration.</value>
        public MetricsInfluxdbConfiguration InfluxdbConfiguration { get; set; }
    }
    /// <summary>
    /// Nio metrics influxdb configuration.
    /// </summary>
    public class MetricsInfluxdbConfiguration
    {
        /// <summary>
        /// Gets or sets the base URI.
        /// </summary>
        /// <value>The base URI.</value>
        public string BaseUri { get; set; }
        /// <summary>
        /// Gets or sets the database.
        /// </summary>
        /// <value>The database.</value>
        public string Database { get; set; }
        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>The name of the user.</value>
        public string UserName { get; set; }
        /// <summary>
        /// Gets or sets the pass word.
        /// </summary>
        /// <value>The pass word.</value>
        public string PassWord { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this
        /// <see cref="T:NIOCoreBase.Metrics.NioMetricsInfluxdbConfiguration"/> create data base if not exists.
        /// </summary>
        /// <value><c>true</c> if create data base if not exists; otherwise, <c>false</c>.</value>
        public bool CreateDataBaseIfNotExists { get; set; }
    }
}
