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
                SalaryPayment SalaryPayment = DB.SalaryPayments.Where(x => x.EmployeeId == collection.EmployeeId).SingleOrDefault();
                SalaryPayment.GrossSalary = collection.GrossSalary;
               
                try
                {
                    
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

        [Route("Salary/GetSalaryById")]
        [HttpGet]
        public IActionResult GetSalaryById(long? EmployeeId)
        {
            var Salaries = DB.SalaryPayments.Where(m => m.EmployeeId == EmployeeId).Select(
                x => new
                {
                    x.Id,
                    x.EmployeeId,
                    x.NetSalary,
                    x.GrossSalary,
                    x.SalaryPeriod,

                }).ToList();
            return Ok(Salaries);
        }

        [Route("Salaries/GetSalaryId")]
        [HttpGet]
        public IActionResult GetSalaryId(long? EmployeeId)
        {
            var Salaries = DB.SalaryPayments.Where(m => m.EmployeeId == 3).Select(
                x => new
                {
                    Id = x.Id,
                }).ToList();
            return Ok(Salaries);

        }

        [Route("Salary/GetSalaryByIds")]
        [HttpGet]
        public IActionResult GetSalaryByIds(long? EmployeeId)
        {
            var Salaries = DB.SalaryPayments.Where(m => m.EmployeeId == EmployeeId).Select(
                x => new
                {
                    x.Id,
                    x.EmployeeId,
                    x.NetSalary,
                    x.GrossSalary,
                    x.SalaryPeriod,

                }).ToList();
            return Ok(Salaries);
        }

    }


}
