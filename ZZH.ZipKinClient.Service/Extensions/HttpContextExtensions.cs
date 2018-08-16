using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZZH.ZipKinClient.Service
{
    public static class HttpContextExtensions
    {
        /// <summary>
        /// 注册HttpContextAccessor
        /// </summary>
        /// <param name="services"></param>
        public static void AddHttpContextAccessorExtensions(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }
        /// <summary>
        /// 使用静态的HttpContext对象
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseStaticHttpContext(this IApplicationBuilder app)
        {
            var httpContextAccessor = app.ApplicationServices.GetRequiredService<IHttpContextAccessor>();
            ZipkinServiceHttpContext.Configure(httpContextAccessor);
            return app;
        }
    }
  
}
