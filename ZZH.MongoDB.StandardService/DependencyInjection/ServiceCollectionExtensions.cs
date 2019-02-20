using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using ZZH.MongoDB.StandardService.MongoDb;

namespace ZZH.MongoDB.StandardService.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        /// <summary>
        /// 注册 MongoDB 服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="optionsAction">配置参数</param>
        /// <returns></returns>
        public static IServiceCollection AddMongoDB(this IServiceCollection services, Action<MongoDbContextOptions> optionsAction)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            if (optionsAction == null)
            {
                throw new ArgumentNullException(nameof(optionsAction));
            }

            var options = new MongoDbContextOptions();
            optionsAction(options);
            services.AddSingleton(new MongoDbContext(options));
            services.AddSingleton<IMongoDatabaseProvider, MongoDatabaseProvider>();

            return services;
        }

        /// <summary>
        /// 注册 MongoDB 服务, 配置数据从配置文件中的 "MongoDB" 节点读取
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddMongoDB(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddSingleton(sp =>
            {
                var configuration = sp.GetRequiredService<IConfiguration>();
                var options = configuration.GetSection("MongoDB").Get<MongoDbContextOptions>();
                return new MongoDbContext(options);
            });
            services.AddSingleton<IMongoDatabaseProvider, MongoDatabaseProvider>();

            return services;
        }
    }
}
