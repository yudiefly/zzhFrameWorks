using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZZH.AutoMapper.Service
{
    /// <summary>
    /// Mapper 映射扩展
    /// </summary>
    public static class MapperExtensions
    {
        /// <summary>
        /// 从源对象映射到目标对象, 若源对象为 null，会返回 null.
        /// </summary>
        /// <typeparam name="TSource">源对象类型</typeparam>
        /// <typeparam name="TDesitination">映射到目标对象的类型</typeparam>
        /// <param name="source">源对象实例</param>
        /// <returns>目标对象</returns>
        public static TDesitination MapTo<TSource, TDesitination>(this TSource source)
            where TDesitination : class, new()
        {
            if (source == null)
                return default(TDesitination);

            return Mapper.Map<TDesitination>(source);
        }

        /// <summary>
        /// 从源对象映射到目标对象。 若源对象为 null，会返回 null.
        /// </summary>
        /// <typeparam name="TDesitination">映射到目标对象的类型</typeparam>
        /// <param name="source">源对象实例</param>
        /// <returns>目标对象</returns>
        public static TDesitination MapTo<TDesitination>(this object source)
            where TDesitination : class, new()
        {
            if (source == null)
                return default(TDesitination);

            return Mapper.Map<TDesitination>(source);
        }

        /// <summary>
        /// 从源对象映射到目标对象。 若源对象为 null，会返回 null.
        /// 源对象必须为集合类型
        /// </summary>
        /// <typeparam name="TDesitination">映射到目标对象的类型</typeparam>
        /// <param name="source">源对象实例</param>
        /// <returns></returns>
        public static List<TDesitination> MapTo<TDesitination>(this IEnumerable<object> source)
            where TDesitination : class, new()
        {
            if (source == null)
                return null;

            return Mapper.Map<List<TDesitination>>(source);
        }
    }
}
