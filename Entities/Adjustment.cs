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
            StaticAdjustments = new HashSet<StaticAdjustment>();
        }

            public long Id { get; set; }
            public string Name { get; set; }
            public double AdjustmentAmount { get; set; }
            public double AdjustmentPercentage { get; set; }
            public bool IsWorkingHourAdjustment { get; set; }
            public bool IsStaticAdjustment { get; set; }
        public virtual ICollection<WorkingHoursAdjustment> WorkingHoursAdjustments { get; set; }
        public virtual ICollection<StaticAdjustment> StaticAdjustments { get; set; }
    }
}
