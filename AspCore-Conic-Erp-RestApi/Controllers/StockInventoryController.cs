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
        private ConicErpContext DB = new ConicErpContext();
        [Route("StockInventory/GetStockInventory")]
        [HttpGet]
        public IActionResult GetStockInventory(DateTime DateFrom, DateTime DateTo)
        {
            var Orders = (from x in DB.StocktakingInventories.ToList()
                            where (x.FakeDate >= DateFrom) && (x.FakeDate <= DateTo)
                            let p = new
                            {
                                x.Id,
                                FakeDate = x.FakeDate.ToString("dd/MM/yyyy"),
                                x.Status,
                                x.Description,
                                StockMovements = (from m in DB.StockMovements.ToList()
                                                      where (m.StocktakingInventoryId == x.Id)
                                                      select new
                                                      {
                                                          m.Id,
                                                          m.ItemsId,
                                                          m.Items.Name,
                                                          m.Items.Barcode,
                                                          m.Qty,
                                                          m.InventoryItemId,
                                                          InventoryName =  m.InventoryItem.Name,
                                                          m.Description
                                                      }),
                            
                            }
                            select p);

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
                    Oprationsy Opx = DB.Oprationsys.Where(d => d.Status == collection.Status && d.TableName == "StocktakingInventory").SingleOrDefault();
                    OprationsysController Op = new OprationsysController();
                    if (Op.ChangeStatus(collection.Id, Opx.Id, "<!" + collection.Id + "!>"))
                    {
                        return Ok(true);
                    }
                    else return Ok(false);
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
