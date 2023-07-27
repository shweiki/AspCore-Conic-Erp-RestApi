using Domain.Entities; using Application.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Linq;

namespace RestApi.Controllers;

[Authorize]

public class OriginItemController : Controller
{
    private readonly IApplicationDbContext DB;
    public OriginItemController(IApplicationDbContext dbcontext)
    {
        DB = dbcontext;
    }
    [Route("OriginItem/GetOriginItem")]
    [HttpGet]
    public IActionResult GetOriginItem()
    {
        var OriginItems = DB.OriginItem.Select(x => new
        {
            x.Id,
            x.Name,
            x.Description,
        }).ToList();

        return Ok(OriginItems);
    }
    [Route("OriginItem/GetActiveOriginItem")]
    [HttpGet]
    public IActionResult GetActiveOriginItem()
    {
        var OriginItems = DB.OriginItem.Select(x => new { value = x.Id, label = x.Name }).ToList();

        return Ok(OriginItems);
    }
    [Route("OriginItem/Create")]
    [HttpPost]
    public IActionResult Create(OriginItem collection)
    {
        if (ModelState.IsValid)
        {
            try
            {
                DB.OriginItem.Add(collection);
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
    [Route("OriginItem/Edit")]
    [HttpPost]
    public IActionResult Edit(OriginItem collection)
    {
        if (ModelState.IsValid)
        {
            OriginItem OriginItem = DB.OriginItem.Where(x => x.Id == collection.Id).SingleOrDefault();
            OriginItem.Name = collection.Name;
            OriginItem.Description = collection.Description;
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
