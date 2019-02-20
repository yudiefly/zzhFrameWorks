using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace ZZH.MongoDB.StandardService.Entities
{
    /// <summary>
    /// 实体，主键为 <see cref="Guid"/>
    /// 注：对于 MongoDB，<see cref="Guid"/> 类型的主键 Id 在插入数据会自动赋值，同时也会在查询时自动绑定。
    /// </summary>
    [Serializable]
    public abstract class MongoDbEntity : MongoDbEntity<Guid>, IMongoDbEntity
    {

    }

    [Serializable]
    public abstract class MongoDbEntity<TPrimaryKey> : IMongoDbEntity<TPrimaryKey>
    {
        /// <summary>
        /// 主键 Id
        /// </summary>
        public virtual TPrimaryKey Id { get; set; }

        /// <summary>
        /// 检查实体是否是临时的, 主键为 null（string）或小于等于默认值（struct）
        /// </summary>
        /// <returns></returns>
        public virtual bool IsTransient()
        {
            if (EqualityComparer<TPrimaryKey>.Default.Equals(Id, default(TPrimaryKey)))
            {
                return true;
            }

            //Workaround for EF Core since it sets int/long to min value when attaching to dbcontext
            if (typeof(TPrimaryKey) == typeof(int))
            {
                return Convert.ToInt32(Id) <= 0;
            }

            if (typeof(TPrimaryKey) == typeof(long))
            {
                return Convert.ToInt64(Id) <= 0;
            }

            return false;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is MongoDbEntity<TPrimaryKey>))
            {
                return false;
            }

            // Same instances must be considered as equal
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            // Transient objects are not considered as equal
            var other = (MongoDbEntity<TPrimaryKey>)obj;
            if (IsTransient() && other.IsTransient())
            {
                return false;
            }

            // Must have a IS-A relation of types or must be same type
            var typeOfThis = GetType();
            var typeOfOther = other.GetType();
            if (!typeOfThis.GetTypeInfo().IsAssignableFrom(typeOfOther) && !typeOfOther.GetTypeInfo().IsAssignableFrom(typeOfThis))
            {
                return false;
            }

            return Id.Equals(other.Id);
        }

        public override int GetHashCode()
        {
            if (Id == null)
            {
                return 0;
            }

            return Id.GetHashCode();
        }

        public static bool operator ==(MongoDbEntity<TPrimaryKey> left, MongoDbEntity<TPrimaryKey> right)
        {
            if (Equals(left, null))
            {
                return Equals(right, null);
            }

            return left.Equals(right);
        }

        public static bool operator !=(MongoDbEntity<TPrimaryKey> left, MongoDbEntity<TPrimaryKey> right)
        {
            return !(left == right);
        }
    }
}
