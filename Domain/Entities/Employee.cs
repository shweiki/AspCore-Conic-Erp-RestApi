﻿#nullable disable

namespace Domain.Entities;

public partial class Employee
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string LatinName { get; set; }
    public DateTime? DateofBirth { get; set; }
    public string Email { get; set; }
    public string PhoneNumber1 { get; set; }
    public string PhoneNumber2 { get; set; }
    public string JobTitle { get; set; }
    public string Description { get; set; }
    public int Status { get; set; }
    public string Type { get; set; }
    public long AccountId { get; set; }
    public string Ssn { get; set; }
    public string Tag { get; set; }
    public string Vaccine { get; set; }
    public string EmployeeUserId { get; set; }
    public virtual TreeAccount Account { get; set; }
    public virtual ICollection<SalaryPayment> SalaryPayments { get; set; }

}
