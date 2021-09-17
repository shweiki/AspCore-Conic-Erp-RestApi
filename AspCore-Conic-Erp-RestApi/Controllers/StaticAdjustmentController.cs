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
    public class StaticAdjustmentController : Controller
    {
        private ConicErpContext DB = new ConicErpContext();

        [Route("StaticAdjustment/Create")]
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

        [Route("StaticAdjustment/GetAdjustments")]
        [HttpGet]
        public IActionResult GetAdjustments()
        {
            var Adjustments = DB.Adjustments.Select(x => new { x.Id, x.Name, x.AdjustmentAmount, x.AdjustmentPercentage }).ToList();

            return Ok(Adjustments);
        }

        [Route("StaticAdjustment/GetAdjustmentLabel")]
        [HttpGet]
        public IActionResult GetAdjustmentLabel()
        {
            var Areas = DB.Adjustments.Select(x => new {

                value = x.Id,
                label = x.Name,
            }).ToList();
            return Ok(Areas);


        }

    }


}
