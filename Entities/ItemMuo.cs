using System;
using System.Collections.Generic;

#nullable disable

namespace Entities
{
    public partial class ItemMuo
    {
        public int Id { get; set; }
        public long? ItemsId { get; set; }
        public string Description { get; set; }
        public int? MenuItemId { get; set; }
        public int? UnitItemId { get; set; }
        public int? OriginItemId { get; set; }

        public virtual Item Items { get; set; }
        public virtual MenuItem MenuItem { get; set; }
        public virtual OriginItem OriginItem { get; set; }
        public virtual UnitItem UnitItem { get; set; }
    }
}
