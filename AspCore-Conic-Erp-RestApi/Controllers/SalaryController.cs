using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Entities;
using Microsoft.AspNetCore.Mvc;

namespace AspCore_Conic_Erp_RestApi.Controllers
{
    [Authorize]
    public class SalaryController : Controller
    {
                private ConicErpContext DB;
        public SalaryController(ConicErpContext dbcontext)
        {
            DB = dbcontext;
        }
        [Route("Salary/Edit")]
        [HttpPost]
        public IActionResult Edit(SalaryPayment collection)
       {
            if (ModelState.IsValid)
            {
                try
                {
                    SalaryPayment SalaryPayment = DB.SalaryPayments.Where(x => x.Id == collection.Id).SingleOrDefault();
                        SalaryPayment.SalaryFrom = collection.SalaryFrom;
                        SalaryPayment.SalaryTo = collection.SalaryTo;
                        SalaryPayment.WorkingHours = collection.WorkingHours;
                        SalaryPayment.GrossSalary = collection.GrossSalary;
                        SalaryPayment.NetSalary = collection.NetSalary;
                        SalaryPayment.Status = collection.Status;
                        SalaryPayment.EmployeeId = collection.EmployeeId;
                        DB.SaveChanges();
                        return Ok(true);
               
                } catch { return Ok(false); }
            }
            return Ok(false);
        }
        [Route("Salary/Create")]
        [HttpPost]
        public IActionResult Create(SalaryPayment collection)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    DB.SalaryPayments.Add(collection);
                    DB.SaveChanges();
                    return Ok(true);

                }
                catch { return Ok(false); }
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
                    x.NetSalary,
                    x.GrossSalary,
                    x.SalaryFrom,
                    x.SalaryTo,
                    x.Status,
                    x.EmployeeId,
                    x.WorkingHours,
                    x.Employee.Name,
                    x.Employee.JobTitle,
                    SalaryAdjustmentLogs = x.SalaryAdjustmentLogs.Select(s => new {
                        s.Id,
                        s.Adjustment.Name,
                        s.AdjustmentAmmount,
                        s.Description,
                        s.Status
                    }).ToList()
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
                    x.Employee.JobTitle,
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
            var Salaries = DB.SalaryPayments.Where(m => m.EmployeeId == Id).Select(
                x => new
                {
                    x.Id,
                    x.NetSalary,
                    x.GrossSalary,
                    SalaryFrom = x.SalaryFrom.AddMonths(1),
                    SalaryTo = x.SalaryTo.AddMonths(1),
                    x.Status,
                    x.EmployeeId,
                    x.WorkingHours,
                    x.Employee.Name,
                    x.Employee.JobTitle,
                 }).ToList().LastOrDefault();
            if(Salaries !=null) return Ok(Salaries);
           else return Ok();
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
