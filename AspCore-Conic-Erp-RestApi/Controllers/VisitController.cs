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
        private ConicErpContext DB = new ConicErpContext();
        [HttpPost]
        [Route("Visit/GetByListQ")]
        public IActionResult GetByListQ(int Limit, string Sort, int Page, string User, DateTime? DateFrom, DateTime? DateTo, int? Status, string Any ,string Type)
        {
            var Visits = DB.Visits.Where(s => (Any != null ? s.Id.ToString().Contains(Any) || s.PaymentMethod.Contains(Any) || s.Vendor.Name.Contains(Any)|| s.Description.Contains(Any)  || s.PhoneNumber.Contains(Any) || s.Name.Contains(Any)|| s.Region.Contains(Any) : true ) && (DateFrom != null ? s.FakeDate >= DateFrom : true)
            && (DateTo != null ? s.FakeDate <= DateTo : true) && (Status != null ? s.Status == Status : true)&&(Type != null ? s.Type == Type : true) && (User != null ? DB.ActionLogs.Where(l =>l.SalesInvoiceId == s.Id && l.UserId == User).SingleOrDefault() != null : true)).Select(x => new
            {
                x.Id,
                x.Discount,
                x.Tax,
                Name = x.Name + x.Vendor.Name == null ?  x.Member.Name : x.Vendor.Name,
                x.FakeDate,
                x.PaymentMethod,
                x.Status,
                x.Region,
                x.DeliveryPrice,
                x.Type,
                x.Description,
                AccountId = x.Vendor.AccountId == null ? x.Member.AccountId : x.Vendor.AccountId,
                x.VendorId,
                x.MemberId,
                x.PhoneNumber,
                x.Vendor,
                TotalCost = x.InventoryMovements.Sum(s => s.Items.CostPrice * s.Qty),
                Total = x.InventoryMovements.Sum(s=>s.SellingPrice *s.Qty) - x.Discount,
            }).ToList();
            Visits =  (Sort == "+id" ? Visits.OrderBy(s => s.Id).ToList() : Visits.OrderByDescending(s => s.Id).ToList());
            return Ok(new {items = Visits.Skip((Page - 1) * Limit).Take(Limit).ToList(), 
            Totals = new {
            Rows = Visits.Count(),
            Totals = Visits.Sum(s => s.Total),
            TotalCost = Visits.Sum(s => s.TotalCost),
            Profit = Visits.Sum(s => s.Total) - Visits.Sum(s => s.TotalCost),
            Cash = Visits.Where(i=>i.PaymentMethod == "Cash").Sum(s => s.Total),
            Receivables = Visits.Where(i=>i.PaymentMethod == "Receivables").Sum(s => s.Total),
            Discount = Visits.Sum(s => s.Discount),
            Visa = Visits.Where(i=>i.PaymentMethod == "Visa").Sum(s => s.Total)
            } });
    }
        [HttpPost]
        [Route("Visit/GetByAny")]
        public IActionResult GetByAny(string Any, int Status)
        {
            if (Any == null || Any == "") return Ok();
            var Visits = DB.Visits.Where(s => s.Status == Status &&  (s.Id.ToString().Contains(Any)  || s.Vendor.Name.Contains(Any) || s.Description.Contains(Any) || s.PhoneNumber.Contains(Any) || s.Name.Contains(Any) || s.Region.Contains(Any) )
            ).Select(x => new
                {
                    x.Id,
                    x.Discount,
                    x.Tax,
                    Name = x.Name, //+ DB.Vendors.Where(v => v.Id == x.VendorId).SingleOrDefault().Name + DB.Members.Where(v => v.Id == x.MemberId).SingleOrDefault().Name,
                    x.FakeDate,
                    x.PaymentMethod,
                    x.Status,
                    x.Region,
                    x.DeliveryPrice,
                    x.Type,
                    x.Description,
                    x.VendorId,
                    x.MemberId,
                    x.PhoneNumber,
                    x.Vendor,
                    Total = x.InventoryMovements.Sum(s => s.SellingPrice * s.Qty) - x.Discount,
                    AccountId = DB.Vendors.Where(v => v.Id == x.VendorId).SingleOrDefault().AccountId.ToString() + DB.Members.Where(v => v.Id == x.MemberId).SingleOrDefault().AccountId.ToString(),
                    InventoryMovements = x.InventoryMovements.Select(imx => new {
                        imx.Id,
                        imx.ItemsId,
                        imx.Items.Name,
                        imx.Items.Ingredients,
                        imx.Items.CostPrice,
                        imx.TypeMove,
                        imx.InventoryItemId,
                        imx.Qty,
                        imx.EXP,
                        imx.SellingPrice,
                        Total = imx.SellingPrice * imx.Qty,
                        imx.Description
                    }).ToList(),
                }).ToList();

            return Ok(Visits);
        }
       
        [Route("Visit/GetByStatus")]
        [HttpGet]
        public IActionResult GetByStatus(DateTime? DateFrom , DateTime? DateTo, int? Status)
        {
            var Invoices = DB.Visits.Where(s =>(DateFrom !=null ? s.FakeDate >= DateFrom : true ) 
            && (DateTo != null ?  s.FakeDate <= DateTo : true) && (Status != null ? s.Status == Status :true)).Select(x => new
            {
                x.Id,
                x.Discount,
                x.Tax,
                Name =DB.Vendors.Where(v=>v.Id == x.VendorId).SingleOrDefault().Name+ DB.Members.Where(v => v.Id == x.MemberId).SingleOrDefault().Name,
                x.FakeDate,
                x.PaymentMethod,
                x.Status,
                x.Region,
                x.DeliveryPrice,
                x.PhoneNumber,
                x.Type,
                x.Description,
                Total = x.InventoryMovements.Sum(s => s.SellingPrice * s.Qty) - x.Discount,
                AccountId =  DB.Vendors.Where(v => v.Id == x.VendorId).SingleOrDefault().AccountId.ToString() + DB.Members.Where(v => v.Id == x.MemberId).SingleOrDefault().AccountId.ToString(),

            }).ToList();


            return Ok(Invoices);
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
                    Visit Invoice = DB.Visits.Where(x => x.Id == collection.Id).SingleOrDefault();

                    Invoice.Type = collection.Type;
                    Invoice.Tax = collection.Tax;
                    Invoice.Discount = collection.Discount;
                    Invoice.Description = collection.Description;
                    Invoice.Status = collection.Status;
                    Invoice.FakeDate = collection.FakeDate;
                    Invoice.PaymentMethod = collection.PaymentMethod;
                    Invoice.Name = collection.Name;
                    Invoice.PhoneNumber = collection.PhoneNumber;
                    Invoice.HourPrice = collection.HourPrice;
                    Invoice.PersonCount = collection.PersonCount;
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
                    Visit Invoice = DB.Visits.Where(x => x.Id == ID).SingleOrDefault();
                    Invoice.PaymentMethod = PaymentMethod;
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
            var Invoices = DB.Visits.Where(s => list.Contains(s.Id)).Select(x => new
            {
                x.Id,
                x.Discount,
                x.Tax,
                Name = DB.Vendors.Where(v => v.Id == x.VendorId).SingleOrDefault().Name + DB.Members.Where(v => v.Id == x.MemberId).SingleOrDefault().Name,
                x.FakeDate,
                x.PaymentMethod,
                x.Status,
                x.Region,
                x.DeliveryPrice,
                x.PhoneNumber,
                x.Type,
                x.Description,
                TotalCost = x.InventoryMovements.Sum(s => s.Items.CostPrice * s.Qty),
                Total = x.InventoryMovements.Sum(s => s.SellingPrice * s.Qty) - x.Discount,
                AccountId = DB.Vendors.Where(v => v.Id == x.VendorId).SingleOrDefault().AccountId.ToString() + DB.Members.Where(v => v.Id == x.MemberId).SingleOrDefault().AccountId.ToString(),

            }).ToList();
            return Ok(new
            {
                items = Invoices.ToList(),
                Totals = new
                {
                    Rows = Invoices.Count(),
                    Totals = Invoices.Sum(s => s.Total),
                    TotalCost = Invoices.Sum(s => s.TotalCost),
                    Profit = Invoices.Sum(s => s.Total) - Invoices.Sum(s => s.TotalCost),
                    Cash = Invoices.Where(i => i.PaymentMethod == "Cash").Sum(s => s.Total),
                    Receivables = Invoices.Where(i => i.PaymentMethod == "Receivables").Sum(s => s.Total),
                    Discount = Invoices.Sum(s => s.Discount),
                    Visa = Invoices.Where(i => i.PaymentMethod == "Visa").Sum(s => s.Total)
                }
            });
        }
        [Route("Visit/GetById")]
        [HttpGet]
        public IActionResult GetById(long? Id)
        {
            var Invoices = DB.Visits.Where(x => x.Id == Id).Select(x => new {
                x.Id,
                x.Name,
                x.Discount,
                x.Tax,
                x.FakeDate,
                x.PaymentMethod,
                x.PersonCount,
                x.HourCount,
                x.Status,
                x.Type,
                x.Description,
                x.PhoneNumber,
                Total = x.PersonCount * x.HourPrice * x.HourCount

            }).SingleOrDefault();

            return Ok(Invoices);
        }


    }
}
