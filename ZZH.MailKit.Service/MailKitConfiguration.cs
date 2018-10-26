using MailKit.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZZH.MailKit.Service
{
    public class MailKitConfiguration : IMailKitConfiguration
    {
        public SecureSocketOptions? SecureSocketOption { get; set; }
    }
}
