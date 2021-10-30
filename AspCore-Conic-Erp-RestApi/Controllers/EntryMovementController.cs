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
        private ConicErpContext DB;
        public EntryMovementController(ConicErpContext dbcontext)
        {
            DB = dbcontext;
        }        [Route("EntryMovement/Create")]
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
        [Route("EntryMovement/GetEntryMovementsByAccountId")]
        [HttpGet]
        public IActionResult GetEntryMovementsByAccountId(long? AccountId)
        {
            var EntryMovements = DB.EntryMovements.Where(l => l.AccountId == AccountId).Select(x => new {
                x.Id,
                x.Credit,
                x.Debit,
                x.EntryId,
                x.Entry.Type,
                x.Entry.FakeDate,
                x.Description,
                TotalRow = 0,
            }).ToList();
 
     

            return Ok(EntryMovements);
        }
    }
}