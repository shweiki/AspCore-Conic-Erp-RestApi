using Application.Common.Enums;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace Application.Features.Members.Queries.GetMemberById;

public class GetMemberByIdQuery : IRequest<object>
{
    public long Id { get; set; }

}
public class GetMemberByIdQueryHandler : IRequestHandler<GetMemberByIdQuery, object?>
{
    private readonly IApplicationDbContext _context;

    public GetMemberByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<object?> Handle(GetMemberByIdQuery request, CancellationToken cancellationToken)
    {
        var member = await _context.Member.Include(x => x.MembershipMovements).Include(x => x.Account.EntryMovements).SingleOrDefaultAsync(x => x.Id == request.Id);

        if (member == null) { return null; }
        return new
        {
            member.Id,
            member.Name,
            member.Description,
            member.Status,
            Style = _context.Oprationsy.Where(o => o.Status == member.Status && o.TableName == "Member").Select(o => new
            {
                o.Color,
                o.ClassName,
                o.IconClass
            }).SingleOrDefault(),
            TotalDebit = member.Account.EntryMovements.Select(d => d.Debit).Sum(),
            TotalCredit = member.Account.EntryMovements.Select(c => c.Credit).Sum(),
            ActiveMemberShip = member.MembershipMovements.Where(f => f.Status == 1).Select(ms => new
            {
                ms.Id,
                ms.Type,
                ms.VisitsUsed,
                ms.Membership.NumberClass
            }).FirstOrDefault()
        };
    }
}
