using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.AspNetCore.Authorization;
using Entities;
using System.Collections.Generic;

namespace AspCore_Conic_Erp_RestApi.Controllers
{
    [Authorize]
    public class EntryAccountingController : Controller
    {
        private ConicErpContext DB;
        public EntryAccountingController(ConicErpContext dbcontext)
        {
            DB = dbcontext;
        }        [HttpGet]
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
        public IActionResult GetByListQ( int Limit, string Sort, int Page, string User, DateTime? DateFrom, DateTime? DateTo, int? Status, string Any)
        {
            var EntryAccountings = DB.EntryAccountings.Where(s => (Any != null ? s.Id.ToString().Contains(Any) || s.Description.Contains(Any) : true) && (DateFrom != null ? s.FakeDate >= DateFrom : true)
            && (DateTo != null ? s.FakeDate <= DateTo : true) && (Status != null ? s.Status == Status : true) &&
            (User != null ? DB.ActionLogs.Where(l => l.EntryId == s.Id && l.UserId == User).SingleOrDefault() != null : true)).Select(x => new
            {
                x.Id,
                EntryMovements =x.EntryMovements.Select(e=> new{
                            e.Id,
                            e.Fktable,
                            e.TableName,
                            e.AccountId,
                            Name = e.Account.Vendors.Where(v => v.AccountId == e.AccountId).SingleOrDefault().Name == null ? e.Account.Members.Where(v => v.AccountId == e.AccountId).SingleOrDefault().Name == null ? e.Account.Name : e.Account.Members.Where(v => v.AccountId == e.AccountId).SingleOrDefault().Name : e.Account.Vendors.Where(v => v.AccountId == e.AccountId).SingleOrDefault().Name,
                            e.Credit,
                            e.Debit
                          }),
                x.Description,
                x.FakeDate,
                x.Status,
                x.Type,
            }).ToList();
            EntryAccountings = (Sort == "+id" ? EntryAccountings.OrderBy(s => s.Id).ToList() : EntryAccountings.OrderByDescending(s => s.Id).ToList());
            return Ok(new
            {
                items = EntryAccountings.Skip((Page - 1) * Limit).Take(Limit).ToList(),
                Totals = new
                {
                    Rows = EntryAccountings.Count(),
                    Totals = EntryAccountings.Sum(s => s.EntryMovements.Sum(s => s.Credit)) -EntryAccountings.Sum(s => s.EntryMovements.Sum(s => s.Debit)),
                    Debit = EntryAccountings.Sum(s=> s.EntryMovements.Sum(s => s.Debit)),
                    Credit = EntryAccountings.Sum(s => s.EntryMovements.Sum(s => s.Credit))
                }
            });
        }
        [HttpPost]
        [Route("EntryAccounting/GetAccountStatement")]
        public IActionResult GetAccountStatement(long? AccountId, long? MergeAccountId,  DateTime DateFrom, DateTime DateTo)
        {
            var EntryMovements = DB.EntryMovements.Where(s =>  (MergeAccountId != null ? s.AccountId == AccountId || s.AccountId == MergeAccountId : s.AccountId == AccountId) && (s.Entry.FakeDate >= DateFrom && s.Entry.FakeDate <= DateTo))
                .Select(x => new { x, x.Entry }).AsEnumerable()
                        .Select(x => new
                          {
                            x.x.Id,
                            x.x.Debit,
                            x.x.Credit,
                            x.x.Description,
                            x.x.EntryId,
                            x.x.Fktable,
                            x.x.TableName,
                            TotalRow = x.x.Credit - x.x.Debit,
                            x.Entry.FakeDate,
                            x.Entry.Status,
                            x.Entry.Type,
                            FkDescription = x.x.TableName !=null? GetFkDescription(x.x.TableName , x.x.Fktable) :""
                            }).ToList();
            double AllTotal = DB.EntryMovements.Where(s => (MergeAccountId != null ? s.AccountId == AccountId || s.AccountId == MergeAccountId : s.AccountId == AccountId)).Sum(s => s.Credit) - DB.EntryMovements.Where(s => (MergeAccountId != null ? s.AccountId == AccountId || s.AccountId == MergeAccountId : s.AccountId == AccountId)).Sum(s => s.Debit);
            if (AllTotal != (EntryMovements.Sum(s => s.Credit) - EntryMovements.Sum(s => s.Debit))) {
                double Balancecarried = AllTotal - (EntryMovements.Sum(s => s.Credit) - EntryMovements.Sum(s => s.Debit)) ;
                EntryMovements.Add(new 
                {
                    Id = Convert.ToInt64(0),
                    Debit = Balancecarried < 0 ? Balancecarried : 0,
                    Credit = Balancecarried > 0 ? Balancecarried : 0,
                    Description = "رصيد الفترة السابقة",
                    EntryId = Convert.ToInt64(0),
                    Fktable = Convert.ToInt64(0),
                    TableName = "BalanceCarried",
                    TotalRow = Balancecarried,
                    FakeDate = DateFrom.Date,
                    Status = 0,
                    Type = "رصيد الفترة السابقة",
                    FkDescription = ""
                }) ;
            }
            return Ok(new
            {
                items = EntryMovements.OrderBy(s => s.FakeDate).ToList(),
                Totals = new
                {
                    Rows = EntryMovements.Count(),
                    Totals = AllTotal,
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
        [Route("EntryAccounting/EditEntryByFktable")]
        [HttpPost]
        public IActionResult EditEntryByFktable(string TableName ,long Fktable , EntryAccounting collection)
        {
            if (ModelState.IsValid)
            {
                try
                {
                   var EntryMovements = DB.EntryMovements.Where(e => e.Fktable == Fktable && e.TableName == TableName).FirstOrDefault();
                    if (EntryMovements == null)
                    {
                        DB.EntryAccountings.Add(collection);
                    }
                    else {
                        var Entry = DB.EntryAccountings.Where(e => e.Id == EntryMovements.EntryId).FirstOrDefault();
                        Entry.FakeDate = collection.FakeDate;
                        Entry.Description = collection.Description;
                        Entry.Status = collection.Status;
                        Entry.Type = collection.Type;
                        DB.EntryMovements.RemoveRange(DB.EntryMovements.Where(x => x.EntryId == Entry.Id).ToList());
                        Entry.EntryMovements = collection.EntryMovements;
                    }
                 
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
                    Name = m.Account.Vendors.Where(v => v.AccountId == x.Id).SingleOrDefault().Name == null ? m.Account.Members.Where(v => v.AccountId == x.Id).SingleOrDefault().Name == null ? m.Account.Name : m.Account.Members.Where(v => v.AccountId == x.Id).SingleOrDefault().Name : m.Account.Vendors.Where(v => v.AccountId == x.Id).SingleOrDefault().Name,
                    m.Description
                    }).ToList()
            }).SingleOrDefault();

            return Ok(Entrys);
        }
        public string GetFkDescription(string TableName, long Fktable)
        {
        ConicErpContext DBx = new ConicErpContext();
        var Object = "";
            switch (TableName)
            {
                case "CashPool":
                    Object = DBx.CashPools.Where(x => x.Id == Fktable).SingleOrDefault().Description;
                    break;
                case "SaleInvoice":
                    Object= DBx.SalesInvoices.Where(x => x.Id == Fktable).SingleOrDefault().Description;
                    break;
                case "PurchaseInvoice":
                    Object= DBx.PurchaseInvoices.Where(x => x.Id == Fktable).SingleOrDefault().Description;
                    break;
                case "EntryAccounting":
                    Object= DBx.EntryAccountings.Where(x => x.Id == Fktable).SingleOrDefault().Description;
                    break;
                case "Payment":
                    Object = DBx.Payments.Where(x => x.Id == Fktable).SingleOrDefault().Description;
                    break;
                case "Receive":
                    Object = DBx.Receives.Where(x => x.Id == Fktable).SingleOrDefault().Description;
                    break;
                case "MembershipMovement":
                    Object = DBx.MembershipMovements.Where(x => x.Id == Fktable).SingleOrDefault().Description;
                    break;
                case "Cheque":
                    Object = DBx.Cheques.Where(x => x.Id == Fktable).SingleOrDefault().Description;
                    break;
            }
            return Object;
        }

    }
}
