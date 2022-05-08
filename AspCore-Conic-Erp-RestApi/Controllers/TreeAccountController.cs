using System.Collections.Generic;
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
                private ConicErpContext DB;
        public TreeAccountController(ConicErpContext dbcontext)
        {
            DB = dbcontext;
        }
        [Route("Account/GetAccount")]
        [HttpGet]
        public IActionResult GetAccount()
        {
            var Accounts = new
            {
                Accounts =  DB.TreeAccounts.Where(i=>i.Status == 0).Select(x=>new
                {
                    x.Id,
                    x.Description,
                    x.Status,
                    x.Code,
                    Name = x.Vendors.Where(v => v.AccountId == x.Id).SingleOrDefault().Name == null ? x.Members.Where(v => v.AccountId == x.Id).SingleOrDefault().Name == null ? x.Name : x.Members.Where(v => v.AccountId == x.Id).SingleOrDefault().Name : x.Vendors.Where(v => v.AccountId == x.Id).SingleOrDefault().Name,
                    x.ParentId,
                    TotalDebit = DB.EntryMovements.Where(l => l.AccountId == x.Id).Select(d => d.Debit).Sum(),
                    TotalCredit = DB.EntryMovements.Where(l => l.AccountId == x.Id).Select(c => c.Credit).Sum(),
                    x.Type,
                }).ToList()

            };
            return Ok(Accounts);

        }
        [HttpGet]

        [Route("Account/CheckIsExist")]
        public IActionResult CheckIsExist(string Name, string Type)
        {
            var Account = DB.TreeAccounts.Where(m => m.Name == Name && m.Type == Type).ToList();

            return Ok(Account.Count() > 0 ? true : false);
        }

        [Route("Account/GetTreeAccount")]
        [HttpGet]
        public IActionResult GetTreeAccount()
        {

            var Accounts = DB.TreeAccounts.Select(x => new
            {
                x.Id,
                x.Description,
                x.Status,
                Code = x.Code ==  null ? "" : x.Code, 
                Name = x.Vendors.Where(v => v.AccountId == x.Id).SingleOrDefault().Name == null ? x.Members.Where(v => v.AccountId == x.Id).SingleOrDefault().Name == null ? x.Name : x.Members.Where(v => v.AccountId == x.Id).SingleOrDefault().Name : x.Vendors.Where(v => v.AccountId == x.Id).SingleOrDefault().Name ,
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
            var ActiveAccounts = DB.TreeAccounts.Where(x => x.Status == 0).Select(x => new { value = x.Id ,
                label = (x.Members.Where(m=>m.AccountId == x.Id).FirstOrDefault().Name !=null ? x.Members.Where(m => m.AccountId == x.Id).FirstOrDefault().Name : x.Vendors.Where(m => m.AccountId == x.Id).FirstOrDefault().Name )+ " - " + x.Name,
              }).ToList();
            return Ok(ActiveAccounts);
        }
        [HttpPost]
        [Route("Account/EditParent")]
        public IActionResult EditParent(long ID, string ParentId)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    TreeAccount account = DB.TreeAccounts.Where(x => x.Id == ID).SingleOrDefault();
   
                    account.ParentId = ParentId;
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
        [HttpGet]
        [Route("Account/GetAccountByAny")]
        public IActionResult GetAccountByAny(string Any)
        {
            if (Any == null) return NotFound();
            Any = Any.ToLower();
            var Accounts = DB.TreeAccounts.Where(a=> !a.Type.Contains("Main")).Search(x => x.Name, x => x.Code, x => x.Id.ToString(), x => x.Type , x =>x.Vendors.Where(v=>v.AccountId==x.Id).SingleOrDefault().Name , x => x.Members.Where(v => v.AccountId == x.Id).SingleOrDefault().Name).Containing(Any)
                .Select(x => new { 
                    x.Id,
                    Name = x.Vendors.Where(v => v.AccountId == x.Id).SingleOrDefault().Name == null ? x.Members.Where(v => v.AccountId == x.Id).SingleOrDefault().Name == null ? x.Name : x.Members.Where(v => v.AccountId == x.Id).SingleOrDefault().Name : x.Vendors.Where(v => v.AccountId == x.Id).SingleOrDefault().Name,
                    x.Code,
                    x.Type 
                }).ToList();

            return Ok(Accounts);
           
        }
        [HttpPost]
        [Route("Account/GetByListQ")]
        public IActionResult GetByListQ(int Limit, string Sort, int Page, int? Status, string Any)
        {
            var Accounts = DB.TreeAccounts.Where(s => (Any != null ? s.Id.ToString().Contains(Any) || s.Name.Contains(Any) : true) && (Status != null ? s.Status == Status : true)).Select(x => new
            {
                x.Id,
                Name = x.Vendors.Where(v => v.AccountId == x.Id).SingleOrDefault().Name == null ? x.Members.Where(v => v.AccountId == x.Id).SingleOrDefault().Name == null ? x.Name : x.Members.Where(v => v.AccountId == x.Id).SingleOrDefault().Name : x.Vendors.Where(v => v.AccountId == x.Id).SingleOrDefault().Name,
                x.Code,
                x.Status,
                x.Type,
                x.Description,
                x.ParentId,
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

        [HttpGet]
        [Route("Account/GetInComeAccounts")]
        public IActionResult GetInComeAccounts()
        {
            var InComeAccounts = DB.TreeAccounts.Where(i =>  i.Type == "InCome").Select(x => new
            {
                value = x.Id,
                Code = x.Code,
                label = x.Name
            }).ToList();
                                 
            return Ok(InComeAccounts);
        }

        [HttpGet]
        [Route("Account/GetMainAccount")]
        public IActionResult GetMainAccount()
        {
            var InComeAccounts = DB.TreeAccounts.Where(i =>  i.Type.Contains("Main")).Select(x => new
            {
                value = x.Id,
                Code = x.Code,
                label = x.Name
            }).ToList();

            return Ok(InComeAccounts);
        }
        [HttpGet]
        [Route("Account/GetPayables")]
        public IActionResult GetPayables(int Limit, string Sort, int Page,bool WithZero ,string Any)
        {
            var Accounts = DB.TreeAccounts.Where(f => (Any != null ? f.Id.ToString().Contains(Any):true)&&(f.EntryMovements.Select(d => d.Credit).Sum() - f.EntryMovements.Select(d => d.Debit).Sum()) < 0).Select(x => new
            {
                x.Id,
                x.Description,
                x.Status,
                x.Code,
                Name = x.Vendors.Where(v => v.AccountId == x.Id).SingleOrDefault().Name == null ? x.Members.Where(v => v.AccountId == x.Id).SingleOrDefault().Name == null ? x.Name : x.Members.Where(v => v.AccountId == x.Id).SingleOrDefault().Name : x.Vendors.Where(v => v.AccountId == x.Id).SingleOrDefault().Name,
                x.ParentId,
                TotalDebit = DB.EntryMovements.Where(l => l.AccountId == x.Id).Select(d => d.Debit).Sum(),
                TotalCredit = DB.EntryMovements.Where(l => l.AccountId == x.Id).Select(c => c.Credit).Sum(),
                x.Type,
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
        [HttpGet]
        [Route("Account/GetReceivables")]
        public IActionResult GetReceivables(int Limit, string Sort, int Page, bool WithZero, string Any)
        {
            var Accounts = DB.TreeAccounts.Where(f => (Any != null ? f.Id.ToString().Contains(Any) : true) && (f.EntryMovements.Select(d => d.Credit).Sum() - f.EntryMovements.Select(d => d.Debit).Sum()) > 0).Select(x => new
            {
                x.Id,
                x.Description,
                x.Status,
                x.Code,
                Name = x.Vendors.Where(v => v.AccountId == x.Id).SingleOrDefault().Name == null ? x.Members.Where(v => v.AccountId == x.Id).SingleOrDefault().Name == null ? x.Name : x.Members.Where(v => v.AccountId == x.Id).SingleOrDefault().Name : x.Vendors.Where(v => v.AccountId == x.Id).SingleOrDefault().Name,
                x.ParentId,
                TotalDebit = DB.EntryMovements.Where(l => l.AccountId == x.Id).Select(d => d.Debit).Sum(),
                TotalCredit = DB.EntryMovements.Where(l => l.AccountId == x.Id).Select(c => c.Credit).Sum(),
                x.Type,
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
        [HttpGet]
        [Route("Account/GetById")]
        public IActionResult GetById(long Id)
        {
            var Account = DB.TreeAccounts.Where(i => i.Id == Id ).Select(x => new
            {
                x.Id,
                Name = x.Vendors.Where(v => v.AccountId == x.Id).SingleOrDefault().Name == null ? x.Members.Where(v => v.AccountId == x.Id).SingleOrDefault().Name == null ? x.Name : x.Members.Where(v => v.AccountId == x.Id).SingleOrDefault().Name : x.Vendors.Where(v => v.AccountId == x.Id).SingleOrDefault().Name,
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
        public IActionResult Create(TreeAccount collection)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // TODO: Add insert logic here
                    DB.TreeAccounts.Add(collection);
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
        [HttpPost]
        [Route("Account/Edit")]
        public IActionResult Edit(TreeAccount collection)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    TreeAccount account = DB.TreeAccounts.Where(x => x.Id == collection.Id).SingleOrDefault();
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

        [HttpPost]
        [Route("Account/Delete")]
        public IActionResult Delete(long? Id)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    TreeAccount collection = DB.TreeAccounts.Where(x => x.Id == Id).SingleOrDefault();
                    if (DB.EntryMovements.Where(x => x.AccountId == collection.Id).SingleOrDefault() != null)
                        return Ok(false);

                    else
                    {
                        DB.TreeAccounts.Remove(collection);
                        DB.SaveChanges();
                        return Ok(true);
                    }
              
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
