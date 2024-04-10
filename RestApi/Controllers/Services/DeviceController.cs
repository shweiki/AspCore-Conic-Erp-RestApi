using Application.Common.Interfaces;
using Domain.Common.Enum;
using Domain.Entities;
using ESC_POS_USB_NET.Printer;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO.Ports;
using System.Text;

namespace RestApi.Controllers.Services;

[Authorize]
public class DeviceController : Controller
{
    private readonly ISender _mediator;
    private readonly IApplicationDbContext DB;
    public DeviceController(IApplicationDbContext dbcontext, ISender mediator)
    {
        DB = dbcontext;
        _mediator = mediator;
    }

    [Route("Device/GetById")]
    [HttpGet]
    public IActionResult GetById(long? Id)
    {
        var Device = DB.Device.Where(x => x.Id == Id).Select(x => new { x.Id, x.Name, x.AutoConnect, x.Status, x.Port, x.MAC, x.Ip, x.Description, x.Type }).SingleOrDefault();

        return Ok(Device);
    }

    [Route("Device/Create")]
    [HttpPost]
    public IActionResult Create(Device collection)
    {
        if (ModelState.IsValid)
        {
            try
            {
                DB.Device.Add(collection);
                DB.SaveChanges();
                return Ok(collection.Id);
            }
            catch
            {
                //Console.WriteLine(collection);
                return Ok(false);
            }
        }
        return Ok(false);
    }

