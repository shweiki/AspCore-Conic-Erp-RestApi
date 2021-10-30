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
                private ConicErpContext DB;
        public OrderInventoryController(ConicErpContext dbcontext)
        {
            DB = dbcontext;
        }

        [HttpPost]
        [Route("OrderInventory/GetByListQ")]
        public IActionResult GetByListQ(int Limit, string Sort, int Page, string User, DateTime? DateFrom, DateTime? DateTo, int? Status, string Any)
        {
            var OrderInventory = DB.OrderInventories.Where(s => (Any != null ? s.Id.ToString().Contains(Any) || s.OrderType.Contains(Any) || s.Description.Contains(Any) : true) && (DateFrom != null ? s.FakeDate >= DateFrom : true)
            && (DateTo != null ? s.FakeDate <= DateTo : true) && (Status != null ? s.Status == Status : true) &&  (User != null ? DB.ActionLogs.Where(l => l.OrderInventoryId == s.Id && l.UserId == User).SingleOrDefault() != null : true)).Select(x => new
            {
                x.Id,
                x.FakeDate,
                x.OrderType,
                x.Status,
                x.Description,
                InventoryMovements = x.InventoryMovements.Select(imx => new {
                    imx.Id,
                    imx.ItemsId,
                    imx.Items.Name,
                    imx.Items.Ingredients,
                    imx.Items.CostPrice,
                    imx.TypeMove,
                    imx.InventoryItemId,
                    imx.Qty,
                    imx.EXP,
                    imx.SellingPrice,
                    imx.Description
                }).ToList(),
            }).ToList();
            OrderInventory = (Sort == "+id" ? OrderInventory.OrderBy(s => s.Id).ToList() : OrderInventory.OrderByDescending(s => s.Id).ToList());
            return Ok(new
            {
                items = OrderInventory.Skip((Page - 1) * Limit).Take(Limit).ToList(),
                Totals = new
                {
                    Rows = OrderInventory.Count()                   
                }
            });
        }

        [Route("OrderInventory/GetByItem")]
        [HttpGet]
        public IActionResult GetByItem(long ItemId, int Limit, string Sort, int Page, string User, DateTime? DateFrom, DateTime? DateTo, int? Status, string Any, string Type)
        {
            var Invoices = DB.InventoryMovements.Where(s => s.OrderInventoryId != null && s.ItemsId == ItemId && (Any != null ? s.Id.ToString().Contains(Any)  || s.Description.Contains(Any) || s.OrderInventory.Description.Contains(Any) || s.OrderInventory.OrderType.Contains(Any) : true) && (DateFrom != null ? s.OrderInventory.FakeDate >= DateFrom : true)
              && (DateTo != null ? s.OrderInventory.FakeDate <= DateTo : true) && (Status != null ? s.Status == Status : true) && (Type != null ? s.OrderInventory.OrderType == Type : true) && (User != null ? DB.ActionLogs.Where(l => l.InventoryMovementId == s.Id && l.UserId == User).SingleOrDefault() != null : true)).Select(x => new
              {
                  x.Id,
                  x.OrderInventoryId,
                  x.OrderInventory.FakeDate,
                  x.OrderInventory.OrderType,
                  x.Status,
                  x.Description,
                  InventoryMovements = x.OrderInventory.InventoryMovements.Select(imx => new {
                      imx.Id,
                      imx.ItemsId,
                      imx.Items.Name,
                      imx.Items.Ingredients,
                      imx.Items.CostPrice,
                      imx.TypeMove,
                      imx.InventoryItemId,
                      imx.Qty,
                      imx.EXP,
                      imx.SellingPrice,
                      imx.Description
                  }).ToList(),
              }).ToList();
            Invoices = (Sort == "+id" ? Invoices.OrderBy(s => s.Id).ToList() : Invoices.OrderByDescending(s => s.Id).ToList());
            return Ok(new
            {
                items = Invoices.Skip((Page - 1) * Limit).Take(Limit).ToList(),
                Totals = new
                {
                    Rows = Invoices.Count(),
                }
            });


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
                    DB.InventoryMovements.RemoveRange(DB.InventoryMovements.Where(x => x.OrderInventoryId == Order.Id).ToList());
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
        [Route("OrderInventory/GetOrderInventoryById")]
        [HttpGet]
        public IActionResult GetOrderInventoryById(long? Id)
        {
            var Orders = DB.OrderInventories.Where(i => i.Id == Id).Select(x => new {

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
                    m.EXP,
                    m.InventoryItemId,
                    InventoryName = m.InventoryItem.Name,
                    m.Description
                }).ToList()
            }).SingleOrDefault();
                      

            return Ok(Orders);
        }

    }
}
