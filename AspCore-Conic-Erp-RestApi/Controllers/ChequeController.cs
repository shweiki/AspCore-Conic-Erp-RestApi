using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Entities; 

namespace AspCore_Conic_Erp_RestApi.Controllers
{
    [Authorize]
    public class ChequeController : Controller
    {
        private ConicErpContext DB = new ConicErpContext();
        [Route("Cheques/GetCheques")]
        [HttpGet]
        public IActionResult GetCheque()
        {
            var Cheques = (from x in DB.Cheques.ToList()
                           select new
                           {
                               x.Id,
                               x.BankAddress,
                               x.BankName,
                               x.ChequeAmount,
                               x.FakeDate,
                               x.Payee,
                               x.PaymentType,
                               x.Status,
                               x.ChequeNumber,
                               x.Currency,
                               x.Description,
                               x.VendorId,
                               x.Vendor.Name,
                         
                           });
            return Ok(Cheques);
        }

        [Route("Cheques/Create")]
        [HttpPost]
        public IActionResult Create(Cheque collection)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    collection.Status = 0;
                    DB.Cheques.Add(collection);
                    DB.SaveChanges();
                    Oprationsy Opx = DB.Oprationsys.Where(d => d.Status == collection.Status && d.TableName == "Cheque").SingleOrDefault();
                    OprationsysController Op = new OprationsysController();
                    if (Op.ChangeStatus(collection.Id, Opx.Id, "<!" + collection.Id + "!>"))
                    {
                        return Ok(true);
                    }
                }
                catch
                {
                    //Console.WriteLine(collection);
                    return Ok(false);
                }
            }
            return Ok(false);
        }


    }
}