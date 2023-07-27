using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class SalaryPayment
{

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
