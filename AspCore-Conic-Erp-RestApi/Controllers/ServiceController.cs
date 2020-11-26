using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Entities; 
using Microsoft.AspNetCore.Mvc;

namespace AspCore_Conic_Erp_RestApi.Controllers
{
    [Authorize]
    public class ServiceController : Controller
    {
        private ConicErpContext DB = new ConicErpContext();
        [Route("Service/GetService")]
        [HttpGet]
        public IActionResult GetService()
        {
            var Services = (from x in DB.Services.ToList()
                             select new
                             {
                                 x.Id,
                                 x.Name,
                                 x.Qty,
                                 x.SellingPrice,
                                 x.ItemId,
                                 ItemName  = DB.Items.Where(i=>i.Id == x.ItemId).SingleOrDefault().Name,
                                 x.Type,
                                 x.Status,
                                 x.Description,
                               
                             });
            return Ok(Services);
        }
        [Route("Service/GetActiveService")]
        [HttpGet]
        public IActionResult GetActiveService()
        {
            var Services = (from x in DB.Services.ToList()
                            select new
                            {
                                x.Id,
                                x.Name,
                                x.Qty,
                                x.SellingPrice,
                                x.ItemId,
                                ItemName = DB.Items.Where(i => i.Id == x.ItemId).SingleOrDefault().Name,
                                x.Type,
                                x.Status,
                                x.Description,

                            });
            return Ok(Services);
        }
        [Route("Service/Create")]
        [HttpPost]
        public IActionResult Create(Service collection)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    DB.Services.Add(collection);
                    DB.SaveChanges();
                    Oprationsy Opx = DB.Oprationsys.Where(d => d.Status == collection.Status && d.TableName == "Service").SingleOrDefault();
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
        [Route("Service/Edit")]
        [HttpPost]
        public IActionResult Edit(Service collection)
        {
            if (ModelState.IsValid)
            {
                    Service Service = DB.Services.Where(x => x.Id == collection.Id).SingleOrDefault();
                    Service.Name = collection.Name;
                    Service.Qty = collection.Qty;
                    Service.SellingPrice = collection.SellingPrice;
                    Service.ItemId = collection.ItemId;
                    Service.Type = collection.Type;
                    Service.Description = collection.Description;
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
