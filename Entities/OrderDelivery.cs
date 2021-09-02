using System;
using System.Collections.Generic;

#nullable disable

namespace Entities
{
    public partial class OrderDelivery
    {
       

        public long Id { get; set; }
        public string Name { get; set; }
        public string DriverName { get; set; }
        public double TotalPill { get; set; }
        public double TotalPrice { get; set; }
        public int Status { get; set; }
        public string Description { get; set; }
        public DateTime FakeDate { get; set; }
        public bool IsPrime { get; set; }
        public string Region { get; set; }
        public double DeliveryPrice { get; set; }
        public long? VendorId { get; set; }
        public long? DriverId { get; set; }
        

        public virtual Vendor Vendor { get; set; }
        public virtual Driver Driver { get; set; }

    }
}
