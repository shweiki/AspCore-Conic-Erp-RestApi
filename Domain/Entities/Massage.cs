using System;

#nullable disable

namespace Domain.Entities;

public partial class Massage
{
    public long Id { get; set; }
    public string Type { get; set; }
    public string Body { get; set; }
    public DateTime SendDate { get; set; }
    public string PhoneNumber { get; set; }
    public int Status { get; set; }
    public string TableName { get; set; }
    public long Fktable { get; set; }
}
