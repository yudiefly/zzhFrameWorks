using MailKit.Security;
using System;

namespace ZZH.MailKit.Service
{
    public interface IMailKitConfiguration
    {
        SecureSocketOptions? SecureSocketOption { get; set; }
    }
}
