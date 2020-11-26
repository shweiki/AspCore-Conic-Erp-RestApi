using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Entities; 
using Microsoft.AspNetCore.Mvc;

namespace AspCore_Conic_Erp_RestApi.Controllers
{
    public class SettingController : Controller
    {
        private ConicErpContext DB = new ConicErpContext();
        [Route("Setting/GetSetting")]
        [HttpGet]
        public IActionResult GetSetting()
        {
            var Settings = (from x in DB.Settings.ToList()
                             select new
                             {
                                 x.Id,
                                 x.Name,
                                 x.Value,
                                 x.Type,
                                 x.Status,
                                 x.Description,
                               
                             });
            return Ok(Settings);
        }
        [Authorize]
        [Route("Setting/GetActiveSetting")]
        [HttpGet]
        public IActionResult GetActiveSetting()
        {
            var Settings = (from x in DB.Settings.ToList()
                            select new
                            {
                                x.Id,
                                x.Name,
                                x.Value,
                                x.Type,
                                x.Status,
                                x.Description,

                            });
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
                    Oprationsy Opx = DB.Oprationsys.Where(d => d.Status == collection.Status && d.TableName == "Setting").SingleOrDefault();
                    OprationsysController Op = new OprationsysController();
                    if (Op.ChangeStatus(collection.Id, Opx.Id, "<!" + collection.Id + "!>"))
                    {
                        return Ok(true);
                    }
                }
                catch
                {
                    //Console.WriteLine(collection);
                    return Ok(false);
                }
            }
            return Ok(false);
        }
        [Route("Setting/Edit")]
        [HttpPost]
        public IActionResult Edit(Setting collection)
        {
            if (ModelState.IsValid)
            {
                    Setting Setting = DB.Settings.Where(x => x.Id == collection.Id).SingleOrDefault();
                    Setting.Name = collection.Name;
                    Setting.Status = collection.Status;
                    Setting.Value = collection.Value;
                    Setting.Type = collection.Type;
                    Setting.Description = collection.Description;
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

    }
}
