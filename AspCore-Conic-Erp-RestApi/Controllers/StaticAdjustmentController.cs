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

        public IActionResult Create(StaticAdjustment collection)
       {
            if (ModelState.IsValid)
            {
                try
                {

                    DB.StaticAdjustments.Add(collection);
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

        [Route("StaticAdjustment/GetStaticAdjustments")]
        [HttpGet]
        public IActionResult GetAdjustments()
        {
            var Adjustments = DB.StaticAdjustments.Select(x => new { x.Id, x.AdjustmentAmount, x.AdjustmentPercentage }).ToList();

            return Ok(Adjustments);
        }

        [Route("StaticAdjustment/GetAdjustmentLabel")]
        [HttpGet]
        public IActionResult GetAdjustmentLabel()
        {
            var Areas = DB.StaticAdjustments.Select(x => new {

                value = x.Id,
            }).ToList();
            return Ok(Areas);


        }

    }


}
