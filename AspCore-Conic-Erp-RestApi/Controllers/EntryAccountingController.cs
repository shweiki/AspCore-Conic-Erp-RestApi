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
        public IActionResult GetEntryAccounting(long AccountID, DateTime DateFrom, DateTime DateTo)
        {
            var EntryMovements = (from x in DB.EntryMovements.Where(i => i.AccountId == AccountID).ToList()
                            where  (x.Entry.FakeDate >= DateFrom) && (x.Entry.FakeDate <= DateTo)
                            let p = new
                            {
                                x.Id,
                                x.Debit,
                                x.Credit,
                                x.Description,
                                x.EntryId,
                                FakeDate = x.Entry.FakeDate.ToString("dd/MM/yyyy"),
                         
                            }
                                  select p);

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
                    Oprationsy Opx = DB.Oprationsys.Where(d => d.Status == collection.Status && d.TableName == "EntryAccounting").SingleOrDefault();
                    OprationsysController Op = new OprationsysController();
                    if (Op.ChangeStatus(collection.Id, Opx.Id, "<!" + collection.Id + "!>"))
                    {
                        return Ok(true);
                    }
                    else return Ok(false);
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
