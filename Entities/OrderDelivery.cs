using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable disable

namespace Entities
{
    public partial class OrderDelivery
    {
       

        public long Id { get; set; }
        public long OrderId { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public double TotalPill { get; set; }
        public double TotalPrice { get; set; }
        public int Status { get; set; }
        public string Content { get; set; }
        public string Description { get; set; }
        public DateTime FakeDate { get; set; }
        public string Region { get; set; }
        public double DeliveryPrice { get; set; }
        public Nullable<long> DriverId { get; set; }

        public virtual Driver Driver { get; set; }

    }
}
