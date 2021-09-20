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
                SalaryPayment SalaryPayment = DB.SalaryPayments.Where(x => x.EmployeeId == collection.EmployeeId && x.status == 0).SingleOrDefault();
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
                    x.status,
                }).ToList();
            return Ok(Salaries);
        }

        [Route("Salary/GetSalary")]
        [HttpGet]
        public IActionResult GetSalary(long? Id)
        {
            var Salaries = DB.SalaryPayments.Where(m => m.Id == Id).Select(
                x => new
                {
                    x.Id,
                    x.NetSalary,
                    x.GrossSalary,
                    x.SalaryPeriod,
                    x.status,
                    EmpId= x.EmployeeId,
                    Name = x.Employee.Name
                }).SingleOrDefault();
            return Ok(Salaries);
        }

        [Route("Salary/GetLastSalaryById")]
        [HttpGet]
        public IActionResult GetLastSalaryById(long? Id)
        {
            var Salaries = DB.SalaryPayments.Where(m => m.EmployeeId == Id && m.status == 0).Select(
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
            var Salaries = DB.SalaryPayments.Where(m => m.EmployeeId == EmployeeId && m.status == 0).Select(
                x => new
                {
                    Id = x.Id,
                }).ToList().FirstOrDefault().Id;
            return Ok(Salaries);

        }

    

    }


}
