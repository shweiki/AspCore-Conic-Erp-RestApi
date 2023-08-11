using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace RestApi.Controllers;

[Authorize]
public class ServiceController : Controller
{
    private readonly IApplicationDbContext DB;
    public ServiceController(IApplicationDbContext dbcontext)
    {
        DB = dbcontext;
    }
    [Route("Service/GetService")]
    [HttpGet]
    public IActionResult GetService()
    {
        var Services = DB.Service.Select(x => new
        {
            x.Id,
            x.Name,
            x.Qty,
            x.SellingPrice,
            x.ItemId,
            ItemName = DB.Item.Where(i => i.Id == x.ItemId).SingleOrDefault().Name,
            x.Type,
            x.Status,
            x.Description,
        }).ToList();

        return Ok(Services);
    }
    [Route("Service/GetActiveService")]
    [HttpGet]
    public IActionResult GetActiveService()
    {
        var Services = DB.Service.Where(i => i.Status == 0).Select(x => new
        {

            x.Id,
            x.Name,
            x.Qty,
            x.SellingPrice,
            x.ItemId,
            ItemName = DB.Item.Where(i => i.Id == x.ItemId).SingleOrDefault().Name,
            x.Type,
            x.Status,
            x.Description,
        }).ToList();


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
                DB.Service.Add(collection);
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
    [Route("Service/Edit")]
    [HttpPost]
    public IActionResult Edit(Service collection)
    {
        if (ModelState.IsValid)
        {
            Service Service = DB.Service.Where(x => x.Id == collection.Id).SingleOrDefault();
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
