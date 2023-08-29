
using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace RestApi.Controllers;

[Authorize]
public class DiscountController : Controller
{
    private readonly IApplicationDbContext DB;
    public DiscountController(IApplicationDbContext dbcontext)
    {
        DB = dbcontext;
    }
    [Route("Discount/GetDiscount")]
    [HttpGet]
    public IActionResult GetDiscount()
    {
        var Discounts = DB.Discount.Select(x => new
        {
            x.Id,
            x.Name,
            x.Type,
            x.Value,
            x.ValueOfDays,
            x.IsPrime,
            x.Status,
            x.Description,
        }).ToList();

        return Ok(Discounts);
    }
    [Route("Discount/GetActiveDiscount")]
    [HttpGet]
    public IActionResult GetActiveDiscount()
    {
        var Discounts = DB.Discount.Select(x => new { value = x.Value, type = x.Type, label = x.Name, x.ValueOfDays }).ToList();

        return Ok(Discounts);
    }
    [Route("Discount/Create")]
    [HttpPost]
    public IActionResult Create(Discount collection)
    {
        if (ModelState.IsValid)
        {
            try
            {
                DB.Discount.Add(collection);
                DB.SaveChanges();
            }
            catch
            {
                //Console.WriteLine(collection);
                return Ok(false);
            }
        }
        return Ok(false);
    }
    [Route("Discount/Edit")]
    [HttpPost]
    public IActionResult Edit(Discount collection)
    {
        if (ModelState.IsValid)
        {
            Discount Discount = DB.Discount.Where(x => x.Id == collection.Id).SingleOrDefault();
            Discount.Name = collection.Name;
            Discount.Value = collection.Value;
            Discount.ValueOfDays = collection.ValueOfDays;
            Discount.Type = collection.Type;
            Discount.Description = collection.Description;
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
