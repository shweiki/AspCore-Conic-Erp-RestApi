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
        public IActionResult GetEntryAccounting(long AccountId, DateTime DateFrom, DateTime DateTo)
        {
            var EntryMovements = DB.EntryMovements.Where(i => i.AccountId == AccountId && i.Entry.FakeDate >= DateFrom && i.Entry.FakeDate <= DateTo).Select(x=>new {
                x.Id,
                x.Debit,
                x.Credit,
                x.Description,
                x.EntryId,
                x.Entry.FakeDate,
            }).ToList();
            return Ok(EntryMovements);
        }
        [HttpPost]
        [Route("EntryAccounting/GetByListQ")]
        public IActionResult GetByListQ(long? AccountId, int Limit, string Sort, int Page, string? User, DateTime? DateFrom, DateTime? DateTo, int? Status, string? Any)
        {
            var EntryMovements = DB.EntryMovements.Where(s => s.AccountId == AccountId && (Any != null ? s.Id.ToString().Contains(Any) || s.Description.Contains(Any) : true) && (DateFrom != null ? s.Entry.FakeDate >= DateFrom : true)
            && (DateTo != null ? s.Entry.FakeDate <= DateTo : true) && (Status != null ? s.Entry.Status == Status : true) &&
            (User != null ? DB.ActionLogs.Where(l => l.PaymentId == s.Id && l.UserId == User).SingleOrDefault() != null : true)).Select(x => new

            {
                x.Id,
                x.Debit,
                x.Credit,
                x.Description,
                x.EntryId,
                x.Entry.FakeDate,
                x.Entry.Status,
                x.Entry.Type,
                EntryDescription= x.Entry.Description,
            }).ToList();
            EntryMovements = (Sort == "+id" ? EntryMovements.OrderBy(s => s.Id).ToList() : EntryMovements.OrderByDescending(s => s.Id).ToList());
            return Ok(new
            {
                items = EntryMovements.Skip((Page - 1) * Limit).Take(Limit).ToList(),
                Totals = new
                {
                    Rows = EntryMovements.Count(),
                    Totals = EntryMovements.Sum(s => s.Credit) - EntryMovements.Sum(s => s.Debit)  ,
                    Debit = EntryMovements.Sum(s => s.Debit),
                    Credit = EntryMovements.Sum(s => s.Credit),
                }
            });
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
                    return Ok(collection.Id);

                }
                catch
                {
                    //Console.WriteLine(collection);
                    return Ok(false);
                }
            }
            else return Ok(false);
        }
        [Route("EntryAccounting/Edit")]
        [HttpPost]
        public IActionResult Edit(EntryAccounting collection)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    EntryAccounting Entry = DB.EntryAccountings.Where(x => x.Id == collection.Id).SingleOrDefault();

                    Entry.FakeDate = collection.FakeDate;
                    Entry.Description = collection.Description;
                    Entry.Status = collection.Status;
                    Entry.Type = collection.Type;
                    DB.EntryMovements.RemoveRange(DB.EntryMovements.Where(x=>x.EntryId == Entry.Id).ToList());
                    Entry.EntryMovements = collection.EntryMovements;
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
        [Route("EntryAccounting/GetEntryById")]
        [HttpGet]
        public IActionResult GetEntryById(long? Id)
        {
            var Entrys = DB.EntryAccountings.Where(x => x.Id == Id).Select(x => new {
                x.Id,
                x.FakeDate,
                x.Status,
                x.Description,
                x.Type,
                EntryMovements = DB.EntryMovements.Where(Im => Im.EntryId == x.Id).Select(m => new {
                    m.Id,
                 m.Debit,
                 m.Credit,
                 m.EntryId,
                 m.AccountId,
                    m.Description

                }).ToList()
            }).SingleOrDefault();

            return Ok(Entrys);
        }


    }
}
