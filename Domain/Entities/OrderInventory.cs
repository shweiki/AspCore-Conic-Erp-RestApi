using System;
using System.Collections.Generic;

#nullable disable

namespace Domain.Entities;

public partial class OrderInventory
{
    public long Id { get; set; }
    public DateTime? FakeDate { get; set; }
    public string OrderType { get; set; }
    public int Status { get; set; }
    public string Description { get; set; }

    public virtual ICollection<InventoryMovement> InventoryMovements { get; set; }
}
