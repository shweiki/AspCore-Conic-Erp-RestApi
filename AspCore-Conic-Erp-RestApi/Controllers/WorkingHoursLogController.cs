using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web;
using Microsoft.AspNetCore.Authorization;
using Entities; 
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AspCore_Conic_Erp_RestApi.Controllers
{
    [Authorize]
    public class WorkingHoursLogController : Controller
    {
        private ConicErpContext DB = new ConicErpContext();

        [Route("EmployeeLog/GetEmployeeLog")]
        [HttpGet]
        public IActionResult GetEmployeeLog()
        {
            var Members = DB.MemberLogs.Select(x => new { x.Id, x.DateTime, x.Description, x.Device.Name , x.Member}).ToList();
                      
            return Ok(Members);
        }
        [Route("EmployeeLog/GetlastLogByEmployeeId")]
        [HttpGet]
        public IActionResult GetlastLogByEmployeeId(long MemberId)
        {

            return Ok(DB.MemberLogs.Where(ml => ml.MemberId == MemberId)?.ToList()?.LastOrDefault()?.DateTime);
        }
        [Route("EmployeeLog/GetEmployeeLogByStatus")]
        [HttpGet]
        public IActionResult GetEmployeeLogByStatus(int Status)
        {
           // DB.MemberLogs.RemoveRange(students);

            var MemberLogs = DB.MemberLogs.Where(x => x.Status == Status && x.DateTime.Date == DateTime.Today).Select(x => new {
                x.Id,
                x.MemberId,
                x.Member.Name,
                x.DateTime,
                x.Description,
                DeviceName = x.Device.Name,
                x.Member.Status,
                Style = DB.Oprationsys.Where(o => o.Status == x.Member.Status && o.TableName == "Member").Select(o=> new { 
                    o.Color,
                o.ClassName,
                o.IconClass
                }).SingleOrDefault(),
                TotalDebit =x.Member.Account.EntryMovements.Select(d => d.Debit).Sum(),
                TotalCredit = x.Member.Account.EntryMovements.Select(c => c.Credit).Sum(),
                ActiveMemberShip = x.Member.MembershipMovements.Where(f => f.MemberId == x.MemberId && f.Status == 1).Select(ms => new { ms.Id, ms.Type }).FirstOrDefault(),
            }).ToList();

            MemberLogs = MemberLogs.GroupBy(a => new { a.MemberId, a.DateTime })
                   .Select(g => g.Last()).ToList();

            return Ok(MemberLogs);
        }
        [Route("EmployeeLog/CheckEmployeeLog")]
        [HttpGet]
        public IActionResult CheckEmployeeLog()
        {
            foreach (MemberLog ML in DB.MemberLogs.Where(x => x.Status >= 0).ToList()) {

                if (DateTime.Today > ML.DateTime) {
                    ML.Status = -1;
                }
            }
            DB.SaveChanges();
            return Ok(true);
        }
        [Route("EmployeeLog/Create")]
        [HttpPost]
        public IActionResult Create(WorkingHoursLog collection)
        {
            if (ModelState.IsValid)
            {
                try
                {
          
                    DB.WorkingHoursLogs.Add(collection);
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
        [Route("EmployeeLog/Logout")]
        [HttpPost]
        public IActionResult Logout(WorkingHoursLog collection)
        {
            var LastStart = DB.WorkingHoursLogs.Where(ml => ml.EmployeeId == collection.EmployeeId)?.ToList()?.LastOrDefault()?.StartDateTime;
            var WorkingLog = DB.WorkingHoursLogs.Where(x => x.StartDateTime == LastStart)?.ToList()?.LastOrDefault()?.EndDateTime;
            
            if (WorkingLog != null) {
                DB.WorkingHoursLogs.Add(collection);
                DB.SaveChanges();
                return Ok(true);
            }

            else
            {
                WorkingHoursLog WorkingHoursLog = DB.WorkingHoursLogs.Where(x => x.EmployeeId == collection.EmployeeId && x.StartDateTime == LastStart).FirstOrDefault();
                WorkingHoursLog.EndDateTime = collection.EndDateTime;
            
            try
            {
                DB.SaveChanges();
                return Ok(true);
            }
            catch
            {
                //Console.WriteLine(collection);
                return Ok(false);
            }
            }
        }
        [HttpGet]
        [Route("EmployeeLog/RemoveDuplicate")]
        public IActionResult RemoveDuplicate()
        {
                try
                {

                    var DuplicateRow = DB.MemberLogs.GroupBy(s => new { s.MemberId, s.DateTime }).Select(grp => grp.Skip(1)).ToList();
                    foreach (var Dup in DuplicateRow)
                    {
                        if (Dup.Count() == 0)
                            continue;
                        List<MemberLog> duplog = Dup.Select(c => new MemberLog { Id = c.Id }).ToList();
                        DB.MemberLogs.RemoveRange(duplog);
                        DB.SaveChanges();
                    }
                    return Ok(true);
                }
                catch
                {
                    //Console.WriteLine(collection);
                    return Ok(false);
                }
            
            return Ok(false);
        }

        [Route("EmployeeLog/GetEmployeeLogById")]
        [HttpGet]
        public IActionResult GetEmployeeLogById(long? Id)
        {
            var WorkingHoursLogs = DB.WorkingHoursLogs.Where(x => x.EmployeeId == Id).Select(x => new {
                x.Status,
                x.StartDateTime,
                x.EndDateTime,
                x.Device.Name,
                x.DeviceId,
                x.Description,
                x.Id,
                x.EmployeeId,
            }).ToList();
            WorkingHoursLogs = WorkingHoursLogs.GroupBy(a => new { a.EmployeeId, a.StartDateTime })
                   .Select(g => g.Last()).ToList();
       
            return Ok(WorkingHoursLogs);
        }
        public Boolean RegisterMemberLog(long? Id , DateTime datetime, string Ip)
        {
            var member = DB.Members.Where(m => m.Id == Id).FirstOrDefault();
            if (member == null) return false;
            var isLogSaveIt = DB.MemberLogs.Where(l => l.MemberId == member.Id).ToList();
            isLogSaveIt = DB.MemberLogs.Where(Ld => Ld.DateTime == datetime).ToList();

            if (isLogSaveIt.Count() <= 0 && member != null)
            {
                var Device =  DB.Devices.Where(x=>x.Ip == Ip).SingleOrDefault();
                var Log = new MemberLog
                {
                    Type = "In",
                    MemberId = member.Id,
                    DateTime = datetime,
                    DeviceId = Device.Id,
                    Status = 0,
                    Description = "Event Log"
                };

                DB.MemberLogs.Add(Log);
                DB.SaveChanges();
                MassageController massage = new MassageController();
                string OwnerPhone = DB.CompanyInfos.FirstOrDefault().PhoneNumber1;
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



                double Total = (from D in DB.EntryMovements.Where(l => l.AccountId == member.AccountId).ToList() select D.Credit).Sum() - (from D in DB.EntryMovements.Where(l => l.AccountId == member.AccountId).ToList() select D.Debit).Sum();
                if (Total > 0)// مدين
                {
                    Msg.Body = "المدين " + member.Name + " - " + member.Id + "متواجد في الصالة وعليه ذمة " + Total + " .";
                    Msg.PhoneNumber = OwnerPhone;
                    //  massage.SendSms(OwnerPhone, Msg.Body);

                    DB.Massages.Add(Msg);

                }
                if (member.Status == 1)// مجمد
                {
                    member.MembershipMovements.Where(MS => MS.Status == 2).SingleOrDefault().MembershipMovementOrders.Where(MSO => MSO.Status == 1).SingleOrDefault().Status = -1;
                }
                if (member.Status == -1)// متهي
                {
                    var ActiveMemberShip = DB.MembershipMovements.Where(f => f.MemberId == member.Id && f.Status > 0).FirstOrDefault();
                    if (ActiveMemberShip != null)
                    {
                        Msg.Body = "عزيزي " + member.Name + " يسعدنا ان تكون متواجد دائماَ معنا في High Fit , نود تذكيرك بتجديد إشتراكك للفترة الحالية";
                        Msg.PhoneNumber = member.PhoneNumber1;
                        //  massage.SendSms(member.PhoneNumber1, Msg.Body);
                        DB.Massages.Add(Msg);

                        Msg.Body = "تم تنبيه " + member.Name + " - " + member.Id + " إنتهى اشتراكه في " + ActiveMemberShip.EndDate + ".";
                        //   massage.SendSms(OwnerPhone, Msg.Body);
                        Msg.PhoneNumber = OwnerPhone;
                        DB.Massages.Add(Msg);
                    }



                }
                if (member.Status == 0)// المتجاوز الفترة الصباحية
                {
                    var ActiveMemberShip = DB.MembershipMovements.Where(f => f.MemberId == member.Id && f.Status > 0).FirstOrDefault();
                    if (ActiveMemberShip != null && ActiveMemberShip.Type == "Morning" && datetime.Hour > 15)
                    {

                        Msg.Body = "عزيزي " + member.Name + "نود تذكيرك بأن الوقت النهائي لدخول الصالة للفترة الصباحية الساعة 3:00 عصرا.دمتم بخير";
                        //   massage.SendSms(member.PhoneNumber1, Msg.Body);
                        Msg.PhoneNumber = member.PhoneNumber1;
                        DB.Massages.Add(Msg);
                        Msg.Body = "تم تنبيه " + member.Name + " - " + member.Id + "تعدى الوقت الصباحي , وتم الدخول في الوقت " + datetime + " .";
                        // massage.SendSms(OwnerPhone, Msg.Body);
                        Msg.PhoneNumber = OwnerPhone;
                        DB.Massages.Add(Msg);
                    }

                }
                if (member.Status == -2)// قائمة السوداء
                {

                    Msg.Body = "المرفوض " + member.Name + " - " + member.Id + "متواجد في الصالة .";
                    // massage.SendSms(OwnerPhone, Msg.Body);
                    Msg.PhoneNumber = OwnerPhone;

                    DB.Massages.Add(Msg);
                }
                if (member.Status == 0)// قائمة معلق
                {
                    var HoldMembership =  DB.MembershipMovements.Where(f => f.MemberId == member.Id && f.Status == -2).FirstOrDefault();
                    var ActiveMemberShip = DB.MembershipMovements.Where(f => f.MemberId == member.Id && f.Status > 0).FirstOrDefault();
                    if (HoldMembership != null && ActiveMemberShip == null)
                    {
                        Msg.Body = "المعلق " + member.Name + " - " + member.Id + "متواجد في الصالة .";
                        Msg.PhoneNumber = OwnerPhone;
                        DB.Massages.Add(Msg);
                    }

                }
                DB.SaveChanges();
                massage.CheckMassages();
                return true;
            }
            else { return false; }
        }

        private long GetDeviceId(int machineNumber)
        {
            throw new NotImplementedException();
        }
    }

}
