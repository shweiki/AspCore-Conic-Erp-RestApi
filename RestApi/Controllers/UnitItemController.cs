
using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace RestApi.Controllers;

[Authorize]
public class UnitItemController : Controller
{
    private readonly IApplicationDbContext DB;
    public UnitItemController(IApplicationDbContext dbcontext)
    {
        DB = dbcontext;
    }
    [Route("UnitItem/GetUnitItem")]
    [HttpGet]
    public IActionResult GetUnitItem()
    {
        var UnitItems = DB.UnitItem.Select(x => new
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
        var UnitItems = DB.UnitItem.Where(i => i.Status == 0).Select(x => new { value = x.Name, label = x.Name }).ToList();

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
                DB.UnitItem.Add(collection);
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
            UnitItem UnitItem = DB.UnitItem.Where(x => x.Id == collection.Id).SingleOrDefault();
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
