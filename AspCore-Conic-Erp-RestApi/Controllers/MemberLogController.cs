using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web;
using Microsoft.AspNetCore.Authorization;
using Entities; 
using Microsoft.AspNetCore.Mvc;

namespace AspCore_Conic_Erp_RestApi.Controllers
{
    [Authorize]
    public class MemberLogController : Controller
    {
        private ConicErpContext DB = new ConicErpContext();

        [Route("MemberLog/GetMemberLog")]
        [HttpGet]
        public IActionResult GetMemberLog()
        {
            var Members = DB.MemberLogs.Select(x => new { x.Id, x.DateTime, x.Description, x.Device.Name , x.Member}).ToList();
                      
            return Ok(Members);
        }
        [Route("MemberLog/GetlastLogByMemberId")]
        [HttpGet]
        public IActionResult GetlastLogByMemberId(long MemberId)
        {

            return Ok(DB.MemberLogs.Where(ml => ml.MemberId == MemberId)?.ToList()?.LastOrDefault()?.DateTime.ToString());
        }
        [Route("MemberLog/GetMemberLogByStatus")]
        [HttpGet]
        public IActionResult GetMemberLogByStatus(int Status)
        {
            var MemberLogs = DB.MemberLogs.Where(x => x.Status == Status).Select(x => new {
                x.Id,
                x.MemberId,
                x.Member.Name,
                DateTime = x.DateTime.ToString("hh:mm tt"),
                x.Description,
                DeviceName = x.Device.Name,
                x.Member.Status,
                TotalDebit = DB.EntryMovements.Where(l => l.AccountId == x.Member.AccountId).Select(d => d.Debit).Sum(),
                TotalCredit = DB.EntryMovements.Where(l => l.AccountId == x.Member.AccountId).Select(c => c.Credit).Sum(),
                ActiveMemberShip = x.Member.MembershipMovements.Where(f => f.MemberId == x.MemberId && f.Status == 1).Select(ms => new { ms.Id, ms.Type }).FirstOrDefault(),
            }).Take(25).ToList();
                          
                             

             MemberLogs = MemberLogs.GroupBy(a => a.MemberId)
                   .Select(g => g.Last()).ToList();

            return Ok(MemberLogs);
        }
        [Route("MemberLog/CheckMemberLog")]
        [HttpGet]
        public IActionResult CheckMemberLog()
        {
            foreach (MemberLog ML in DB.MemberLogs.Where(x => x.Status >= 0).ToList()) {

                if (DateTime.Today > ML.DateTime) {
                    ML.Status = -1;
                }
            }
            DB.SaveChanges();
            return Ok(true);
        }
        [Route("MemberLog/Create")]
        [HttpPost]
        public IActionResult Create(MemberLog collection)
        {
            if (ModelState.IsValid)
            {
                try
                {
          
                    DB.MemberLogs.Add(collection);
                    DB.SaveChanges();
  
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

        [Route("MemberLog/GetMemberLogByID")]
        [HttpGet]
        public IActionResult GetMemberLogByID(long? ID)
        {
            var MemberLogs = DB.MemberLogs.Where(i => i.MemberId == ID).Select(x => new {
                x.Status,
                x.Type,
                x.DateTime,
                x.Device.Name,
                x.DeviceId,
                x.Description,
                x.Id
            }).ToList();
                          
            return Ok(MemberLogs);
        }
        public Boolean RegisterMemberLog(long? ID , DateTime datetime)
        {
            var member = DB.Members.Where(m => m.Id == ID).FirstOrDefault();

            var isLogSaveIt = DB.MemberLogs.Where(l => l.MemberId == member.Id).ToList();
            isLogSaveIt = DB.MemberLogs.Where(Ld => Ld.DateTime == datetime).ToList();

            if (isLogSaveIt.Count() <= 0 && member != null)
            {
                var Log = new MemberLog
                {
                    Type = "In",
                    MemberId = member.Id,
                    DateTime = datetime,
                    DeviceId = 3,
                    Status = 0,
                    Description = "Event Log"
                };

                DB.MemberLogs.Add(Log);
                DB.SaveChanges();
                MassageController massage = new MassageController();
                string OwnerPhone = DB.CompanyInfos.FirstOrDefault().PhoneNumber1;
                if (OwnerPhone.Length == 10) OwnerPhone = OwnerPhone.Substring(1, 9);

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
                    var ActiveMemberShip = (from MS in DB.MembershipMovements.Where(f => f.MemberId == member.Id && f.Status > 0).ToList()
                                            select MS
                      ).FirstOrDefault();
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
                    var ActiveMemberShip = (from MS in DB.MembershipMovements.Where(f => f.MemberId == member.Id && f.Status > 0).ToList()
                                            select new { MS.Type }
                                              ).FirstOrDefault();
                    if (ActiveMemberShip.Type == "Morning" && datetime.Hour > 15)
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
                    var HoldMembership = (from MS in DB.MembershipMovements.Where(f => f.MemberId == member.Id && f.Status == -2).ToList()
                                          select new
                                          {
                                              MS.Id,
                                              MS.Membership.Name,
                                              MS.VisitsUsed,
                                              MS.Type,
                                              MS.StartDate,
                                              MS.EndDate,
                                              MS.Description,
                                          }
                      ).FirstOrDefault();
                    var ActiveMemberShip = (from MS in DB.MembershipMovements.Where(f => f.MemberId == member.Id && f.Status > 0).ToList()
                                            select new
                                            {
                                                MS.Id,
                                                MS.Membership.Name,
                                                MS.VisitsUsed,
                                                MS.Type,
                                                MS.StartDate,
                                                MS.EndDate,
                                                MS.Description,
                                            }
                      ).FirstOrDefault();
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

        

    }

}
