using System.Threading.Tasks;
//using Abp.Net.Mail;
using MimeKit;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;
using System.Net.Mail;
using System.Collections.Generic;

namespace ZZH.MailKit.Service
{
    public class MailKitEmailSender : EmailSenderBase
    {
        private readonly IMailKitSmtpBuilder _smtpBuilder;

        /// <summary>
        /// 邮件附件列表
        /// </summary>
        public List<string> Attachments { set; get; }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="smtpEmailSenderConfiguration"></param>
        /// <param name="smtpBuilder"></param>
        public MailKitEmailSender(
            IEmailSenderConfiguration smtpEmailSenderConfiguration,
            IMailKitSmtpBuilder smtpBuilder)
            : base(
                  smtpEmailSenderConfiguration)
        {
            _smtpBuilder = smtpBuilder;
        }
        /// <summary>
        /// 异步发送邮件
        /// </summary>
        /// <param name="from">发件人</param>
        /// <param name="to">收件人（邮箱地址用分号隔开）</param>
        /// <param name="cc">抄送（邮箱地址用分号隔开）</param>
        /// <param name="subject">主题</param>
        /// <param name="body">邮件内容</param>
        /// <param name="isBodyHtml">是否发送html格式邮件</param>
        /// <param name="bcc">密件抄送（邮箱地址用分号隔开）</param>
        /// <returns></returns>
        public override async Task SendAsync(string from, string to, string cc, string subject, string body, bool isBodyHtml = true, string bcc = "")
        {
            using (var client = BuildSmtpClient())
            {
                var message = BuildMimeMessage(from, to, cc, subject, body, isBodyHtml, bcc);                
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }

        }
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="from">发件人</param>
        /// <param name="to">收件人（邮箱地址用分号隔开）</param>
        /// <param name="cc">抄送（邮箱地址用分号隔开）</param>
        /// <param name="subject">主题</param>
        /// <param name="body">邮件内容</param>
        /// <param name="isBodyHtml">是否发送html格式邮件</param>
        /// <param name="bcc">密件抄送（邮箱地址用分号隔开）</param>
        /// <returns></returns>
        public override void Send(string from, string to, string cc, string subject, string body, bool isBodyHtml = true, string bcc = "")
        {
            using (var client = BuildSmtpClient())
            {
                var message = BuildMimeMessage(from, to, cc, subject, body, isBodyHtml, bcc);
                client.Send(message);
                client.Disconnect(true);
            }
        }
        /// <summary>
        /// 异步发送邮件（需要构造邮件对象）
        /// </summary>
        /// <param name="mail">邮件对象</param>
        /// <returns></returns>
        protected override async Task SendEmailAsync(MailMessage mail)
        {
            using (var client = BuildSmtpClient())
            {
                var message = mail.ToMimeMessage();
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }
        /// <summary>
        /// 发送邮件（需要构造邮件对象）
        /// </summary>
        /// <param name="mail">邮件对象</param>
        protected override void SendEmail(MailMessage mail)
        {
            using (var client = BuildSmtpClient())
            {
                var message = mail.ToMimeMessage();
                client.Send(message);
                client.Disconnect(true);
            }
        }
        /// <summary>
        /// 创建Smtp客户端对象
        /// </summary>
        /// <returns></returns>
        protected virtual SmtpClient BuildSmtpClient()
        {
            return _smtpBuilder.Build();
        }
        /// <summary>
        /// 创建MimeMessage对象
        /// </summary>
        /// <param name="from">发件人</param>
        /// <param name="to">收件人（邮箱地址用分号隔开）</param>
        /// <param name="cc">抄送（邮箱地址用分号隔开）</param>
        /// <param name="subject">主题</param>
        /// <param name="body">邮件内容</param>
        /// <param name="isBodyHtml">是否发送html格式邮件</param>
        /// <param name="bcc">密件抄送（邮箱地址用分号隔开）</param>
        /// <param name="Attachments">附件列表</param>
        /// <returns></returns>
        private static MimeMessage BuildMimeMessage(string from, string to, string cc, string subject, string body, bool isBodyHtml = true, string bcc = "")
        {
            var bodyType = isBodyHtml ? "html" : "plain";
            var message = new MimeMessage
            {
                Subject = subject,
                Body = new TextPart(bodyType)
                {
                    Text = body
                }                
            };

            message.From.Add(new MailboxAddress(from));
            message.To.Add(new MailboxAddress(to));
            if (!string.IsNullOrEmpty(cc))
                message.Cc.Add(new MailboxAddress(cc));
            if (!string.IsNullOrEmpty(bcc))
                message.Bcc.Add(new MailboxAddress(bcc));
            

            return message;
        }
    }
}
