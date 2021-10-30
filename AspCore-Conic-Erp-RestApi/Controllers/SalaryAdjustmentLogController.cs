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
    public class SalaryAdjustmentLogController : Controller
    {
                private ConicErpContext DB;
        public SalaryAdjustmentLogController(ConicErpContext dbcontext)
        {
            DB = dbcontext;
        }

        [Route("SalaryAdjustmentLog/Create")]
        [HttpPost]
        public IActionResult Create(SalaryAdjustmentLog collection)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    DB.SalaryAdjustmentLogs.Add(collection);
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
        [Route("SalaryAdjustmentLog/Edit")]
        [HttpPost]
        public IActionResult Edit(SalaryAdjustmentLog collection)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    SalaryAdjustmentLog SalaryAdjustmentLog = DB.SalaryAdjustmentLogs.Where(x => x.Id == collection.Id).SingleOrDefault();
                    SalaryAdjustmentLog.SalaryPaymentId = collection.SalaryPaymentId;
                    SalaryAdjustmentLog.Status = collection.Status;
                    SalaryAdjustmentLog.AdjustmentAmmount = collection.AdjustmentAmmount;
                    SalaryAdjustmentLog.AdjustmentId = collection.AdjustmentId;
                    SalaryAdjustmentLog.Description = collection.Description;
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
        [Route("SalaryAdjustmentLog/Delete")]
        [HttpPost]
        public IActionResult Delete(long Id)
        {
        
                try
                {
                    SalaryAdjustmentLog SalaryAdjustmentLog = DB.SalaryAdjustmentLogs.Where(x => x.Id == Id).SingleOrDefault();
      
                    DB.SalaryAdjustmentLogs.Remove(SalaryAdjustmentLog);
                    DB.SaveChanges();
                    return Ok(true);
                }
                catch
                {
                    //Console.WriteLine(collection);
                    return Ok(false);
                }
        }
        [Route("SalaryAdjustmentLog/GetSalaryAdjustmentLog")]
        [HttpGet]
        public IActionResult GetSalaryAdjustmentLog()
        {
            try
            {
                var Adjustments = DB.SalaryAdjustmentLogs.Select(x => new
                {
                    x.Id,
                    x.AdjustmentAmmount,
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
        [Route("SalaryAdjustmentLog/GetById")]
        [HttpGet]
        public IActionResult GetById(long? Id)
        {
            try
            {
                var Adjustments = DB.SalaryAdjustmentLogs.Where(m => m.Id == Id).Select(x => new{
                        x.Id,
                        x.AdjustmentAmmount,
                        x.Description,
                        x.SalaryPaymentId,
                        x.AdjustmentId,
                        x.SalaryPayment.Employee.Name,
                        x.SalaryPayment.GrossSalary,
                        x.SalaryPayment.WorkingHours,
                        x.SalaryPayment.EmployeeId,
                    }).SingleOrDefault();
                return Ok(Adjustments);
            }
            catch
            {
                //Console.WriteLine(collection);
                return Ok(false);
            }
        }
        [Route("SalaryAdjustmentLog/GetSalaryAdjustmentLogBySalaryId")]
        [HttpGet]
        public IActionResult GetSalaryAdjustmentLogBySalaryId(long? SalId)
        {
            try
            {
                var Adjustments = DB.SalaryAdjustmentLogs.Where(m => m.SalaryPaymentId == SalId && m.Status == 0).Select(
                    x => new
                    {
                        x.Id,
                        x.AdjustmentAmmount,
                        x.Description,
                    //    StartDate = x.WorkingHoursLog.StartDateTime,
                    //    EndDate = x.WorkingHoursLog.EndDateTime,
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
        [Route("SalaryAdjustmentLog/GetByListQ")]
        public IActionResult GetByListQ(int Limit, string Sort, int Page,  DateTime? DateFrom, DateTime? DateTo, int? Status)
        {
            var Adjustments = DB.SalaryAdjustmentLogs.Where(s =>
            ( s.Status == Status)).Select(x => new
            {
                x.Id,
                x.AdjustmentAmmount,
                x.AdjustmentId,
                x.Description,
                x.SalaryPaymentId,
                x.Status,
                AdjustmentName = x.Adjustment.Name,
              //  StartDate = x.WorkingHoursLog.StartDateTime,
              //  EndDate = x.WorkingHoursLog.EndDateTime,
                x.Adjustment,
                x.SalaryPayment,
               
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
