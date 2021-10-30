using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Entities; 
using Microsoft.AspNetCore.Mvc;

namespace AspCore_Conic_Erp_RestApi.Controllers
{
    [Authorize]
    public class MembershipController : Controller
    {
                private ConicErpContext DB;
        public MembershipController(ConicErpContext dbcontext)
        {
            DB = dbcontext;
        }

        [Route("Membership/GetMembership")]
        [HttpGet]
        public IActionResult GetMembership()
        {
            var Memberships = DB.Memberships.Select(x => new
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
                TotalMembers = DB.MembershipMovements.Where(l => l.MembershipId == x.Id && l.Status > 0).Count(),
            }).ToList();

            return Ok(Memberships);
        }

        [Route("Membership/GetActiveMembership")]
        [HttpGet]
        public IActionResult GetActiveMembership()
        {
            var Memberships = DB.Memberships.Where(x => x.Status == 0).Select(
                x => new {
                x.Id,
                x.Name ,
                x.Description ,
                x.FullDayPrice ,
                x.MinFreezeLimitDays,
                x.MaxFreezeLimitDays,
                x.MorningPrice ,
                x.NumberDays ,
                x.Status ,
                x.Tax ,
                x.Rate
            }).ToList();
            return Ok(Memberships);
        }
        [Route("Membership/Create")]
        [HttpPost]
        public IActionResult Create(Membership collection)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    DB.Memberships.Add(collection);
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

        [Route("Membership/Edit")]
        [HttpPost]
        public IActionResult Edit(Membership collection)
        {
            if (ModelState.IsValid)
            {
                Membership Membership = DB.Memberships.Where(x => x.Id == collection.Id).SingleOrDefault();
                Membership.Name = collection.Name;
                Membership.NumberDays = collection.NumberDays;
                Membership.MinFreezeLimitDays = collection.MinFreezeLimitDays;
                Membership.MaxFreezeLimitDays = collection.MaxFreezeLimitDays;
                Membership.FullDayPrice = collection.FullDayPrice;
                Membership.MorningPrice = collection.MorningPrice;
                Membership.Tax = collection.Tax;
                Membership.Rate = collection.Rate;
                Membership.Description = collection.Description;
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


    }

}
