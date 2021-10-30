using System;
using System.Data;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Entities; 
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using NinjaNye.SearchExtensions;

namespace AspCore_Conic_Erp_RestApi.Controllers
{
    [Authorize]
    public class VisitController : Controller
    {
                private ConicErpContext DB;
        public VisitController(ConicErpContext dbcontext)
        {
            DB = dbcontext;
        }
        [HttpPost]
        [Route("Visit/GetByListQ")]
        public IActionResult GetByListQ(int Limit, string Sort, int Page, string User, DateTime? DateFrom, DateTime? DateTo, int? Status, string Any ,string Type)
        {
            var visits = DB.Visits.Where(s => (Any != null ? s.Id.ToString().Contains(Any) || s.PaymentMethod.Contains(Any) || s.Name.Contains(Any)|| s.Description.Contains(Any)  || s.PhoneNumber.Contains(Any) || s.Name.Contains(Any) : true ) && (DateFrom != null ? s.FakeDate >= DateFrom : true)
            && (DateTo != null ? s.FakeDate <= DateTo : true) && (Status != null ? s.Status == Status : true)&&(Type != null ? s.Type == Type : true) && (User != null ? DB.ActionLogs.Where(l =>l.SalesInvoiceId == s.Id && l.UserId == User).SingleOrDefault() != null : true)).Select(x => new
            {
                x.Id,
                x.Name,
                x.Discount,
                x.Tax,
                x.FakeDate,
                x.PaymentMethod,
                x.PersonCount,
                x.HourCount,
                x.HourPrice,
                x.Status,
                x.Type,
                x.Description,
                x.PhoneNumber,
                Total = x.PersonCount * (x.HourCount * 2) * x.HourPrice - x.Discount
            }).ToList();
            return Ok(new
            {
                items = visits.ToList(),
                Totals = new
                {
                    Rows = visits.Count(),
                    Totals = visits.Sum(s => s.Total),
                    Cash = visits.Where(i => i.PaymentMethod == "Cash").Sum(s => s.Total),
                    Discount = visits.Sum(s => s.Discount),
                    Visa = visits.Where(i => i.PaymentMethod == "Visa").Sum(s => s.Total),
                    Coupon = visits.Where(i => i.PaymentMethod == "Coupon").Sum(s => s.Total)
                }
            });
        }
        [HttpPost]
        [Route("Visit/GetByAny")]
        public IActionResult GetByAny(string Any, int Status)
        {
            if (Any == null || Any == "") return Ok();
            var Visits = DB.Visits.Where(s => s.Status == Status &&  (s.Id.ToString().Contains(Any)  || s.Name.Contains(Any) || s.Description.Contains(Any) || s.PhoneNumber.Contains(Any) || s.Name.Contains(Any) )
            ).Select(x => new
                {
                x.Id,
                x.Name,
                x.Discount,
                x.Tax,
                x.FakeDate,
                x.PaymentMethod,
                x.PersonCount,
                x.HourCount,         
                x.HourPrice,
                x.Status,
                x.Type,
                x.Description,
                x.PhoneNumber,
                Total = x.PersonCount * (x.HourCount * 2) * x.HourPrice - x.Discount
            }).ToList();
            return Ok(Visits);
        }
       
        [Route("Visit/GetByStatus")]
        [HttpGet]
        public IActionResult GetByStatus(DateTime? DateFrom , DateTime? DateTo, int? Status)
        {
            var visits = DB.Visits.Where(s =>(DateFrom !=null ? s.FakeDate >= DateFrom : true ) 
            && (DateTo != null ?  s.FakeDate <= DateTo : true) && (Status != null ? s.Status == Status :true)).Select(x => new
            {
                x.Id,
                x.Name,
                x.Discount,
                x.Tax,
                x.FakeDate,
                x.PaymentMethod,
                x.PersonCount,
                x.HourCount,
                x.HourPrice,
                x.Status,
                x.Type,
                x.Description,
                x.PhoneNumber,
                TimeOut = x.FakeDate.AddMinutes(x.HourCount * 60),
                Total = x.PersonCount * (x.HourCount * 2) * x.HourPrice - x.Discount
            }).ToList();
            return Ok(new
            {
                items = visits.ToList(),
                Totals = new
                {
                    Rows = visits.Count(),
                    TotalPerson = visits.Sum(s => s.PersonCount),
                }
            });
        }
        [HttpPost]
        [Route("Visit/Create")]

