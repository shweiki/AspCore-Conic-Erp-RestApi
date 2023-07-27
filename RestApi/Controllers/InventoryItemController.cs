using Domain.Entities; using Application.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Linq;


namespace RestApi.Controllers;

[Authorize]
public class InventoryItemController : Controller
{
    private readonly IApplicationDbContext DB;
    public InventoryItemController(IApplicationDbContext dbcontext)
    {
        DB = dbcontext;
    }

    [Route("InventoryItem/Create")]
    [HttpPost]
    public IActionResult Create(InventoryItem collection)
    {
        if (ModelState.IsValid)
        {
            try
            {
                DB.InventoryItem.Add(collection);
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

    [Route("InventoryItem/Edit")]
    [HttpPost]
    public IActionResult Edit(InventoryItem collection)
    {
        if (ModelState.IsValid)
        {
            InventoryItem InventoryItem = DB.InventoryItem.Where(x => x.Id == collection.Id).SingleOrDefault();
            InventoryItem.Name = collection.Name;
            InventoryItem.Description = collection.Description;
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

    [Route("InventoryItem/GetInventoryItem")]
    [HttpGet]
    public IActionResult GetInventoryItem()
    {
        var InventoryItems = DB.InventoryItem.Select(x => new
        {
            x.Id,
            x.Name,
            x.Description,
            x.Status,
            //  InventoryQty = InventoryQty(x.Id)
        }).ToList();

        return Ok(InventoryItems);
    }

    [Route("InventoryItem/GetActiveInventory")]
    [HttpGet]
    public IActionResult GetActiveInventory()
    {
        var InventoryItems = DB.InventoryItem.Where(x => x.Status == 0).Select(x => new { value = x.Id, label = x.Name }).ToList();
        return Ok(InventoryItems);

    }

    [Route("InventoryItem/InventoryQty")]
    [HttpPost]
    public IActionResult InventoryQty(long Id)
    {
        var InventoryQty = (from i in DB.InventoryMovement.Where(o => o.InventoryItemId == Id).ToList()
                            group i by i.ItemsId into g
                            select new
                            {

                                Item = DB.Item.Where(x => x.Id == g.Key).Select(x => new
                                {
                                    x.Id,
                                    x.CostPrice,
                                    x.Name,
                                    x.Barcode
                                }).SingleOrDefault(),
                                QtyIn = g.Where(d => d.TypeMove == "In").Sum(qc => qc.Qty),
                                QtyOut = g.Where(d => d.TypeMove == "Out").Sum(qc => qc.Qty)
                            }).ToList();

        return Ok(InventoryQty);
    }
}
