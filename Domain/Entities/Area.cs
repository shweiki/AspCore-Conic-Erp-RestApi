#nullable disable

namespace Domain.Entities;

public partial class Area
{
    public long Id { get; set; }
    public string Adress1 { get; set; }
    public string Adress2 { get; set; }
    public string Adress3 { get; set; }
    public double? DelievryPrice { get; set; }
    public int Status { get; set; }
    public virtual List<Vendor> Vendors { get; set; }

}
