using Application.Common.Interfaces;
using Application.Features.MembershipMovement.Queries.GetMembershipMovementList;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestApi.Models;

namespace RestApi.Controllers.WorkSpace;

[Authorize]
public class MembershipMovementController : Controller
{
    private readonly IApplicationDbContext DB;
    public IConfiguration _configuration { get; }
    private readonly ISender _mediator;

    public MembershipMovementController(IApplicationDbContext dbcontext, IConfiguration configuration, ISender mediator)
    {
        DB = dbcontext;
        _configuration = configuration;
        _mediator = mediator;
    }
    [Route("MembershipMovement/GetMembershipMovementList")]
    [HttpGet]
    public async Task<IActionResult> GetMembershipMovementList([FromQuery] SearchOptions options)
    {
        var query = new GetMembershipMovementListQuery
        {
            Start = options.Page,
            Limit = options.Limit,
            CreatedBy = options.Any,
            SortBy = options.Sort,
            IsDesc = options.Sort[0].Equals("+"),
        };

        var result = await _mediator.Send(query);
        return Ok(result);

    }
    public async Task<IActionResult> GetMembershipMovementByMemberId(long MemberId)
    {
        var MembershipMovements = await DB.MembershipMovement.Where(z => z.MemberId == MemberId).Select(MS => new
        {
            MS.Id,
            MS.Membership.Name,
            MS.Membership.MinFreezeLimitDays,
            MS.Membership.MaxFreezeLimitDays,
            MS.VisitsUsed,
            MS.Type,
            MS.TotalAmmount,
            MS.MemberId,
            MS.MembershipId,
            MS.DiscountDescription,
            MS.Description,
            StartDate = DateOnly.Parse(MS.StartDate.ToShortDateString()),
            EndDate = DateOnly.Parse(MS.EndDate.ToShortDateString()),
            MS.Discount,
            MS.Created,
            MS.CreatedBy,
            MS.Status,
            MS.Tax,
            TotalDays = Math.Ceiling((MS.EndDate.Date - MS.StartDate.Date).TotalDays),
            Remaining = Math.Ceiling((MS.EndDate.Date - DateTime.Today).TotalDays),
            MembershipMovementOrders = MS.MembershipMovementOrders.Select(MSO => new
            {
                MSO.Id,
                MSO.Type,
                MSO.StartDate,
                MSO.EndDate,
                MSO.Status,
                MSO.Description,
                MSO.Created,
                MSO.CreatedBy
            }).ToList(),
        }).ToListAsync();



        return Ok(MembershipMovements);
    }
    [Route("MembershipMovement/GetMembershipMovementById")]
    [HttpGet]
    public IActionResult GetMembershipMovementById(long Id)
    {
        var MembershipMovements = DB.MembershipMovement.Where(z => z.Id == Id).Select(x => new
        {
            x.Id,
            x.VisitsUsed,
            x.Type,
            x.TotalAmmount,
            x.MemberId,
            x.MembershipId,
            x.DiscountDescription,
            x.Description,
            StartDate = x.StartDate.Date,
            x.EndDate,
            x.Discount,
            x.Tax,
            x.Status,
            x.Created,
            x.CreatedBy
        }).SingleOrDefault();


        return Ok(MembershipMovements);
    }
    [Route("MembershipMovement/GetMembershipMovementByStatus")]
    [HttpGet]
    public IActionResult GetMembershipMovementByStatus(int? Status)
    {
        var MembershipMovements = DB.MembershipMovement.Where(z => z.Status == Status).Select(x => new
        {
            x.Id,
            x.TotalAmmount,
            x.Tax,
            x.StartDate,
            x.EndDate,
            x.Type,
            x.VisitsUsed,
            x.Discount,
            x.DiscountDescription,
            x.Description,
            x.Status,
            x.Created,
            x.CreatedBy,
            x.MemberId,
            x.Member.AccountId,
            MemberName = x.Member.Name,
            MembershipName = x.Membership.Name,
        }).ToList();


        return Ok(MembershipMovements);
    }
    [HttpGet]
    [Route("MembershipMovement/GetMembershipMovementByMembershipId")]
    public IActionResult GetMembershipMovementByMembershipId(long? MembershipId, DateTime? DateIn)
    {
        var MembershipMovements = DB.MembershipMovement.Where(z =>
         (MembershipId == null || z.MembershipId == MembershipId) &&
         (DateIn == null || DateIn >= z.StartDate.Date && z.EndDate.Date >= DateIn)).Select(x => new
         {
             x.Id,
             x.TotalAmmount,
             x.Tax,
             x.StartDate,
             x.EndDate,
             x.Type,
             x.VisitsUsed,
             x.Discount,
             x.DiscountDescription,
             x.Description,
             x.Status,
             x.Created,
             x.CreatedBy,
             x.MemberId,
             x.Member.PhoneNumber1,
             x.Member.AccountId,
             DateofBirth = x.Member.DateofBirth.ToString(),
             MemberName = DB.Member.Where(m => m.Id == x.MemberId).SingleOrDefault().Name,
             MembershipName = DB.Membership.Where(m => m.Id == x.MembershipId).SingleOrDefault().Name,
         }).ToList();
        return Ok(MembershipMovements);
    }
    [Route("MembershipMovement/GetMembershipMovementByDateIn")]
    [HttpGet]
    public IActionResult GetMembershipMovementByDateIn(DateTime DateIn)
    {

        var MembershipMovements = DB.MembershipMovement.Where(z => DateIn >= z.StartDate.Date && z.EndDate.Date >= DateIn).Select(x => new
        {
            x.Id,
            x.TotalAmmount,
            x.Tax,
            StartDate = x.StartDate.ToShortDateString(),
            EndDate = x.EndDate.ToShortDateString(),
            x.Type,
            x.VisitsUsed,
            x.Discount,
            x.DiscountDescription,
            x.Description,
            x.Status,
            x.Created,
            x.CreatedBy,
            x.MemberId,
            x.Member.PhoneNumber1,
            x.Member.AccountId,
            DateofBirth = x.Member.DateofBirth.Value.Date.ToShortDateString(),
            MemberName = x.Member.Name,
            MembershipName = x.Membership.Name,
            Total = x.Member.Account.EntryMovements.Sum(i => i.Credit) - x.Member.Account.EntryMovements.Sum(i => i.Debit),
        }).ToList();


        return Ok(MembershipMovements);
    }
    [Route("MembershipMovement/CheckEntryAccountForMembershipMovement")]
    [HttpGet]
    public IActionResult CheckEntryAccountForMembershipMovement()
    {
        try
        {
            var MembershipMovements = DB.MembershipMovement.ToList();
            foreach (var membershipMovement in MembershipMovements)
            {
                var entry = DB.EntryMovement.Where(x => x.TableName == "MembershipMovement" && x.Fktable == membershipMovement.Id).FirstOrDefault();
                if (entry == null)
                {
                    DB.EntryAccounting.Add(new EntryAccounting
                    {
                        Description = "",
                        FakeDate = membershipMovement.StartDate,
                        Status = 0,
                        Type = "Auto",
                        EntryMovements = new List<EntryMovement>() { new EntryMovement {
                        TableName = "MembershipMovement",
                        Fktable =membershipMovement.Id,
                        AccountId =DB.Member.Where(x=>x.Id == membershipMovement.MemberId).SingleOrDefault().AccountId,// membershipMovement.Member.AccountId,
                        Description ="اشتراك رقم " + membershipMovement.Id ,
                        Credit =membershipMovement.TotalAmmount,
                        Debit =0,
                      },
                      new EntryMovement {
                        TableName = "MembershipMovement",
                        Fktable =membershipMovement.Id,
                        AccountId = 3,
                        Description ="اشتراك رقم " + membershipMovement.Id ,
                        Credit =0,
                        Debit =membershipMovement.TotalAmmount,
                      },
                    }
                    });
                    DB.SaveChanges();

                }
                continue;
            }

            return Ok(true);
        }
        catch (Exception ex)
        {
            return Ok(ex.Message);
        }

    }
    [Route("MembershipMovement/Create")]
    [HttpPost]
    public async Task<IActionResult> Create(MembershipMovement collection)
    {
        if (ModelState.IsValid)
        {
            try
            {
                collection.StartDate = collection.StartDate.Date;
                collection.EndDate = collection.EndDate.Date.AddDays(1).AddSeconds(-1);
                DB.MembershipMovement.Add(collection);
                await DB.SaveChangesAsync(new CancellationToken(), User.Identity.Name);
                return Created("", collection);

            }
            catch (Exception ex)
            {
                //Console.WriteLine(collection);
                return Forbid(ex.Message);
            }
        }
        return Forbid("False Valid");
    }
    [Route("MembershipMovement/Edit")]
    [HttpPost]
    public IActionResult Edit(MembershipMovement collection)
    {
        if (ModelState.IsValid)
        {
            try
            {
                var MemberShipMovement = DB.MembershipMovement.Where(x => x.Id == collection.Id).SingleOrDefault();
                MemberShipMovement.StartDate = collection.StartDate.Date;
                MemberShipMovement.EndDate = collection.EndDate.Date.AddDays(1).AddSeconds(-1);
                MemberShipMovement.Tax = collection.Tax;
                MemberShipMovement.TotalAmmount = collection.TotalAmmount;
                MemberShipMovement.Type = collection.Type;
                MemberShipMovement.Discount = collection.Discount;
                MemberShipMovement.Description = collection.Description;
                MemberShipMovement.DiscountDescription = collection.DiscountDescription;
                MemberShipMovement.MembershipId = collection.MembershipId;

                DB.SaveChanges();
                return Ok(collection.Id);

            }
            catch (Exception ex)
            {
                //Console.WriteLine(collection);
                return Forbid(ex.Message);
            }
        }
        return Forbid("False Valid");
    }
    [Route("MembershipMovement/Delete")]
    [HttpPost]
    public async Task<IActionResult> Delete(long Id)
    {
        try
        {
            var entryAccount = await DB.EntryMovement.Where(x => x.TableName == "MembershipMovement" && x.Fktable == Id).FirstOrDefaultAsync();
            if (entryAccount is not null)
            {
                var entryAccounting = await DB.EntryAccounting.Include(x => x.EntryMovements).SingleOrDefaultAsync(x => x.Id == entryAccount.EntryId);
                DB.EntryMovement.RemoveRange(entryAccounting.EntryMovements);
                DB.EntryAccounting.Remove(entryAccounting);
            }
            var membershipmovement = await DB.MembershipMovement.Include(x => x.MembershipMovementOrders).SingleOrDefaultAsync(x => x.Id == Id);
            DB.MembershipMovementOrder.RemoveRange(membershipmovement.MembershipMovementOrders);
            DB.MembershipMovement.Remove(membershipmovement);

            await DB.SaveChangesAsync();
            return Ok(true);

            //   else return Forbid("Can't found entry account");

        }
        catch (Exception ex)
        {
            return Forbid(ex.Message);

        }
    }
}