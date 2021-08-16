﻿using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Entities; 
using Microsoft.AspNetCore.Mvc;
using NinjaNye.SearchExtensions;

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
                    Name = x.Name + "-" + x.Vendors.Where(v => v.AccountId == x.Id).SingleOrDefault().Name + "-" + x.Members.Where(v => v.AccountId == x.Id).SingleOrDefault().Name,
                    x.ParentId,
                    TotalDebit = DB.EntryMovements.Where(l => l.AccountId == x.Id).Select(d => d.Debit).Sum(),
                    TotalCredit = DB.EntryMovements.Where(l => l.AccountId == x.Id).Select(c => c.Credit).Sum(),
                    x.Type,
                }).ToList()

            };
            return Ok(Accounts);

        }
        [Route("Account/GetTreeAccount")]
        [HttpGet]
        public IActionResult GetTreeAccount()
        {

            var Accounts = DB.Accounts.Select(x => new
            {
                x.Id,
                x.Description,
                x.Status,
                x.Code,
                Name = x.Name + "-" + x.Vendors.Where(v => v.AccountId == x.Id).SingleOrDefault().Name + "-" + x.Members.Where(v => v.AccountId == x.Id).SingleOrDefault().Name,
                x.ParentId,
                TotalDebit = DB.EntryMovements.Where(l => l.AccountId == x.Id).Select(d => d.Debit).Sum(),
                TotalCredit = DB.EntryMovements.Where(l => l.AccountId == x.Id).Select(c => c.Credit).Sum(),
                x.Type,
                children = new { }
            }).ToList();
            
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

        [HttpGet]
        [Route("Account/GetAccountByAny")]
        public IActionResult GetAccountByAny(string Any)
        {
            if (Any == null) return NotFound();
            Any = Any.ToLower();
            var Accounts = DB.Accounts.Where(a=> a.Type != "Main").Search(x => x.Name, x => x.Code, x => x.Id.ToString(), x => x.Type , x =>x.Vendors.Where(v=>v.AccountId==x.Id).SingleOrDefault().Name , x => x.Members.Where(v => v.AccountId == x.Id).SingleOrDefault().Name).Containing(Any)
                .Select(x => new { 
                    x.Id,
                    Name = x.Name + "-" + x.Vendors.Where(v => v.AccountId == x.Id).SingleOrDefault().Name + "-" + x.Members.Where(v => v.AccountId == x.Id).SingleOrDefault().Name,
                    x.Code,
                    x.Type 
                }).ToList();

            return Ok(Accounts);
           
        }
        [HttpPost]
        [Route("Account/GetByListQ")]
        public IActionResult GetByListQ(int Limit, string Sort, int Page, int? Status, string Any)
        {
            var Accounts = DB.Accounts.Where(s => (Any != null ? s.Id.ToString().Contains(Any) || s.Name.Contains(Any) : true) && (Status != null ? s.Status == Status : true)).Select(x => new
            {
                x.Id,
                Name = x.Name + "-" + x.Vendors.Where(v => v.AccountId == x.Id).SingleOrDefault().Name + "-" + x.Members.Where(v => v.AccountId == x.Id).SingleOrDefault().Name,
                x.Code,
                x.Status,
                x.Type,
                x.Description,
                TotalDebit = x.EntryMovements.Select(d => d.Debit).Sum(),
                TotalCredit = x.EntryMovements.Select(c => c.Credit).Sum(),
            }).ToList();
            Accounts = (Sort == "+id" ? Accounts.OrderBy(s => s.Id).ToList() : Accounts.OrderByDescending(s => s.Id).ToList());
            return Ok(new
            {
                items = Accounts.Skip((Page - 1) * Limit).Take(Limit).ToList(),
                Totals = new
                {
                    Rows = Accounts.Count(),
                    Totals = Accounts.Sum(s => s.TotalCredit - s.TotalDebit),
                    TotalCredit = Accounts.Sum(s => s.TotalCredit),
                    TotalDebit = Accounts.Sum(s => s.TotalDebit),
                }
            });
        }
        
       [Route("Account/GetInComeAccounts")]
        [HttpGet]
        public IActionResult GetInComeAccounts()
        {
            var InComeAccounts = DB.Accounts.Where(i =>  i.Type == "InCome").Select(x => new
            {
                value = x.Id,
                label = x.Name
            }).ToList();
                                 
            return Ok(InComeAccounts);
        }
        [Route("Account/GetMainAccount")]
        [HttpGet]
        public IActionResult GetMainAccount()
        {
            var InComeAccounts = DB.Accounts.Where(i =>  i.Type == "Main").Select(x => new
            {
                value = x.Id,
                label = x.Name
            }).ToList();

            return Ok(InComeAccounts);
        }
        [Route("Account/GetById")]
        [HttpGet]
        public IActionResult GetById(long Id)
        {
            var Account = DB.Accounts.Where(i => i.Id == Id ).Select(x => new
            {
                x.Id,
                Name = x.Name + "-" + x.Vendors.Where(v => v.AccountId == x.Id).SingleOrDefault().Name + "-" + x.Members.Where(v => v.AccountId == x.Id).SingleOrDefault().Name,
                x.Code,
                x.Status,
                x.Type,
                x.Description,
                x.ParentId,
            }).SingleOrDefault();

            return Ok(Account);
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
                try
                {
                    Account account = DB.Accounts.Where(x => x.Id == collection.Id).SingleOrDefault();
                    account.Name = collection.Name;
                    account.Code = collection.Code;
                    account.Type = collection.Type;
                    account.Description = collection.Description;
                    account.ParentId = collection.ParentId;
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
