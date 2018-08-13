using System;

namespace ZZH.MongoDB.Service
{
    public interface IEntityBase<T>
    {
        T Id { get; }
    }
    public abstract class AggregateBase : IEntityBase<string>
    {
        public string Id { get; set; }

        public AggregateBase()
        {
            Id = DateTime.Now.ToString("yyyyMMddHHmmssffff");
        }
    }

    public abstract class ValueObjectBase
    {
        public Guid Id { get; set; }

        public ValueObjectBase()
        {
            Id = Guid.NewGuid();
        }
    }
}
