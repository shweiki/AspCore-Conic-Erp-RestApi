using System;

#nullable disable

namespace Domain.Entities;

public partial class BillOfEntery
{
    public long Id { get; set; }
    public string BonId { get; set; }
    public string ItemsIds { get; set; }
    public DateTime FakeDate { get; set; }
    public string ST9 { get; set; }
    public string Description { get; set; }
    public int Status { get; set; }
    public long PurchaseInvoiceId { get; set; }
}
