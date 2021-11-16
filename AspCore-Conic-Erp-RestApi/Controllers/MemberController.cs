using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using Microsoft.AspNetCore.Authorization;
using Entities;
using Microsoft.AspNetCore.Mvc;

namespace AspCore_Conic_Erp_RestApi.Controllers
{
    [Authorize]
    public class MemberController : Controller
    {
                private ConicErpContext DB;
        public MemberController(ConicErpContext dbcontext)
        {
            DB = dbcontext;
        }
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
                x.Vaccine,
                TotalDebit = x.Account.EntryMovements.Select(d => d.Debit).Sum(),
                TotalCredit = x.Account.EntryMovements.Select(c => c.Credit).Sum()
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
                x.Vaccine,
                TotalDebit = x.Account.EntryMovements.Select(d => d.Debit).Sum(),
                TotalCredit = x.Account.EntryMovements.Select(c => c.Credit).Sum()
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

        [Route("Member/GetMemberByAny")]
        [HttpGet]
        public IActionResult GetMemberByAny(string Any)
        {
            Any.ToLower();
            var Members = DB.Members.Where(m => m.Id.ToString().Contains(Any) || m.Name.ToLower().Contains(Any) || m.Ssn.Contains(Any) || m.PhoneNumber1.Replace("0", "").Replace(" ", "").Contains(Any.Replace("0", "").Replace(" ", "")) || m.PhoneNumber2.Replace("0","").Replace(" ","").Contains(Any.Replace("0", "").Replace(" ", "")) || m.Tag.Contains(Any))
                .Select(x => new { x.Id, x.Name, x.Ssn, x.PhoneNumber1, x.Tag }).ToList();

            return Ok(Members);
        }
        [HttpPost]
        [Route("Member/GetByListQ")]
        public IActionResult GetByListQ(int Limit, string Sort, int Page,int? Status, string Any)
        {
            var Members = DB.Members.Where(s => (Any != null ? s.Id.ToString().Contains(Any) || s.Name.Contains(Any) : true)
            && (Status != null ? s.Status == Status : true)).Select(x => new
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
                x.Vaccine,
               // lastLogByMember= DB.DeviceLogs.Where(l=> l.TableName =="Member" && l.Fk == x.Id.ToString()).ToList().OrderBy(o=>o.DateTime).LastOrDefault().DateTime.ToString() + ' ',
                MembershipsCount = x.MembershipMovements.Count(),
                TotalDebit = x.Account.EntryMovements.Select(d => d.Debit).Sum(),
                TotalCredit = x.Account.EntryMovements.Select(c => c.Credit).Sum(),
            }).ToList();
            Members = (Sort == "+id" ? Members.OrderBy(s => s.Id).ToList() : Members.OrderByDescending(s => s.Id).ToList());
            return Ok(new
            {
                items = Members.Skip((Page - 1) * Limit).Take(Limit).ToList(),
                Totals = new
                {
                    Rows = Members.Count(),
                    Totals = Members.Sum(s => s.TotalCredit - s.TotalDebit),
                    TotalCredit = Members.Sum(s => s.TotalCredit),
                    TotalDebit = Members.Sum(s => s.TotalDebit),
                }
            });
        }

        [Route("Member/CheckMemberIsExist")]
        [HttpGet]
        public IActionResult CheckMemberIsExist(string Ssn , string PhoneNumber)
        {
            var Members = DB.Members.Where(m => m.Ssn == Ssn || m.PhoneNumber1.Replace("0", "") == PhoneNumber.Replace("0", "")).ToList();

            return Ok(Members.Count() > 0 ? true : false);
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
                    Name = DB.Members.Where(m => m.Id == x.MemberId).SingleOrDefault().Name,
                  MembershipName = DB.Memberships.Where(m => m.Id == x.MembershipId).SingleOrDefault().Name,
                    x.VisitsUsed,
                    x.Type,
                    x.StartDate,
                    x.EndDate,
                    x.TotalAmmount,
                    x.Description,
                    x.Status,
                  x.Member.Vaccine,
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
                x.Vaccine,
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
                    var ParentAccount = DB.Accounts.Where(i => i.Description == "Member").SingleOrDefault();
                    ParentAccount = ParentAccount ??= new Account { Id = 0, ParentId = 0, Code = "0" };
                    Account NewAccount = new Account
                    {
                        Type = "Member",
                        Description = collection.Description,
                        Status = 0,
                        Code = ParentAccount.Code + '-' + DB.Accounts.Where(i => i.ParentId == ParentAccount.Id).Count() + 1,
                        ParentId = ParentAccount.Id
                    };
                    DB.Accounts.Add(NewAccount);
                    DB.SaveChanges();
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
                member.Vaccine = collection.Vaccine;
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
        [Route("Member/GetMemberById")]
        [HttpGet]
        public IActionResult GetMemberById(long? Id)
        {
            var Members = DB.Members.Where(m => m.Id == Id).Select(
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
                    x.Vaccine,
                    MembershipsCount = x.MembershipMovements.Count(),
                    HaveFaceOnDevice = DB.FingerPrints.Where(f=>f.Fk == x.Id.ToString() && f.TableName == "Member").Count() > 0 ? true : false,
                    Avatar = Url.Content("~/Images/Member/" + x.Id + ".jpeg"),
                    TotalDebit = DB.EntryMovements.Where(l => l.AccountId == x.AccountId).Select(d => d.Debit).Sum(),
                    TotalCredit = DB.EntryMovements.Where(l => l.AccountId == x.AccountId).Select(c => c.Credit).Sum(),
                    x.AccountId,
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
                }).SingleOrDefault();
            return Ok(Members);
        }
        [Route("Member/FixPhoneNumber")]
        [HttpGet]
        public IActionResult FixPhoneNumber()
        {
            DB.Members.Where(i => i.PhoneNumber1 != null).ToList().ForEach(s => {
                s.PhoneNumber1 = s.PhoneNumber1.Replace(" ", "");
                s.PhoneNumber1 = s.PhoneNumber1.Length == 10 ? s.PhoneNumber1.Substring(1) : s.PhoneNumber1; 
            });

            DB.SaveChanges();
     

            return Ok(true);
        }

        [Route("Member/CheckMembers")]
        [HttpGet]
        public IActionResult CheckMembers()
        {
           var Members = DB.Members?.ToList();
        
            foreach (var M in Members)
            {
                int OStatus = M.Status;

               if (DB.MembershipMovements.Where(x=>x.MemberId == M.Id).Count() <= 0)
               {
                   M.Status = -1;
               }

                var ActiveMemberShip = M.MembershipMovements.Where(m => m.Status == 1).SingleOrDefault();
                if (ActiveMemberShip == null) {
                    M.Status = -1;
                }
                if (OStatus == -2) M.Status = -2;
            }
            //     CheckBlackListActionLogMembers();

            DB.SaveChanges();

            return Ok(true);
        }
       
        [Route("Member/CheckBlackListActionLogMembers")]
        [HttpGet]
        public IActionResult CheckBlackListActionLogMembers()
        {
            var Members = DB.Members?.ToList();

            foreach (var M in Members)
            {
                int OStatus = M.Status;

                var LastLog = DB.ActionLogs.Where(x => x.MemberId == M.Id && x.Opration.TableName =="Member" )?.OrderBy(o => o.PostingDateTime).ToList().LastOrDefault();
                if (LastLog != null)
                {
                    M.Status = DB.Oprationsys.Where(x => x.Id == LastLog.OprationId).SingleOrDefault().Status;
                }
                DB.SaveChanges();
            }
            return Ok(true);
        }
    }


}