    [Route("Device/Edit")]
    [HttpPost]
    public IActionResult Edit(Device collection)
    {
        if (ModelState.IsValid)
        {
            try
            {
                var Device = DB.Device.SingleOrDefault(x => x.Id == collection.Id);
                Device.Name = collection.Name;
                Device.Type = collection.Type;
                Device.Ip = collection.Ip;
                Device.MAC = collection.MAC;
                Device.Port = collection.Port;
                Device.AutoConnect = collection.AutoConnect;
                Device.Status = collection.Status;
                Device.LastSetDateTime = collection.LastSetDateTime;
                Device.Description = collection.Description;
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
    [Route("Device/OpenCashDrawer")]
    [HttpGet]
    public IActionResult OpenCashDrawer(string Com)
    {
        if (Com != null)
        {
            Encoding enc = Encoding.Unicode;
            SerialPort sp = new SerialPort();
            sp.PortName = Com;
            sp.Encoding = enc;
            sp.BaudRate = 38400;
            sp.Parity = Parity.None;
            sp.DataBits = 8;
            sp.StopBits = StopBits.One;
            sp.DtrEnable = true;
            try
            {
                sp.Open();
                sp.Write(char.ConvertFromUtf32(28699) + char.ConvertFromUtf32(9472) + char.ConvertFromUtf32(3365));
                sp.Close();
                return Ok(Com);
            }
            catch
            {
                return Ok("this Com Is Not Find");
            }
        }
        else return Ok(false);
    }
    [HttpGet]
    [Route("Device/DirectlyPrint")]
    public IActionResult DirectlyPrint(string PrinterName, string Ip, string PortName)
    {
        Printer printer = new Printer(PrinterName);

        printer.FullPaperCut();
        printer.PrintDocument();
        return Ok(true);
    }
    [AllowAnonymous]

    [Route("Device/GetDevice")]
    [HttpGet]
    public IActionResult GetDevice()
    {
        var Device = DB.Device.Select(d => new { d.Id, d.Name, d.AutoConnect, d.Status, d.Port, d.Ip, d.Type, d.Description }).ToList();
        return Ok(Device);
    }

    [Authorize]
    [Route("Device/GetUsersForDevice")]
    [HttpGet]

    public async Task<IActionResult> GetUsersForDevice()
    {
        var members = await DB.Member.Where(x => x.Status == (int)MemberStatus.Active).Select(x => new
        {
            Id = x.Id.ToString(),
            x.Name
        }).ToListAsync();
        return Ok(members);
    }
    //[Route("Device/StartEnrollUser")]
    //[HttpGet]
    //public IActionResult StartEnrollUser(long DeviceId, string UserId)
    //{
    //    var device = DB.Device.SingleOrDefault(x => x.Id == DeviceId);
    //    if (device is null) return BadRequest("Device not found");

    //    return Ok(_zktServices.EnrollUser(device.Ip, device.Port, UserId));
    //}
    //[Route("Device/SetUser")]
    //[HttpGet]
    //public IActionResult SetUser(long DeviceId, string UserId, string Name)
    //{
    //    var device = DB.Device.SingleOrDefault(x => x.Id == DeviceId);

    //    if (device is null) return BadRequest("Device not found");

    //    return Ok(_zktServices.PutUser(device.Ip, device.Port, UserId, Name));

    //}

    //public bool CheckDeviceHere(int? Id)
    //{
    //    bool isDeviceConnected = false;

    //    try
    //    {
    //        var Device = DB.Device.Where(x => x.Id == Id).SingleOrDefault();
    //        if (Device == null) return isDeviceConnected;

    //        (isDeviceConnected, string description) = _zktServices.ConnectByIp(Device.Ip, Device.Port);
    //        Device.Status = isDeviceConnected ? 1 : 0;
    //        Device.LastSetDateTime = DateTime.Now;
    //        Device.Description = description;

    //        DB.SaveChanges();
    //    }
    //    catch (Exception ex)
    //    {

    //    }
    //    return isDeviceConnected;

    //}

    //[Route("Device/SetAll")]
    //[HttpGet]
    //public IActionResult SetAll(long DeviceId, string TableName)
    //{
    //    var device = DB.Device.SingleOrDefault(x => x.Id == DeviceId);

    //    if (device is null) return BadRequest("Device not found");

    //    dynamic List = new List<dynamic>();
    //    if (TableName == "Member")
    //    {
    //        DateTime last = DateTime.Today.AddMonths(-3);
    //        List = DB.Member.Where(x => x.MembershipMovements.Count() != 0 && (x.MembershipMovements != null ? x.MembershipMovements.OrderByDescending(x => x.Id).LastOrDefault().EndDate >= last : false))
    //           .Select(s => new { s.Id, s.Name }).ToList();
    //    }
    //    if (TableName == "Employee")
    //    {
    //        List = DB.Employee.Where(x => x.Status == 0)
    //           .Select(s => new { s.Id, s.Name }).ToList();
    //    }
    //    foreach (var O in List)
    //    {
    //        _zktServices.PutUser(device.Ip, device.Port, O.Id, O.Name);
    //    }

    //    return Ok(true);

    //}

    //[HttpGet]
    //[Route("Device/GetAllLog")]
    //public IActionResult GetAllLog(long DeviceId, string TableName, bool WithClear = false)
    //{
    //    var device = DB.Device.SingleOrDefault(x => x.Id == DeviceId);

    //    if (device is null) return BadRequest("Device not found");

    //    IList<ZktLogRecord> zktLogRecord = _zktServices.GetLogData(device.Ip, device.Port);
    //    if (zktLogRecord is not null && zktLogRecord.Count > 0)
    //    {
    //        foreach (var record in zktLogRecord)
    //        {
    //            var member = DB.Member.Where(m => m.Id == record.IndRegID).SingleOrDefault();
    //            var Employee = DB.Employee.Where(m => m.Id == record.IndRegID).SingleOrDefault();
    //            if (member != null) TableName = "Member";
    //            if (Employee != null) TableName = "Employee";
    //            DateTime datetime = DateTime.Parse(record.DateTimeRecord);
    //            datetime = new DateTime(datetime.Year, datetime.Month, datetime.Day, datetime.Hour, datetime.Minute, 0);
    //            var isLogSaveIt = DB.DeviceLog.Where(l => l.Fk == record.IndRegID.ToString() && l.TableName == TableName && l.DateTime == datetime).Count();
    //            if (isLogSaveIt <= 0)
    //            {
    //                DeviceLog Log = new DeviceLog
    //                {
    //                    Type = "In",
    //                    DateTime = DateTime.Parse(record.DateTimeRecord),
    //                    DeviceId = DeviceId,
    //                    Status = 0,
    //                    TableName = TableName,
    //                    Fk = record.IndRegID.ToString(),
    //                    Description = ""
    //                };
    //                if (Log.DateTime < DateTime.Today)
    //                    Log.Status = -1;
    //                DB.DeviceLog.Add(Log);
    //                DB.SaveChanges();
    //            }
    //            else
    //            {
    //                continue;
    //            }
    //        }
    //        DB.SaveChanges();

    //        return Ok(zktLogRecord.ToList());
    //    }
    //    else
    //    {
    //        return Ok("There Don't Have Log ");
    //    }

    //}
    //[Route("Device/ClearUserLog")]
    //[HttpGet]
    //public IActionResult ClearUserLog(long DeviceId)
    //{
    //    var device = DB.Device.SingleOrDefault(x => x.Id == DeviceId);

    //    if (device is null) return BadRequest("Device not found");
    //    return Ok(_zktServices.ClearLog(device.Ip, device.Port));

    //}
    //[Route("Device/ClearAdministrators")]
    //[HttpGet]
    //public IActionResult ClearAdministrators(long DeviceId)
    //{
    //    var device = DB.Device.SingleOrDefault(x => x.Id == DeviceId);

    //    if (device is null) return BadRequest("Device not found");
    //    return Ok(_zktServices.ClearAdministrators(device.Ip, device.Port));
    //}
    //[Route("Device/RestartDevice")]
    //[HttpGet]
    //public IActionResult RestartDevice(long DeviceId)
    //{
    //    var device = DB.Device.SingleOrDefault(x => x.Id == DeviceId);

    //    if (device is null) return BadRequest("Device not found");
    //    return Ok(_zktServices.Restart(device.Ip, device.Port));
    //}
    //[Route("Device/TurnOff")]
    //[HttpGet]
    //public IActionResult TurnOff(long DeviceId)
    //{
    //    var device = DB.Device.SingleOrDefault(x => x.Id == DeviceId);

    //    if (device is null) return BadRequest("Device not found");
    //    return Ok(_zktServices.TurnOff(device.Ip, device.Port));
    //}

}
