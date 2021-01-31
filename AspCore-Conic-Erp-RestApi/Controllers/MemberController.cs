using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web;
using Microsoft.AspNetCore.Authorization;
using Entities;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

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
                TotalCredit = DB.EntryMovements.Where(l => l.AccountId == x.AccountId).Select(c => c.Credit).Sum()
            }).ToList();

            return Ok(Members);
        }
        [Route("Member/GetPayablesMember")]
        [HttpGet]
        public IActionResult GetPayablesMember()
        {
            var Members = DB.Members.Where(f => (f.Account.EntryMovements.Select(d => d.Credit).Sum() - f.Account.EntryMovements.Select(d => d.Debit).Sum()) < 0).Select(x => new
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
                TotalCredit = DB.EntryMovements.Where(l => l.AccountId == x.AccountId).Select(c => c.Credit).Sum()
            }).ToList();

            return Ok(Members);
        }
        [Route("Member/GetMember")]
        [HttpGet]
        public IActionResult GetMember()
        {
            var Members = DB.Members.Select(x => new { x.Id, x.Name, x.Ssn, x.PhoneNumber1, x.Tag }).ToList();

            return Ok(Members);
        }

        [Route("Member/GetActiveMember")]
        [HttpGet]
        public IActionResult GetActiveMember()
        {
            try
            {

                var membershiplist = DB.ActionLogs.Where(l => l.MembershipMovementId != null && l.PostingDateTime >= DateTime.Today).Select(x => x.MembershipMovementId).ToList();

              var Members = DB.MembershipMovements.Where(x => membershiplist.Contains(x.Id)).ToList().Select(x => new
                {
                    x.Id,
                    Name = DB.Memberships.Where(m => m.Id == x.MembershipId).SingleOrDefault().Name,
                    x.VisitsUsed,
                    x.Type,
                    x.StartDate,
                    x.EndDate,
                    x.TotalAmmount,
                    x.Description,
                    x.Status,
                 // lastLog = DB.MemberLogs.Where(ml => ml.MemberId == x.MemberId).LastOrDefault().DateTime,
                  x.MemberId
                }).ToList();
                return Ok(Members);
            }
            catch
            {
                return Ok("None Active");

            }
        }

        [Route("Member/GetMemberByStatus")]
        [HttpGet]
        public IActionResult GetMemberByStatus(int Status)
        {
            var Members = DB.Members.Where(f => f.Status == Status).Select(x => new
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
                TotalCredit = DB.EntryMovements.Where(l => l.AccountId == x.AccountId).Select(c => c.Credit).Sum()
                // Avatar = Url.Content("~/Images/Member/" + x.Id + ".jpeg").ToString(),
            }).ToList();

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
            var Members = DB.Members.Where(m => m.Id == ID).Select(
                x => new
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
                    Avatar = Url.Content("~/Images/Member/" + x.Id + ".jpeg"),
                    TotalDebit = DB.EntryMovements.Where(l => l.AccountId == x.AccountId).Select(d => d.Debit).Sum(),
                    TotalCredit = DB.EntryMovements.Where(l => l.AccountId == x.AccountId).Select(c => c.Credit).Sum(),
                    x.AccountId,
                   // lastLog =DB.MemberLogs.Where(ml=>ml.MemberId == x.Id).ToList().LastOrDefault().DateTime.ToString(),
                    ActiveMemberShip = DB.MembershipMovements.Where(f => f.MemberId == x.Id && f.Status > 0).Select(MS => new
                    {
                        MS.Id,
                        MS.Membership.Name,
                        MS.VisitsUsed,
                        MS.Type,
                        MS.StartDate,
                        MS.EndDate,
                        MS.Description,
                    }).FirstOrDefault(),

                    ServiceInvoices = DB.SalesInvoices.Where(f => f.MemberId == x.Id && f.IsPrime == true).Select(SI => new
                    {
                        SI.Id,
                        SI.Name,
                        SI.Status,
                        SI.Description,
                        FakeDate= SI.FakeDate.ToString(CultureInfo.CurrentUICulture.DateTimeFormat.ShortDatePattern),
                        InventoryMovements = SI.InventoryMovements.Select(m => new
                        {
                            m.Id,
                            m.Status,
                            m.Items.Name,
                            m.Qty,
                            m.SellingPrice,
                            m.Description
                        }).ToList(),

                    }).ToList(),
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
                int OStatus = M.Status;

            
                if (M.MembershipMovements.Count() == 0)
                {
                    M.Status = -1;
                }
                var ActiveMemberShip = M.MembershipMovements.Where(m => m.Status == 1).SingleOrDefault();

                if (ActiveMemberShip != null)
                {

                    var HowManyDaysLeft = (ActiveMemberShip.EndDate - DateTime.Today).TotalDays;
                    if (HowManyDaysLeft == 3)
                    {
                        Massage msg = new Massage();
                        msg.Body = "عزيزي " + M.Name + " يسعدنا ان تكون متواجد دائماَ معنا في High Fit , نود تذكيرك بان اشتراك الحالي سينتهي بعد 3 ايام وبتاريخ " + ActiveMemberShip.EndDate.ToString("dd/MM/yyyy") + " وشكرا";
                        msg.Status = 0;
                        msg.TableName = "Member";
                        msg.Fktable = M.Id;
                        msg.PhoneNumber = M.PhoneNumber1;
                        msg.SendDate = DateTime.Today;
                        msg.Type = "رسالة تذكير";
                        DB.Massages.Add(msg);
                        DB.SaveChanges();
                    }

                }
                if (OStatus == -2)
                {

                    M.Status = -2;
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
