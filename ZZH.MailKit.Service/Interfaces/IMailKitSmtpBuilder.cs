using MailKit.Net.Smtp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZZH.MailKit.Service
{
    public interface IMailKitSmtpBuilder
    {
        SmtpClient Build();
    }
}
