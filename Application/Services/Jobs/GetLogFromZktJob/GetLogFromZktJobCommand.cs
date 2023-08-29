using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Application.Services.Jobs.GetLogFromZktJob;

public class GetLogFromZktJobCommand : IRequest<string>
{
}

public class GetLogFromZktJobCommandHandler : IRequestHandler<GetLogFromZktJobCommand, string>
{
    private readonly ISender _mediator;
    private readonly IApplicationDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly IZktServices _zktServices;

    public GetLogFromZktJobCommandHandler(ISender mediator, IApplicationDbContext context, IConfiguration configuration, IZktServices zktServices)
    {
        _mediator = mediator;
        _context = context;
        _configuration = configuration;
        _zktServices = zktServices;
    }

    public async Task<string> Handle(GetLogFromZktJobCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var devices = await _context.Device.ToListAsync();
            foreach (var device in devices)
            {
                if (device is null) continue;
                IList<ZktLogRecord> zktLogRecord = _zktServices.GetLogData(device.Ip, device.Port);
                if (zktLogRecord is not null && zktLogRecord.Count > 0)
                {
                    foreach (var record in zktLogRecord)
                    {
                        var TableName = "";
                        var member = _context.Member.Where(m => m.Id == record.IndRegID).SingleOrDefault();
                        var Employee = _context.Employee.Where(m => m.Id == record.IndRegID).SingleOrDefault();
                        if (member != null) TableName = "Member";
                        if (Employee != null) TableName = "Employee";
                        DateTime datetime = DateTime.Parse(record.DateTimeRecord);
                        datetime = new DateTime(datetime.Year, datetime.Month, datetime.Day, datetime.Hour, datetime.Minute, 0);
                        var isLogSaveIt = _context.DeviceLog.Where(l => l.Fk == record.IndRegID.ToString() && l.TableName == TableName && l.DateTime == datetime).Count();
                        if (isLogSaveIt <= 0)
                        {
                            DeviceLog Log = new DeviceLog
                            {
                                Type = "In",
                                DateTime = DateTime.Parse(record.DateTimeRecord),
                                DeviceId = device.Id,
                                Status = 0,
                                TableName = TableName,
                                Fk = record.IndRegID.ToString(),
                                Description = ""
                            };
                            if (Log.DateTime < DateTime.Today)
                                Log.Status = -1;
                            _context.DeviceLog.Add(Log);
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
            }
            _context.SaveChanges();
        }
        catch (SqlException e)
        {
            Console.WriteLine(e);
            return "";

        }
        finally
        {

        }

        return "";
    }

    public bool UpdateFromZkBioReserved(string id)
    {
        try
        {
            var connectionString = _configuration.GetConnectionString("ZkbiotimeConnection");
            if (string.IsNullOrWhiteSpace(connectionString)) { return false; }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                String sql = "update [zkbiotime].[dbo].[iclock_transaction] set reserved = 'true' where id = " + id + "";

                using (SqlCommand command = new(sql, connection))
                {
                    command.ExecuteNonQuery();

                }
                connection.Close();
                return true;
            }
        }
        catch (SqlException e)
        {
            Console.WriteLine(e);
            return false;
        }
    }
    public async Task<bool> RegisterLog(string Id, DateTime datetime, string Ip)
    {
        long ID = Convert.ToInt32(Id);
        string TableName = "";
        var member = _context.Member.Where(m => m.Id == ID).SingleOrDefault();
        var Employee = _context.Employee.Where(m => m.Id == ID).SingleOrDefault();
        if (member != null) TableName = "Member";
        if (Employee != null) TableName = "Employee";

        var isLogSaveIt = _context.DeviceLog.Where(l => l.Fk == Id && l.TableName == TableName).ToList();
        isLogSaveIt = _context.DeviceLog.Where(Ld => Ld.DateTime == datetime).ToList();
        var Device = _context.Device.Where(x => x.Ip == Ip).SingleOrDefault();
        if (isLogSaveIt.Count <= 0 && Device != null)
        {
            var Log = new DeviceLog
            {
                Type = "In",
                DateTime = datetime,
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
