﻿
using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace RestApi.Controllers;

[Authorize]
public class EntryAccountingController : Controller
{
    private readonly IApplicationDbContext DB;
    public EntryAccountingController(IApplicationDbContext dbcontext)
    {
        DB = dbcontext;
    }
    [HttpGet]
    public IActionResult GetEntryAccounting(long AccountId, DateTime DateFrom, DateTime DateTo)
    {
        var EntryMovements = DB.EntryMovement.Where(i => i.AccountId == AccountId && i.Entry.FakeDate >= DateFrom && i.Entry.FakeDate <= DateTo).Select(x => new
        {
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
    public IActionResult GetByListQ(int Limit, string Sort, int Page, string User, DateTime? DateFrom, DateTime? DateTo, int? Status, string Any)
    {
        var EntryAccountings = DB.EntryAccounting.Where(s => (Any == null || s.Id.ToString().Contains(Any) || s.Description.Contains(Any)) && (DateFrom == null || s.FakeDate >= DateFrom)
        && (DateTo == null || s.FakeDate <= DateTo) && (Status == null || s.Status == Status)
        //  && (User == null || DB.ActionLog.Where(l => l.TableName == "EntryAccounting" && l.Fktable == s.Id.ToString() && l.UserId == User).SingleOrDefault() != null)
        ).Select(x => new
        {
            x.Id,
            EntryMovements = x.EntryMovements.Select(e => new
            {
                e.Id,
                e.Fktable,
                e.TableName,
                e.AccountId,
                e.Account.Name,
                e.Credit,
                e.Debit,
                e.Description
            }),
            TotalCredit = x.EntryMovements.Sum(s => s.Credit),
            TotalDebit = x.EntryMovements.Sum(s => s.Debit),
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
                Totals = EntryAccountings.Sum(s => s.EntryMovements.Sum(s => s.Credit)) - EntryAccountings.Sum(s => s.EntryMovements.Sum(s => s.Debit)),
                Debit = EntryAccountings.Sum(s => s.EntryMovements.Sum(s => s.Debit)),
                Credit = EntryAccountings.Sum(s => s.EntryMovements.Sum(s => s.Credit))
            }
        });
    }
    [HttpPost]
    [Route("EntryAccounting/GetAccountStatement")]
    public IActionResult GetAccountStatement(long? AccountId, long? MergeAccountId, DateTime DateFrom, DateTime DateTo)
    {
        var EntryMovements = DB.EntryMovement.Where(s => (MergeAccountId != null ? s.AccountId == AccountId || s.AccountId == MergeAccountId : s.AccountId == AccountId) && (s.Entry.FakeDate >= DateFrom && s.Entry.FakeDate <= DateTo))
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
                        FkDescription = x.x.TableName != null ? GetFkDescription(x.x.TableName, x.x.Fktable) : ""
                    }).ToList();
        double AllTotal = DB.EntryMovement.Where(s => (MergeAccountId != null ? s.AccountId == AccountId || s.AccountId == MergeAccountId : s.AccountId == AccountId)).Sum(s => s.Credit) - DB.EntryMovement.Where(s => (MergeAccountId != null ? s.AccountId == AccountId || s.AccountId == MergeAccountId : s.AccountId == AccountId)).Sum(s => s.Debit);
        if (AllTotal != (EntryMovements.Sum(s => s.Credit) - EntryMovements.Sum(s => s.Debit)))
        {
            double Balancecarried = AllTotal - (EntryMovements.Sum(s => s.Credit) - EntryMovements.Sum(s => s.Debit));
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
            });
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
                DB.EntryAccounting.Add(collection);
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
                EntryAccounting Entry = DB.EntryAccounting.Where(x => x.Id == collection.Id).SingleOrDefault();

                Entry.FakeDate = collection.FakeDate;
                Entry.Description = collection.Description;
                Entry.Status = collection.Status;
                Entry.Type = collection.Type;
                DB.EntryMovement.RemoveRange(DB.EntryMovement.Where(x => x.EntryId == Entry.Id).ToList());
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
    public IActionResult EditEntryByFktable(string TableName, long Fktable, EntryAccounting collection)
    {
        if (ModelState.IsValid)
        {
            try
            {
                var EntryMovements = DB.EntryMovement.Where(e => e.Fktable == Fktable && e.TableName == TableName).FirstOrDefault();
                if (EntryMovements == null)
                {
                    DB.EntryAccounting.Add(collection);
                }
                else
                {
                    var Entry = DB.EntryAccounting.Where(e => e.Id == EntryMovements.EntryId).FirstOrDefault();
                    Entry.FakeDate = collection.FakeDate;
                    Entry.Description = collection.Description;
                    Entry.Status = collection.Status;
                    Entry.Type = collection.Type;
                    DB.EntryMovement.RemoveRange(DB.EntryMovement.Where(x => x.EntryId == Entry.Id).ToList());
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
    public async Task<IActionResult> GetEntryById(long? Id)
    {
        var item = await DB.EntryAccounting.Include(x => x.EntryMovements).SingleOrDefaultAsync(x => x.Id == Id);
        var result = new
        {
            item.Id,
            item.FakeDate,
            item.Status,
            item.Description,
            item.Type,
            EntryMovements = item.EntryMovements.Select(m => new
            {
                m.Id,
                m.Debit,
                m.Credit,
                m.EntryId,
                m.AccountId,
                m.Account.Name,
                m.Description
            }).ToList()
        };

        return Ok(result);
    }
    public string GetFkDescription(string TableName, long Fktable)
    {
        var Object = "";
        switch (TableName)
        {
            case "CashPool":
                Object = DB.CashPool.Where(x => x.Id == Fktable).SingleOrDefault().Description;
                break;
            case "SaleInvoice":
                Object = DB.SalesInvoice.Where(x => x.Id == Fktable).SingleOrDefault().Description;
                break;
            case "PurchaseInvoice":
                Object = DB.PurchaseInvoice.Where(x => x.Id == Fktable).SingleOrDefault().Description;
                break;
            case "EntryAccounting":
                Object = DB.EntryAccounting.Where(x => x.Id == Fktable).SingleOrDefault().Description;
                break;
            case "Payment":
                Object = DB.Payment.Where(x => x.Id == Fktable).SingleOrDefault().Description;
                break;
            case "Receive":
                Object = DB.Receive.Where(x => x.Id == Fktable).SingleOrDefault().Description;
                break;
            case "MembershipMovement":
                Object = DB.MembershipMovement.Where(x => x.Id == Fktable).SingleOrDefault().Description;
                break;
            case "Cheque":
                Object = DB.Cheque.Where(x => x.Id == Fktable).SingleOrDefault().Description;
                break;
        }
        return Object;
    }

}
