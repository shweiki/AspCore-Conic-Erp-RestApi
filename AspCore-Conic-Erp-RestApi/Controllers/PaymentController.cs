using System;
using System.Data;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Entities; 
using Microsoft.AspNetCore.Mvc;


namespace AspCore_Conic_Erp_RestApi.Controllers
{
    [Authorize]
    public class PaymentController : Controller
    {
        private ConicErpContext DB = new ConicErpContext();
        [HttpPost]
        [Route("Payment/GetByListQ")]
        public IActionResult GetByListQ(int Limit, string Sort, int Page, string? User, DateTime? DateFrom, DateTime? DateTo, int? Status, string? Any)
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
                x.MemberId,
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
                x.MemberId,
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
                AccountId = (x.Vendor == null) ? x.Member.AccountId : x.Vendor.AccountId,
               x.Status
            }).ToList();

            return Ok(Payments);
        }
        [Route("Payment/GetPaymentByStatus")]
        [HttpGet]
        public IActionResult GetPaymentByStatus(int? Status)
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
               ObjectId = x.VendorId == null ? x.MemberId : x.VendorId,
                x.Status,
                x.Description,
                AccountId = (x.Vendor == null) ? x.Member.AccountId : x.Vendor.AccountId,
            }).ToList();
                

            return Ok(Payments);
        }
        [HttpPost]
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



    }
}
