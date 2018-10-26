using NSubstitute;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using ZZH.MailKit.Service;

namespace ZZH.Mailkit.Service.Test
{
    public class MailSenderHelper
    {
        private MailKitEmailSender mailSender;
        /// <summary>
        /// 初始化创建一个发送对象
        /// </summary>
        /// <param name="stmpServer">smtp服务器地址</param>
        /// <param name="userName">用户名（通常为邮箱地址）</param>
        /// <param name="pwd">密码</param>
        /// <param name="port">smtp服务器端口(默认为：25)</param>
        public MailSenderHelper(string stmpServer, string userName, string pwd, int port = 25)
        {
            var mailConfig = Substitute.For<ISmtpEmailSenderConfiguration>();

            mailConfig.Host.Returns(stmpServer);
            mailConfig.UserName.Returns(userName);
            mailConfig.Password.Returns(pwd);
            mailConfig.Port.Returns(port);
            mailConfig.EnableSsl.Returns(false);

            var mailSender = new MailKitEmailSender(mailConfig, new DefaultMailKitSmtpBuilder(mailConfig, new MailKitConfiguration()));
        }

        public void ShouldSend()
        {
            mailSender.Send("from_mail_address", "to_mail_address", "subject", "body", true);
        }

        //[Fact]
        public async Task ShouldSendAsync()
        {
            await mailSender.SendAsync("from_mail_address", "to_mail_address", "subject", "body", true);
        }

        public async Task ShouldSendMailMessageAsync()
        {
            var mailMessage = new MailMessage("from_mail_address", "to_mail_address", "subject", "body")
            { IsBodyHtml = true };

            await mailSender.SendAsync(mailMessage);
        }

        public void ShouldSendMailMessage()
        {
            var mailMessage = new MailMessage("from_mail_address", "to_mail_address", "subject", "body")
            { IsBodyHtml = true };
           
            mailSender.Send(mailMessage);
        }

    }
}
