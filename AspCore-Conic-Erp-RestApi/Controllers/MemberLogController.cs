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
using System.Data.SqlClient;

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

            return Ok(DB.MemberLogs.Where(ml => ml.MemberId == MemberId)?.ToList()?.LastOrDefault()?.DateTime);
        }
        [Route("MemberLog/GetMemberLogByStatus")]
        [HttpGet]
        public IActionResult GetMemberLogByStatus(int Status)
        {
            // Get Log From ZkBio Data base 
            GetFromZkBio();
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
        [Route("MemberLog/RemoveDuplicate")]
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
            
        }

        [Route("MemberLog/GetMemberLogById")]
        [HttpGet]
        public IActionResult GetMemberLogById(long? Id)
        {
            var MemberLogs = DB.MemberLogs.Where(x => x.MemberId == Id).Select(x => new {
                x.Status,
                x.Type,
                x.DateTime,
                x.Device.Name,
                x.DeviceId,
                x.Description,
                x.Id,
                x.MemberId
            }).ToList();

            MemberLogs = MemberLogs.GroupBy(a => new { a.MemberId, a.DateTime })
                   .Select(g => g.Last()).ToList();
       
            return Ok(MemberLogs);
        }     
  
        public Boolean RegisterMemberLog(long? Id , DateTime datetime, string Ip)
        {
            var member = DB.Members.Where(m => m.Id == Id).FirstOrDefault();
            if (member == null) return false;
            var isLogSaveIt = DB.MemberLogs.Where(l => l.MemberId == member.Id).ToList();
            isLogSaveIt = DB.MemberLogs.Where(Ld => Ld.DateTime == datetime).ToList();
            var Device = DB.Devices.Where(x => x.Ip == Ip).SingleOrDefault();

            if (isLogSaveIt.Count() <= 0 && member != null && Device !=null)
            {
                var Log = new MemberLog
                {
                    Type = "In",
                    MemberId = member.Id,
                    DateTime = datetime,
                    DeviceId = Device.Id,
                    Status = 0,
                    Description = "Event Log"
                };

               Create(Log);
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
        public bool GetFromZkBio()
        {
            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

                builder.DataSource = "" + Environment.MachineName + "\\SQLEXPRESS";
                builder.UserID = "sa";
                builder.Password = "Taha123456++";
                builder.InitialCatalog = "zkbiotime";

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();

                    DateTime StartToday = DateTime.Today;
                    DateTime EndToday = StartToday.AddHours(23).AddMinutes(59).AddSeconds(59);

                    String sql = "SELECT L.punch_time, L.emp_code , T.ip_address FROM zkbiotime.dbo.iclock_transaction as L" +
                        " INNER JOIN  [zkbiotime].[dbo].[iclock_terminal] as T ON L.terminal_id= T.id" +
                        " where L.punch_time >='"+
                        StartToday.ToString() + "' And  L.punch_time <='"
                        + EndToday.ToString() + "'";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                if (reader[1].ToString() == "0") continue;

                                DateTime action_time = DateTime.Parse(reader[0].ToString());
                                action_time = new DateTime(action_time.Year, action_time.Month, action_time.Day, action_time.Hour, action_time.Minute, 0);

                                long objectId = long.Parse(reader[1].ToString());
                                string Ip = reader[2].ToString();
                                RegisterMemberLog(objectId, action_time, Ip);

                            }
                            return true;

                        }
                    }
                }
            }
            catch (SqlException e)
            {
                return false;
            }
        }
        private long GetDeviceId(int machineNumber)
        {
            throw new NotImplementedException();
        }
    }

}
