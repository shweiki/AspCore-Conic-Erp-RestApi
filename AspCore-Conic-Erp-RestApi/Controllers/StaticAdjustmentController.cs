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
        public IActionResult GetStaticAdjustments()
        {
            var Adjustments = DB.StaticAdjustments.Select(x => new { x.Id, x.AdjustmentAmount,  x.Adjustment.Name}).ToList();

            return Ok(Adjustments);
        }

        [Route("StaticAdjustment/GetStaticAdjustmentsById")]
        [HttpGet]
        public IActionResult GetStaticAdjustmentsById( long? Id)
        {
            var Static = DB.StaticAdjustments.Where(x => x.SalaryPaymentId == Id).Select(x => new {

                 x.Id,
                 x.AdjustmentAmount,
                 x.Description,
                 AdjustmentName = x.Adjustment.Name,
            }).ToList();
            return Ok(Static);
        }
        [Route("StaticAdjustment/GetStaticAdjustmentsListQ")]
        [HttpGet]
        public IActionResult GetStaticAdjustmentsListQ(long? Id)
        {
            var Static = DB.StaticAdjustments.Where(x => x.SalaryPaymentId == Id).Select(x => new {

                x.Id,
                x.AdjustmentAmount,
                x.Description,
                AdjustmentName = x.Adjustment.Name,
            }).ToList();
            return Ok(Static);
        }

    }


}
