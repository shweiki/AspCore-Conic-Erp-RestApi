using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Entities; 
using Microsoft.AspNetCore.Mvc;

namespace AspCore_Conic_Erp_RestApi.Controllers
{
    [Authorize]
    public class ItemController : Controller
    {
        private ConicErpContext DB = new ConicErpContext();

        [Route("Item/GetItem")]
        [HttpGet]
        public IActionResult GetItem()
        {
            var Items = DB.Items.Select(x => new { x.Id, x.Name, x.Barcode, x.SellingPrice, x.OtherPrice }).ToList();

            return Ok(Items);
        }
        [Route("Item/GetIsPrimeItem")]
        [HttpGet]
        public IActionResult GetIsPrimeItem()
        {
            var Items = (from x in DB.Items.ToList()
                         where (x.IsPrime == true)
                         select new
                         {
                             x.Id,
                             x.Name,
                             x.CostPrice,
                             x.SellingPrice,
                             x.OtherPrice,
                             x.LowOrder,
                             x.Tax,
                             x.IsPrime,
                             x.Rate,
                             x.Barcode,
                             x.Description,
                             InventoryQty = CalculateInventoryItemQty(x.Id),

                         });
            return Ok(Items);
        }
        [Route("Item/GetActiveItem")]
        [HttpGet]
        public IActionResult GetActiveItem()
        {
            var Items = (from x in DB.Items.ToList()
                         where (x.Status == 0)
                         select new
                         {
                             x.Id,
                             x.Name,
                             x.CostPrice,
                             x.SellingPrice,
                             x.OtherPrice,
                             x.LowOrder,
                             x.Tax,
                             x.IsPrime,
                             x.Rate,
                             x.Barcode,
                             x.Description,
                             InventoryQty = CalculateInventoryItemQty(x.Id),

                         });
            return Ok(Items);
        }
        [Route("Item/GetItemMove")]
        [HttpGet]
        public IActionResult GetItemMove(long ItemID, DateTime DateFrom, DateTime DateTo)
        {
            var SalesInvoiceMove = (from x in DB.InventoryMovements.Where(i => i.SalesInvoiceId != null).ToList()
                                    where (x.ItemsId == ItemID)  && (x.SalesInvoice.FakeDate >= DateFrom) && (x.SalesInvoice.FakeDate <= DateTo)
                         let p = new
                         { 
                             x.Id,
                             x.SellingPrice,
                             x.Qty,
                             x.Status,
                             x.Tax,
                             x.TypeMove,
                             Name = x.SalesInvoice?.Vendor?.Name + " " + x.SalesInvoice?.Member?.Name ,
                             FakeDate = x.SalesInvoice.FakeDate.ToString("dd/MM/yyyy") ,
                             x.Description,
                             x.SalesInvoiceId,
                             Type = "مبيعات"
                         }
                         select p);
            var PurchaseInvoiceMove = (from x in DB.InventoryMovements.Where(i => i.PurchaseInvoiceId != null).ToList()
                                       where (x.ItemsId == ItemID) &&  (x.PurchaseInvoice.FakeDate >= DateFrom) && (x.PurchaseInvoice.FakeDate <= DateTo)
                                    let p = new
                                    {
                                        x.Id,
                                        x.SellingPrice,
                                        x.Qty,
                                        x.Status,
                                        x.Tax,
                                        x.TypeMove,
                                        Name = x.PurchaseInvoice?.Vendor?.Name,
                                        FakeDate = x.PurchaseInvoice.FakeDate.Value.ToString("dd/MM/yyyy"),
                                        x.Description,
                                        x.PurchaseInvoiceId,
                                        Type = "مشتريات"

                                    }
                                       select p);
            var OrderInventoryMove = (from x in DB.InventoryMovements.Where(i => i.OrderInventoryId != null).ToList()
                                       where (x.ItemsId == ItemID) && (x.OrderInventory.FakeDate >= DateFrom) && (x.OrderInventory.FakeDate <= DateTo)
                                       let p = new
                                       {
                                           x.Id,
                                           x.SellingPrice,
                                           x.Qty,
                                           x.Status,
                                           x.Tax,
                                           x.TypeMove,
                                           Name = x.OrderInventory?.OrderType,
                                           FakeDate = x.OrderInventory.FakeDate.Value.ToString("dd/MM/yyyy"),
                                           x.Description,
                                           x.OrderInventoryId,
                                           Type = "سند مخزون"

                                       }
                                      select p);
            return Ok(new { OrderInventoryMove, PurchaseInvoiceMove, SalesInvoiceMove });
        }
        [Route("Item/Create")]
        [HttpPost]
        public IActionResult Create(Item collection)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    DB.Items.Add(collection);
                    DB.SaveChanges();
                    Oprationsy Opx = DB.Oprationsys.Where(d => d.Status == collection.Status && d.TableName == "Item").SingleOrDefault();
                    OprationsysController Op = new OprationsysController();
                    if (Op.ChangeStatus(collection.Id, Opx.Id, "<!" + collection.Id + "!>"))
                    {
                        return Ok(true);
                    }
                }
                catch
                {
                    //Console.WriteLine(collection);
                    return Ok(false);
                }
            }
            return Ok(false);
        }

        [Route("Item/Edit")]
        [HttpPost]
        public IActionResult Edit(Item collection)
        {
            if (ModelState.IsValid)
            {
                Item item = DB.Items.Where(x => x.Id == collection.Id).SingleOrDefault();
                item.Name = collection.Name;
                item.CostPrice = collection.CostPrice;
                item.SellingPrice = collection.SellingPrice;
                item.OtherPrice = collection.OtherPrice;
                item.LowOrder = collection.LowOrder;
                item.Tax = collection.Tax;
                item.Barcode = collection.Barcode;
                item.IsPrime = collection.IsPrime;
                item.Description = collection.Description;
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

     
        public dynamic CalculateInventoryItemQty(long? ItemID)
        {
            return from x in DB.InventoryMovements.Where(i => i.ItemsId == ItemID && i.Status == 0).ToList()
                   group x by x.InventoryItemId into g
                   select new
                   {
                       InventoryItemID = g.Key,
                       InventoryName = DB.InventoryItems.Where(a => a.Id == g.Key).Select(c => c.Name).FirstOrDefault(),
                       QtyIn = g.Where(d => d.TypeMove == "In").Sum(qc => qc.Qty),
                       QtyOut = g.Where(d => d.TypeMove == "Out").Sum(qc => qc.Qty)
                   };
        }

    }

}
