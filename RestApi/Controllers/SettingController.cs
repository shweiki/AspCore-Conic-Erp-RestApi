using Domain.Entities; using Application.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace RestApi.Controllers;

public class SettingController : Controller
{
    private readonly IApplicationDbContext DB;
    public SettingController(IApplicationDbContext dbcontext)
    {
        DB = dbcontext;
    }
    [AllowAnonymous]
    [Route("Setting/GetSetting")]
    [HttpGet]
    public IActionResult GetSetting()
    {
        var Settings = DB.Setting.Select(x => new
        {
            x.Id,
            x.Name,
            x.Value,
            x.Type,
            x.Status,
            x.Description,
        }).ToList();
        return Ok(Settings);
    }
    [Authorize]
    [Route("Setting/GetActiveSetting")]
    [HttpGet]
    public IActionResult GetActiveSetting()
    {
        var Settings = DB.Setting.Where(i => i.Status == 0).Select(x => new
        {
            x.Id,
            x.Name,
            x.Value,
            x.Type,
            x.Status,
            x.Description,
        }).ToList();
        return Ok(Settings);
    }
    [Route("Setting/GetProperties")]
    [HttpGet]
    public IActionResult GetProperties(string ObjName)
    {

        PropertyInfo[] myPropertyInfo;
        // Get the properties of 'Type' class object.
        myPropertyInfo = Type.GetType(ObjName).GetProperties();
        return Ok(myPropertyInfo);
    }
    [Route("Setting/Create")]
    [HttpPost]
    public IActionResult Create(Setting collection)
    {
        if (ModelState.IsValid)
        {
            try
            {
                DB.Setting.Add(collection);
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

    [Route("Setting/CheckUpdate")]
    [HttpGet]
    public IActionResult CheckUpdate()
    {
        ProcessStartInfo info = new ProcessStartInfo("C:\\ConicErpDeploy-main\\Update.bat");
        info.UseShellExecute = true;
        info.Verb = "runas";
        Process.Start(info);
        return Ok("Run");
        /* System.Diagnostics.Process process = new System.Diagnostics.Process();
         System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
         startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
         startInfo.FileName = "cmd.exe";
         startInfo.Arguments = "iisreset /stop echo 'IIS Is Stop' cd.. cd.. cd ConicErpDeploy-main mkdir Test  git remote add origin 'https://github.com/shweiki/ConicErpDeploy.git' echo'add origin' git fetch origin echo 'add origin' iisreset / start echo 'IIS Is Start' ";
         startInfo.Verb = "runas";
         process.StartInfo = startInfo;
         process.Start();
         var version = "Ok";
         return Ok(version); */
    }
    [Route("Setting/RestDefualtSetting")]
    [HttpGet]
    public IActionResult RestDefualtSetting()
    {
        try
        {
            DB.Setting.RemoveRange(DB.Setting.ToList());
            DB.SaveChanges();
            return Ok(true);

        }
        catch
        {
            //Console.WriteLine(collection);
            return Ok(false);
        }

    }
    [Route("Setting/Edit")]
    [HttpPost]
    public IActionResult Edit(Setting collection)
    {
        if (ModelState.IsValid)
        {
            try
            {
                Setting Setting = DB.Setting.Where(x => x.Name == collection.Name).SingleOrDefault();
                if (Setting != null)
                {
                    Setting.Name = collection.Name;
                    Setting.Status = collection.Status;
                    Setting.Value = collection.Value;
                    Setting.Type = collection.Type;
                    Setting.Description = collection.Description;
                }
                else
                {
                    DB.Setting.Add(collection);
                }
                DB.SaveChanges();
                return Ok(collection);
            }
            catch
            {
                //Console.WriteLine(collection);
                return Ok(false);
            }
        }
        return Ok(false);
    }

}
