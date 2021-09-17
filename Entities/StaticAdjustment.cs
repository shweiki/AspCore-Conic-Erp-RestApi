﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    public partial class StaticAdjustment
    {
        
        public long Id { get; set; }
        public double AdjustmentAmount { get; set; }
        public double AdjustmentPercentage { get; set; }
        public long AdjustmentId { get; set; }
        public long SalaryPaymentId { get; set; }

        public virtual SalaryPayment SalaryPayment { get; set; }
        public virtual Adjustment Adjustment { get; set; }
    }
}
