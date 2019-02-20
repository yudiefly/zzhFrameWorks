using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZZH.AutoMapper.Service.DependencyInjection
{
    /// <summary>
    /// IServiceCollection依赖注入的扩展方法
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 添加 AutoMapper 服务
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddAutoMapper(this IServiceCollection services)
        {
            new MapperBuilder().RegisterAssemblies().Build();

            return services;
        }

        /// <summary>
        /// 添加 AutoMapper 服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="optionAction">配置项</param>
        /// <returns></returns>
        public static IServiceCollection AddAutoMapper(this IServiceCollection services, Action<MapperOptions> optionAction)
        {
            if (optionAction == null)
            {
                throw new ArgumentNullException(nameof(optionAction));
            }

            new MapperBuilder().Use(optionAction).Build();

            return services;
        }
    }
}
