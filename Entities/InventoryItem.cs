using System;
using System.Collections.Generic;

#nullable disable

namespace Entities
{
    public partial class InventoryItem
    {
        public InventoryItem()
        {
            InventoryMovements = new HashSet<InventoryMovement>();
            StockMovements = new HashSet<StockMovement>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int Status { get; set; }
        public string Description { get; set; }
        public bool IsPrime { get; set; }

        public virtual ICollection<InventoryMovement> InventoryMovements { get; set; }
        public virtual ICollection<StockMovement> StockMovements { get; set; }
    }
}
