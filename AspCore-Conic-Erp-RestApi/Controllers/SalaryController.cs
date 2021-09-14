using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web;
using Microsoft.AspNetCore.Authorization;
using Entities;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace AspCore_Conic_Erp_RestApi.Controllers
{
    [Authorize]
    public class SalaryController : Controller
    {
        private ConicErpContext DB = new ConicErpContext();

        [Route("Salary/Create")]
        [HttpPost]

        public IActionResult Create(SalaryPayment collection)
       {
            if (ModelState.IsValid)
            {
                try
                {
                    
                    DB.SalaryPayments.Add(collection);
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
    }


}
