using Application.Common.Interfaces;
using MediatR;

namespace Application.Features.DeviceLog.Commands.AddDeviceLog;

public class AddDeviceLogCommand : IRequest<bool>
{
    public string Id { get; set; }
    public DateTime Datetime { get; set; }
    public string Ip { get; set; } = "";
}
public class AddDeviceLogCommandHandler : IRequestHandler<AddDeviceLogCommand, bool>
{

    private readonly IApplicationDbContext _context;

    public AddDeviceLogCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<bool> Handle(AddDeviceLogCommand request, CancellationToken cancellationToken)
    {
        long Id = Convert.ToInt32(request.Id);
        string TableName = "";
        var member = _context.Member.Where(m => m.Id == Id).SingleOrDefault();
        if (member != null) TableName = "Member";

        var Employee = _context.Employee.Where(m => m.Id == Id).SingleOrDefault();

        if (Employee != null) TableName = "Employee";

        var isLogSaveIt = _context.DeviceLog.Where(l => l.Fk == request.Id && l.TableName == TableName).ToList();
        isLogSaveIt = _context.DeviceLog.Where(Ld => Ld.DateTime == request.Datetime).ToList();
        var Device = _context.Device.Where(x => x.Ip == request.Ip).SingleOrDefault();
        if (isLogSaveIt.Count <= 0 && Device != null)
        {
            var Log = new Domain.Entities.DeviceLog
            {
                Type = "In",
                DateTime = request.Datetime,
                DeviceId = Device.Id,
                Status = 0,
                Description = "Event Log",
                TableName = TableName,
                Fk = Id.ToString(),
            };
            _context.DeviceLog.Add(Log);
            await _context.SaveChangesAsync();

            return true;
        }
        else
        {
            return false;
        }
    }
}