using System;
using System.Collections.Generic;

#nullable disable

namespace Entities
{
    public partial class Membership
    {
        public Membership()
        {
            MembershipMovements = new HashSet<MembershipMovement>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int NumberDays { get; set; }
        public double MorningPrice { get; set; }
        public double FullDayPrice { get; set; }
        public double? Tax { get; set; }
        public double? Rate { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public int? MinFreezeLimitDays { get; set; }
        public int? MaxFreezeLimitDays { get; set; }

        public virtual ICollection<MembershipMovement> MembershipMovements { get; set; }
    }
}
