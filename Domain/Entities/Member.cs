﻿#nullable disable

using Domain.Common;

namespace Domain.Entities;

public partial class Member : AuditEntity
{

    public long Id { get; set; }
    public string Name { get; set; }
    public DateTime? DateofBirth { get; set; }
    public string Email { get; set; }
    public string PhoneNumber1 { get; set; }
    public string PhoneNumber2 { get; set; }
    public string Description { get; set; }
    public int Status { get; set; }
    public string Type { get; set; }
    public long AccountId { get; set; }
    public string Ssn { get; set; }
    public string Tag { get; set; }
    public string Vaccine { get; set; }

    public virtual TreeAccount Account { get; set; }
    public virtual ICollection<MembershipMovement> MembershipMovements { get; set; }
    public virtual ICollection<Payment> Payments { get; set; }
    public virtual ICollection<Receive> Receives { get; set; }
    public virtual ICollection<SalesInvoice> SalesInvoices { get; set; }
}
