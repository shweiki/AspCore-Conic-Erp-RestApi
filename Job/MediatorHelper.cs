using Application.Services.Jobs.CheckDeviceLogJob;
using Application.Services.Jobs.FixPhoneNumberJob;
using Application.Services.Jobs.GetMemberLogFromZktDataBaseJob;
using Application.Services.Jobs.ScanMemberStatueJob;
using Application.Services.Systems.FixBase64ToPathWithLoadedJob;
using Application.Services.Jobs.BackupJob;
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
    public async Task BackupJobCommand()
    {
        await _mediator.Send(new BackupJobCommand());
    }
    public async Task GetMemberLogFromZktDataBaseJobCommand()
    {
        await _mediator.Send(new GetMemberLogFromZktDataBaseJobCommand());
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