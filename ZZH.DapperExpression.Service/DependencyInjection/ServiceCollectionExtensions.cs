using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using ZZH.DapperExpression.Service.Data;
using ZZH.DapperExpression.Service.DbContext;

namespace ZZH.DapperExpression.Service.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        #region SqlServer

        /// <summary>
        /// 注册基于 Dapper 的 SqlServer 服务
        /// </summary>
        /// <typeparam name="TDbContext">DB 上下文对象</typeparam>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddDapperSqlServer<TDbContext>(this IServiceCollection services)
           where TDbContext : ActiveDbContext
        {
            DapperConfigurator.ConfigureSqlServer();
            services.AddScoped<TDbContext>();

            return services;
        }

        /// <summary>
        /// 注册基于 Dapper 的 SqlServer 服务，DB 上下文为 <see cref="SqlServerDbContext"/>
        /// </summary>
        /// <param name="services"></param>
        /// <param name="connectionName">连接字符串名称, 从 appsetting 的 ConnectionStrings 节点中读取</param>
        /// <returns></returns>
        public static IServiceCollection AddDapperDefaultSqlServer(this IServiceCollection services, string connectionName)
        {
            return AddDapperSqlServer<SqlServerDbContext>(services, connectionName);
        }

        /// <summary>
        /// 注册基于 Dapper 的 SqlServer 服务
        /// </summary>
        /// <typeparam name="TDbContext">DB 上下文对象</typeparam>
        /// <param name="services"></param>
        /// <param name="connectionName">连接字符串名称, 从 appsetting 的 ConnectionStrings 节点中读取</param>
        /// <returns></returns>
        public static IServiceCollection AddDapperSqlServer<TDbContext>(this IServiceCollection services, string connectionName)
            where TDbContext : ActiveDbContext, new()
        {
            if (connectionName == null)
            {
                throw new ArgumentNullException(nameof(connectionName));
            }

            DapperConfigurator.ConfigureSqlServer();
            services.AddScoped(sp =>
            {
                var configuration = sp.GetRequiredService<IConfiguration>();
                var connectionString = configuration.GetConnectionString(connectionName);
                return new TDbContext
                {
                    ConnectionString = connectionString
                };
            });

            return services;
        }

        /// <summary>
        /// 注册基于 Dapper 的 SqlServer 服务，DB 上下文为 <see cref="SqlServerDbContext"/>
        /// </summary>
        /// <param name="services"></param>
        /// <param name="actionOptions">DB 配置参数</param>
        /// <returns></returns>
        public static IServiceCollection AddDapperDefaultSqlServer(this IServiceCollection services, Action<IServiceProvider, ConnectionOptions> actionOptions)
        {
            return AddDapperSqlServer<SqlServerDbContext>(services, actionOptions);
        }

        /// <summary>
        /// 注册基于 Dapper 的 SqlServer 服务
        /// </summary>
        /// <typeparam name="TDbContext">DB 上下文对象</typeparam>
        /// <param name="services"></param>
        /// <param name="actionOptions">DB 配置参数</param>
        /// <returns></returns>
        public static IServiceCollection AddDapperSqlServer<TDbContext>(this IServiceCollection services, Action<IServiceProvider, ConnectionOptions> actionOptions)
            where TDbContext : ActiveDbContext, new()
        {
            if (actionOptions == null)
            {
                throw new ArgumentNullException(nameof(actionOptions));
            }

            DapperConfigurator.ConfigureSqlServer();
            services.AddScoped(sp =>
            {
                var options = new ConnectionOptions();
                actionOptions(sp, options);
                return new TDbContext
                {
                    ConnectionString = options.ConnectionString
                };
            });

            return services;
        }

        #endregion

        #region MySql

        /// <summary>
        /// 注册基于 Dapper 的 MySql 服务
        /// </summary>
        /// <typeparam name="TDbContext">DB 上下文对象</typeparam>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddDapperMySql<TDbContext>(this IServiceCollection services)
           where TDbContext : ActiveDbContext
        {
            DapperConfigurator.ConfigureMySql();
            services.AddScoped<TDbContext>();

            return services;
        }

        /// <summary>
        /// 注册基于 Dapper 的 MySql 服务
        /// </summary>
        /// <typeparam name="TDbContext">DB 上下文对象</typeparam>
        /// <param name="services"></param>
        /// <param name="connectionName">连接字符串名称, 从 appsettings 的 ConnectionStrings 节点中读取</param>
        /// <returns></returns>
        public static IServiceCollection AddDapperMySql<TDbContext>(this IServiceCollection services, string connectionName)
           where TDbContext : ActiveDbContext, new()
        {
            if (connectionName == null)
            {
                throw new ArgumentNullException(nameof(connectionName));
            }

            DapperConfigurator.ConfigureMySql();
            services.AddScoped(sp =>
            {
                var configuration = sp.GetRequiredService<IConfiguration>();
                var connectionString = configuration.GetConnectionString(connectionName);
                return new TDbContext
                {
                    ConnectionString = connectionString
                };
            });

            return services;
        }

        /// <summary>
        /// 注册基于 Dapper 的 MySql 服务
        /// </summary>
        /// <typeparam name="TDbContext">DB 上下文对象</typeparam>
        /// <param name="services"></param>
        /// <param name="actionOptions">DB 配置参数</param>
        /// <returns></returns>
        public static IServiceCollection AddDapperMySql<TDbContext>(this IServiceCollection services, Action<IServiceProvider, ConnectionOptions> actionOptions)
            where TDbContext : ActiveDbContext, new()
        {
            if (actionOptions == null)
            {
                throw new ArgumentNullException(nameof(actionOptions));
            }

            DapperConfigurator.ConfigureMySql();
            services.AddScoped(sp =>
            {
                var options = new ConnectionOptions();
                actionOptions(sp, options);
                return new TDbContext
                {
                    ConnectionString = options.ConnectionString
                };
            });

            return services;
        }

        #endregion
    }
}
