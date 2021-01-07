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
            var Items = DB.Items.Select(x => new { x.Id, x.Name, x.Barcode, x.SellingPrice, x.OtherPrice ,x.CostPrice }).ToList();

            return Ok(Items);
        }
        [Route("Item/GetIsPrimeItem")]
        [HttpGet]
        public IActionResult GetIsPrimeItem()
        {
            var Items = DB.Items.Where(i => i.IsPrime == true).Select(x => new {
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
             //   InventoryQty = CalculateInventoryItemQty(x.Id),
            }).ToList();
                         
          
            return Ok(Items);
        }
        [Route("Item/GetActiveItem")]
        [HttpGet]
        public IActionResult GetActiveItem()
        {
            var Items = DB.Items.Where(i => i.Status == 0).Select(x => new {
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
               // InventoryQty = CalculateInventoryItemQty(x.Id),
            }).ToList();

            return Ok(Items);
        }
        [Route("Item/GetItemMove")]
        [HttpGet]
        public IActionResult GetItemMove(long ItemID, DateTimeOffset DateFrom, DateTimeOffset DateTo)
        {
            var SalesInvoiceMove = DB.InventoryMovements.Where(i => i.SalesInvoiceId != null && i.ItemsId == ItemID && i.SalesInvoice.FakeDate >= DateFrom && i.SalesInvoice.FakeDate <= DateTo).Select(x => new
            {
                x.Id,
                x.SellingPrice,
                x.Qty,
                x.Status,
                x.Tax,
                x.TypeMove,
                Name = DB.SalesInvoices.Where(S=>S.VendorId == x.SalesInvoice.VendorId).SingleOrDefault().Name + " " + DB.SalesInvoices.Where(S => S.MemberId == x.SalesInvoice.MemberId).SingleOrDefault().Name,
                FakeDate = x.SalesInvoice.FakeDate.ToString("dd/MM/yyyy"),
                x.Description,
                x.SalesInvoiceId,
                x.ItemsId,
                Type = "مبيعات"
            }).ToList();

            var PurchaseInvoiceMove = DB.InventoryMovements.Where(i => i.PurchaseInvoiceId != null && i.ItemsId == ItemID && i.PurchaseInvoice.FakeDate >= DateFrom && i.PurchaseInvoice.FakeDate <= DateTo).Select(x => new
            {
                x.Id,
                x.SellingPrice,
                x.Qty,
                x.Status,
                x.Tax,
                x.TypeMove,
                Name = DB.PurchaseInvoices.Where(S => S.VendorId == x.PurchaseInvoice.VendorId).SingleOrDefault().Name + " ",
                FakeDate = x.PurchaseInvoice.FakeDate.Value.ToString("dd/MM/yyyy"),
                x.Description,
                x.PurchaseInvoiceId,
                Type = "مشتريات",
                                x.ItemsId
            }).ToList();

            var OrderInventoryMove = DB.InventoryMovements.Where(i => i.OrderInventoryId != null && i.ItemsId == ItemID && i.OrderInventory.FakeDate >= DateFrom && i.OrderInventory.FakeDate <= DateTo).Select(x => new {
                x.Id,
                x.SellingPrice,
                x.Qty,
                x.Status,
                x.Tax,
                x.TypeMove,
                Name =  "سند " +x.OrderInventory.OrderType,
                FakeDate = x.OrderInventory.FakeDate.Value.ToString("dd/MM/yyyy"),
                x.Description,
                x.OrderInventoryId,
                Type = "سند مخزون",
                                x.ItemsId
            }).ToList();

            return Ok(new { OrderInventoryMove, PurchaseInvoiceMove, SalesInvoiceMove });
        }

        [Route("Item/GetItemByID")]
        [HttpGet]
        public IActionResult GetItemByID(long ID)
        {
            var Item = DB.Items.Where(x=>x.Id == ID).Select(x=> new 
                        {
                            x.Id,
                            x.Name,
                            x.CostPrice,
                            x.SellingPrice,
                            x.OtherPrice,
                            x.LowOrder,
                            x.Tax,
                            x.Rate,
                            x.Barcode,
                            x.Description,
                            x.IsPrime,
                            x.Status,
                          //  InventoryQty = CalculateInventoryItemQty(x.Id)
                        }).SingleOrDefault();
            return Ok(Item);
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
                    return Ok(collection);

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

        [Route("Item/CalculateInventoryItemQty")]
        [HttpPost]
        public IActionResult CalculateInventoryItemQty(long ID)
        {
            return Ok(from x in DB.InventoryMovements.Where(i => i.ItemsId == ID && i.Status == 0).ToList()
                   group x by x.InventoryItemId into g
                   select new
                   {
                       InventoryItemID = g.Key,
                       InventoryName = DB.InventoryItems.Where(a => a.Id == g.Key).Select(c => c.Name).FirstOrDefault(),
                       QtyIn = g.Where(d => d.TypeMove == "In").Sum(qc => qc.Qty),
                       QtyOut = g.Where(d => d.TypeMove == "Out").Sum(qc => qc.Qty)
                   });
        }

    }

}
