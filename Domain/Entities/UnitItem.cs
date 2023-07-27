using System.Collections.Generic;

#nullable disable

namespace Domain.Entities;

public partial class UnitItem
{

    public int Id { get; set; }
    public string Name { get; set; }
    public int Status { get; set; }
    public string Description { get; set; }
    public bool IsPrime { get; set; }

    public virtual ICollection<ItemMuo> ItemMuos { get; set; }
}
