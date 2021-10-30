using System.Data;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Entities; 
using Microsoft.AspNetCore.Mvc;

namespace AspCore_Conic_Erp_RestApi.Controllers
{
    [Authorize]
    public class ReportController : Controller
    {
                private ConicErpContext DB;
        public ReportController(ConicErpContext dbcontext)
        {
            DB = dbcontext;
        }

        [HttpPost]
        [Route("Report/GetByListQ")]
        public IActionResult GetByListQ(int Limit, string Sort, int Page,  string Any)
        {
            var Reports = DB.Reports.Where(s => (Any != null ? s.Id.ToString().Contains(Any) || s.Name.Contains(Any) || s.Type.Contains(Any) : true)  ).Select(x => new
            {
             x.Id,
             x.Name ,
             x.Type ,
             x.EmailSent ,
             x.Html ,
             x.Printer ,
             x.Icon 
            }).ToList();
            Reports = (Sort == "+id" ? Reports.OrderBy(s => s.Id).ToList() : Reports.OrderByDescending(s => s.Id).ToList());
            return Ok(new
            {
                items = Reports.Skip((Page - 1) * Limit).Take(Limit).ToList(),
                Totals = new
                {
                    Rows = Reports.Count(),
                  
                }
            });
        }
        [Route("Report/GetReportByType")]
        [HttpGet]
        public IActionResult GetReportByType(string Type)
        {
            var Reports = DB.Reports.Where(i => i.Type == Type).Select(x => new
            {
                x.Id,
                x.Name,
                x.Type,
                x.EmailSent,
                x.Html,
                x.Printer,
                x.Icon
            }).ToList();
            return Ok(Reports);
        }
        [Route("Report/GetReport")]
        [HttpGet]
        public IActionResult GetReport(int Id)
        {
            var Reports = DB.Reports.Where(i => i.Id == Id ).Select(x => new
            {
                x.Id,
                x.Name,
                x.EmailSent,
                x.Html,
                x.Printer,
                x.Icon
            }).ToList();
            return Ok(Reports);
        }  
        
        [Route("Report/GetTotal")]
        [HttpGet]
        public IActionResult GetTotal()
        {                            

            return Ok(DB.Reports.Count());
        }

        [Route("Report/Create")]
        [HttpPost]
        public IActionResult Create(Report collection)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // TODO: Add insert logic here
                    DB.Reports.Add(collection);
                    DB.SaveChanges();
                    return Ok(collection.Id);

                }
                catch
                {
                    //Console.WriteLine(collection);
                    return Ok(false);
                }
            }
            else return Ok(false);
        }

        [Route("Report/Edit")]
        [HttpPost]
        public IActionResult Edit(Report collection)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Report Report = DB.Reports.Where(x => x.Id == collection.Id).SingleOrDefault();
       
                    Report.Name = collection.Name;
                    Report.Type = collection.Type;
                    Report.EmailSent = collection.EmailSent;
                    Report.Html = collection.Html;
                    Report.Printer = collection.Printer;
                    Report.Icon = collection.Icon;

                    DB.SaveChanges();

                    return Ok(true);

                }
                catch
                {
                    //Console.WriteLine(collection);
                    return Ok(false);
                }
            }
            else return Ok(false);
        }

        [Route("Report/GetReportById")]
        [HttpGet]
        public IActionResult GetReportById(long? Id)
        {
            var Report = DB.Reports.Where(i => i.Id == Id).Select(x => new {
                x.Id,
                x.Name,
                x.Type,
                x.EmailSent,
                x.Html,
                x.Printer,
                x.Icon
            }).SingleOrDefault();
            return Ok(Report);
        }

    }
}