        public IActionResult Create(Visit collection)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // TODO: Add insert logic here
                  // collection.FakeDate = collection.FakeDate.ToLocalTime();
                    DB.Visits.Add(collection);
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
        [Route("Visit/Edit")]
        [HttpPost]
        public IActionResult Edit(Visit collection)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Visit visit = DB.Visits.Where(x => x.Id == collection.Id).SingleOrDefault();

                    visit.Type = collection.Type;
                    visit.Tax = collection.Tax;
                    visit.Discount = collection.Discount;
                    visit.Description = collection.Description;
                    visit.Status = collection.Status;
                    visit.FakeDate = collection.FakeDate;
                    visit.PaymentMethod = collection.PaymentMethod;
                    visit.Name = collection.Name;
                    visit.PhoneNumber = collection.PhoneNumber;
                    visit.HourPrice = collection.HourPrice;
                    visit.HourCount = collection.HourCount;
                    visit.PersonCount = collection.PersonCount;
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
        [Route("Visit/EditPaymentMethod")]
        public IActionResult EditPaymentMethod(long ID , string PaymentMethod)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Visit visit = DB.Visits.Where(x => x.Id == ID).SingleOrDefault();
                    visit.PaymentMethod = PaymentMethod;
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
        [Route("Visit/GetByListId")]
        [HttpGet]
        public IActionResult GetByListId(string listid)
        {
            List<long> list = listid.Split(',').Select(long.Parse).ToList();
            var visits = DB.Visits.Where(s => list.Contains(s.Id)).Select(x => new
            {
                x.Id,
                x.Name,
                x.Discount,
                x.Tax,
                x.FakeDate,
                x.PaymentMethod,
                x.PersonCount,
                x.HourCount,
                x.HourPrice,
                x.Status,
                x.Type,
                x.Description,
                x.PhoneNumber,
                TimeOut = x.FakeDate.AddMinutes(x.HourCount * 60),
                Total = x.PersonCount * (x.HourCount * 2) * x.HourPrice - x.Discount
            }).ToList();
            return Ok(new
            {
                items = visits.ToList(),
                Totals = new
                {
                    Rows = visits.Count(),
                    Totals = visits.Sum(s => s.Total),
                    Cash = visits.Where(i => i.PaymentMethod == "Cash").Sum(s => s.Total),
                    Discount = visits.Sum(s => s.Discount),
                    Visa = visits.Where(i => i.PaymentMethod == "Visa").Sum(s => s.Total),
                    Coupon = visits.Where(i => i.PaymentMethod == "Coupon").Sum(s => s.Total)
                }
            });
        }
        [Route("Visit/GetById")]
        [HttpGet]
        public IActionResult GetById(long? Id)
        {
            var visit = DB.Visits.Where(x => x.Id == Id).Select(x => new {
                x.Id,
                x.Name,
                x.Discount,
                x.Tax,
                x.FakeDate,
                x.PaymentMethod,
                x.PersonCount,
                x.HourCount,
                x.HourPrice,
                x.Status,
                x.Type,
                x.Description,
                x.PhoneNumber,
                TimeOut = x.FakeDate.AddMinutes(x.HourCount * 60),
                Total = x.PersonCount * (x.HourCount *2  )* x.HourPrice - x.Discount

            }).SingleOrDefault();

            return Ok(visit);
        }


    }
}
