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

        [Route("Salary/Update")]
        [HttpPost]

        public IActionResult Update(SalaryPayment collection)
       {
            if (ModelState.IsValid)
            {
                try
                {
                    SalaryPayment SalaryPayment = DB.SalaryPayments.Where(x => x.Id == collection.Id).SingleOrDefault();
                    if (collection.GrossSalary > 0)
                    {
                        SalaryPayment.WorkingHours = collection.WorkingHours;
                        SalaryPayment.GrossSalary = collection.GrossSalary;
                        DB.SaveChanges();
                        return Ok(true);
                    }
                    else
                    {
                        SalaryPayment.WorkingHours = collection.WorkingHours;
                        DB.SaveChanges();
                        return Ok(true);
                    }
                } catch { return Ok(false); }
             

            }
            return Ok(false);

        }

        [Route("Salary/GetSalaryByEmployeeId")]
        [HttpGet]
        public IActionResult GetSalaryByEmployeeId(long? EmployeeId)
        {
            var Salaries = DB.SalaryPayments.Where(m => m.EmployeeId == EmployeeId).Select(
                x => new
                {
                    x.Id,
                    x.EmployeeId,
                    x.NetSalary,
                    x.GrossSalary,
                    x.WorkingHours,
                    x.SalaryFrom,
                    x.SalaryTo,
                    x.Status,
                }).ToList();
            return Ok(Salaries);
        }

        [Route("Salary/GetById")]
        [HttpGet]
        public IActionResult GetById(long? Id)
        {
            var Salaries = DB.SalaryPayments.Where(m => m.Id == Id).Select(
                x => new
                {
                    x.Id,
                    x.NetSalary,
                    x.GrossSalary,
                    x.SalaryFrom,
                    x.SalaryTo,
                    x.Status,
                    x.EmployeeId,
                    x.WorkingHours,
                    x.Employee.Name,
                    SalaryAdjustmentLogs =  x.SalaryAdjustmentLogs.Select(s=>new { 
                    s.Id,
                    s.Adjustment.Name,
                    s.AdjustmentAmmount,
                    s.Description,
                    s.Status
                    }).ToList()
                }).SingleOrDefault();
            return Ok(Salaries);
        }

        [Route("Salary/GetLastSalaryById")]
        [HttpGet]
        public IActionResult GetLastSalaryById(long? Id)
        {
            var Salaries = DB.SalaryPayments.Where(m => m.EmployeeId == Id && m.Status == 0).Select(
                x => new
                {
                  Id= x.Id,
                  
                }).SingleOrDefault();
            return Ok(Salaries.Id);
        }


        [Route("Salary/GetSalaryId")]
        [HttpGet]
        public IActionResult GetSalaryId(long? EmployeeId)
        {
            var Salaries = DB.SalaryPayments.Where(m => m.EmployeeId == EmployeeId && m.Status == 0).Select(
                x => new
                {
                    Id = x.Id,
                }).ToList().FirstOrDefault().Id;
            return Ok(Salaries);

        }

    

    }


}
