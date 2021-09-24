using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    public partial class SalaryPayment
    {
        public SalaryPayment()
        {
            SalaryAdjustmentLogs = new HashSet<SalaryAdjustmentLog>();
        }
        public long Id { get; set; }
            public long EmployeeId { get; set; }
            public double GrossSalary { get; set; }
            public double NetSalary { get; set; }
            public DateTime SalaryFrom { get; set; }
            public DateTime SalaryTo { get; set; }
            public int Status { get; set; }
            public int WorkingHours { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual ICollection<SalaryAdjustmentLog> SalaryAdjustmentLogs { get; set; }

        
    }
}
