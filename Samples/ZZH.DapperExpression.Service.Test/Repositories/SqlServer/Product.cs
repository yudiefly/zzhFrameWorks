using System;
using System.Collections.Generic;
using System.Text;

namespace ZZH.DapperExpression.Service.Test.Repositories.SqlServer
{
    internal class Product
    {
        public Guid ID { get; set; }

        public string NO { get; set; }

        public string Name { get; set; }

        public double Weight { get; set; }

        public DateTime InBound { get; set; }

        public string Remark { get; set; }

        public bool IsDeleted { get; set; }
    }
}
