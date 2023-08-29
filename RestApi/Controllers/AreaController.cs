
using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace RestApi.Controllers;

[Authorize]
public class AreaController : Controller
{
    private readonly IApplicationDbContext DB;
    public AreaController(IApplicationDbContext dbcontext)
    {
        DB = dbcontext;
    }

    [Route("Area/GetAreas")]
    [HttpGet]
    public IActionResult GetAreas()

    {
        var Areas = DB.Area.Select(x => new
        {
            x.Id,
            x.Adress1,
            x.Adress2,
            x.Adress3,
            x.DelievryPrice,
            x.Status

        }).ToList();

        return Ok(Areas);
    }

    [Route("Area/GetAreasLabel")]
    [HttpGet]
    public IActionResult GetAreasLabel()
    {
        var Areas = DB.Area.Where(x => x.Status == 0).Select(x => new
        {

            value = x.Id,
            label = x.Adress1,
            price = x.DelievryPrice
        }).ToList();
        return Ok(Areas);


    }
    [Route("Area/Create")]
    [HttpPost]
    public IActionResult Create(Area collection)
    {
        if (ModelState.IsValid)
        {
            try
            {
                // TODO: Add insert logic here
                collection.Status = 0;
                DB.Area.Add(collection);
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

    // DELETE: Banks/5
    [Route("Area/Edit")]
    [HttpPost]
    public IActionResult Edit(Area collection)
    {
        if (ModelState.IsValid)
        {
            Area area = DB.Area.Where(x => x.Id == collection.Id).SingleOrDefault();
            area.Adress1 = collection.Adress1;
            area.Adress2 = collection.Adress2;
            area.Adress3 = collection.Adress3;
            area.DelievryPrice = collection.DelievryPrice;
            area.Status = collection.Status;
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