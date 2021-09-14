using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    public partial class SalaryPayment
    {
        public SalaryPayment()
        {
            WorkingHoursAdjustments = new HashSet<WorkingHoursAdjustment>();
            
        }
        public long Id { get; set; }
            public long EmployeeId { get; set; }
            public double GrossSalary { get; set; }
            public double NetSalary { get; set; }
            public DateTime SalaryPeriod { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual ICollection<WorkingHoursAdjustment> WorkingHoursAdjustments { get; set; }
    }
}
