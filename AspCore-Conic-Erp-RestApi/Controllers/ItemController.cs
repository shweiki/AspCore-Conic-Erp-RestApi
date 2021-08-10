using Entities; 
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NinjaNye.SearchExtensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AspCore_Conic_Erp_RestApi.Controllers
{
    [Authorize]
    public class ItemController : Controller
    {
        private ConicErpContext DB = new ConicErpContext();
        [HttpGet]
        [Route("Item/GetItem")]
        public IActionResult GetItem()
        {
            var Items = DB.Items.Select(x => new { x.Id, x.Name, x.Barcode, x.SellingPrice, x.OtherPrice ,x.CostPrice }).ToList();

            return Ok(Items);
        }
        [HttpPost]
        [Route("Item/GetByListQ")]
        public IActionResult GetByListQ(int Limit, string Sort, int Page, int? Status, string? Any)
        {
            var Items = DB.Items.Where(s => (Any != null ? s.Id.ToString().Contains(Any) || s.Name.Contains(Any) || s.Category.Contains(Any) || s.Barcode.Contains(Any) : true) && (Status != null ? s.Status == Status : true))
              .Select(x=>new   {
                x.Id,
                x.Name,
                x.CostPrice,
                x.SellingPrice,
                x.OtherPrice,
                x.LowOrder,
                x.Tax,
                x.Status,
                x.IsPrime,
                x.Rate,
                x.Barcode,
                x.Category,
                x.Description,
                x.Ingredients,
                TotalIn = x.InventoryMovements.Where(x => x.TypeMove == "In").Sum(s => s.Qty),
                TotalOut = x.InventoryMovements.Where(x => x.TypeMove == "Out").Sum(s => s.Qty),
              }).ToList();
            Items = (Sort == "+id" ? Items.OrderBy(s => s.Id).ToList() : Items.OrderByDescending(s => s.Id).ToList());
            return Ok(new
            {
                items = Items.Skip((Page - 1) * Limit).Take(Limit).ToList(),
                Totals = new
                {
                    Rows = Items.Count(),
                    TotalIn = Items.Sum(s => s.TotalIn),
                    TotalOut = Items.Sum(s => s.TotalOut),
                    Totals = Items.Sum(s => s.TotalIn) - Items.Sum(s => s.TotalOut),

                }
            });
        }
        [Route("Item/GetItemByAny")]
        [HttpGet]
        public IActionResult GetItemByAny(string Any)
        {
            if (Any == null) return NotFound();
           Any =  Any.ToLower();
            var Items = DB.Items.Search(x => x.Name, x =>x.Barcode , x=> x.Id.ToString() ,x=>x.Category ).Containing(Any)
                .Select(x => new { x.Id, x.Name, x.Barcode, x.SellingPrice, x.OtherPrice, x.CostPrice, x.Category }).ToList();

            return Ok(Items);
        }
        [HttpGet]
        [Route("Item/CheckItemIsExist")]
        public IActionResult CheckItemIsExist(string? Name, string? BarCode)
        {
            var Items = DB.Items.Where(m => (BarCode != null ? m.Barcode.ToLower() == BarCode.ToLower() : false)
             ||(Name != null ? m.Name.ToLower()== Name.ToLower() : false)).ToList();

            return Ok(Items.Count() > 0 ? true : false);
        }
        [HttpGet]
        [Route("Item/GetIsPrimeItem")]
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
                x.Category,
                x.Description,
                x.Ingredients,
             //   InventoryQty = CalculateInventoryItemQty(x.Id),
            }).ToList();
                         
          
            return Ok(Items);
        }
        [HttpPost]
        [Route("Item/GetLowOrder")]
        public IActionResult GetLowOrder(int Limit, string Sort, int Page, int? Status, string? Any)
        {
            var Items = DB.Items.Where(s => (s.InventoryMovements != null ?
            s.InventoryMovements.Where(d => d.TypeMove == "In").Sum(qc => qc.Qty) - s.InventoryMovements.Where(d => d.TypeMove == "Out").Sum(qc => qc.Qty) < s.LowOrder
            : false) && (Any != null ? s.Id.ToString().Contains(Any) || s.Name.Contains(Any) || s.Category.Contains(Any) || s.Barcode.Contains(Any) : true) && (Status != null ? s.Status == Status : true))
             .Select(x => new {
          x.Id,
          x.Name,
          x.CostPrice,
          x.SellingPrice,
          x.OtherPrice,
          x.LowOrder,
          x.Tax,
          x.Status,
          x.IsPrime,
          x.Rate,
          x.Barcode,
          x.Category,
          x.Description,
          x.Ingredients,
          TotalIn = x.InventoryMovements.Where(x => x.TypeMove == "In").Sum(s => s.Qty),
          TotalOut = x.InventoryMovements.Where(x => x.TypeMove == "Out").Sum(s => s.Qty),
      }
).ToList();
            Items = (Sort == "+id" ? Items.OrderBy(s => s.Id).ToList() : Items.OrderByDescending(s => s.Id).ToList());
            return Ok(new
            {
                items = Items.Skip((Page - 1) * Limit).Take(Limit).ToList(),
                Totals = new
                {
                    Rows = Items.Count(),
                    TotalIn = Items.Sum(s => s.TotalIn),
                    TotalOut = Items.Sum(s => s.TotalOut),
                    Totals = Items.Sum(s => s.TotalIn) - Items.Sum(s => s.TotalOut),

                }
            });
        }
        [Route("Item/GetEXP")]
        [HttpPost]
        public IActionResult GetEXP(DateTime? DateFrom, DateTime? DateTo, int Limit, string Sort, int Page, int? Status, string? Any)
        {
            var Items = DB.InventoryMovements.Where(s => (DateTo != null ? s.EXP <= DateTo : true)&& (DateFrom != null ? s.EXP >= DateFrom : true))
            .Select(x => new {
                x.Items.Id,
                x.Items.Name,
                x.Items.CostPrice,
                x.SellingPrice,
                x.Items.OtherPrice,
                x.Items.LowOrder,
                x.Items.Tax,
                x.Status,
                x.Items.IsPrime,
                x.Items.Rate,
                x.Items.Barcode,
                x.Items.Category,
                x.Description,
                x.Items.Ingredients,
                TotalIn = x.Items.InventoryMovements.Where(x => x.TypeMove == "In").Sum(s=>s.Qty),
                TotalOut = x.Items.InventoryMovements.Where(x => x.TypeMove == "Out").Sum(s => s.Qty),
            }).ToList();
            Items = (Sort == "+id" ? Items.OrderBy(s => s.Id).ToList() : Items.OrderByDescending(s => s.Id).ToList());
            return Ok(new
            {
                items = Items.Skip((Page - 1) * Limit).Take(Limit).ToList(),
                Totals = new
                {
                    Rows = Items.Count(),
                    TotalIn = Items.Sum(s => s.TotalIn),
                    TotalOut = Items.Sum(s => s.TotalOut),
                    Totals = Items.Sum(s => s.TotalIn) - Items.Sum(s => s.TotalOut),

                }
            });
           
        }
        [HttpGet]
        [Route("Item/GetActiveItem")]
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
                x.Category,
                x.Ingredients,
                // InventoryQty = CalculateInventoryItemQty(x.Id),
            }).ToList();

            return Ok(Items);
        }
        [HttpGet]
        [Route("Item/GetItemMove")]
        public IActionResult GetItemMove(long ItemId, DateTime DateFrom, DateTime DateTo)
        {
            var SalesInvoiceMove = DB.InventoryMovements.Where(i => i.SalesInvoiceId != null && i.ItemsId == ItemId && i.SalesInvoice.FakeDate >= DateFrom && i.SalesInvoice.FakeDate <= DateTo).Select(x => new
            {
                x.Id,
                x.SellingPrice,
                x.Qty,
                x.Status,
                x.Tax,
                x.TypeMove,
                Name = DB.SalesInvoices.Where(S=>S.VendorId == x.SalesInvoice.VendorId).SingleOrDefault().Name + " " + DB.SalesInvoices.Where(S => S.MemberId == x.SalesInvoice.MemberId).SingleOrDefault().Name,
                x.SalesInvoice.FakeDate,
                x.Description,
                x.SalesInvoiceId,
                x.ItemsId,
                Type = "مبيعات"
            }).ToList();

            var PurchaseInvoiceMove = DB.InventoryMovements.Where(i => i.PurchaseInvoiceId != null && i.ItemsId == ItemId && i.PurchaseInvoice.FakeDate >= DateFrom && i.PurchaseInvoice.FakeDate <= DateTo).Select(x => new
            {
                x.Id,
                x.SellingPrice,
                x.Qty,
                x.Status,
                x.Tax,
                x.TypeMove,
                Name = DB.PurchaseInvoices.Where(S => S.VendorId == x.PurchaseInvoice.VendorId).SingleOrDefault().Name + " ",
                x.PurchaseInvoice.FakeDate,
                x.Description,
                x.PurchaseInvoiceId,
                Type = "مشتريات",
                x.ItemsId
            }).ToList();

            var OrderInventoryMove = DB.InventoryMovements.Where(i => i.OrderInventoryId != null && i.ItemsId == ItemId && i.OrderInventory.FakeDate >= DateFrom && i.OrderInventory.FakeDate <= DateTo).Select(x => new {
                x.Id,
                x.SellingPrice,
                x.Qty,
                x.Status,
                x.Tax,
                x.TypeMove,
                Name =  "سند " +x.OrderInventory.OrderType,
                x.OrderInventory.FakeDate,
                x.Description,
                x.OrderInventoryId,
                Type = "سند مخزون",
                x.ItemsId
            }).ToList();

            return Ok(new { OrderInventoryMove, PurchaseInvoiceMove, SalesInvoiceMove });
        }
        [HttpGet]
        [Route("Item/GetItemById")]
        public IActionResult GetItemById(long Id)
        {
            var Item = DB.Items.Where(x=>x.Id == Id).Select(x=> new 
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
                            x.Category,
                            x.Ingredients,

                //  InventoryQty = CalculateInventoryItemQty(x.Id)
            }).SingleOrDefault();
            return Ok(Item);
        }
        [HttpGet]
        [Route("Item/GetItemByBarcode")]
        public IActionResult GetItemByBarcode(string? BarCode )
        {
            var Item = DB.Items.Where(x=>x.Barcode == BarCode).Select(x=> new 
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
                            x.Category,
                            x.Ingredients,
                //  InventoryQty = CalculateInventoryItemQty(x.Id)
            }).FirstOrDefault();
            if(Item != null)
            return Ok(Item);
            else return Ok(false);
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
                item.Category = collection.Category;
                item.Ingredients = collection.Ingredients;

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
        public IActionResult CalculateInventoryItemQty(long Id)
        {
            var InventoryItemsQty = from x in DB.InventoryMovements.Where(i => i.ItemsId == Id && i.Status >= 0).ToList()
                                    group x by x.InventoryItemId into g
                                    select new
                                    {
                                        InventoryItemId = g.Key,
                                        InventoryName = DB.InventoryItems.Where(a => a.Id == g.Key).Select(c => c.Name).FirstOrDefault(),
                                        QtyIn = g.Where(d => d.TypeMove == "In").Sum(qc => qc.Qty),
                                        QtyOut = g.Where(d => d.TypeMove == "Out").Sum(qc => qc.Qty)
                                    };

            return Ok(InventoryItemsQty);
        }
        [Route("Item/GetInventoryItemEXP")]
        [HttpPost]
        public IActionResult GetInventoryItemEXP(long Id)
        {
            var InventoryItemsExp = from x in DB.InventoryMovements.Where(i => i.ItemsId == Id && i.Status == 0).ToList()
                                    group x by x.EXP into g
                                    select new
                                    {
                                        Exp = g.Key,
                                    };

            return Ok(InventoryItemsExp);
        }
        
       [HttpGet]

        [Route("Item/CalculateCostPrice")]
        public  IActionResult CalculateCostPrice()
        {
            DB.InventoryMovements.Where(i=> i.PurchaseInvoiceId != null).ToList().ForEach(s => DB.Items.Where(x => x.Id == s.ItemsId).SingleOrDefault().CostPrice = s.SellingPrice);

            DB.SaveChanges();
            return Ok(true);
        }
    }

}
