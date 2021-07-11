using System;
using System.Collections.Generic;

#nullable disable

namespace Entities
{
    public partial class InventoryMovement
    {
        public long Id { get; set; }
        public long ItemsId { get; set; }
        public string TypeMove { get; set; }
        public double Qty { get; set; }
        public double SellingPrice { get; set; }
        public double? Tax { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public int InventoryItemId { get; set; }
        public long? SalesInvoiceId { get; set; }
        public long? PurchaseInvoiceId { get; set; }
        public long? OrderInventoryId { get; set; }
        public long? WorkShopId { get; set; }

        // public DateTime MFG { get; set; }
        public DateTime EXP { get; set; }

        public virtual InventoryItem InventoryItem { get; set; }
        public virtual Item Items { get; set; }
        public virtual WorkShop WorkShop { get; set; }

        public virtual OrderInventory OrderInventory { get; set; }
        public virtual PurchaseInvoice PurchaseInvoice { get; set; }
        public virtual SalesInvoice SalesInvoice { get; set; }
    }
}
