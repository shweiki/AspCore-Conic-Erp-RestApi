using System;
using System.Collections.Generic;

#nullable disable

namespace Domain;

public partial class StocktakingInventory
{


    public long Id { get; set; }
    public DateTime FakeDate { get; set; }
    public int Status { get; set; }
    public string Description { get; set; }

    public virtual ICollection<StockMovement> StockMovements { get; set; }
}
