using System;
using System.Collections.Generic;

#nullable disable

namespace Entities
{
    public partial class StockMovement
    {
        public long Id { get; set; }
        public long? ItemsId { get; set; }
        public double Qty { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public int InventoryItemId { get; set; }
        public long StocktakingInventoryId { get; set; }

        public virtual InventoryItem InventoryItem { get; set; }
        public virtual Item Items { get; set; }
        public virtual StocktakingInventory StocktakingInventory { get; set; }
    }
}
