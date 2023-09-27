using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.MembershipMovementOrder.Queries.GetAllMembershipMovementOrder;


public class GetAllMembershipMovementOrderQuery : IRequest<List<Object>>
{

}
public class GetAllMembershipMovementOrderQueryHandler : IRequestHandler<GetAllMembershipMovementOrderQuery, List<Object>>
{
    private readonly IApplicationDbContext _context;

    public GetAllMembershipMovementOrderQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Object>> Handle(GetAllMembershipMovementOrderQuery request, CancellationToken cancellationToken)
    {
        var items = await _context.MembershipMovementOrder.Include(x => x.MemberShipMovement).Include(x => x.MemberShipMovement.Membership).Include(x => x.MemberShipMovement.Member).ToListAsync();

        if (items == null) { return null; }
        return items.Select(x => new
        {
            x.Id,
            x.Type,
            x.Status,
            x.Description,
            x.EditorName,
            MemberName = x.MemberShipMovement.Member.Name,
            MemberShipMovementName = x.MemberShipMovement.Membership.Name,
            MemberShipMovementId = x.MemberShipMovement.Id,
            StartDate = x.StartDate.ToShortDateString(),
            EndDate = x.EndDate.ToShortDateString(),
            TotalDays = Math.Ceiling((x.EndDate.Date - x.StartDate.Date).TotalDays),
        }).ToList<Object>();
    }
}
