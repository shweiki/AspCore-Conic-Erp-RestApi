using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace RestApi.Controllers.WorkSpace;

[Authorize]
public class MemberController : Controller
{
    private readonly IApplicationDbContext DB;
    private IConfiguration _configuration;
    private readonly ISender _mediator;


    public MemberController(IApplicationDbContext dbcontext, IConfiguration configuration, ISender mediator)
    {
        DB = dbcontext;
        _configuration = configuration;
        _mediator = mediator;

    }
    [Route("Member/GetReceivablesMember")]
    [HttpGet]
    public IActionResult GetReceivablesMember()
    {
        var Members = DB.Member.Where(f => f.Account.EntryMovements.Sum(s => s.Credit) - f.Account.EntryMovements.Sum(s => s.Debit) > 0)
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
            TotalDebit = x.Account.EntryMovements.Sum(s => s.Debit),
            TotalCredit = x.Account.EntryMovements.Sum(s => s.Credit)
            //   TotalDebit = DB.EntryMovement.Where(l => l.AccountId == x.AccountId).Sum(s => s.Debit),
            //  TotalCredit = DB.EntryMovement.Where(l => l.AccountId == x.AccountId).Sum(s => s.Credit)
        }).ToList();

        return Ok(Members);
    }
    [Route("Member/GetPayablesMember")]
    [HttpGet]
    public IActionResult GetPayablesMember()
    {
        var Members = DB.Member.Where(f => f.Account.EntryMovements.Sum(s => s.Credit) - f.Account.EntryMovements.Sum(s => s.Debit) < 0).Select(x => new
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
            TotalDebit = x.Account.EntryMovements.Sum(s => s.Debit),
            TotalCredit = x.Account.EntryMovements.Sum(s => s.Credit)
        }).ToList();

        return Ok(Members);
    }
    [Route("Member/GetMember")]
    [HttpGet]
    public IActionResult GetMember()
    {
        var Members = DB.Member.Select(x => new { x.Id, x.Name, x.Ssn, x.PhoneNumber1, x.Tag }).ToList();

        return Ok(Members);
    }

    [Route("Member/GetMemberByAny")]
    [HttpGet]
    public IActionResult GetMemberByAny(string Any)
    {
        Any.ToLower();
        var Members = DB.Member.Where(m => m.Id.ToString().Contains(Any) || m.Name.ToLower().Contains(Any) || m.Ssn.Contains(Any) || m.PhoneNumber1.Replace("0", "").Replace(" ", "").Contains(Any.Replace("0", "").Replace(" ", "")) || m.PhoneNumber2.Replace("0", "").Replace(" ", "").Contains(Any.Replace("0", "").Replace(" ", "")) || m.Tag.Contains(Any))
            .Select(x => new { x.Id, x.Name, x.Ssn, x.PhoneNumber1, x.Tag }).ToList();

        return Ok(Members);
    }
    [HttpPost]
    [Route("Member/GetByListQ")]
    public async Task<IActionResult> GetByListQ(int Limit, string Sort, int Page, int? Status, string Any)
    {

        var Members = await DB.Member.Where(s => (Any == null || s.Id.ToString().Contains(Any) || s.Name.ToLower().Contains(Any) || s.Ssn.Contains(Any) || s.PhoneNumber1.Replace("0", "").Replace(" ", "").Contains(Any.Replace("0", "").Replace(" ", "")) || s.PhoneNumber2.Replace("0", "").Replace(" ", "").Contains(Any.Replace("0", "").Replace(" ", "")) || s.Tag.Contains(Any))
    && (Status == null || s.Status == Status)).Include(x => x.Account).Include(x => x.Account.EntryMovements).Select(x => new
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
        TotalDebit = x.Account.EntryMovements.Sum(d => d.Debit),
        TotalCredit = x.Account.EntryMovements.Sum(c => c.Credit),
    }).ToListAsync();
        Members = Sort == "+id" ? Members.OrderBy(s => s.Id).ToList() : Members.OrderByDescending(s => s.Id).ToList();
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
        var Members = DB.Member.Where(m => m.Ssn == Ssn || m.PhoneNumber1.Replace("0", "") == PhoneNumber.Replace("0", "")).ToList();

        return Ok(Members.Count() > 0);
    }

    [Route("Member/GetActiveMember")]
    [HttpGet]
    public IActionResult GetActiveMember()
    {
        try
        {

            var membershiplist = DB.ActionLog.Where(l => l.TableName == "Membership" && l.Fktable != null && l.PostingDateTime >= DateTime.Today).Select(x => long.Parse(x.Fktable)).ToList();

            var Members = DB.MembershipMovement.Where(x => membershiplist.Contains(x.Id)).ToList().Select(x => new
            {
                x.Id,
                DB.Member.Where(m => m.Id == x.MemberId).SingleOrDefault().Name,
                MembershipName = DB.Membership.Where(m => m.Id == x.MembershipId).SingleOrDefault().Name,
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
        var Members = DB.Member.Where(f => f.Status == Status).Select(x => new
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
            TotalDebit = DB.EntryMovement.Where(l => l.AccountId == x.AccountId).Select(d => d.Debit).Sum(),
            TotalCredit = DB.EntryMovement.Where(l => l.AccountId == x.AccountId).Select(c => c.Credit).Sum()
            // Avatar = Url.Content("~/Images/Member/" + x.Id + ".jpeg").ToString(),
        }).ToList();

        return Ok(Members);
    }

    [Route("Member/Create")]
    [HttpPost]
    public async Task<IActionResult> Create(Member collection)
    {
        if (ModelState.IsValid)
        {
            try
            {
                collection.Account = new TreeAccount
                {
                    Type = "Member",
                    Description = collection.Description,
                    Status = 0,
                    Code = "",
                    ParentId = DB.TreeAccount.Where(x => x.Type == "Members-Main").SingleOrDefault().Code
                };
                var result = await DB.Member.AddAsync(collection);

                await DB.SaveChangesAsync(new CancellationToken(), User.Identity.Name);

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
    public async Task<IActionResult> Edit(Member collection)
    {
        if (ModelState.IsValid)
        {
            Member member = DB.Member.Where(x => x.Id == collection.Id).SingleOrDefault();
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
                await DB.SaveChangesAsync(new CancellationToken(), User.Identity.Name);
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

        var member = await DB.Member.Include(x => x.MembershipMovements).Include(x => x.Account.EntryMovements).SingleOrDefaultAsync(m => m.Id == Id);
        if (member is null) return BadRequest();
        var membershipMovement = member.MembershipMovements.ToList();
        if (membershipMovement.Count == 0)
        {
            member.Status = -1;
        }
        else
        {
            foreach (var MS in membershipMovement.OrderBy(o => o.Id))
            {
                await MembershipMovementController.ScanMembershipMovementById(MS.Id, DB, _configuration);
            }
        }
        await DB.SaveChangesAsync();

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
               HaveFaceOnDevice = false,// DB.FingerPrint.Where(f=>f.Fk == x.Id.ToString() && f.TableName == "Member").Count() > 0 ? true : false,
               TotalDebit = member.Account.EntryMovements.Select(d => d.Debit).Sum(),
               TotalCredit = member.Account.EntryMovements.Select(c => c.Credit).Sum(),
               ActiveMemberShip = member.MembershipMovements.Where(f => f.Status > 0).Select(MS => new
               {
                   MS.Id,
                   MS.Membership.Name,
                   MS.Membership.NumberClass,
                   MS.VisitsUsed,
                   MS.Type,
                   StartDate = MS.StartDate.ToShortDateString(),
                   EndDate = MS.EndDate.ToShortDateString(),
                   TotalDays = Math.Ceiling((MS.EndDate.Date - MS.StartDate.Date).TotalDays),
                   Remaining = Math.Ceiling((MS.EndDate.Date - DateTime.Today).TotalDays),
                   MS.Description,
               }).FirstOrDefault(),
           });

    }
    [Route("Member/Delete")]
    [HttpPost]
    public async Task<IActionResult> Delete(long Id)
    {
        try
        {
            var member = await DB.Member.Include(x => x.MembershipMovements).Include(x => x.Payments).Include(x => x.Receives).Include(x => x.SalesInvoices).Include(x => x.Account.EntryMovements).SingleOrDefaultAsync(m => m.Id == Id);

            if (member is not null)
            {
                DB.Member.Remove(member);
                DB.TreeAccount.Remove(member.Account);
                await DB.SaveChangesAsync(new CancellationToken(), User.Identity.Name);
                return Ok(true);
            }
            else return Forbid("Can't found member");

        }
        catch (Exception ex)
        {
            return Forbid(ex.Message);

        }
    }

    [Route("Member/MergeTwoMembers")]
    [HttpPost]
    public async Task<IActionResult> MergeTwoMembers(long IntoId, long CurrentId)
    {
        try
        {
            if (IntoId == CurrentId) return Forbid("Same Ids");

            var currentMember = await DB.Member.Include(x => x.MembershipMovements).Include(x => x.Payments).Include(x => x.Receives).Include(x => x.SalesInvoices).Include(x => x.Account.EntryMovements).SingleOrDefaultAsync(m => m.Id == CurrentId);
            var intoMember = await DB.Member.Include(x => x.MembershipMovements).Include(x => x.Payments).Include(x => x.Receives).Include(x => x.SalesInvoices).Include(x => x.Account.EntryMovements).SingleOrDefaultAsync(m => m.Id == IntoId);
            foreach (var membershipMovement in currentMember.MembershipMovements)
            {
                membershipMovement.MemberId = intoMember.Id;

            }
            foreach (var payment in currentMember.Payments)
            {
                payment.MemberId = intoMember.Id;

            }
            foreach (var receive in currentMember.Receives)
            {
                receive.MemberId = intoMember.Id;
            }
            foreach (var salesInvoice in currentMember.SalesInvoices)
            {
                salesInvoice.MemberId = intoMember.Id;
            }
            foreach (var entryMovement in currentMember.Account.EntryMovements)
            {
                entryMovement.AccountId = intoMember.AccountId;
            }
            foreach (var deviceLog in DB.DeviceLog.Where(x => x.TableName == "Member" && x.Fk == currentMember.Id.ToString()).ToList())
            {
                deviceLog.Fk = intoMember.Id.ToString();
            }
            foreach (var fileData in DB.FileData.Where(x => x.TableName == "Member" && x.Fktable == currentMember.Id).ToList())
            {
                fileData.Fktable = intoMember.Id;
            }
            await Delete(currentMember.Id);
            await DB.SaveChangesAsync(new CancellationToken(), User.Identity.Name);
            return Ok();
        }
        catch (Exception ex)
        {
            return Forbid(ex.Message);
        }

    }

    [Route("Member/CheckBlackListActionLogMembers")]
    [HttpGet]
    public async Task<IActionResult> CheckBlackListActionLogMembers()
    {
        var Members = DB.Member?.ToList();

        foreach (var M in Members)
        {
            int OStatus = M.Status;

            var LastLog = DB.ActionLog.Where(x => x.TableName == "Member" && x.Fktable == M.Id.ToString())?.OrderBy(o => o.PostingDateTime).ToList().LastOrDefault();
            if (LastLog != null)
            {
                M.Status = DB.Oprationsy.Where(x => x.Id == LastLog.OprationId).SingleOrDefault().Status;
            }
            await DB.SaveChangesAsync(new CancellationToken(), User.Identity.Name);
        }
        return Ok(true);
    }
}
