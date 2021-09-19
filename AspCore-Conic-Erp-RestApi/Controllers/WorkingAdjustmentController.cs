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
            try
            {
                var Adjustments = DB.WorkingHoursAdjustments.Select(x => new
                {
                    x.Id,
                    x.AdjustmentAmmount,
                    x.Tax,
                    x.Description,

                }).ToList();

                return Ok(Adjustments);
            }
            catch
            {
                //Console.WriteLine(collection);
                return Ok(false);
            }

            return Ok(false);
        }

        [Route("WorkingAdjustment/GetWorkingAdjustmentBySalaryId")]
        [HttpGet]
        public IActionResult GetWorkingAdjustmentBySalaryId(long? SalId)
        {
            try
            {
                var Adjustments = DB.WorkingHoursAdjustments.Where(m => m.SalaryPaymentId == SalId && m.Status == 0).Select(
                    x => new
                    {
                        x.Id,
                        x.AdjustmentAmmount,
                        x.Tax,
                        x.Description,
                        StartDate = x.WorkingHoursLog.StartDateTime,
                        EndDate = x.WorkingHoursLog.EndDateTime,
                        AdjustmentName = x.Adjustment.Name,
                        Salary = x.SalaryPayment.GrossSalary,


                    }).ToList();
                return Ok(Adjustments);
            }
            catch
            {
                //Console.WriteLine(collection);
                return Ok(false);
            }

            return Ok(false);
        }

    }


}
