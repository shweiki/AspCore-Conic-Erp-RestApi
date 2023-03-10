using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspCore_Conic_Erp_RestApi.Controllers;

[Authorize]
public class ActionLogController : Controller
{
    private ConicErpContext DB;
    public ActionLogController(ConicErpContext dbcontext)
    {
        DB = dbcontext;
    }
    public Boolean Create(ActionLog LogOpratio)
    {
        try
        {
            LogOpratio.PostingDateTime = DateTime.Now;

            // TODO: Add insert logic here
            DB.ActionLogs.Add(LogOpratio);
            DB.SaveChanges();
            //  Console.WriteLine(collection);
            //return Json(collection);
            return (true);
        }
        catch
        {
            //Console.WriteLine(collection);
            return (false);
        }
    }
    [Route("ActionLog/GetLogByObjTable")]
    [HttpGet]
    public async Task<IActionResult> GetLogByObjTable(string TableName, int Id)
    {
        List<ActionLog> ActionLogs = await DB.ActionLogs.Where(l => l.TableName == TableName && l.Fktable == Id.ToString()).ToListAsync();

        return Ok(ActionLogs);
    }


}
