
using Domain.Entities; using Application.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestApi.Controllers;

[Authorize]
public class ActionLogController : Controller
{
    private readonly IApplicationDbContext DB;

    public ActionLogController(IApplicationDbContext dbcontext)
    {
        DB = dbcontext;
    }
    public async Task<Boolean> Create(ActionLog LogOpratio)
    {
        try
        {
            LogOpratio.PostingDateTime = DateTime.Now;

            // TODO: Add insert logic here
            DB.ActionLog.Add(LogOpratio);
            await DB.SaveChangesAsync();
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
        List<ActionLog> ActionLogs = await DB.ActionLog.Where(l => l.TableName == TableName && l.Fktable == Id.ToString()).ToListAsync();

        return Ok(ActionLogs);
    }


}
