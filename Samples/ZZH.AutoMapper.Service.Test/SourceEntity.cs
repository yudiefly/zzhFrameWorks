using System;
using System.Collections.Generic;
using System.Text;

namespace ZZH.AutoMapper.Service.Test
{   
    public class SourceEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime InBound { get; set; }

        public bool IsDeleted { get; set; }

        public string Comment { get; set; }
    }
}
