using System;

#nullable disable

namespace Domain.Entities;

public partial class DeviceLog
{
    public long Id { get; set; }
    public string Type { get; set; }
    public DateTime DateTime { get; set; }
    public string Description { get; set; }
    public int Status { get; set; }
    public long DeviceId { get; set; }
    public string TableName { get; set; }
    public string Fk { get; set; }
    public virtual Device Device { get; set; }
}
