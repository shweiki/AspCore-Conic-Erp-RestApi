
using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace RestApi.Controllers;

[Authorize]
public class MenuItemController : Controller
{
    private readonly IApplicationDbContext DB;

    public MenuItemController(IApplicationDbContext dbcontext)
    {
        DB = dbcontext;
    }

    [Route("MenuItem/GetMenuItem")]
    [HttpGet]
    public IActionResult GetMenuItem()
    {
        var MenuItems = DB.MenuItem.Select(x => new
        {
            x.Id,
            x.Name,
            x.Description,
            x.IsPrime,
            x.Status,
        }).ToList();

        return Ok(MenuItems);
    }

    [Route("MenuItem/GetActiveMenuItem")]
    [HttpGet]
    public IActionResult GetActiveMenuItem()
    {

        var MenuItems = DB.MenuItem.Where(x => x.Status == 0).Select(x => new { value = x.Name, label = x.Name }).ToList();
        return Ok(MenuItems);
    }


    [Route("MenuItem/Create")]
    [HttpPost]
    public IActionResult Create(MenuItem collection)
    {
        if (ModelState.IsValid)
        {
            try
            {
                DB.MenuItem.Add(collection);
                DB.SaveChanges();
                return Ok(collection);

            }
            catch
            {
                //Console.WriteLine(collection);
                return NotFound();
            }
        }
        return NotFound();
    }

    [Route("MenuItem/Edit")]
    [HttpPost]
    public IActionResult Edit(MenuItem collection)
    {
        if (ModelState.IsValid)
        {
            MenuItem MenuItem = DB.MenuItem.Where(x => x.Id == collection.Id).SingleOrDefault();
            MenuItem.Name = collection.Name;
            MenuItem.Description = collection.Description;
            try
            {
                DB.SaveChanges();
                return Ok();
            }
            catch
            {
                //Console.WriteLine(collection);
                return NotFound();
            }
        }
        return NotFound();
    }

}