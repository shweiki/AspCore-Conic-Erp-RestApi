﻿using System;
using System.Collections.Generic;

#nullable disable

namespace Entities
{
    public partial class BillOfEntery
    {
        public BillOfEntery()
        {
          //  InventoryMovements = new HashSet<InventoryMovement>();
        }

        public long Id { get; set; }
        public string BonId { get; set; }
        public string ItemsIds { get; set; }
        public DateTime? FakeDate { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public long PurchaseInvoiceId { get; set; }

    }
}
