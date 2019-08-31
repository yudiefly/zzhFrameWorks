using System;
using System.Collections.Generic;
using System.Text;

namespace ZZH.NLog.Kafka
{
    /// <summary>
    /// Logstash event.
    /// </summary>
    public class LogstashEvent
    {
        /// <summary>
        /// Gets or sets the appname.
        /// </summary>
        /// <value>The appname.</value>
        public string appname { set; get; }
        /// <summary>
        /// Gets or sets the level.
        /// </summary>
        /// <value>The level.</value>

        public string level { set; get; }
        /// <summary>
        /// Gets or sets the name of the logger.
        /// </summary>
        /// <value>The name of the logger.</value>

        public string logger_name { set; get; }
        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>The message.</value>


        public string message { set; get; }
        /// <summary>
        /// Gets or sets the timestamp.
        /// </summary>
        /// <value>The timestamp.</value>

        public string @timestamp { get; set; }
        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>The version.</value>

        public UInt64 version { set; get; }
        /// <summary>
        /// Gets or sets the hostname.
        /// </summary>
        /// <value>The hostname.</value>

        public string HOSTNAME { set; get; }
        /// <summary>
        /// Gets or sets the name of the thread.
        /// </summary>
        /// <value>The name of the thread.</value>

        public string thread_name { get; set; }
        /// <summary>
        /// Gets or sets the stack trace.
        /// </summary>
        /// <value>The stack trace.</value>

        public string stack_trace { set; get; }
        /// <summary>
        /// Gets or sets the line number.
        /// </summary>
        /// <value>The line number.</value>

        public string line_number { get; set; }
        /// <summary>
        /// Gets or sets the class.
        /// </summary>
        /// <value>The class.</value>

        public string @class { set; get; }
        /// <summary>
        /// Gets or sets the method.
        /// </summary>
        /// <value>The method.</value>

        public string method { set; get; }

    }
}
