using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Entities; 
using Microsoft.AspNetCore.Mvc;

namespace AspCore_Conic_Erp_RestApi.Controllers
{
    [Authorize]
    public class UnitItemController : Controller
    {
                private ConicErpContext DB;
        public UnitItemController(ConicErpContext dbcontext)
        {
            DB = dbcontext;
        }
        [Route("UnitItem/GetUnitItem")]
        [HttpGet]
        public IActionResult GetUnitItem()
        {
            var UnitItems = DB.UnitItems.Select(x => new
            {
                x.Id,
                x.Name,
                x.Description,
            }).ToList();

            return Ok(UnitItems);
        }
        [Route("UnitItem/GetActiveUnitItem")]
        [HttpGet]
        public IActionResult GetActiveUnitItem()
        {
            var UnitItems = DB.UnitItems.Where(i=>i.Status==0).Select(x => new { value = x.Name, label = x.Name }).ToList();

            return Ok(UnitItems);
        }
        [Route("UnitItem/Create")]
        [HttpPost]
        public IActionResult Create(UnitItem collection)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    DB.UnitItems.Add(collection);
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
        [Route("UnitItem/Edit")]
        [HttpPost]
        public IActionResult Edit(UnitItem collection)
        {
            if (ModelState.IsValid)
            {
                    UnitItem UnitItem = DB.UnitItems.Where(x => x.Id == collection.Id).SingleOrDefault();
                    UnitItem.Name = collection.Name;
                    UnitItem.Description = collection.Description;
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
