using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Entities; 
using Microsoft.AspNetCore.Mvc;

namespace AspCore_Conic_Erp_RestApi.Controllers
{
    [Authorize]
    public class EntryMovementController : Controller
    {
        private ConicErpContext DB = new ConicErpContext();
        [Route("EntryMovement/Create")]
        [HttpPost]
        public IActionResult Create(IList<EntryMovement> EntryMoves)
        {
            try
            {
                var EntryMovements = from field in EntryMoves
                           select new EntryMovement
                           {
                               AccountId = field.AccountId,
                               Description = field.Description,
                               Debit = field.Debit,
                               Credit = field.Credit,
                               EntryId = field.EntryId,
                           };
                DB.EntryMovements.AddRange(EntryMovements);
                DB.SaveChanges();
                    return Ok(true);
            }
            catch
            {
                //Console.WriteLine(collection);
                return Ok(false);
            }

        }
        [Route("EntryMovement/GetEntryMovementsByAccountID")]
        [HttpGet]
        public IActionResult GetEntryMovementsByAccountID(long? AccountID)
        {
            var EntryMovements = (from D in DB.EntryMovements.Where(l => l.AccountId == AccountID).ToList()
                                  let e = new
                                  {
                                      D.Id,
                                      D.Credit,
                                      D.Debit,
                                      D.EntryId,
                                      D.Entry.Type,
                                      D.Entry.FakeDate,
                                      D.Description,
                                  }
                                  select e);
     

            return Ok(EntryMovements);
        }
    }
}