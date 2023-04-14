using System;

#nullable disable

namespace Domain;

public partial class OrderRestaurant
{


    public long Id { get; set; }
    public long OrderId { get; set; }
    public string Name { get; set; }
    public string PhoneNumber { get; set; }
    public double TotalPill { get; set; }
    public double TotalPrice { get; set; }
    public int Status { get; set; }
    public string Content { get; set; }
    public string Description { get; set; }
    public DateTime FakeDate { get; set; }
    public string TableNo { get; set; }
    public Nullable<long> VendorId { get; set; }

    public virtual Vendor Vendor { get; set; }

}
