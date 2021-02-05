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
                ObjectID = x.VendorId == null ? x.MemberId : x.VendorId,
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
                ObjectID = x.VendorId == null ? x.MemberId : x.VendorId,
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
