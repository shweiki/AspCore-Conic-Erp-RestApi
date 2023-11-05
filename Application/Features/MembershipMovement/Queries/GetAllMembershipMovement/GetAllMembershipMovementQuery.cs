using Application.Common.Enums;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.MembershipMovement.Queries.GetAllMembershipMovement;


public class GetAllMembershipMovementQuery : IRequest<List<Object>>
{

}
public class GetAllMembershipMovementQueryHandler : IRequestHandler<GetAllMembershipMovementQuery, List<Object>>
{
    private readonly IApplicationDbContext _context;

    public GetAllMembershipMovementQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Object>> Handle(GetAllMembershipMovementQuery request, CancellationToken cancellationToken)
    {
        var items = await _context.MembershipMovement.Include(x => x.Member).Include(x => x.Membership).ToListAsync();

        if (items == null) { return null; }
        return items.Select(x => new
        {
            x.Id,
            x.Membership.Name,
            MemberName = x.Member.Name,
            x.Membership.MinFreezeLimitDays,
            x.Membership.MaxFreezeLimitDays,
            x.VisitsUsed,
            x.Type,
            x.TotalAmmount,
            x.MemberId,
            x.MembershipId,
            x.DiscountDescription,
            x.Description,
            StartDate = x.StartDate.ToShortDateString(),
            EndDate = x.EndDate.ToShortDateString(),
            x.Discount,
            x.EditorName,
            Status = MembershipMovementStatus.GetName(typeof(MembershipMovementStatus), x.Status)?.ToString(),
            x.Tax,
            TotalDays = Math.Ceiling((x.EndDate.Date - x.StartDate.Date).TotalDays),
           // Remaining = Math.Ceiling((x.EndDate.Date - DateTime.Today).TotalDays),
        }).ToList<Object>();

    }
}
