using Application.Services.Jobs.RecoveryDataBaseJob;
using Application.Services.Jobs.CheckDeviceLogJob;
using Application.Services.Jobs.FixBase64ToPathWithLoadedJob;
using Application.Services.Jobs.FixPhoneNumberJob;
using Application.Services.Jobs.GetLogFromZktJob;
using Application.Services.Jobs.ScanMemberStatueJob;
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
    public async Task FixBase64ToPathWithLoadedJobCommand()
    {
        await _mediator.Send(new FixBase64ToPathWithLoadedJobCommand());
    }
    public async Task RecoveryDataBaseJobCommand()
    {
        await _mediator.Send(new RecoveryDataBaseJobCommand());
    }
    public async Task GetLogFromZktJobCommand()
    {
        await _mediator.Send(new GetLogFromZktJobCommand());
    }
    public async Task CheckDeviceLogJobCommand()
    {
        await _mediator.Send(new CheckDeviceLogJobCommand());
    }
    public async Task FixPhoneNumberJobCommand()
    {
        await _mediator.Send(new FixPhoneNumberJobCommand());
    }

}