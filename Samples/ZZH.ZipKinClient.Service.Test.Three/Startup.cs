using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ZZH.ZipKinClient.Service.DependencyInjection;

namespace ZZH.ZipKinClient.Service.Test.Three
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpContextAccessorExtensions();
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticHttpContext();
            app.UseMvc();

            //var lifetime = app.ApplicationServices.GetService<IApplicationLifetime>();            
            //lifetime.ApplicationStarted.Register(() =>
            //{
            //    ZipKinClientHelper.Init("ZZH.ZipKinClient.Service.Test.Three", "http://localhost:9411");
            //    ZipKinClientHelper.RegisterHandle(loggerFactory);
            //});

            //lifetime.ApplicationStopped.Register(() => ZipKinClientHelper.UnRegisterHandle());

            app.AddZipKin(loggerFactory, "ZZH.ZipKinClient.Service.Test.Three", "http://localhost:9411");

        }
    }
}
