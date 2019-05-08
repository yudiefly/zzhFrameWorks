using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZZH.ZipKinClient.Service.DependencyInjection
{
    public static class ZipkinApplicationBuilderExtensions
    {
        public static void AddZipKin(this IApplicationBuilder app, ILoggerFactory loggerFactory, string ServiceName, string ZipKinServer)
        {
            var lifetime = app.ApplicationServices.GetService<IApplicationLifetime>();
            lifetime.ApplicationStarted.Register(() =>
            {
                ZipKinClientHelper.Init(ServiceName, ZipKinServer);
                ZipKinClientHelper.RegisterHandle(loggerFactory);
            });

            lifetime.ApplicationStopped.Register(() => ZipKinClientHelper.UnRegisterHandle());
        }
    }
}
