using System;
using System.Collections.Generic;

#nullable disable

namespace Entities
{
    public partial class SalesInvoice
    {
        public SalesInvoice()
        {
            InventoryMovements = new HashSet<InventoryMovement>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public double? Tax { get; set; }
        public DateTime FakeDate { get; set; }
        public string PaymentMethod { get; set; }
        public double Discount { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public long? VendorId { get; set; }
        public bool IsPrime { get; set; }
        public long? MemberId { get; set; }
        public string Type { get; set; }
        public double DeliveryPrice { get; set; }
        public string Region { get; set; }
        public string PhoneNumber { get; set; }
        public virtual Member Member { get; set; }
        public virtual Vendor Vendor { get; set; }
        public virtual ICollection<InventoryMovement> InventoryMovements { get; set; }
 

    }
}
