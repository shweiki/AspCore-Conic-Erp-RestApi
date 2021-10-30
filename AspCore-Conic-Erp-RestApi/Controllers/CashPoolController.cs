using System;
using System.Data;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Entities; 
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace AspCore_Conic_Erp_RestApi.Controllers
{
    [Authorize]
    public class CashPoolController : Controller
    {
        private ConicErpContext DB;
        public CashPoolController(ConicErpContext dbcontext)
        {
            DB = dbcontext;
        }        [HttpPost]
        [Route("CashPool/GetByListQ")]
        public IActionResult GetByListQ(int Limit, string Sort, int Page, string User, DateTime? DateFrom, DateTime? DateTo, int? Status, string Any ,string Type)
        {
            var List = DB.CashPools.Where(s => (Any != null ? s.Id.ToString().Contains(Any) || s.Description.Contains(Any)  : true ) && (DateFrom != null ? s.DateTime >= DateFrom : true)
            && (DateTo != null ? s.DateTime <= DateTo : true) && (Status != null ? s.Status == Status : true)&&(Type != null ? s.Type == Type : true) && (User != null ? DB.ActionLogs.Where(l =>l.SalesInvoiceId == s.Id && l.UserId == User).SingleOrDefault() != null : true)).Select(x => new
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
                x.EditorName,
                x.TableName,
                x.Fktable,
                Totals = 0
            }).ToList();
            List =  (Sort == "+id" ? List.OrderBy(s => s.Id).ToList() : List.OrderByDescending(s => s.Id).ToList());
            return Ok(new {items = List.Skip((Page - 1) * Limit).Take(Limit).ToList(), 
            Totals = new {
                Rows = List.Count(),
                Cash = List.Sum(s => s.TotalCash),
                Coins = List.Sum(s => s.TotalCoins),
                Visa = List.Sum(s => s.TotalVisa),
                Reject = List.Sum(s => s.TotalReject),
                Outlay = List.Sum(s => s.TotalOutlay),
                Restitution = List.Sum(s => s.TotalRestitution),
            } });
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
                    DB.CashPools.Add(collection);
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
                    CashPool Invoice = DB.CashPools.Where(x => x.Id == collection.Id).SingleOrDefault();

                    Invoice.Type = collection.Type;
                    Invoice.TotalCash = collection.TotalCash;
                    Invoice.TotalCoins = collection.TotalCoins;
                    Invoice.TotalVisa = collection.TotalVisa;
                    Invoice.TotalReject = collection.TotalReject;
                    Invoice.TotalOutlay = collection.TotalOutlay;
                    Invoice.TotalRestitution = collection.TotalRestitution;
                    Invoice.DateTime = collection.DateTime;
                    Invoice.Status = collection.Status;
                    Invoice.Description = collection.Description;
                    Invoice.EditorName = collection.EditorName;
                    Invoice.TableName = collection.TableName;
                    Invoice.Fktable = collection.Fktable;
                 
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
            var List = DB.CashPools.Where(x => x.Id == Id).Select(x => new {
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
                x.EditorName,
                x.TableName,
                x.Fktable,
            }).SingleOrDefault();

            return Ok(List);
        }
  

    }
}
