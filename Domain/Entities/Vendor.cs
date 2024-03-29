﻿#nullable disable

namespace Domain.Entities;

public partial class Vendor
{

    public long Id { get; set; }
    public string Name { get; set; }
    public string Ssn { get; set; }
    public string Region { get; set; }
    public string Email { get; set; }
    public string PhoneNumber1 { get; set; }
    public string PhoneNumber2 { get; set; }
    public string Fax { get; set; }
    public string Description { get; set; }
    public double? CreditLimit { get; set; }
    public int Status { get; set; }
    public bool IsPrime { get; set; }
    public string Type { get; set; }
    public long AccountId { get; set; }
    public long? AreaId { get; set; }
    public string UserId { get; set; }
    public string Pass { get; set; }
    public virtual TreeAccount Account { get; set; }
    public virtual Area Area { get; set; }
    public virtual ICollection<Cheque> Cheques { get; set; }
    public virtual ICollection<Payment> Payments { get; set; }
    public virtual ICollection<PurchaseInvoice> PurchaseInvoices { get; set; }
    public virtual ICollection<WorkShop> WorkShops { get; set; }
    public virtual ICollection<SalesInvoice> SalesInvoices { get; set; }
    public virtual ICollection<OrderDelivery> OrderDeliveries { get; set; }
}
