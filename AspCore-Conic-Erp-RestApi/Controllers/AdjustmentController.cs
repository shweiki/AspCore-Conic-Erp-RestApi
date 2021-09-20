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

        [Route("Adjustment/GetAdjustments")]
        [HttpGet]
        public IActionResult GetAdjustments()
        {
            try
            {
                var Adjustments = DB.Adjustments
                    .Select(x => new { x.Id, x.Name, x.AdjustmentAmount, x.Type, x.IsWorkingHourAdjustment, x.IsStaticAdjustment }).ToList();

                return Ok(Adjustments);
            }
            catch
            {
                //Console.WriteLine(collection);
                return Ok(false);
            }
        
    }
        [Route("Adjustment/GetRAdjustments")]
        [HttpGet]
        public IActionResult GetRAdjustments()
        {
            try
            {
                var Adjustments = DB.Adjustments.Where(x => x.IsWorkingHourAdjustment == true)
                    .Select(x => new { x.Id, x.Name, x.AdjustmentAmount, x.Type, x.IsWorkingHourAdjustment, x.IsStaticAdjustment }).ToList();

                return Ok(Adjustments);
            }
            catch
            {
                //Console.WriteLine(collection);
                return Ok(false);
            }
        }
        [Route("Adjustment/GetPAdjustments")]
        [HttpGet]
        public IActionResult GetPAdjustments()
        {
            try
            {
                var Adjustments = DB.Adjustments.Where(x => x.IsStaticAdjustment == true)
                    .Select(x => new { x.Id, x.Name, x.AdjustmentAmount, x.Type, x.IsWorkingHourAdjustment, x.IsStaticAdjustment }).ToList();

                return Ok(Adjustments);
            }

            catch
            {
                //Console.WriteLine(collection);
                return Ok(false);
            }
        }

        [Route("Adjustment/GetAdjustmentLabel")]
        [HttpGet]
        public IActionResult GetAdjustmentLabel()
        {
            try
            {
                var Areas = DB.Adjustments.Select(x => new
                {

                    value = x.Id,
                    label = x.Name,
                    amount = x.AdjustmentAmount,
                    isstatic = x.IsStaticAdjustment,
                    iswork = x.IsWorkingHourAdjustment,
                }).ToList();
                return Ok(Areas);
            }
            catch
            {
                //Console.WriteLine(collection);
                return Ok(false);
            }
        }

    }


}
