using System;
using System.Collections.Generic;
using System.Text;

namespace ZZH.AutoMapper.Service
{
    /// <summary>
    /// AutoMapper 映射对象。
    /// 建立与目标类型的映射关系
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class MapperAttribute : Attribute
    {
        /// <summary>
        /// 要建立映射关系的来源类型，可将来源对象转换为此注解对象
        /// </summary>
        public Type SourceType { get; }

        /// <summary>
        /// 是否可以反转。设置为 <see cref="true"/> 表示注解类型与目标类型可以互相转换。
        /// </summary>
        public bool CanReverse { get; }

        /// <summary>
        /// 初始化一个新的<see cref="MapperAttribute"/>对象
        /// </summary>
        /// <param name="sourceType">要建立映射关系的目标类型</param>
        /// <param name="canReverse">是否允许将注解的类型转换为来源类型, 默认为 false</param>
        public MapperAttribute(Type sourceType, bool canReverse = false)
        {
            SourceType = sourceType;
            CanReverse = canReverse;
        }
    }
}
