using System;
using System.Collections.Generic;

#nullable disable

namespace Entities
{
    public partial class Discount
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public bool IsPrime { get; set; }
        public int Status { get; set; }
        public string Description { get; set; }
        public double Value { get; set; }
        public int? ValueOfDays { get; set; }
    }
}
