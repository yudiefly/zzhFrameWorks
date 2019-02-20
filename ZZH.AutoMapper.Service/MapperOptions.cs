using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace ZZH.AutoMapper.Service
{
    /// <summary>
    /// 映射器选项
    /// </summary>
    public class MapperOptions
    {
        internal List<Assembly> AssembliesToRegister { get; } = new List<Assembly>();

        /// <summary>
        /// 注册程序集
        /// </summary>
        /// <param name="assemblies">要注册的程序集</param>
        /// <returns></returns>
        public MapperOptions AddRegisterAssemblies(params Assembly[] assemblies)
        {
            if (assemblies != null)
            {
                AssembliesToRegister.AddRange(assemblies);
            }

            return this;
        }

        /// <summary>
        /// 指定一个类型，表示要注册该类型所归属的程序集
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public MapperOptions RegisterFromAssemblyContaining<T>()
        {
            var assembly = IntrospectionExtensions.GetTypeInfo(typeof(T)).Assembly;
            AssembliesToRegister.Add(assembly);

            return this;
        }
    }
}
