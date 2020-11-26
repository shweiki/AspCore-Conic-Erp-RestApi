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
    public class MemberController : Controller
    {
        private ConicErpContext DB = new ConicErpContext();
        [Route("Member/GetReceivablesMember")]
        [HttpGet]
        public IActionResult GetReceivablesMember()
        {
            var Members = DB.Members.Where(f => (f.Account.EntryMovements.Select(d => d.Credit).Sum() - f.Account.EntryMovements.Select(d => d.Debit).Sum()) > 0).Select(x => new
            {
                x.Id,
                x.Name,
                x.Ssn,
                x.PhoneNumber1,
                x.PhoneNumber2,
                x.Status,
                x.Type,
                x.AccountId,
                x.Tag,
                TotalDebit = DB.EntryMovements.Where(l => l.AccountId == x.AccountId).Select(d => d.Debit).Sum(),
                TotalCredit =DB.EntryMovements.Where(l => l.AccountId == x.AccountId).Select(c => c.Credit).Sum() 
            }).ToList();

            return Ok(Members);
        }
        [Route("Member/GetPayablesMember")]
        [HttpGet]
        public IActionResult GetPayablesMember()
        {
            var Members = from x in DB.Members.Where(f => (f.Account.EntryMovements.Select(d => d.Credit).Sum() - f.Account.EntryMovements.Select(d => d.Debit).Sum()) < 0).ToList()
                          select new
                          {
                              x.Id,
                              x.Name,
                              x.Ssn,
                              x.PhoneNumber1,
                              x.PhoneNumber2,
                              x.Status,
                              x.Type,
                              x.AccountId,
                              x.Tag,
                              TotalDebit = (from D in DB.EntryMovements.Where(l => l.AccountId == x.AccountId).ToList() select D.Debit).Sum(),
                              TotalCredit = (from C in DB.EntryMovements.Where(l => l.AccountId == x.AccountId).ToList() select C.Credit).Sum(),
                          };

            return Ok(Members);
        }
        [Route("Member/GetMember")]
        [HttpGet]
        public IActionResult GetMember()
        {
            var Members = DB.Members.Select(x => new { x.Id, x.Name, x.Ssn, x.PhoneNumber1 , x.Tag}).ToList();
                      
            return Ok(Members);
        }

        [Route("Member/GetActiveMember")]
        [HttpGet]
        public IActionResult GetActiveMember()
        {
            var membershiplist = DB.ActionLogs.Where(l => l.MembershipMovementId != null && l.PostingDateTime >= DateTime.Today).Select(x => x.MembershipMovementId).ToList();
            var memberships = DB.MembershipMovements.Where(x => membershiplist.Contains(x.Id)).ToList();
            var Members = (from x in memberships.ToList()
                           select new
                           {
                               x.Member.Id,
                               x.Member.Name,
                               x.Member.Ssn,
                               x.Member.PhoneNumber1,
                               x.Member.PhoneNumber2,
                               x.Status,
                               x.Member.Tag,
                               x.Member.AccountId,
                               ActiveMemberShip = new
                               {
                                   x.Id,
                                   x.Membership.Name,
                                   x.VisitsUsed,
                                   x.Type,
                                   x.StartDate,
                                   x.EndDate,
                                   x.TotalAmmount,
                                   x.Description,
                               }
                           }).ToList();

            return Ok(Members);
       
        

    }

    [Route("Member/GetMemberByStatus")]
        [HttpGet]
        public IActionResult GetMemberByStatus(int Status)
        {
            var Members = from x in DB.Members.Where(f => f.Status == Status).ToList()
                          select new
                          {
                              x.Id,
                              x.Name,
                              x.Ssn,
                              x.PhoneNumber1,
                              x.PhoneNumber2,
                              x.Status,
                              x.Type,
                              x.AccountId,
                              x.Tag,
                              TotalDebit = (from D in DB.EntryMovements.Where(l => l.AccountId == x.AccountId).ToList() select D.Debit).Sum(),
                              TotalCredit = (from C in DB.EntryMovements.Where(l => l.AccountId == x.AccountId).ToList() select C.Credit).Sum(),
                              // Avatar = Url.Content("~/Images/Member/" + x.Id + ".jpeg").ToString(),
                          };
            return Ok(Members);
        }
        
        [Route("Member/Create")]
        [HttpPost]
        public IActionResult Create(Member collection)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Account NewAccount = new Account
                    {
                        Type = "Member",
                        Description = collection.Description,
                        Status = 0,
                        Code = ""
                    };
                    DB.Accounts.Add(NewAccount);
                    DB.SaveChanges();
                    collection.Status = 0;
                    collection.AccountId = NewAccount.Id;
                    DB.Members.Add(collection);
                    DB.SaveChanges();
                    Oprationsy Opx = DB.Oprationsys.Where(d => d.Status == collection.Status && d.TableName == "Member").SingleOrDefault();
                    OprationsysController Op = new OprationsysController();
                    if (Op.ChangeStatus(collection.Id, Opx.Id, "<!" + collection.Id + "!>"))
                    {
                        DB.SaveChanges();
                        return Ok(collection.Id);
                    }
                    else return Ok(false);
                }
                catch
                {
                    //Console.WriteLine(collection);
                    return Ok(false);
                }
            }
            return Ok(false);
        }

        [Route("Member/Edit")]
        [HttpPost]
        public IActionResult Edit(Member collection)
        {
            if (ModelState.IsValid)
            {
                Member member = DB.Members.Where(x => x.Id == collection.Id).SingleOrDefault();
                member.Name = collection.Name;
                member.Ssn = collection.Ssn;
                member.Email = collection.Email;
                member.PhoneNumber1 = collection.PhoneNumber1;
                member.PhoneNumber2 = collection.PhoneNumber2;
                member.DateofBirth = collection.DateofBirth;
                member.Description = collection.Description;
                member.Status = collection.Status;
                member.Type = collection.Type;
                member.Tag = collection.Tag;
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
            return Ok(false);
        }
        [Route("Member/GetMemberByID")]
        [HttpGet]
        public IActionResult GetMemberByID(long? ID)
        {
            var Members = (from x in DB.Members.ToList()
                           where x.Id == ID
                           select new
                           {
                               x.Id,
                               x.Name,
                               x.Ssn,
                               x.DateofBirth,
                               x.Email,
                               x.PhoneNumber1,
                               x.PhoneNumber2,
                               x.Description,
                               x.Status,
                               x.Type,
                               x.Tag,
                               HaveFaceOnDevice = x.MemberFaces.Count() > 0 ? true : false, 
                               Avatar = "", //Url.Content("~/Images/Member/" + x.Id + ".jpeg"),
                               x.AccountId,
                               lastLog =x.MemberLogs.LastOrDefault()?.DateTime,
                               ActiveMemberShip = (from MS in DB.MembershipMovements.Where(f => f.MemberId == x.Id && f.Status > 0).ToList()
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
                                                   ).FirstOrDefault(),
                       
                               ServiceInvoices = (from SI in DB.SalesInvoices.Where(f => f.MemberId == x.Id && f.IsPrime == true).ToList()
                                                  let e = new
                                                  {
                                                      SI.Id,
                                                      SI.Name,
                                                      SI.Status,
                                                      SI.Description,
                                                      InventoryMovements = (from m in SI.InventoryMovements.ToList()
                                                                            select new
                                                                            {
                                                                                m.Id,
                                                                                m.Status,
                                                                                m.Items.Name,
                                                                                m.Qty,
                                                                                m.SellingPrice,
                                                                                m.Description
                                                                            })
                                                  }
                                                  select e),
                               TotalDebit = (from D in DB.EntryMovements.Where(l => l.AccountId == x.AccountId).ToList() select D.Debit).Sum(),
                               TotalCredit = (from D in DB.EntryMovements.Where(l => l.AccountId == x.AccountId).ToList() select D.Credit).Sum(),
                               Opration = (from a in DB.Oprationsys.ToList()
                                           where (a.Status == x.Status) && (a.TableName == "Member")
                                           select new
                                           {
                                               a.Id,
                                               a.OprationName,
                                               a.Status,
                                               a.OprationDescription,
                                               a.ArabicOprationDescription,
                                               a.IconClass,
                                               a.ClassName
                                           }).FirstOrDefault(),
                              
                           }).SingleOrDefault();
            return Ok(Members);
        }

  
        [Route("Member/CheckMembers")]
        [HttpGet]
        public IActionResult CheckMembers()
        {
            IList<Member> Members = DB.Members?.ToList();

            foreach (Member M in Members)
            {
                if (M.MembershipMovements.Count() == 0)
                {
                    M.Status = -1;
                }
                var ActiveMemberShip = M.MembershipMovements.Where(m => m.Status == 0).SingleOrDefault();

                if (ActiveMemberShip != null) {

                    var HowManyDaysLeft = (ActiveMemberShip.EndDate - DateTime.Today).TotalDays;
                    if (HowManyDaysLeft == 3  ){
                        Massage msg = new Massage();
                        msg.Body = "عزيزي " + M.Name + " يسعدنا ان تكون متواجد دائماَ معنا في High Fit , نود تذكيرك بان اشتراك الحالي سينتهي بعد 3 ايام وبتاريخ "+ ActiveMemberShip.EndDate.ToString("dd/MM/yyyy") + " وشكرا";
                        msg.Status = 0;
                        msg.TableName ="Member";
                        msg.Fktable =M.Id;
                        msg.PhoneNumber =M.PhoneNumber1;
                        msg.SendDate = DateTime.Today;
                        msg.Type = "رسالة تذكير";
                        DB.Massages.Add(msg);
                        DB.SaveChanges();
                    }
                      
                }
            }

            //    if (M.Status == -2)
            //        EnableToDevice = false;

            //    if (M.Status == -1)
            //            EnableToDevice = false;
            //        if (M.Status == 0)
            //            EnableToDevice = true;
            //        if (M.Status == 1)
            //            EnableToDevice = true;
            //        if (M.Status == 2)
            //            EnableToDevice = true;

            //    if (DeviceIsEnable)
            //        if (DoorZtk.LastSetDateTime < DateTime.Today)
            //        {
            //            device.EnableMemberToDevice(DoorZtk.ID, M.Id, EnableToDevice);
            //        }

            //}

            //    DoorZtk.LastSetDateTime = DateTime.Today;

            /////device.DisconnectDeviceHere((int)DoorZtk.ID);
            DB.SaveChanges();

            return Ok(true);
        }

    }

}
