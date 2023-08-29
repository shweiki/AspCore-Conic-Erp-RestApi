using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RestApi.Controllers;

[Authorize]
public class SalaryController : Controller
{
    private readonly IApplicationDbContext DB;
    public SalaryController(IApplicationDbContext dbcontext)
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
                SalaryPayment SalaryPayment = DB.SalaryPayment.Where(x => x.Id == collection.Id).SingleOrDefault();
                SalaryPayment.SalaryFrom = collection.SalaryFrom;
                SalaryPayment.SalaryTo = collection.SalaryTo;
                SalaryPayment.WorkingHours = collection.WorkingHours;
                SalaryPayment.GrossSalary = collection.GrossSalary;
                SalaryPayment.NetSalary = collection.NetSalary;
                SalaryPayment.Status = collection.Status;
                SalaryPayment.EmployeeId = collection.EmployeeId;
                DB.SaveChanges();
                return Ok(true);

            }
            catch { return Ok(false); }
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
                DB.SalaryPayment.Add(collection);
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
        var Salaries = DB.SalaryPayment.Where(m => m.EmployeeId == EmployeeId).Select(
            x => new
            {
                x.Id,
                x.NetSalary,
                x.GrossSalary,
                x.SalaryFrom,
                x.SalaryTo,
                DaysCount = 30,//(new DateTime(x.SalaryTo.Year, x.SalaryTo.Month, x.SalaryTo.Day) - new DateTime(x.SalaryFrom.Year, x.SalaryFrom.Month, x.SalaryFrom.Day)).Days + 1,
                x.Status,
                x.EmployeeId,
                x.WorkingHours,
                x.Employee.Name,
                x.Employee.JobTitle,
                SalaryAdjustmentLogs = x.SalaryAdjustmentLogs.Select(s => new
                {
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
        var Salaries = DB.SalaryPayment.Where(m => m.Id == Id).Select(
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
                DaysCount = 30,//(new DateTime(x.SalaryTo.Year, x.SalaryTo.Month, x.SalaryTo.Day) - new DateTime(x.SalaryFrom.Year, x.SalaryFrom.Month, x.SalaryFrom.Day)).Days + 1,
                SalaryAdjustmentLogs = x.SalaryAdjustmentLogs.Select(s => new
                {
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
        var Salaries = DB.SalaryPayment.Where(m => m.EmployeeId == Id).Select(
            x => new
            {
                x.Id,
                x.NetSalary,
                x.GrossSalary,
                SalaryFrom = x.SalaryFrom.AddMonths(1),
                SalaryTo = x.SalaryTo.AddMonths(1),
                DaysCount = 30,// (new DateTime(x.SalaryTo.Year, x.SalaryTo.Month, x.SalaryTo.Day) - new DateTime(x.SalaryFrom.Year, x.SalaryFrom.Month, x.SalaryFrom.Day)).Days + 1,
                x.Status,
                x.EmployeeId,
                x.WorkingHours,
                x.Employee.Name,
                x.Employee.JobTitle,
            }).ToList().LastOrDefault();
        if (Salaries != null) return Ok(Salaries);
        else return Ok();
    }
    [Route("Salary/GetSalaryId")]
    [HttpGet]
    public IActionResult GetSalaryId(long? EmployeeId)
    {
        var Salaries = DB.SalaryPayment.Where(m => m.EmployeeId == EmployeeId && m.Status == 0).Select(
            x => new
            {
                Id = x.Id,
            }).ToList().FirstOrDefault().Id;
        return Ok(Salaries);

    }
}
