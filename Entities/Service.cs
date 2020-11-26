using System;
using System.Collections.Generic;

#nullable disable

namespace Entities
{
    public partial class Service
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Qty { get; set; }
        public double SellingPrice { get; set; }
        public long ItemId { get; set; }
        public string Type { get; set; }
        public bool IsPrime { get; set; }
        public int Status { get; set; }
        public string Description { get; set; }

        public virtual Item Item { get; set; }
    }
}
