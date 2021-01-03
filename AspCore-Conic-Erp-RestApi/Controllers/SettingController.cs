﻿using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Entities; 
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace AspCore_Conic_Erp_RestApi.Controllers
{
    public class SettingController : Controller
    {
        private ConicErpContext DB = new ConicErpContext();
        [Route("Setting/GetSetting")]
        [HttpGet]
        public IActionResult GetSetting()
        {
            var Settings = DB.Settings.Select(x => new {
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
            var Settings = DB.Settings.Where(i=>i.Status==0).Select(x => new {
                x.Id,
                x.Name,
                x.Value,
                x.Type,
                x.Status,
                x.Description,
            }).ToList();
            return Ok(Settings);
        }
        [Route("Setting/Create")]
        [HttpPost]
        public IActionResult Create(Setting collection)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    DB.Settings.Add(collection);
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
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "/C copy /b Image1.jpg + Archive.rar Image2.jpg";
            startInfo.Verb = "runas";
            process.StartInfo = startInfo;
            process.Start();
            var version = "Ok";
            return Ok(version);
        }

        [Route("Setting/Edit")]
        [HttpPost]
        public IActionResult Edit(Setting collection)
        {
            if (ModelState.IsValid)
            {
                Setting Setting = DB.Settings.Where(x => x.Name == collection.Name).SingleOrDefault();
                if (Setting != null) {
                    Setting.Name = collection.Name;
                    Setting.Status = collection.Status;
                    Setting.Value = collection.Value;
                    Setting.Type = collection.Type;
                    Setting.Description = collection.Description;
                } else
                {
                    DB.Settings.Add(collection);
                }
                try
                {
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
}
