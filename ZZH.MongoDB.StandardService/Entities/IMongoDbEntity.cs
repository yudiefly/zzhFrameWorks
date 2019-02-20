using System;
using System.Collections.Generic;
using System.Text;

namespace ZZH.MongoDB.StandardService.Entities
{
    /// <summary>
    /// 实体，主键类型为 <see cref="Guid"/>
    /// </summary>
    public interface IMongoDbEntity : IMongoDbEntity<Guid>
    {

    }

    /// <summary>
    /// 实体
    /// </summary>
    /// <typeparam name="TPrimaryKey">实体主键</typeparam>
    public interface IMongoDbEntity<TPrimaryKey>
    {
        /// <summary>
        /// 实体主键
        /// </summary>
        TPrimaryKey Id { get; set; }

        /// <summary>
        /// 检查是否是临时的实体
        /// </summary>
        /// <returns></returns>
        bool IsTransient();
    }
}
