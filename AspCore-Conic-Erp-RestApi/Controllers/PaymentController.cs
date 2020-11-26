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
            var Payments = (from x in DB.Payments.ToList()
                            where (x.FakeDate >= DateFrom) && (x.FakeDate <= DateTo) && (x.Status == Status)
                            let p = new 
                         {
                                x.Id,
                                x.TotalAmmount,
                                x.Type,
                                x.EditorName,
                                Name = x.Vendor?.Name + " " + x.Member?.Name + " - " + x.Name,
                                x.FakeDate,
                                x.PaymentMethod,
                                x.Status,
                                x.Description,
                                AccountID = (x.Vendor?.AccountId != null) ? x.Vendor?.AccountId :   x.MemberId
                         }
                            select p);
            
            return Ok(Payments.ToList());
    }
        [Route("Payment/GetPaymentsByMemberId")]
        [HttpGet]
        public IActionResult GetPaymentsByMemberId(long? MemberId)
        {
            var Payments = (from PS in DB.Payments?.Where(z => z.MemberId == MemberId)?.ToList()
                            let p = new
                            {
                                PS.Id,
                                PS.TotalAmmount,
                                PS.Type,
                                PS.EditorName,
                                Name = PS.Vendor?.Name + " " + PS.Member?.Name + " - " + PS.Name,
                                PS.FakeDate,
                                PS.PaymentMethod,
                                ObjectID = PS.VendorId == null ? PS.MemberId : PS.VendorId,
                                PS.Description,
                                AccountID = (PS.Vendor == null) ? PS.Member.AccountId : PS.Vendor.AccountId,
                                Status = (from a in DB.Oprationsys.ToList()
                                          where (a.Status == PS.Status) && (a.TableName == "Payment")
                                          select new
                                          {
                                              a.Id,
                                              a.OprationName,
                                              a.Status,
                                              a.OprationDescription,
                                              a.ArabicOprationDescription,
                                              a.IconClass,
                                              a.ClassName
                                          }).FirstOrDefault(),

                            }
                            select p);

            return Ok(Payments);
        }
        [Route("Payment/GetPaymentByStatus")]
        [HttpGet]
        public IActionResult GetPaymentByStatus(int? Status)
        {
            var Payments = (from x in DB.Payments.ToList()
                            where (x.Status == Status)
                            let p = new
                            {
                                x.Id,
                                x.TotalAmmount,
                                x.Type,
                                x.EditorName,
                                Name = x.Vendor?.Name + " " + x.Member?.Name + " - " + x.Name,
                                x.FakeDate ,
                                x.PaymentMethod,
                                ObjectID = x.VendorId == null ? x.MemberId : x.VendorId,
                                x.Status,
                                x.Description,
                                AccountID = (x.Vendor == null) ? x.Member.AccountId : x.Vendor.AccountId,
                          
                            }
                            select p); ;

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
                    Oprationsy Opx = DB.Oprationsys.Where(d => d.Status == collection.Status && d.TableName == "Payment").SingleOrDefault();
                    OprationsysController Op = new OprationsysController();
                    if (Op.ChangeStatus(collection.Id, Opx.Id, "<!" + collection.Id + "!>"))
                    {
                        return Ok(collection.Id);
                    }
                    else return Ok(false);
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
