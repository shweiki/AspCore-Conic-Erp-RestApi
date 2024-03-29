﻿#nullable disable

namespace Domain.Entities;

public partial class MenuItem
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Status { get; set; }
    public string Description { get; set; }
    public bool IsPrime { get; set; }

    public virtual ICollection<ItemMuo> ItemMuos { get; set; }
}
