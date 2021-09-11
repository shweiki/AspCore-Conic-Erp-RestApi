using System;
using System.Collections.Generic;

#nullable disable

namespace Entities
{
    public partial class WorkingHoursAdjustment
    {
        public WorkingHoursAdjustment()
        {
            WorkingHoursLogs = new HashSet<WorkingHoursLog>();
            SalaryPayments = new HashSet<SalaryPayment>();
            Adjustments = new HashSet<Adjustment>();
        }

        public long Id { get; set; }
        public double AdjustmentAmmount { get; set; }
        public double? Tax { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public long WorkingHoursId { get; set; }
        public int AdjustmentId { get; set; }
        public string SalaryPaymentId { get; set; }

        public virtual ICollection<WorkingHoursLog> WorkingHoursLogs { get; set; }
        public virtual ICollection<SalaryPayment> SalaryPayments { get; set; }
        public virtual ICollection<Adjustment> Adjustments { get; set; }
    }
}
