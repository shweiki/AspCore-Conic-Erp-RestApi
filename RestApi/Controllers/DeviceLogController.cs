
using Domain.Entities; using Application.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace RestApi.Controllers;

[Authorize]
public class DeviceLogController : ControllerBase
{
    private readonly IApplicationDbContext DB;
    public IConfiguration _configuration { get; }
    private IMemoryCache _memoryCache;

    public DeviceLogController(IApplicationDbContext dbcontext, IConfiguration configuration, IMemoryCache memoryCache)
    {
        DB = dbcontext;
        _configuration = configuration;
        _memoryCache = memoryCache;
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

        var deviceLog = await DB.DeviceLog.Where(ml => ml.Fk == UserId && ml.TableName == TableName)?.OrderByDescending(x => x.Id)?.FirstOrDefaultAsync();
        if (deviceLog is null)
        {
            return Ok();
        }
        return Ok(deviceLog.DateTime);


    }
    [Route("DeviceLog/GetByStatus")]
    [HttpGet]
    public async Task<IActionResult> GetByStatus(int Status, string TableName, int Limit, string Sort, int Page, string Any)
    {
        // Get Log From ZkBio Data base 

        // GetFromZkBio(TableName); for v5l speed ztk

        DeviceController Device = new(DB);

        foreach (var D in await DB.Device.ToListAsync())
        {
            Device.GetAllLog(D.Id, "Member");
            Device.GetAllLog(D.Id, "Employee", true);
        }

        var DeviceLogs = await DB.DeviceLog.Where(x => x.Status == Status && x.TableName == TableName && (Any == null || x.Fk.ToString().Contains(Any) || x.DateTime.ToString().Contains(Any))).ToListAsync();

        DeviceLogs = (Sort == "+id" ? DeviceLogs.OrderBy(s => s.Id).ToList() : DeviceLogs.OrderByDescending(s => s.Id).ToList());

        DeviceLogs = DeviceLogs.GroupBy(a => new { a.Fk, a.DateTime }).Select(g => g.Last()).ToList();

        var itemsQuery = DeviceLogs.Skip((Page - 1) * Limit).Take(Limit).ToList();

        var result = new List<dynamic>();

        foreach (var deviceLog in itemsQuery)
        {
            var User = await GetFkData(deviceLog.Fk, TableName, DB);
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

    public static async Task<dynamic> GetFkData(string Fktable, string TableName, IApplicationDbContext DB)
    {
        dynamic Object = TableName switch
        {
            "Member" => await DB.Member.Include(x => x.MembershipMovements).Include(x => x.Account.EntryMovements).Where(x => x.Id == Convert.ToInt32(Fktable)).Select(x => new
            {
                x.Id,
                x.Name,
                x.Description,
                x.Status,
                Style = DB.Oprationsy.Where(o => o.Status == x.Status && o.TableName == "Member").Select(o => new
                {
                    o.Color,
                    o.ClassName,
                    o.IconClass
                }).SingleOrDefault(),
                TotalDebit = x.Account.EntryMovements.Select(d => d.Debit).Sum(),
                TotalCredit = x.Account.EntryMovements.Select(c => c.Credit).Sum(),
                ActiveMemberShip = x.MembershipMovements.Where(f => f.Status == 1).Select(ms => new
                {
                    ms.Id,
                    ms.Type,
                    ms.VisitsUsed,
                    ms.Membership.NumberClass
                }).FirstOrDefault()
            }).SingleOrDefaultAsync(),
            "Employee" => await DB.Employee.SingleOrDefaultAsync(x => x.Id == Convert.ToInt32(Fktable)),
            _ => null,
        };
        return Object;
    }

    [Route("DeviceLog/CheckDeviceLog")]
    [HttpGet]
    public IActionResult CheckDeviceLog()
    {

        bool IsWorkingCheckDeviceLog = false;
        if (_memoryCache.TryGetValue(IsWorkingCheckDeviceLog, out IsWorkingCheckDeviceLog))
        {
            if (IsWorkingCheckDeviceLog)
                return Ok("IsWorkingCheckDeviceLog");
        }

        var cacheEntryOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(60))
                  //    .SetSlidingExpiration(TimeSpan.FromMinutes(60))
                  .SetPriority(CacheItemPriority.High);

        _memoryCache.Set(IsWorkingCheckDeviceLog, true, cacheEntryOptions);
        try
        {
            RemoveDuplicate();

            var deviceLogs = DB.DeviceLog.Where(x => x.Status >= 0 && x.TableName == "Member").ToList();

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
                DB.SaveChanges();

            }
            return Ok(true);

        }
        catch (Exception ex)
        {
            return Ok(ex.Message);
        }
        finally
        {
            _memoryCache.Remove(IsWorkingCheckDeviceLog);
        }
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
    [HttpGet]
    [Route("DeviceLog/RemoveDuplicate")]
    public IActionResult RemoveDuplicate()
    {
        try
        {
            var DuplicateRow = DB.DeviceLog.GroupBy(s => new { s.TableName, s.Fk, s.DateTime }).Select(grp => grp.Skip(1)).ToList();
            foreach (var Dup in DuplicateRow)
            {
                if (!Dup.Any())
                    continue;
                DB.DeviceLog.RemoveRange(Dup);
                DB.SaveChanges();
            }
            return Ok(true);
        }
        catch
        {
            //Console.WriteLine(collection);
            return Ok(false);
        }
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

    public async Task<ActionResult> RegisterLog(string Id, DateTime datetime, string Ip)
    {
        long ID = Convert.ToInt32(Id);
        string TableName = "";
        var member = DB.Member.Where(m => m.Id == ID).SingleOrDefault();
        var Employee = DB.Employee.Where(m => m.Id == ID).SingleOrDefault();
        if (member != null) TableName = "Member";
        if (Employee != null) TableName = "Employee";

        var isLogSaveIt = DB.DeviceLog.Where(l => l.Fk == Id && l.TableName == TableName).ToList();
        isLogSaveIt = DB.DeviceLog.Where(Ld => Ld.DateTime == datetime).ToList();
        var Device = DB.Device.Where(x => x.Ip == Ip).SingleOrDefault();
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
            DB.DeviceLog.Add(Log);
            await DB.SaveChangesAsync();

            /*
             MassageController massage = new MassageController();
             string OwnerPhone = DB.CompanyInfo.FirstOrDefault().PhoneNumber1;
             if (OwnerPhone != null && OwnerPhone.Length == 10) OwnerPhone = OwnerPhone.Substring(1, 9);


             if (member.PhoneNumber1.Length == 10) member.PhoneNumber1 = member.PhoneNumber1.Substring(1, 9);

             var Msg = new Massage
             {
                 TableName = "Member",
                 Type = "SMS",
                 Fktable = member.Id,
                 Status = 0,
                 SendDate = datetime,
                 Body = "",
                 PhoneNumber = ""
             };



             double Total = (from D in DB.EntryMovement.Where(l => l.AccountId == member.AccountId).ToList() select D.Credit).Sum() - (from D in DB.EntryMovement.Where(l => l.AccountId == member.AccountId).ToList() select D.Debit).Sum();
             if (Total > 0)// مدين
             {
                 Msg.Body = "المدين " + member.Name + " - " + member.Id + "متواجد في الصالة وعليه ذمة " + Total + " .";
                 Msg.PhoneNumber = OwnerPhone;
                 //  massage.SendSms(OwnerPhone, Msg.Body);

                 DB.Massage.Add(Msg);

             }
             if (member.Status == 1)// مجمد
             {
                 member.MembershipMovement.Where(MS => MS.Status == 2).SingleOrDefault().MembershipMovementOrder.Where(MSO => MSO.Status == 1).SingleOrDefault().Status = -1;
             }
             if (member.Status == -1)// متهي
             {
                 var ActiveMemberShip = DB.MembershipMovement.Where(f => f.MemberId == member.Id && f.Status > 0).FirstOrDefault();
                 if (ActiveMemberShip != null)
                 {
                     Msg.Body = "عزيزي " + member.Name + " يسعدنا ان تكون متواجد دائماَ معنا في High Fit , نود تذكيرك بتجديد إشتراكك للفترة الحالية";
                     Msg.PhoneNumber = member.PhoneNumber1;
                     //  massage.SendSms(member.PhoneNumber1, Msg.Body);
                     DB.Massage.Add(Msg);

                     Msg.Body = "تم تنبيه " + member.Name + " - " + member.Id + " إنتهى اشتراكه في " + ActiveMemberShip.EndDate + ".";
                     //   massage.SendSms(OwnerPhone, Msg.Body);
                     Msg.PhoneNumber = OwnerPhone;
                     DB.Massage.Add(Msg);
                 }



             }
             if (member.Status == 0)// المتجاوز الفترة الصباحية
             {
                 var ActiveMemberShip = DB.MembershipMovement.Where(f => f.MemberId == member.Id && f.Status > 0).FirstOrDefault();
                 if (ActiveMemberShip != null && ActiveMemberShip.Type == "Morning" && datetime.Hour > 15)
                 {

                     Msg.Body = "عزيزي " + member.Name + "نود تذكيرك بأن الوقت النهائي لدخول الصالة للفترة الصباحية الساعة 3:00 عصرا.دمتم بخير";
                     //   massage.SendSms(member.PhoneNumber1, Msg.Body);
                     Msg.PhoneNumber = member.PhoneNumber1;
                     DB.Massage.Add(Msg);
                     Msg.Body = "تم تنبيه " + member.Name + " - " + member.Id + "تعدى الوقت الصباحي , وتم الدخول في الوقت " + datetime + " .";
                     // massage.SendSms(OwnerPhone, Msg.Body);
                     Msg.PhoneNumber = OwnerPhone;
                     DB.Massage.Add(Msg);
                 }

             }
             if (member.Status == -2)// قائمة السوداء
             {

                 Msg.Body = "المرفوض " + member.Name + " - " + member.Id + "متواجد في الصالة .";
                 // massage.SendSms(OwnerPhone, Msg.Body);
                 Msg.PhoneNumber = OwnerPhone;

                 DB.Massage.Add(Msg);
             }
             if (member.Status == 0)// قائمة معلق
             {
                 var HoldMembership =  DB.MembershipMovement.Where(f => f.MemberId == member.Id && f.Status == -2).FirstOrDefault();
                 var ActiveMemberShip = DB.MembershipMovement.Where(f => f.MemberId == member.Id && f.Status > 0).FirstOrDefault();
                 if (HoldMembership != null && ActiveMemberShip == null)
                 {
                     Msg.Body = "المعلق " + member.Name + " - " + member.Id + "متواجد في الصالة .";
                     Msg.PhoneNumber = OwnerPhone;
                     DB.Massage.Add(Msg);
                 }

             }
                            massage.CheckMassages();
*/
            return Ok();
        }
        else { return NotFound(); }
    }
    /*
     public void GetFromZkBio(string TableName)
     {
         try
         {
             SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

             builder.DataSource = "" + Environment.MachineName + "\\SQLEXPRESS";
             builder.UserID = "sa";
             builder.Password = "Taha123456++";
             builder.InitialCatalog = "zkbiotime";
             builder.MultipleActiveResultSets = true;

             using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
             {
                 connection.Open();

                 String sql = "SELECT  L.punch_time, L.emp_code , T.ip_address  ,L.id FROM zkbiotime.dbo.iclock_transaction as L" +
                     " INNER JOIN  [zkbiotime].[dbo].[iclock_terminal] as T ON L.terminal_id= T.id" +
                     " where L.reserved Is null";

                 using (SqlCommand command = new(sql, connection))
                 {
                     using (SqlDataReader reader = command.ExecuteReader())
                     {
                         if (reader.HasRows)
                         {
                             while (reader.Read())
                             {
                                 if (reader[1].ToString() == "0") continue;

                                 DateTime action_time = DateTime.Parse(reader[0].ToString());
                                 action_time = new DateTime(action_time.Year, action_time.Month, action_time.Day, action_time.Hour, action_time.Minute, 0);

                                 string objectId = reader[1].ToString();
                                 string Ip = reader[2].ToString();
                                 _ = RegisterLog(objectId, action_time, Ip);

                                 UpdateFromZkBioReserved(reader[3].ToString());

                             }
                         }
                     }
                 }
             }
         }
         catch (SqlException e)
         {
             Console.WriteLine(e);
         }
     }
     public void UpdateFromZkBioReserved(string id)
     {
         try
         {
             SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

             builder.DataSource = "" + Environment.MachineName + "\\SQLEXPRESS";
             builder.UserID = "sa";
             builder.Password = "Taha123456++";
             builder.InitialCatalog = "zkbiotime";
             builder.MultipleActiveResultSets = true;

             using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
             {
                 connection.Open();

                 String sql = "update [zkbiotime].[dbo].[iclock_transaction] set reserved = 'true' where id = " + id + "";

                 using (SqlCommand command = new(sql, connection))
                 {
                     command.ExecuteNonQuery();

                 }
                 connection.Close();

             }
         }
         catch (SqlException e)
         {
             Console.WriteLine(e);
         }
     }
    */
}
