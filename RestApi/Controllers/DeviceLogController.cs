using Application.Common.Interfaces;
using Application.Features.Members.Queries.GetMemberById;
using Domain.Entities;
using Infrastructure.Persistence;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestApi.Helper;

namespace RestApi.Controllers;

[Authorize]
public class DeviceLogController : ControllerBase
{
    private readonly IApplicationDbContext DB;
    private readonly ISender _mediator;

    public DeviceLogController(IApplicationDbContext dbcontext, ISender mediator)
    {
        DB = dbcontext;
        _mediator = mediator;
    }
    [Route("DeviceLog/GetDeviceLog")]
    [HttpGet]
    public IActionResult GetDeviceLog()
    {
        var DeviceLogs = DB.DeviceLog.Select(x => new { x.Id, x.DateTime, x.Description, x.Device.Name }).ToList();

        return Ok(DeviceLogs);
    }
    [Route("DeviceLog/GetById")]
    [HttpGet]
    public IActionResult GetById(long Id)
    {

        return Ok(DB.DeviceLog.Where(ml => ml.Id == Id).SingleOrDefault());
    }
    [Route("DeviceLog/GetlastLogByUserId")]
    [HttpGet]
    public async Task<IActionResult> GetlastLogByUserId(string UserId, string TableName)
    {
        var deviceLog = DB.DeviceLog.Where(ml => ml.Fk == UserId && ml.TableName == TableName).AsQueryable();
        deviceLog = deviceLog.OrderByDescending(x => x.Id);
        var lastLog = await deviceLog.FirstOrDefaultAsync();
        if (lastLog is null)
        {
            return Ok();
        }
        return Ok(lastLog.DateTime);
    }
    [Route("DeviceLog/GetByStatus")]
    [HttpGet]
    public async Task<IActionResult> GetByStatus(int Status, string TableName, int Limit, string Sort, int Page, string Any)
    {
        //  await _mediator.Send(new GetMemberLogFromZktDataBaseJobCommand());

        var deviceLogs = DB.DeviceLog.Where(x => x.Status == Status && x.TableName == TableName && (Any == null || x.Fk.ToString().Contains(Any) || x.DateTime.ToString().Contains(Any))).AsQueryable();

        deviceLogs = (Sort == "+id" ? deviceLogs.OrderBy(s => s.Id) : deviceLogs.OrderByDescending(s => s.Id));

        var itemsQuery = await deviceLogs.Skip((Page - 1) * Limit).Take(Limit).ToListAsync();

        var groupedQuery = itemsQuery.GroupBy(a => new { a.Fk, a.DateTime }).Select(g => g.Last());

        var result = new List<dynamic>();

        foreach (var deviceLog in groupedQuery)
        {
            var query = new GetMemberByIdQuery
            {
                Id = Convert.ToInt32(deviceLog.Fk)
            };
            var User = await _mediator.Send(query);

            result.Add(new
            {
                deviceLog.Id,
                deviceLog.DateTime,
                deviceLog.Description,
                deviceLog.Fk,
                deviceLog.TableName,
                User
            });
        }

        return Ok(result);
    }

    [Route("DeviceLog/Create")]
    [HttpPost]
    public IActionResult Create(DeviceLog collection)
    {
        if (ModelState.IsValid)
        {
            try
            {

                DB.DeviceLog.Add(collection);
                DB.SaveChanges();
                return Ok(true);
            }
            catch
            {
                //Console.WriteLine(collection);
                return Ok(false);
            }
        }
        return Ok(false);
    }
    [Route("DeviceLog/Edit")]
    [HttpPost]
    public IActionResult Edit(DeviceLog collection)
    {
        if (ModelState.IsValid)
        {
            try
            {
                DeviceLog DeviceLog = DB.DeviceLog.Where(x => x.Id == collection.Id).SingleOrDefault();
                DeviceLog.DeviceId = collection.DeviceId;
                DeviceLog.Fk = collection.Fk;
                DeviceLog.TableName = collection.TableName;
                DeviceLog.DateTime = collection.DateTime;
                DeviceLog.Type = collection.Type;
                DeviceLog.Description = collection.Description;
                DeviceLog.Status = collection.Status;
                DB.SaveChanges();
                return Ok(true);

            }
            catch
            {
                //Console.WriteLine(collection);
                return Ok(false);
            }
        }
        return Ok(false);
    }

    [Route("DeviceLog/GetLogByUserId")]
    [HttpGet]
    public async Task<IActionResult> GetLogByUserId(string UserId, string TableName, DateTime? DateFrom, DateTime? DateTo)
    {
        var DeviceLogs = await DB.DeviceLog.Where(x => x.Fk == UserId && x.TableName == TableName && x.DateTime >= DateFrom && x.DateTime <= DateTo).Select(x => new
        {
            x.Status,
            x.Type,
            x.DateTime,
            x.Device.Name,
            x.DeviceId,
            x.Description,
            x.Id,
            x.Fk,
            x.TableName
        }).ToListAsync();

        return Ok(DeviceLogs);
    }
    public static void RegisterLog(string Id, DateTime datetime, string Ip)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseSqlServer(AppConfig.GetDefaultConnection());
        using (var dbcontext = new ApplicationDbContext(optionsBuilder.Options))
        {
            long ID = Convert.ToInt32(Id);
            string TableName = "";
            var member = dbcontext.Member.Where(m => m.Id == ID).SingleOrDefault();
            var Employee = dbcontext.Employee.Where(m => m.Id == ID).SingleOrDefault();
            if (member != null) TableName = "Member";
            if (Employee != null) TableName = "Employee";

            var isLogSaveIt = dbcontext.DeviceLog.Where(l => l.Fk == Id && l.TableName == TableName).ToList();
            isLogSaveIt = dbcontext.DeviceLog.Where(Ld => Ld.DateTime == datetime).ToList();
            var Device = dbcontext.Device.Where(x => x.Ip == Ip).SingleOrDefault();
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
                dbcontext.DeviceLog.Add(Log);
                dbcontext.SaveChanges();


            }
        }

    }

}
