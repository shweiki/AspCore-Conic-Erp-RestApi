using System;
using System.Collections.Generic;

#nullable disable

namespace Entities
{
    public partial class WorkShop
    {
        public WorkShop()
        {
            InventoryMovements = new HashSet<InventoryMovement>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public double? Tax { get; set; }
        public DateTime? FakeDate { get; set; }
        public string PaymentMethod { get; set; }
        public double TotalAmmount { get; set; }
        public double Discount { get; set; }
        public DateTime DeliveryDate { get; set; }
        public double? LowCost { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public long VendorId { get; set; }

        public virtual Vendor Vendor { get; set; }
        public virtual ICollection<InventoryMovement> InventoryMovements { get; set; }
    }
}
