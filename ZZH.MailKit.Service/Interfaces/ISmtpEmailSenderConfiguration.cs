using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZZH.MailKit.Service
{
    /// <summary>
    /// Defines configurations to used by SmtpClient object.
    /// </summary>
    public interface ISmtpEmailSenderConfiguration : IEmailSenderConfiguration
    {
        /// <summary>
        /// SMTP Host name/IP.
        /// </summary>
        string Host { get; }

        /// <summary>
        /// SMTP Port.
        /// </summary>
        int Port { get; }

        /// <summary>
        /// User name to login to SMTP server.
        /// </summary>
        string UserName { get; }

        /// <summary>
        /// Password to login to SMTP server.
        /// </summary>
        string Password { get; }

        /// <summary>
        /// Domain name to login to SMTP server.
        /// </summary>
        string Domain { get; }

        /// <summary>
        /// Is SSL enabled?
        /// </summary>
        bool EnableSsl { get; }

        /// <summary>
        /// Use default credentials?
        /// </summary>
        bool UseDefaultCredentials { get; }
    }
}
