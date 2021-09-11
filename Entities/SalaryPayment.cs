using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    public partial class SalaryPayment
    {
       
            public long Id { get; set; }
            public long EmployeeId { get; set; }
            public double GrossSalary { get; set; }
            public double NetSalary { get; set; }
            public DateTime SalaryPeriod { get; set; }
       
    }
}
