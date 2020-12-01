using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Entities; 
using Microsoft.AspNetCore.Mvc;

namespace AspCore_Conic_Erp_RestApi.Controllers
{
    [Authorize]
    public class TreeAccountController : Controller
    {
        private ConicErpContext DB = new ConicErpContext();
        [Route("Account/GetAccount")]
        [HttpGet]
        public IActionResult GetAccount()
        {
            var Accounts = new
            {
                Accounts =  DB.Accounts.Where(i=>i.Status == 0).Select(x=>new
                {
                    x.Id,
                    x.Description,
                    x.Status,
                    x.Code,
                    x.Name,
                    TotalDebit = DB.EntryMovements.Where(l => l.AccountId == x.Id).Select(d => d.Debit).Sum(),
                    TotalCredit = DB.EntryMovements.Where(l => l.AccountId == x.Id).Select(c => c.Credit).Sum(),
                    x.Type,
                }).ToList()

            };
            return Ok(Accounts);

        }

        [Route("Account/GetActiveAccounts")]
        [HttpGet]
        public IActionResult GetActiveAccounts()
        {
            var ActiveAccounts = DB.Accounts.Where(x => x.Status == 0).Select(x => new { value = x.Id ,
                label = (x.Members.Where(m=>m.AccountId == x.Id).FirstOrDefault().Name !=null ? x.Members.Where(m => m.AccountId == x.Id).FirstOrDefault().Name : x.Vendors.Where(m => m.AccountId == x.Id).FirstOrDefault().Name )+ " - " + x.Name,
              }).ToList();
            return Ok(ActiveAccounts);


        }
        [Route("Account/GetInComeAccounts")]
        [HttpGet]
        public IActionResult GetInComeAccounts()
        {
            var InComeAccounts = DB.Accounts.Where(i => i.Status == 0 && i.Type == "InCome").Select(x => new
            {
                value = x.Id,
                label = x.Name
            }).ToList();
                                 
            return Ok(InComeAccounts);
        }
        [Route("Account/Create")]
        [HttpPost]
        public IActionResult Create(Account collection)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // TODO: Add insert logic here
                    DB.Accounts.Add(collection);
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
        [Route("Account/Edit")]
        [HttpPost]
        public IActionResult Edit(Account collection)
        {
            if (ModelState.IsValid)
            {
                Account account = DB.Accounts.Where(x => x.Id == collection.Id).SingleOrDefault();
                account.Code = collection.Code;
                account.Type = collection.Type;
                account.Description = collection.Description;
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
            else return Ok(false);
        }


    }
}
