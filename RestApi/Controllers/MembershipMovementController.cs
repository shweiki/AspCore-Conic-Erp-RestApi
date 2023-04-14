using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestApi.Controllers;

[Authorize]
public class MembershipMovementController : Controller
{
    private readonly ConicErpContext DB;
    public IConfiguration Configuration { get; }

    public MembershipMovementController(ConicErpContext dbcontext, IConfiguration configuration)
    {
        DB = dbcontext;
        Configuration = configuration;
    }

    [Route("MembershipMovement/Create")]
    [HttpPost]
    public IActionResult Create(MembershipMovement collection)
    {
        if (ModelState.IsValid)
        {
            try
            {

                DB.MembershipMovements.Add(collection);
                DB.SaveChanges();
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
                var MemberShipMovement = DB.MembershipMovements.Where(x => x.Id == collection.Id).SingleOrDefault();

                MemberShipMovement.StartDate = collection.StartDate;
                MemberShipMovement.EndDate = collection.EndDate;
                MemberShipMovement.Tax = collection.Tax;
                MemberShipMovement.TotalAmmount = collection.TotalAmmount;
                MemberShipMovement.Type = collection.Type;
                MemberShipMovement.Discount = collection.Discount;
                MemberShipMovement.Description = collection.Description;
                MemberShipMovement.DiscountDescription = collection.DiscountDescription;
                MemberShipMovement.MembershipId = collection.MembershipId;
                MemberShipMovement.EditorName = collection.EditorName;

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
            var entryAccount = await DB.EntryMovements.Where(x => x.TableName == "MembershipMovement" && x.Fktable == Id).FirstOrDefaultAsync();
            if (entryAccount is not null)
            {
                var entryAccounting = await DB.EntryAccountings.Include(x => x.EntryMovements).SingleOrDefaultAsync(x => x.Id == entryAccount.EntryId);
                DB.EntryMovements.RemoveRange(entryAccounting.EntryMovements);
                DB.EntryAccountings.Remove(entryAccounting);
            }
            var membershipmovement = await DB.MembershipMovements.Include(x => x.MembershipMovementOrders).SingleOrDefaultAsync(x => x.Id == Id);
            DB.MembershipMovementOrders.RemoveRange(membershipmovement.MembershipMovementOrders);
            DB.MembershipMovements.Remove(membershipmovement);

            await DB.SaveChangesAsync();
            return Ok(true);

            //   else return Forbid("Can't found entry account");

        }
        catch (Exception ex)
        {
            return Forbid(ex.Message);

        }
    }

    public static async Task<bool> ScanMembershipMovementById(long ID, ConicErpContext DB, IConfiguration Configuration)
    {

        MembershipMovement MS = await DB.MembershipMovements.Include(x => x.Member).Include(x => x.Membership).Include(x => x.MembershipMovementOrders).SingleOrDefaultAsync(x => x.Id == ID);

        var member = MS.Member;
        int OStatus = member.Status;
        double TotalMembershipMovementOrders = MS.MembershipMovementOrders.Where(x => x.Status == -2 || x.Status == -3).ToList().Aggregate(0.0, (acc, x) => acc + (x.EndDate - x.StartDate).TotalDays);
        int MembershipNumberDays = MS.Membership.NumberDays;// DB.Memberships.Where(m => m.Id == MS.MembershipId).FirstOrDefault().NumberDays;
        MS.EndDate = MS.StartDate.AddDays(MembershipNumberDays + TotalMembershipMovementOrders);
        if (DateTime.Now >= MS.StartDate.Date && DateTime.Now <= MS.EndDate.Date.AddHours(23).AddMinutes(59).AddSeconds(59))
        {

            MS.Status = 1;
            member.Status = 0;
            var HowManyDaysLeft = (MS.EndDate - DateTime.Now).TotalDays;
            if (HowManyDaysLeft == 3)
            {
                Massage msg = new();
                msg.Body = "عزيزي " + member.Name + " يسعدنا ان تكون متواجد دائماَ معنا في High Fit , نود تذكيرك بان اشتراك الحالي سينتهي بعد 3 ايام وبتاريخ " + MS.EndDate + " وشكرا";
                msg.Status = 0;
                msg.TableName = "Member";
                msg.Fktable = member.Id;
                msg.PhoneNumber = member.PhoneNumber1;
                msg.SendDate = DateTime.Now;
                msg.Type = "رسالة تذكير";
                DB.Massages.Add(msg);
            }

        }
        else
        {

            if (MS.StartDate.Date > DateTime.Now)
            {// معلق
                MS.Status = -2;
            }
            else
            {

                MS.Status = -1;
                member.Status = -1;
            }
        }

        foreach (var MSO in MS.MembershipMovementOrders.Where(x => x.Status == 1 || x.Status == 2).ToList())
        {
            if (MSO.Status == 2)
            {
                MS.EndDate = MS.EndDate.AddDays((MSO.EndDate - MSO.StartDate).TotalDays);
                MSO.Status = -2;
                continue;
            }
            if ((DateTime.Now >= MSO.StartDate.Date && DateTime.Now <= MSO.EndDate.Date.AddHours(23).AddMinutes(59).AddSeconds(59)))
            {
                if (MSO.Type == "Freeze")
                {
                    MS.Status = 2;
                    member.Status = 1;
                }
                if (MSO.Type == "Extra")
                {
                    MS.Status = 3;
                    member.Status = 2;

                }
            }
            else
            {
                if (MSO.Type == "Extra")
                {
                    MS.EndDate = MS.EndDate.AddDays((MSO.EndDate - MSO.StartDate).TotalDays);
                    MSO.Status = -3;
                }
                if (DateTime.Now > MSO.EndDate)
                {

                    MS.EndDate = MS.EndDate.AddDays((MSO.EndDate - MSO.StartDate).TotalDays);
                    MSO.Status = -3;
                }

            }
            if ((MS.EndDate.Date.AddHours(23).AddMinutes(59).AddSeconds(59) > DateTime.Now))
            {


                MS.Status = 1;
                member.Status = 1;

            }
            if (MS == null)
            {
                member.Status = -1;
            }


            await DB.SaveChangesAsync();

        }
        var DeviceLogs = await DB.DeviceLogs.Where(x => x.Fk == member.Id.ToString() && x.TableName == "Member" && (x.DateTime >= MS.StartDate.Date && x.DateTime <= MS.EndDate.Date.AddHours(23).AddMinutes(59).AddSeconds(59))).ToListAsync();
        if (DeviceLogs != null && DeviceLogs.Count > 0)
        {
            DeviceLogs = DeviceLogs.GroupBy(a => a.DateTime.Day).Select(g => g.Last()).ToList();
            MS.VisitsUsed = DeviceLogs.Count();
            int NumberClass = (int)(MS.Membership.NumberClass == null ? 0 : MS.Membership.NumberClass);

            var WhenFinishNumberOfClassUnActiveMemberShip = Configuration["GymConfiguration:WhenFinishNumberOfClassUnActiveMemberShip"];
            if (WhenFinishNumberOfClassUnActiveMemberShip == "True")
            {
                if (MS.Membership.NumberClass <= MS.VisitsUsed)
                {
                    MS.EndDate = DeviceLogs.LastOrDefault().DateTime;
                    MS.Status = -1;
                    member.Status = -1;
                }
            }
        }
        if (OStatus == -2) member.Status = -2;
        await DB.SaveChangesAsync();

        return true;

    }

    [Route("MembershipMovement/GetMembershipMovementByMemberId")]
    [HttpGet]
    public IActionResult GetMembershipMovementByMemberId(long? MemberId)
    {
        var MembershipMovements = DB.MembershipMovements.Where(z => z.MemberId == MemberId).Select(MS => new
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
            MS.StartDate,
            MS.EndDate,
            MS.Discount,
            MS.EditorName,
            MS.Status,
            MS.Tax,
            TotalDays = Math.Ceiling((MS.EndDate - MS.StartDate).TotalDays),
            Remaining = Math.Ceiling((MS.EndDate - DateTime.Now).TotalDays),
            MembershipMovementOrders = MS.MembershipMovementOrders.Select(MSO => new
            {
                MSO.Id,
                MSO.Type,
                MSO.StartDate,
                MSO.EndDate,
                MSO.Status,
                MSO.Description,
                MSO.EditorName,

            }).ToList(),
        }).ToList();



        return Ok(MembershipMovements);
    }
    [Route("MembershipMovement/GetMembershipMovementById")]
    [HttpGet]
    public IActionResult GetMembershipMovementById(long? Id)
    {
        var MembershipMovements = DB.MembershipMovements.Where(z => z.Id == Id).Select(x => new
        {
            x.Id,
            x.VisitsUsed,
            x.Type,
            x.TotalAmmount,
            x.MemberId,
            x.MembershipId,
            x.DiscountDescription,
            x.Description,
            x.StartDate,
            x.EndDate,
            x.Discount,
            x.Tax,
            x.Status,
            x.EditorName,
        }).SingleOrDefault();


        return Ok(MembershipMovements);
    }
    [Route("MembershipMovement/GetMembershipMovementByStatus")]
    [HttpGet]
    public IActionResult GetMembershipMovementByStatus(int? Status)
    {
        var MembershipMovements = DB.MembershipMovements.Where(z => z.Status == Status).Select(x => new
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
            x.EditorName,
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
        var MembershipMovements = DB.MembershipMovements.Where(z =>
         (MembershipId == null || z.MembershipId == MembershipId) &&
         (DateIn == null || DateIn >= z.StartDate && z.EndDate >= DateIn)).Select(x => new
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
             x.EditorName,
             x.MemberId,
             x.Member.PhoneNumber1,
             x.Member.AccountId,
             MemberName = DB.Members.Where(m => m.Id == x.MemberId).SingleOrDefault().Name,
             MembershipName = DB.Memberships.Where(m => m.Id == x.MembershipId).SingleOrDefault().Name,
         }).ToList();
        return Ok(MembershipMovements);
    }
    [Route("MembershipMovement/GetMembershipMovementByDateIn")]
    [HttpGet]
    public IActionResult GetMembershipMovementByDateIn(DateTime DateIn)
    {
        var MembershipMovements = DB.MembershipMovements.Where(z => DateIn >= z.StartDate && z.EndDate >= DateIn).Select(x => new
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
            x.EditorName,
            x.MemberId,
            x.Member.AccountId,
            MemberName = DB.Members.Where(m => m.Id == x.MemberId).SingleOrDefault().Name,
            MembershipName = DB.Memberships.Where(m => m.Id == x.MembershipId).SingleOrDefault().Name,
        }).ToList();


        return Ok(MembershipMovements);
    }
    [Route("MembershipMovement/CheckEntryAccountForMembershipMovement")]
    [HttpGet]
    public IActionResult CheckEntryAccountForMembershipMovement()
    {
        var MembershipMovements =  DB.MembershipMovements.ToList();
        foreach (var membershipMovement in MembershipMovements)
        {
            var entry =  DB.EntryMovements.Where(x => x.TableName == "MembershipMovement" && x.Fktable == membershipMovement.Id).FirstOrDefault();
            if (entry == null)
            {
                DB.EntryAccountings.Add(new EntryAccounting
                {
                    Description = "",
                    FakeDate = membershipMovement.StartDate,
                    Status = 0,
                    Type = "Auto",
                    EntryMovements = new List<EntryMovement>() { new EntryMovement {
                        TableName = "MembershipMovement",
                        Fktable =membershipMovement.Id,
                        AccountId =DB.Members.Where(x=>x.Id == membershipMovement.MemberId).SingleOrDefault().AccountId,// membershipMovement.Member.AccountId,
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
}