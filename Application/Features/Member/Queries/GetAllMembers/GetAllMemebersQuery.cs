using Application.Common.Enums;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Member.Queries.GetAllMembers;


public class GetAllMemebersQuery : IRequest<List<Object>>
{

}
public class GetAllMemebersQueryHandler : IRequestHandler<GetAllMemebersQuery, List<Object>>
{
    private readonly IApplicationDbContext _context;

    public GetAllMemebersQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Object>> Handle(GetAllMemebersQuery request, CancellationToken cancellationToken)
    {
        var items = await _context.Member.Include(x => x.MembershipMovements).Include(x => x.Account.EntryMovements).ToListAsync();

        if (items == null) { return null; }
        return items.Select(x => new
        {
            x.Id,
            x.Name,
            x.Ssn,
            x.PhoneNumber1,
            x.PhoneNumber2,
            Status = MemberStatus.GetName(typeof(int), x.Status)?.ToString(),
            x.Type,
            x.AccountId,
            x.Tag,
            x.Vaccine,
            MembershipsCount = x.MembershipMovements.Count(),
            TotalDebit = x.Account.EntryMovements.Sum(d => d.Debit),
            TotalCredit = x.Account.EntryMovements.Sum(c => c.Credit),
        }).ToList<Object>();
    }
}
