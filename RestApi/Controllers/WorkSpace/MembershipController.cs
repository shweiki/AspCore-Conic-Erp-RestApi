using Application.Common.Enums;
using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace RestApi.Controllers.WorkSpace;

[Authorize]
public class MembershipController : Controller
{
    private readonly IApplicationDbContext DB;
    public MembershipController(IApplicationDbContext dbcontext)
    {
        DB = dbcontext;
    }

    [Route("Membership/GetMembership")]
    [HttpGet]
    public IActionResult GetMembership()
    {
        return Ok(DB.Membership.Select(x => new
        {
            x.Id,
            x.Name,
            x.NumberDays,
            x.FullDayPrice,
            x.MorningPrice,
            x.Tax,
            x.Rate,
            x.MinFreezeLimitDays,
            x.MaxFreezeLimitDays,
            x.Description,
            x.Status,
            x.NumberClass,
            TotalMembers = DB.MembershipMovement.Where(l => l.MembershipId == x.Id && l.Status > 0).Count(),
        }).ToList());
    }

    [Route("Membership/GetActiveMembership")]
    [HttpGet]
    public async Task<IActionResult> GetActiveMembership()
    {
        var Memberships = await DB.Membership.Where(x => x.Status == (int)MembershipStatus.Active).Select(
            x => new
            {
                x.Id,
                x.Name,
                x.Description,
                x.FullDayPrice,
                x.MinFreezeLimitDays,
                x.MaxFreezeLimitDays,
                x.MorningPrice,
                x.NumberDays,
                x.Status,
                x.Tax,
                x.Rate,
                x.NumberClass,
                TotalMembers = DB.MembershipMovement.Where(l => l.MembershipId == x.Id).Count(),

            }).ToListAsync();
        return Ok(Memberships);
    }
    [Route("Membership/Create")]
    [HttpPost]
    public async Task<IActionResult> Create(Membership collection)
    {
        if (ModelState.IsValid)
        {
            try
            {
                DB.Membership.Add(collection);
                await DB.SaveChangesAsync(new CancellationToken(), User.Identity.Name);
                return Ok(true);

            }
            catch
            {
                return BadRequest();

            }
        }
        return BadRequest();
    }

    [Route("Membership/Edit")]
    [HttpPost]
    public async Task<IActionResult> Edit(Membership collection)
    {
        if (ModelState.IsValid)
        {
            try
            {
                Membership Membership = DB.Membership.Where(x => x.Id == collection.Id).SingleOrDefault();
                Membership.Name = collection.Name;
                Membership.NumberDays = collection.NumberDays;
                Membership.MinFreezeLimitDays = collection.MinFreezeLimitDays;
                Membership.MaxFreezeLimitDays = collection.MaxFreezeLimitDays;
                Membership.FullDayPrice = collection.FullDayPrice;
                Membership.MorningPrice = collection.MorningPrice;
                Membership.Tax = collection.Tax;
                Membership.Rate = collection.Rate;
                Membership.Description = collection.Description;
                Membership.NumberClass = collection.NumberClass;

                await DB.SaveChangesAsync(new CancellationToken(), User.Identity.Name);
                return Ok(true);
            }
            catch
            {
                return BadRequest();
            }
        }
        return BadRequest(false);
    }


}
