#nullable disable
using Domain.Common.Enum;

namespace Domain.Entities;

public partial class Device
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public string Ip { get; set; }
    public string MAC { get; set; }
    public int Port { get; set; }
    public DeviceStatus Status { get; set; }
    public bool AutoConnect { get; set; }
    public DateTime? LastSetDateTime { get; set; }
    public string Description { get; set; }

    public virtual ICollection<DeviceLog> DeviceLogs { get; set; }
}
