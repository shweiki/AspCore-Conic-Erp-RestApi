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
    public class WorkingAdjustmentController : Controller
    {
        private ConicErpContext DB = new ConicErpContext();

        [Route("WorkingAdjustment/Create")]
        [HttpPost]

        public IActionResult Create(WorkingHoursAdjustment collection)
       {
            if (ModelState.IsValid)
            {
                try
                {

                    DB.WorkingHoursAdjustments.Add(collection);
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

        [Route("WorkingAdjustment/GetWorkingAdjustment")]
        [HttpGet]
        public IActionResult GetWorkingAdjustment()
        {
            var Adjustments = DB.Adjustments.Select(x => new { x.Id, x.Name, x.AdjustmentAmount, x.AdjustmentPercentage }).ToList();

            return Ok(Adjustments);
        }

    }


}
