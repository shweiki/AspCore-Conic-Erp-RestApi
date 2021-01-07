using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.AspNetCore.Authorization;
using Entities; 

namespace AspCore_Conic_Erp_RestApi.Controllers
{
    [Authorize]
    public class EntryAccountingController : Controller
    {
        private ConicErpContext DB = new ConicErpContext();
        [HttpGet]
        public IActionResult GetEntryAccounting(long AccountID, DateTimeOffset DateFrom, DateTimeOffset DateTo)
        {
            var EntryMovements = DB.EntryMovements.Where(i => i.AccountId == AccountID && i.Entry.FakeDate >= DateFrom && i.Entry.FakeDate <= DateTo).Select(x=>new {
                x.Id,
                x.Debit,
                x.Credit,
                x.Description,
                x.EntryId,
                FakeDate = x.Entry.FakeDate.ToString("dd/MM/yyyy"),
            }).ToList();
                   

            return Ok(EntryMovements);
        }

        [HttpPost]
        [Route("EntryAccounting/Create")]
        public IActionResult Create(EntryAccounting collection)
        {

            if (ModelState.IsValid)
            {
                collection.Status = 0;
                try
                {
                    // TODO: Add insert logic here
                    DB.EntryAccountings.Add(collection);
                    DB.SaveChanges();
                    return Ok(true);

                }
                catch
                {
                    //Console.WriteLine(collection);
                    return Ok(false);
                }
            }
            else return Ok(false);
        }

    }
}
