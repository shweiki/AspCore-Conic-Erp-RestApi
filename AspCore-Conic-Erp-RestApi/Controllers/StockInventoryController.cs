using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.AspNetCore.Authorization;
using Entities; 

namespace AspCore_Conic_Erp_RestApi.Controllers
{
    [Authorize]

    public class StockInventoryController : Controller
    {
                private ConicErpContext DB;
        public StockInventoryController(ConicErpContext dbcontext)
        {
            DB = dbcontext;
        }
        [Route("StockInventory/GetStockInventory")]
        [HttpGet]
        public IActionResult GetStockInventory(DateTime DateFrom, DateTime DateTo)
        {
            var Orders = DB.StocktakingInventories.Where(i => i.FakeDate >= DateFrom && i.FakeDate <= DateTo).Select(x => new
            {
                x.Id,
                x.FakeDate,
                x.Status,
                x.Description,
                StockMovements = DB.StockMovements.Where(i => i.StocktakingInventoryId == x.Id).Select(m => new
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
                    DB.StocktakingInventories.Add(collection);
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
}
