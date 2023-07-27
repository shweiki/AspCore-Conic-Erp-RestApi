using Application.Common.Interfaces;
using Application.Features.Members.Service;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.SystemNotifications.Commands.ScanMemberStatueJob;

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


        var Members = await _context.Member.Include(x => x.MembershipMovements).ToListAsync();

        foreach (var member in Members)
        {
            try
            {


                int OStatus = member.Status;

                var MembershipMovements = member.MembershipMovements.ToList();

                if (MembershipMovements.Count() <= 0)
                {
                    member.Status = -1;
                }
                else
                {
                    foreach (var MS in MembershipMovements.OrderBy(o => o.Id))
                    {
                        var query = new ScanMembershipMovementByIdService
                        {
                            Id = MS.Id
                        };
                        await _mediator.Send(query);

                    }
                }
                if (OStatus == -2) member.Status = -2;

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