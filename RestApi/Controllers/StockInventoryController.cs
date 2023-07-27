using Domain.Entities; using Application.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data;
using System.Linq;

namespace RestApi.Controllers;

[Authorize]

public class StockInventoryController : Controller
{
    private readonly IApplicationDbContext DB;
    public StockInventoryController(IApplicationDbContext dbcontext)
    {
        DB = dbcontext;
    }
    [Route("StockInventory/GetStockInventory")]
    [HttpGet]
    public IActionResult GetStockInventory(DateTime DateFrom, DateTime DateTo)
    {
        var Orders = DB.StocktakingInventory.Where(i => i.FakeDate >= DateFrom && i.FakeDate <= DateTo).Select(x => new
        {
            x.Id,
            x.FakeDate,
            x.Status,
            x.Description,
            StockMovements = DB.StockMovement.Where(i => i.StocktakingInventoryId == x.Id).Select(m => new
            {
                m.Id,
                m.ItemsId,
                m.Items.Name,
                m.Items.Barcode,
                m.Qty,
                m.InventoryItemId,
                InventoryName = m.InventoryItem.Name,
                m.Description
            }).ToList()


        }).ToList();


        return Ok(Orders);
    }


    [HttpPost]
    [Route("StockInventory/Create")]
    public IActionResult Create(StocktakingInventory collection)
    {
        if (ModelState.IsValid)
        {
            collection.Status = 0;
            try
            {
                // TODO: Add insert logic here
                DB.StocktakingInventory.Add(collection);
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


}
