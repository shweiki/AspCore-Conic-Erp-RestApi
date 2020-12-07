using System;
using System.Collections.Generic;

#nullable disable

namespace Entities
{
    public partial class Cash
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Pcip { get; set; }
        public int? Status { get; set; }
        public bool IsPrime { get; set; }
        public string Description { get; set; }
        public string Btcash { get; set; }
        public long? AccountId { get; set; }

        public virtual Account Account { get; set; }
    }
}
