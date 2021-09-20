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
        }

        [HttpPost]
        [Route("WorkingAdjustment/GetByListQ")]
        public IActionResult GetByListQ(int Limit, string Sort, int Page,  DateTime? DateFrom, DateTime? DateTo, int? Status)
        {
            var Adjustments = DB.WorkingHoursAdjustments.Where(s =>
           ((DateFrom != null ? s.WorkingHoursLog.StartDateTime >= DateFrom : true) || (DateFrom != null ? s.WorkingHoursLog.EndDateTime >= DateFrom : true)) &&
                ((DateFrom != null ? s.WorkingHoursLog.StartDateTime <= DateTo : true) || (DateFrom != null ? s.WorkingHoursLog.EndDateTime <= DateTo : true)) &&
            ( s.Status == Status)).Select(x => new
            {
                x.Id,
                x.AdjustmentAmmount,
                x.AdjustmentId,
                x.Description,
                x.SalaryPaymentId,
                x.Status,
                x.Tax,
                AdjustmentName = x.Adjustment.Name,
                StartDate = x.WorkingHoursLog.StartDateTime,
                EndDate = x.WorkingHoursLog.EndDateTime,
                x.WorkingHoursLogId,
                x.Adjustment,
                x.SalaryPayment,
                x.WorkingHoursLog,
               
            }).ToList();
            Adjustments = (Sort == "+id" ? Adjustments.OrderBy(s => s.Id).ToList() : Adjustments.OrderByDescending(s => s.Id).ToList());
            return Ok(new
            {
                totals = Adjustments.Skip((Page - 1) * Limit).Take(Limit).ToList(),
                Totals = new
                {
                    Rows = Adjustments.Count(),
                    Totals = Adjustments.Sum(s => s.AdjustmentAmmount),
                    Cash = Adjustments.Where(i => i.Adjustment.Type == "CashValue").Sum(s => s.AdjustmentAmmount),
                    Hours = Adjustments.Where(i => i.Adjustment.Type == "Hours").Sum(s => s.AdjustmentAmmount),
                    // Discount = Adjustments.Sum(s => s.Discount),
                    Percentage = Adjustments.Where(i => i.Adjustment.Type == "Percentage").Sum(s => s.AdjustmentAmmount)
                }
            });
        }

    }


}
