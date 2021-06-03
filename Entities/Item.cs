using System;
using System.Collections.Generic;

#nullable disable

namespace Entities
{
    public partial class Item
    {
        public Item()
        {
            InventoryMovements = new HashSet<InventoryMovement>();
            ItemMuos = new HashSet<ItemMuo>();
            Services = new HashSet<Service>();
            StockMovements = new HashSet<StockMovement>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public double? CostPrice { get; set; }
        public double? SellingPrice { get; set; }
        public double? OtherPrice { get; set; }
        public double? LowOrder { get; set; }
        public double? Tax { get; set; }
        public double? Rate { get; set; }
        public string Barcode { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public string Ingredients { get; set; }

        public int Status { get; set; }
        public bool IsPrime { get; set; }

        public virtual ICollection<InventoryMovement> InventoryMovements { get; set; }
        public virtual ICollection<ItemMuo> ItemMuos { get; set; }
        public virtual ICollection<Service> Services { get; set; }
        public virtual ICollection<StockMovement> StockMovements { get; set; }
    }
}
