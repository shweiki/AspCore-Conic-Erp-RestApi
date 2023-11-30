using Application.Features.Member.Queries.GetAllMembers;
using Application.Features.MemberShips.Queries.GetMemberShipList;

namespace Application.Features.MembershipMovement.Queries.GetMembershipMovementList;

public class MembershipMovementDto
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
    public DateTime Created { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? LastModified { get; set; }
    public string? LastModifiedBy { get; set; }
    public bool IsDeleted { get; set; }
    public MemberDto Member { get; set; }
    public MembershipDto Membership { get; set; }

}
