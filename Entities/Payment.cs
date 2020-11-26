using System;
using System.Collections.Generic;

#nullable disable

namespace Entities
{
    public partial class Payment
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public DateTime FakeDate { get; set; }
        public string PaymentMethod { get; set; }
        public double TotalAmmount { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public long? VendorId { get; set; }
        public bool IsPrime { get; set; }
        public long? MemberId { get; set; }
        public string Type { get; set; }
        public string EditorName { get; set; }

        public virtual Member Member { get; set; }
        public virtual Vendor Vendor { get; set; }
    }
}
