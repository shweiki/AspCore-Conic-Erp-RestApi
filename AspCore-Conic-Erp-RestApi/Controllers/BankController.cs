using System.Data;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Entities; 

namespace AspCore_Conic_Erp_RestApi.Controllers
{
    [Authorize]
    public class BankController : Controller
    {
        private ConicErpContext DB = new ConicErpContext();

        // GET: Banks
        [Route("Banks/GetBanks")]
        [HttpGet]
        public IActionResult GetBanks()
        {
            var Banks = DB.Banks.Select(x => new {
                x.Id,
                x.Iban,
                x.AccountId,
                x.Name,
                x.Description,
                x.AccountNumber,
                x.AccountType,
                x.BranchName,
                x.Currency
            }).ToList();

            return Ok(Banks);
        }

        // POST: Banks
        [Route("Banks/Create")]
        [HttpPost]

        public IActionResult Create(Bank collection)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // TODO: Add insert logic here
                    DB.Banks.Add(collection);
                    DB.SaveChanges();

                }
                catch
                {
                    //Console.WriteLine(collection);
                    return Ok(false);
                }
            }
            return Ok(false);
        }

        // DELETE: Banks/5
        [Route("Banks/Edit")]
        [HttpPost]
        public IActionResult Edit(Bank collection)
        {
            if (ModelState.IsValid)
            {
                Bank bank = DB.Banks.Where(x => x.Id == collection.Id).SingleOrDefault();
                bank.Name = collection.Name;
                bank.Description = collection.Description;
                bank.Iban = collection.Iban;
                bank.AccountType = collection.AccountType;
                bank.BranchName = collection.BranchName;
                bank.AccountNumber = collection.AccountNumber;
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