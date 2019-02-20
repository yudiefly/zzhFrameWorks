using System;
using System.Collections.Generic;
using System.Text;

namespace ZZH.AutoMapper.Service
{
    /// <summary>
    /// 建立注解成员与对应的成员之间的映射关系
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class MapMemberAttribute : Attribute
    {
        /// <summary>
        /// 映射的成员名称
        /// </summary>
        public string Member { get; }

        /// <summary>
        /// 初始化一个新的<see cref="MapMemberAttribute"/>对象
        /// </summary>
        /// <param name="member">要映射的成员名称</param>
        public MapMemberAttribute(string member)
        {
            Member = member;
        }
    }
}
