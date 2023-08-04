using Application.Common.Interfaces;
using Application.Features.Members.Queries.GetMemberById;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace RestApi.Controllers;

[Authorize]
public class DeviceLogController : ControllerBase
{
    private readonly IApplicationDbContext DB;
    public readonly IConfiguration _configuration;
    private readonly IMemoryCache _memoryCache;
    private readonly ISender _mediator;

    public DeviceLogController(IApplicationDbContext dbcontext, IConfiguration configuration, IMemoryCache memoryCache, ISender mediator)
    {
        DB = dbcontext;
        _configuration = configuration;
        _memoryCache = memoryCache;
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

}
