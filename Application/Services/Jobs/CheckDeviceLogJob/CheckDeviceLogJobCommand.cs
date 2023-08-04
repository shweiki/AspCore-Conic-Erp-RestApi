using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Jobs.CheckDeviceLogJob;

public class CheckDeviceLogJobCommand : IRequest<string>
{
}

public class CheckDeviceLogJobCommandHandler : IRequestHandler<CheckDeviceLogJobCommand, string>
{
    private readonly ISender _mediator;
    private readonly IApplicationDbContext _context;

    public CheckDeviceLogJobCommandHandler(ISender mediator, IApplicationDbContext context)
    {
        _mediator = mediator;
        _context = context;
    }

    public async Task<string> Handle(CheckDeviceLogJobCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await RemoveDuplicate();

            var deviceLogs = await _context.DeviceLog.Where(x => x.Status >= 0 && x.TableName == "Member").ToListAsync();

            foreach (var ML in deviceLogs)
            {

                if (DateTime.Today > ML.DateTime.Date)
                {
                    ML.Status = -1;
                }
                else
                {
                    ML.Status = 0;
                }
                _context.SaveChanges();

            }
            return "";

        }
        catch (Exception ex)
        {
            return "";
        }
        finally
        {
        }

    }
    public async Task RemoveDuplicate()
    {
        try
        {
            var DuplicateRow = await _context.DeviceLog.GroupBy(s => new { s.TableName, s.Fk, s.DateTime }).Select(grp => grp.Skip(1)).ToListAsync();
            foreach (var Dup in DuplicateRow)
            {
                if (!Dup.Any())
                    continue;
                _context.DeviceLog.RemoveRange(Dup);
                _context.SaveChanges();
            }
        }
        catch
        {

        }
    }

}