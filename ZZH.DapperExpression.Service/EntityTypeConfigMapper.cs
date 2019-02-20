using DapperExtensions.Mapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZZH.DapperExpression.Service
{
    /// <summary>
    /// 实体映射设置对象。
    /// 可自定义设置对应的 Schema、 Table 名、主键 Id、属性对应关系等相关设置，可参考 https://github.com/tmsmith/Dapper-Extensions/wiki/AutoClassMapper。
    /// 主键设置说明：
    ///     若有多个以 Id 结尾的属性，会选取第一个作为默认的主键；
    ///     可自定义设置主键，主键类型：
    ///         NotAKey -- 表示定义该属性不作为主键，可用于排除框架自动将某些 Id 结尾的属性设为主键；
    ///         Identity -- 主键类型为整数，且自增长, 该值自动生成；
    ///         Guid 主键类型为 Guid, 该值由框架自动生成；
    ///         Assigned -- 自定义类型（非整数和 Guid 类型），该值需要手动赋值。
    /// 参考文档 https://github.com/tmsmith/Dapper-Extensions/wiki/KeyTypes
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EntityTypeConfigMapper<T> : AutoClassMapper<T>
        where T : class
    {
    }
}
