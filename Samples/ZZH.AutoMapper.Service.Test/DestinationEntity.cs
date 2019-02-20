using System;
using System.Collections.Generic;
using System.Text;

namespace ZZH.AutoMapper.Service.Test
{
   
    [Mapper(typeof(SourceEntity))]
    public class DestinationEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime InBound { get; set; }

        public bool IsDeleted { get; set; }

        [MapMember(nameof(SourceEntity.Comment))]
        public string Remark { get; set; }
    }
}
