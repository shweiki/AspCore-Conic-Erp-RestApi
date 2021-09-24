using System;
using System.Collections.Generic;

#nullable disable

namespace Entities
{
    public partial class SalaryAdjustmentLog
    {
       

        public long Id { get; set; }
        public double AdjustmentAmmount { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public long AdjustmentId { get; set; }
        public long? SalaryPaymentId { get; set; }

        public virtual SalaryPayment SalaryPayment { get; set; }
        public virtual Adjustment Adjustment { get; set; }
    }
}
