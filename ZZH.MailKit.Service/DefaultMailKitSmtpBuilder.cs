using MailKit.Net.Smtp;
using MailKit.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZZH.MailKit.Service
{
    public class DefaultMailKitSmtpBuilder : IMailKitSmtpBuilder, ITransientDependency
    {
        private readonly ISmtpEmailSenderConfiguration _smtpEmailSenderConfiguration;
        private readonly IMailKitConfiguration _zzhMailKitConfiguration;

        public DefaultMailKitSmtpBuilder(ISmtpEmailSenderConfiguration smtpEmailSenderConfiguration, IMailKitConfiguration zzhMailKitConfiguration)
        {
            _smtpEmailSenderConfiguration = smtpEmailSenderConfiguration;
            _zzhMailKitConfiguration = zzhMailKitConfiguration;
        }

        public virtual SmtpClient Build()
        {
            var client = new SmtpClient();

            try
            {
                ConfigureClient(client);
                return client;
            }
            catch
            {
                client.Dispose();
                throw;
            }
        }

        protected virtual void ConfigureClient(SmtpClient client)
        {
            client.Connect(
                _smtpEmailSenderConfiguration.Host,
                _smtpEmailSenderConfiguration.Port,
                GetSecureSocketOption()
            );

            if (_smtpEmailSenderConfiguration.UseDefaultCredentials)
            {
                return;
            }

            client.Authenticate(
                _smtpEmailSenderConfiguration.UserName,
                _smtpEmailSenderConfiguration.Password
            );
        }

        protected virtual SecureSocketOptions GetSecureSocketOption()
        {
            if (_zzhMailKitConfiguration.SecureSocketOption.HasValue)
            {
                return _zzhMailKitConfiguration.SecureSocketOption.Value;
            }

            return _smtpEmailSenderConfiguration.EnableSsl
                ? SecureSocketOptions.SslOnConnect
                : SecureSocketOptions.StartTlsWhenAvailable;
        }
    }
}
