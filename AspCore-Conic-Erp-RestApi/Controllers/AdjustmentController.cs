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
    public class AdjustmentController : Controller
    {
        private ConicErpContext DB = new ConicErpContext();

        [Route("Adjustment/Create")]
        [HttpPost]

        public IActionResult Create(Adjustment collection)
       {
            if (ModelState.IsValid)
            {
                try
                {

                    DB.Adjustments.Add(collection);
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
