using System;
using System.Collections.Generic;

#nullable disable

namespace Entities
{
    public partial class WorkingHoursLog
    {
        public WorkingHoursLog()
        {
            WorkingHoursAdjusments = new HashSet<WorkingHoursAdjustment>();
        }

        public long Id { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public long EmployeeId { get; set; }
        public long DeviceId { get; set; }

        public virtual Device Device { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual ICollection<WorkingHoursAdjustment> WorkingHoursAdjusments { get; set; }
    }
}