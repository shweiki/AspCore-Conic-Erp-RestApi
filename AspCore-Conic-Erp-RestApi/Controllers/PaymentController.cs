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
    public class PaymentController : Controller
    {
                private ConicErpContext DB;
        public PaymentController(ConicErpContext dbcontext)
        {
            DB = dbcontext;
        }
        [HttpPost]
        [Route("Payment/GetByListQ")]
        public IActionResult GetByListQ(int Limit, string Sort, int Page, string User, DateTime? DateFrom, DateTime? DateTo, int? Status, string Any)
        {
            var Invoices = DB.Payments.Where(s => (Any != null ? s.Id.ToString().Contains(Any) || s.Vendor.Name.Contains(Any) : true) && (DateFrom != null ? s.FakeDate >= DateFrom : true)
            && (DateTo != null ? s.FakeDate <= DateTo : true) && (Status != null ? s.Status == Status : true) &&
            (User != null ? DB.ActionLogs.Where(l => l.PaymentId == s.Id && l.UserId == User).SingleOrDefault() != null : true)).Select(x => new

            {
                x.Id,
                x.TotalAmmount,
                x.Type,
                x.EditorName,
                Name = x.Vendor.Name + " " + x.Member.Name + " - " + x.Name,
                x.FakeDate,
                x.PaymentMethod,
                x.Status,
                x.Description,
                x.VendorId,
                x.MemberId,
                ObjectId = x.VendorId == null ? x.MemberId : x.VendorId,
                AccountId = (x.Vendor == null) ? x.Member.AccountId : x.Vendor.AccountId,
            }).ToList();
            Invoices = (Sort == "+id" ? Invoices.OrderBy(s => s.Id).ToList() : Invoices.OrderByDescending(s => s.Id).ToList());
            return Ok(new
            {
                items = Invoices.Skip((Page - 1) * Limit).Take(Limit).ToList(),
                Totals = new
                {
                    Rows = Invoices.Count(),
                    Totals = Invoices.Sum(s => s.TotalAmmount),
                    Cash = Invoices.Where(i => i.PaymentMethod == "Cash").Sum(s => s.TotalAmmount),
                    Cheque = Invoices.Where(i => i.PaymentMethod == "Cheque").Sum(s => s.TotalAmmount),
                    Visa = Invoices.Where(i => i.PaymentMethod == "Visa").Sum(s => s.TotalAmmount)
                }
            });
        }

        [Route("Payment/GetPayment")]
        [HttpGet]
        public ActionResult GetPayment(DateTime DateFrom, DateTime DateTo , int Status)
        {
            var Payments = DB.Payments.Where(i => i.FakeDate >= DateFrom && i.FakeDate <= DateTo && i.Status == Status).Select(x => new
            {
                x.Id,
                x.TotalAmmount,
                x.Type,
                x.EditorName,
                Name = x.Vendor.Name + " " + x.Member.Name + " - " + x.Name,
                x.FakeDate,
                x.PaymentMethod,
                x.Status,
                x.Description,
                x.VendorId,
                x.MemberId,
                ObjectId = x.VendorId == null ? x.MemberId : x.VendorId,
                AccountId = (x.Vendor == null) ? x.Member.AccountId : x.Vendor.AccountId,
            }).ToList();
     
            
            return Ok(Payments.ToList());
    }
        [Route("Payment/GetPaymentsByMemberId")]
        [HttpGet]
        public IActionResult GetPaymentsByMemberId(long? MemberId)
        {
            var Payments = DB.Payments.Where(i => i.MemberId == MemberId).Select(x => new {
                x.Id,
                x.TotalAmmount,
                x.Type,
                x.EditorName,
                Name = x.Vendor.Name + " " + x.Member.Name + " - " + x.Name,
                x.FakeDate,
                x.PaymentMethod,
                ObjectId = x.VendorId == null ? x.MemberId : x.VendorId,
                x.Description,
                x.VendorId,
                x.MemberId,
                AccountId = (x.Vendor == null) ? x.Member.AccountId : x.Vendor.AccountId,
               x.Status
            }).ToList();

            return Ok(Payments);
        }
        [Route("Payment/GetPaymentsByVendorId")]
        [HttpGet]
        public IActionResult GetPaymentsByVendorId(long? VendorId)
        {
            var Payments = DB.Payments.Where(i => i.VendorId == VendorId).Select(x => new {
                x.Id,
                x.TotalAmmount,
                x.Type,
                x.EditorName,
                Name = x.Vendor.Name + " " + x.Member.Name + " - " + x.Name,
                x.FakeDate,
                x.PaymentMethod,
                ObjectId = x.VendorId == null ? x.MemberId : x.VendorId,
                x.Description,
                x.VendorId,
                x.MemberId,
                AccountId = (x.Vendor == null) ? x.Member.AccountId : x.Vendor.AccountId,
                x.Status
            }).ToList();

            return Ok(Payments);
        }
        [Route("Payment/GetById")]
        [HttpGet]
        public IActionResult GetById(long? Id)
        {
            var Payment = DB.Payments.Where(i => i.Id == Id).Select(x => new {
                x.Id,
                x.TotalAmmount,
                x.Type,
                x.EditorName,
                Name = x.Vendor.Name + " " + x.Member.Name + " - " + x.Name,
                x.FakeDate,
                x.PaymentMethod,
                ObjectId = x.VendorId == null ? x.MemberId : x.VendorId,
                x.Description,
                x.MemberId,
                x.VendorId,
                AccountId = (x.Vendor == null) ? x.Member.AccountId : x.Vendor.AccountId,
                x.Status
            }).SingleOrDefault();

            return Ok(Payment);
        }
        [Route("Payment/GetPaymentByStatus")]
        [HttpGet]
        public IActionResult GetPaymentByStatus(int Limit, string Sort, int Page, int? Status)
        {
            var Payments = DB.Payments.Where(i => i.Status == Status).Select(x => new
            {
                x.Id,
                x.TotalAmmount,
                x.Type,
                x.EditorName,
                Name = x.Vendor.Name + " " + x.Member.Name + " - " + x.Name,
                x.FakeDate,
                x.PaymentMethod,
                x.Status,
                x.Description,
                ObjectId = x.VendorId == null ? x.MemberId : x.VendorId,
                x.VendorId,
                x.MemberId,
                AccountId = (x.Vendor == null) ? x.Member.AccountId : x.Vendor.AccountId,
            }).ToList();
            Payments = (Sort == "+id" ? Payments.OrderBy(s => s.Id).ToList() : Payments.OrderByDescending(s => s.Id).ToList());
            return Ok(new
            {
                items = Payments.Skip((Page - 1) * Limit).Take(Limit).ToList(),
                Totals = new
                {
                    Rows = Payments.Count(),
                    Totals = Payments.Sum(s => s.TotalAmmount),
                    Cash = Payments.Where(i => i.PaymentMethod == "Cash").Sum(s => s.TotalAmmount),
                    Cheque = Payments.Where(i => i.PaymentMethod == "Cheque").Sum(s => s.TotalAmmount),
                    Visa = Payments.Where(i => i.PaymentMethod == "Visa").Sum(s => s.TotalAmmount)
                }
            });
                

        }
        [Route("Payment/GetPaymentByListId")]
        [HttpGet]
        public IActionResult GetPaymentByListId(string listid)
        {
            List<long> list = listid.Split(',').Select(long.Parse).ToList();
            var Payments = DB.Payments.Where(s => list.Contains(s.Id)).Select(x => new
            {
                x.Id,
                x.TotalAmmount,
                x.Type,
                x.EditorName,
                Name = x.Vendor.Name + " " + x.Member.Name + " - " + x.Name,
                x.FakeDate,
                x.PaymentMethod,
                x.Status,
                x.Description,
                x.VendorId,
                x.MemberId,
                ObjectId = x.VendorId == null ? x.MemberId : x.VendorId,
                AccountId = (x.Vendor == null) ? x.Member.AccountId : x.Vendor.AccountId,
            }).ToList();
            return Ok(new
            {
                items = Payments.ToList(),
                Totals = new
                {
                    Rows = Payments.Count(),
                    Totals = Payments.Sum(s => s.TotalAmmount),
                    Cash = Payments.Where(i => i.PaymentMethod == "Cash").Sum(s => s.TotalAmmount),
                    Cheque = Payments.Where(i => i.PaymentMethod == "Cheque").Sum(s => s.TotalAmmount),
                    Visa = Payments.Where(i => i.PaymentMethod == "Visa").Sum(s => s.TotalAmmount)
                }
            });
        }
        [HttpPost]
        [Route("Payment/Create")]
        public IActionResult Create(Payment collection)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // TODO: Add insert logic here
                    DB.Payments.Add(collection);
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
        [Route("Payment/Edit")]
        [HttpPost]
        public IActionResult Edit(Payment collection)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Payment payment = DB.Payments.Where(x => x.Id == collection.Id).SingleOrDefault();
                    payment.Name = collection.Name;
                    payment.FakeDate = collection.FakeDate;
                    payment.PaymentMethod = collection.PaymentMethod;
                    payment.TotalAmmount = collection.TotalAmmount;
                    payment.Description = collection.Description;
                    payment.VendorId = collection.VendorId;
                    payment.IsPrime = collection.IsPrime;
                    payment.MemberId = collection.MemberId;
                    payment.Type = collection.Type;
                    payment.EditorName = collection.EditorName;
                  
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
        [HttpPost]
        [Route("Payment/EditPaymentMethod")]
        public IActionResult EditPaymentMethod(long ID, string PaymentMethod)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Payment payment = DB.Payments.Where(x => x.Id == ID).SingleOrDefault();
                    payment.PaymentMethod = PaymentMethod;
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
