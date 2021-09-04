using System;
using System.Collections.Generic;

#nullable disable

namespace Entities
{
    public partial class Visit
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public double? Tax { get; set; }
        public DateTime FakeDate { get; set; }
        public string PaymentMethod { get; set; }
        public double Discount { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public int PersonCount { get; set; }
        public string Type { get; set; }
        public double HourCount { get; set; }
        public double HourPrice { get; set; }
        public string PhoneNumber { get; set; }
    }
}