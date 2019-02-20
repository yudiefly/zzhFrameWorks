using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ZZH.AutoMapper.Service.Utils
{
    internal static class AssemblyUtil
    {
        /// <summary>
        /// 排除的程序集前缀名称
        /// </summary>
        public static string[] ExcludeAssemblyPrefixs = { "Microsoft", "System", "mscorlib", "netstandard", "WindowsBase", "Newtonsoft", "JetBrains", };

        /// <summary>
        /// 获取程序集集合，其中不包含默认指定排除前缀的程序集
        /// </summary>
        /// <param name="excludeAssemblyPrefixs">要排除的程序集前缀</param>
        /// <returns></returns>
        public static List<Assembly> GetAssemblies(params string[] excludeAssemblyPrefixs)
        {
            var prefixs = ExcludeAssemblyPrefixs;
            if (excludeAssemblyPrefixs != null)
            {
                prefixs = ExcludeAssemblyPrefixs.Union(excludeAssemblyPrefixs).ToArray();
            }

            return AppDomain.CurrentDomain.GetAssemblies().Where(a => !prefixs.Any(s => a.FullName.StartsWith(s))).ToList();
        }

        /// <summary>
        /// 获取指定的前缀的程序集集合
        /// </summary>
        /// <param name="includeAssemblyPrefixs">指定的前缀</param>
        /// <returns></returns>
        public static List<Assembly> GetSpecialAssemblies(string[] includeAssemblyPrefixs)
        {
            return AppDomain.CurrentDomain.GetAssemblies().Where(a => includeAssemblyPrefixs.Any(s => a.FullName.StartsWith(s))).ToList();
        }
    }
}
