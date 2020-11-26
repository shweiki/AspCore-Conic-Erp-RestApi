using System;
using System.Collections.Generic;

#nullable disable

namespace Entities
{
    public partial class UnitItem
    {
        public UnitItem()
        {
            ItemMuos = new HashSet<ItemMuo>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int Status { get; set; }
        public string Description { get; set; }
        public bool IsPrime { get; set; }

        public virtual ICollection<ItemMuo> ItemMuos { get; set; }
    }
}
