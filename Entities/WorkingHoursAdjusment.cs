using System;
using System.Collections.Generic;

#nullable disable

namespace Entities
{
    public partial class WorkingHoursAdjustment
    {
       

        public long Id { get; set; }
        public double AdjustmentAmmount { get; set; }
        public double? Tax { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public long WorkingHoursLogId { get; set; }
        public int AdjustmentId { get; set; }
        public string SalaryPaymentId { get; set; }

        public virtual WorkingHoursLog WorkingHoursLog { get; set; }
        public virtual SalaryPayment SalaryPayment { get; set; }
        public virtual Adjustment Adjustment { get; set; }
    }
}
