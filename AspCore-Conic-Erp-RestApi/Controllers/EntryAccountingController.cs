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
