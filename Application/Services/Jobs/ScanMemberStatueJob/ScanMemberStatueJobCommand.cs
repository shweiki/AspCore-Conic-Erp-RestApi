using Application.Common.Enums;
using Application.Common.Interfaces;
using Application.Features.MemberShips.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Jobs.ScanMemberStatueJob;

public class ScanMemberStatueJobCommand : IRequest<string>
{
}

public class ScanMemberStatueJobCommandHandler : IRequestHandler<ScanMemberStatueJobCommand, string>
{
    private readonly ISender _mediator;
    private readonly IApplicationDbContext _context;

    public ScanMemberStatueJobCommandHandler(ISender mediator, IApplicationDbContext context)
    {
        _mediator = mediator;
        _context = context;
    }

    public async Task<string> Handle(ScanMemberStatueJobCommand request, CancellationToken cancellationToken)
    {
        var queryable = _context.Member.AsQueryable();
        var members = await queryable.ToListAsync();

        foreach (var member in members)
        {
            try
            {
                int OStatus = member.Status;

                if (member.MembershipMovements.Count() <= 0)
                {
                    member.Status = (int)MemberStatus.Deactivate;
                }
                else
                {

                    foreach (var membershipMovement in member.MembershipMovements.Where(x => x.Status > 0).OrderBy(o => o.StartDate))
                    {
                        var query = new ScanMembershipMovementByIdService
                        {
                            Id = membershipMovement.Id
                        };
                        member.Status = (int)await _mediator.Send(query);
                    }
                }
                if (OStatus == (int)MemberStatus.BlackList) member.Status = (int)MemberStatus.BlackList;

                await _context.SaveChangesAsync(cancellationToken);

            }
            catch (Exception ex)
            {
                return "";
            }
            finally
            {

            }
        }

        return "";
    }
}