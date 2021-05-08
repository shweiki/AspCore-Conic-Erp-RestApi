using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.AspNetCore.Authorization;
using Entities; 

namespace AspCore_Conic_Erp_RestApi.Controllers
{
    [Authorize]

    public class OrderInventoryController : Controller
    {
        private ConicErpContext DB = new ConicErpContext();
        [Route("OrderInventory/GetOrderInventory")]
        [HttpGet]
        public IActionResult GetOrderInventory(DateTime DateFrom, DateTime DateTo)
        {
            var Orders = DB.OrderInventories.Where(i => i.FakeDate >= DateFrom && i.FakeDate <= DateTo).Select(x => new {
                x.Id,
                FakeDate = x.FakeDate.Value.ToString("dd/MM/yyyy"),
                x.OrderType,
                x.Status,
                x.Description,
                InventoryMovements = DB.InventoryMovements.Where(i => i.OrderInventoryId == x.Id).Select(m => new
                {
                    m.Id,
                    m.Items.Name,
                    m.Qty,
                    InventoryName = m.InventoryItem.Name,
                    m.Description
                }).ToList()
            }).ToList(); 
                        
            return Ok(Orders);
        }


        [HttpPost]
        [Route("OrderInventory/Create")]
        public IActionResult Create(OrderInventory collection)
        {
            if (ModelState.IsValid)
            {
                collection.Status = 0;
                try
                {
                    // TODO: Add insert logic here
                    DB.OrderInventories.Add(collection);
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
        [Route("OrderInventory/Edit")]
        [HttpPost]
        public IActionResult Edit(OrderInventory collection)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    OrderInventory Order = DB.OrderInventories.Where(x => x.Id == collection.Id).SingleOrDefault();

                    Order.OrderType = collection.OrderType;
                    Order.Description = collection.Description;
                    Order.Status = collection.Status;
                    Order.FakeDate = collection.FakeDate;
                    DB.InventoryMovements.RemoveRange(Order.InventoryMovements);
                    Order.InventoryMovements = collection.InventoryMovements;
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
        [Route("OrderInventory/GetOrderInventoryByID")]
        [HttpGet]
        public IActionResult GetOrderInventoryByID(long? ID)
        {
            var Orders = DB.OrderInventories.Where(i => i.Id == ID).Select(x => new {

                x.Id,
                x.FakeDate,
                x.OrderType,
                x.Status,
                x.Description,
                InventoryMovements = DB.InventoryMovements.Where(i => i.OrderInventoryId == x.Id).Select(m => new {
                    m.Id,
                    m.Qty,
                    m.ItemsId,
                    m.TypeMove,
                    m.Items.Name,
                    m.InventoryItemId,
                    InventoryName = m.InventoryItem.Name,
                    m.Description
                }).ToList()
            }).SingleOrDefault();
                      

            return Ok(Orders);
        }

    }
}
