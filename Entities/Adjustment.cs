using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    public partial class Adjustment
    {
        public Adjustment()
        {
            WorkingHoursAdjustments = new HashSet<WorkingHoursAdjustment>();
        }

            public long Id { get; set; }
            public string Name { get; set; }
            public double AdjustmentAmount { get; set; }
            public double AdjustmentPercentage { get; set; }
        public virtual ICollection<WorkingHoursAdjustment> WorkingHoursAdjustments { get; set; }
    }
}
