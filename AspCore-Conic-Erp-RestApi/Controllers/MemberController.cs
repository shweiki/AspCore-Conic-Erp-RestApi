using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AspCore_Conic_Erp_RestApi.Controllers;

[Authorize]
public class MemberController : Controller
{
    private ConicErpContext DB;
    public IConfiguration Configuration { get; }

    public MemberController(ConicErpContext dbcontext, IConfiguration configuration)
    {
        DB = dbcontext;
        Configuration = configuration;

    }
    [Route("Member/GetReceivablesMember")]
    [HttpGet]
    public IActionResult GetReceivablesMember()
    {
        var Members = DB.Members.Include(x => x.Account.EntryMovements).Where(f => (f.Account.EntryMovements.Select(d => d.Credit).Sum() - f.Account.EntryMovements.Select(d => d.Debit).Sum()) > 0).AsQueryable()
        .Select(x => new
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
        var Members = DB.Members.Include(x => x.Account.EntryMovements).Where(f => (f.Account.EntryMovements.Select(d => d.Credit).Sum() - f.Account.EntryMovements.Select(d => d.Debit).Sum()) < 0).AsQueryable().Select(x => new
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
        var Members = DB.Members.Where(m => m.Id.ToString().Contains(Any) || m.Name.ToLower().Contains(Any) || m.Ssn.Contains(Any) || m.PhoneNumber1.Replace("0", "").Replace(" ", "").Contains(Any.Replace("0", "").Replace(" ", "")) || m.PhoneNumber2.Replace("0", "").Replace(" ", "").Contains(Any.Replace("0", "").Replace(" ", "")) || m.Tag.Contains(Any))
            .Select(x => new { x.Id, x.Name, x.Ssn, x.PhoneNumber1, x.Tag }).ToList();

        return Ok(Members);
    }
    [HttpPost]
    [Route("Member/GetByListQ")]
    public IActionResult GetByListQ(int Limit, string Sort, int Page, int? Status, string Any)
    {
        var Members = DB.Members.Where(s => (Any == null || s.Id.ToString().Contains(Any) || s.Name.ToLower().Contains(Any) || s.Ssn.Contains(Any) || s.PhoneNumber1.Replace("0", "").Replace(" ", "").Contains(Any.Replace("0", "").Replace(" ", "")) || s.PhoneNumber2.Replace("0", "").Replace(" ", "").Contains(Any.Replace("0", "").Replace(" ", "")) || s.Tag.Contains(Any))
        && (Status == null || s.Status == Status)).Select(x => new
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
    public IActionResult CheckMemberIsExist(string Ssn, string PhoneNumber)
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

                TreeAccount NewAccount = new TreeAccount
                {
                    Type = "Member",
                    Description = collection.Description,
                    Status = 0,
                    Code = "",
                    ParentId = DB.TreeAccounts.Where(x => x.Type == "Members-Main").SingleOrDefault().Code

                };
                DB.TreeAccounts.Add(NewAccount);
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
    public async Task<IActionResult> GetMemberById(long? Id)
    {

        var member = await DB.Members.Include(x => x.MembershipMovements).Include(x => x.Account.EntryMovements).SingleOrDefaultAsync(m => m.Id == Id);
        if (member == null) return BadRequest();
        foreach (var MS in member.MembershipMovements.Where(m => m.MemberId == Id).ToList())
        {
            await MembershipMovementController.ScanMembershipMovementById(MS.Id, DB, Configuration);
        }
        return Ok(
           new
           {
               member.Id,
               member.Name,
               member.Ssn,
               member.DateofBirth,
               member.Email,
               member.PhoneNumber1,
               member.PhoneNumber2,
               member.Description,
               member.Status,
               member.Type,
               member.Tag,
               member.Vaccine,
               member.AccountId,
               MembershipsCount = member.MembershipMovements.Count(),
               HaveFaceOnDevice = false,// DB.FingerPrints.Where(f=>f.Fk == x.Id.ToString() && f.TableName == "Member").Count() > 0 ? true : false,
               TotalDebit = member.Account.EntryMovements.Select(d => d.Debit).Sum(),
               TotalCredit = member.Account.EntryMovements.Select(c => c.Credit).Sum(),
               ActiveMemberShip = member.MembershipMovements.Where(f => f.Status > 0).Select(MS => new
               {
                   MS.Id,
                   MS.Membership.Name,
                   MS.Membership.NumberClass,
                   MS.VisitsUsed,
                   MS.Type,
                   MS.StartDate,
                   MS.EndDate,
                   MS.Description,
               }).FirstOrDefault(),
           });
    }
    [Route("Member/FixPhoneNumber")]
    [HttpGet]
    public IActionResult FixPhoneNumber()
    {
        DB.Members.Where(i => i.PhoneNumber1 != null).ToList().ForEach(s =>
        {
            s.PhoneNumber1 = s.PhoneNumber1.Replace(" ", "");
            s.PhoneNumber1 = s.PhoneNumber1.Length == 10 ? s.PhoneNumber1.Substring(1) : s.PhoneNumber1;
        });

        DB.SaveChanges();


        return Ok(true);
    }

    [Route("Member/CheckMembers")]
    [HttpGet]
    public async Task<IActionResult> CheckMembers()
    {
        var Members = await DB.Members.Include(x => x.MembershipMovements).ToListAsync();

        foreach (var member in Members)
        {
            int OStatus = member.Status;

            var MembershipMovements = member.MembershipMovements.ToList();

            if (MembershipMovements.Count() <= 0)
            {
                member.Status = -1;
            }
            else
            {
                foreach (var MS in MembershipMovements)
                {
                    await MembershipMovementController.ScanMembershipMovementById(MS.Id, DB, Configuration);
                }
            }
            if (OStatus == -2) member.Status = -2;

            await DB.SaveChangesAsync();
        }
        //CheckBlackListActionLogMembers();

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

            var LastLog = DB.ActionLogs.Where(x => x.MemberId == M.Id && x.Opration.TableName == "Member")?.OrderBy(o => o.PostingDateTime).ToList().LastOrDefault();
            if (LastLog != null)
            {
                M.Status = DB.Oprationsys.Where(x => x.Id == LastLog.OprationId).SingleOrDefault().Status;
            }
            DB.SaveChanges();
        }
        return Ok(true);
    }
}
