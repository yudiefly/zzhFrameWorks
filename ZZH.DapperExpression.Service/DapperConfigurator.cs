using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using ZZH.DapperExpression.Service.Utils;

namespace ZZH.DapperExpression.Service
{
    /// <summary>
    /// Dapper 配置器
    /// </summary>
    public static class DapperConfigurator
    {
        public static void ConfigureSqlServer()
        {
            ConfigureSqlServer(AssemblyUtil.GetAssemblies());
        }

        public static void ConfigureSqlServer(Type mappingTypeInAssembly)
        {
            var assemblies = new List<Assembly> { mappingTypeInAssembly.Assembly };
            ConfigureSqlServer(assemblies);
        }

        public static void ConfigureSqlServer(IList<Assembly> assemblies)
        {
            ConfigureSqlServer(typeof(EntityTypeConfigMapper<>), assemblies);
        }

        public static void ConfigureSqlServer(Type defaultMapper, IList<Assembly> assemblies)
        {
            DapperExtensions.DapperExtensions.Configure(defaultMapper, assemblies, new DapperExtensions.Sql.SqlServerDialect());
        }

        public static void ConfigureMySql()
        {
            ConfigureMySql(AssemblyUtil.GetAssemblies());
        }

        public static void ConfigureMySql(Type mappingTypeInAssembly)
        {
            var assemblies = new List<Assembly> { mappingTypeInAssembly.Assembly };
            ConfigureMySql(assemblies);
        }

        public static void ConfigureMySql(IList<Assembly> assemblies)
        {
            ConfigureMySql(typeof(EntityTypeConfigMapper<>), assemblies);
        }

        public static void ConfigureMySql(Type defaultMapper, IList<Assembly> assemblies)
        {
            DapperExtensions.DapperExtensions.Configure(defaultMapper, assemblies, new DapperExtensions.Sql.MySqlDialect());
        }
    }
}
