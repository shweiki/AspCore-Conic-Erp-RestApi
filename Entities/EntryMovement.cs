using System;
using System.Collections.Generic;

#nullable disable

namespace Entities
{
    public partial class EntryMovement
    {
        public long Id { get; set; }
        public long AccountId { get; set; }
        public string Description { get; set; }
        public double Debit { get; set; }
        public double Credit { get; set; }
        public long EntryId { get; set; }
        public string TableName { get; set; }
        public long Fktable { get; set; }
        public virtual Account Account { get; set; }
        public virtual EntryAccounting Entry { get; set; }
    }
}
