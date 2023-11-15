using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace RestApi.Controllers;

[Authorize]
public class CashPoolController : Controller
{
    private readonly IApplicationDbContext DB;
    public CashPoolController(IApplicationDbContext dbcontext)
    {
        DB = dbcontext;
    }
    [HttpPost]
    [Route("CashPool/GetByListQ")]
    public IActionResult GetByListQ(int Limit, string Sort, int Page, string User, DateTime? DateFrom, DateTime? DateTo, int? Status, string Any, string Type)
    {
        var List = DB.CashPool.Where(s => (Any != null ? s.Id.ToString().Contains(Any) || s.Description.Contains(Any) : true) && (DateFrom != null ? s.DateTime >= DateFrom : true)
        && (DateTo != null ? s.DateTime <= DateTo : true) && (Status != null ? s.Status == Status : true) && (Type != null ? s.Type == Type : true) && (User != null ? DB.ActionLog.Where(l => l.TableName == "CashPool" && l.Fktable == s.Id.ToString() && l.UserId == User).SingleOrDefault() != null : true)).Select(x => new
        {
            x.Id,
            x.Type,
            x.TotalCash,
            x.TotalCoins,
            x.TotalVisa,
            x.TotalReject,
            x.TotalOutlay,
            x.TotalRestitution,
            x.DateTime,
            x.Status,
            x.Description,
            x.Created,
            x.CreatedBy,
            x.TableName,
            x.Fktable,
            Totals = 0
        }).ToList();
        List = (Sort == "+id" ? List.OrderBy(s => s.Id).ToList() : List.OrderByDescending(s => s.Id).ToList());
        return Ok(new
        {
            items = List.Skip((Page - 1) * Limit).Take(Limit).ToList(),
            Totals = new
            {
                Rows = List.Count(),
                Cash = List.Sum(s => s.TotalCash),
                Coins = List.Sum(s => s.TotalCoins),
                Visa = List.Sum(s => s.TotalVisa),
                Reject = List.Sum(s => s.TotalReject),
                Outlay = List.Sum(s => s.TotalOutlay),
                Restitution = List.Sum(s => s.TotalRestitution),
            }
        });
    }

    [HttpPost]
    [Route("CashPool/Create")]

    public IActionResult Create(CashPool collection)
    {
        if (ModelState.IsValid)
        {
            try
            {
                // TODO: Add insert logic here
                collection.DateTime = DateTime.Now;
                DB.CashPool.Add(collection);
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
    [Route("CashPool/Edit")]
    [HttpPost]
    public IActionResult Edit(CashPool collection)
    {
        if (ModelState.IsValid)
        {
            try
            {
                CashPool item = DB.CashPool.Where(x => x.Id == collection.Id).SingleOrDefault();

                item.Type = collection.Type;
                item.TotalCash = collection.TotalCash;
                item.TotalCoins = collection.TotalCoins;
                item.TotalVisa = collection.TotalVisa;
                item.TotalReject = collection.TotalReject;
                item.TotalOutlay = collection.TotalOutlay;
                item.TotalRestitution = collection.TotalRestitution;
                item.DateTime = collection.DateTime;
                item.Status = collection.Status;
                item.Description = collection.Description;
                item.TableName = collection.TableName;
                item.Fktable = collection.Fktable;

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
    [Route("CashPool/GetCashPoolById")]
    [HttpGet]
    public IActionResult GetCashPoolById(long? Id)
    {
        var List = DB.CashPool.Where(x => x.Id == Id).Select(x => new
        {
            x.Id,
            x.Type,
            x.TotalCash,
            x.TotalCoins,
            x.TotalVisa,
            x.TotalReject,
            x.TotalOutlay,
            x.TotalRestitution,
            x.DateTime,
            x.Status,
            x.Description,
            x.Created,
            x.CreatedBy,
            x.TableName,
            x.Fktable,
        }).SingleOrDefault();

        return Ok(List);
    }


}
