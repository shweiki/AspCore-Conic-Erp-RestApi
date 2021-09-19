using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web;
using Microsoft.AspNetCore.Authorization;
using Entities; 
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AspCore_Conic_Erp_RestApi.Controllers
{
    [Authorize]
    public class WorkingHoursLogController : Controller
    {
        private ConicErpContext DB = new ConicErpContext();

        [Route("WorkingHourLog/GetWorkingHourId")]
        [HttpGet]
        public IActionResult GetWorkingHourId(long? EmployeeId)
        {
            try
            {
                
                 var WorkingHoursLogs = DB.WorkingHoursLogs?.Where(m => m.EmployeeId == EmployeeId && m.Status == 0)?.Select(
                    x => new
                    {
                        x.Id,

                    })?.ToList()?.LastOrDefault()?.Id;
               
                return Ok(WorkingHoursLogs);
            }

            catch
            {
                return Ok(false);
            }

        }

        [Route("WorkingHourLog/GetEmployeeMounthLog")]
        [HttpGet]
        public IActionResult GetEmployeeMounthLog(long? EmpId)
        {
           
            try
            {
                var Logs = DB.WorkingHoursLogs.Where(x=> x.EmployeeId == EmpId && x.Status == 0 ).Select
                    (x => new {
                        x.Id, 
                        x.StartDateTime, 
                        x.EndDateTime,
                        x.Description,
                        DeviceName =x.Device.Name,
                        WorkTime = x.EndDateTime-x.StartDateTime,
                    }).ToList();

                return Ok(Logs);
            }
            catch
            {
                //Console.WriteLine(collection);
                return Ok(false);
            }

            return Ok(false);
        }
   
   
     
        [Route("WorkingHourLog/Create")]
        [HttpPost]
        public IActionResult Create(WorkingHoursLog collection)
        {
            if (ModelState.IsValid)
            {
                try
                {
          
                    DB.WorkingHoursLogs.Add(collection);
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
        [Route("WorkingHourLog/Logout")]
        [HttpPost]
        public IActionResult Logout(WorkingHoursLog collection)
        {
            var LastStart = DB.WorkingHoursLogs.Where(ml => ml.EmployeeId == collection.EmployeeId)?.ToList()?.LastOrDefault()?.StartDateTime;
            var WorkingLog = DB.WorkingHoursLogs.Where(x => x.StartDateTime == LastStart )?.ToList()?.LastOrDefault()?.EndDateTime;
            
            if (WorkingLog == null) {

                WorkingHoursLog WorkingHoursLog = DB.WorkingHoursLogs.Where(x => x.EmployeeId == collection.EmployeeId && x.StartDateTime == LastStart).FirstOrDefault();
                WorkingHoursLog.EndDateTime = collection.EndDateTime;
                DB.SaveChanges();
                return Ok(true);

            }

            else 
            {
                
                DB.WorkingHoursLogs.Add(collection);
                DB.SaveChanges();
                return Ok(true);

            }
        }
       

        [Route("WorkingHourLog/GetEmployeeLogById")]
        [HttpGet]
        public IActionResult GetEmployeeLogById(long? Id)
        {
            try
            {
                var WorkingHoursLogs = DB.WorkingHoursLogs.Where(x => x.EmployeeId == Id).Select(x => new
                {
                    x.Status,
                    x.StartDateTime,
                    x.EndDateTime,
                    x.Device.Name,
                    x.DeviceId,
                    x.Description,
                    x.Id,
                    x.EmployeeId,
                }).ToList();
                WorkingHoursLogs = WorkingHoursLogs.GroupBy(a => new { a.EmployeeId, a.StartDateTime })
                       .Select(g => g.Last()).ToList();

                return Ok(WorkingHoursLogs);
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
