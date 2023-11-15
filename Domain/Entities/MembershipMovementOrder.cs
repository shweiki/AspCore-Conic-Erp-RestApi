#nullable disable

using Domain.Common;

namespace Domain.Entities;

public partial class MembershipMovementOrder : AuditEntity
{
    public int Id { get; set; }
    public string Type { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int Status { get; set; }
    public string Description { get; set; }
    public long MemberShipMovementId { get; set; }

    public virtual MembershipMovement MemberShipMovement { get; set; }
}
