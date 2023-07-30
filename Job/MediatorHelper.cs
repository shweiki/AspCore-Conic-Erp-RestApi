using Application.Features.SystemNotifications.Commands.ScanMemberStatueJob;
using MediatR;


namespace Jobs;

public class MediatorHelper
{
    private readonly ISender _mediator;

    public MediatorHelper(ISender mediator)
    {
        _mediator = mediator;
    }

    public async Task ScanMemberStatueJobCommand()
    {
        await _mediator.Send(new ScanMemberStatueJobCommand());
    }

}