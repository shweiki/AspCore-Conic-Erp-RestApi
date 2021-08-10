using System;
using System.Collections.Generic;

#nullable disable

namespace Entities
{
    public partial class Account
    {
        public Account()
        {
            Banks = new HashSet<Bank>();
            Cashes = new HashSet<Cash>();
            EntryMovements = new HashSet<EntryMovement>();
            Members = new HashSet<Member>();
            Vendors = new HashSet<Vendor>();
        }

        public long Id { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public string Code { get; set; }
        public string Type { get; set; }
        public bool IsPrime { get; set; }
        public string Name { get; set; }
        public long ParentId { get; set; }

        public virtual ICollection<Bank> Banks { get; set; }
        public virtual ICollection<Cash> Cashes { get; set; }
        public virtual ICollection<EntryMovement> EntryMovements { get; set; }
        public virtual ICollection<Member> Members { get; set; }
        public virtual ICollection<Vendor> Vendors { get; set; }
    }
}
