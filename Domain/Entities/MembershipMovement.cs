﻿#nullable disable

using Domain.Common;

namespace Domain.Entities;

public partial class MembershipMovement : AuditEntity
{
    public long Id { get; set; }
    public double TotalAmmount { get; set; }
    public double? Tax { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Type { get; set; }
    public int VisitsUsed { get; set; }
    public double Discount { get; set; }
    public string Description { get; set; }
    public int Status { get; set; }
    public long MemberId { get; set; }
    public int MembershipId { get; set; }
    public string DiscountDescription { get; set; }

    public virtual Member Member { get; set; }
    public virtual Membership Membership { get; set; }
    public virtual ICollection<MembershipMovementOrder> MembershipMovementOrders { get; set; }
}
