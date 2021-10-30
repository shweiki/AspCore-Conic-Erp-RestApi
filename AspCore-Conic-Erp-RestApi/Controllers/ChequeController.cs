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
        private ConicErpContext DB;
        public ChequeController(ConicErpContext dbcontext)
        {
            DB = dbcontext;
        }        [Route("Cheques/GetCheques")]
        [HttpGet]
        public IActionResult GetCheque()
        {
            var Cheques = DB.Cheques.Select(x=>new {
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
            }).ToList();
                          

                         
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