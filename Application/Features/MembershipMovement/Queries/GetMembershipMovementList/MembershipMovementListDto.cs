namespace Application.Features.MembershipMovement.Queries.GetMembershipMovementList;

public class MembershipMovementListDto
{
    public MembershipMovementListDto()
    {
        Items = new List<MembershipMovementDto>();
    }

    public IList<MembershipMovementDto> Items { get; set; }
    public int TotalCount { get; set; }
    public int TotalCountBeforeFilter { get; set; }

}
