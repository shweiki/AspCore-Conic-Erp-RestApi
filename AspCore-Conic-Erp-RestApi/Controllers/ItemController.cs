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
          private ConicErpContext DB;
        public ItemController(ConicErpContext dbcontext)
        {
            DB = dbcontext;
        }
        [HttpGet]
        [Route("Item/GetItem")]
        public IActionResult GetItem()
        {
            var Items = DB.Items.Select(x => new { x.Id, x.Name,x.Address , x.Model ,x.Type,x.SN, x.Barcode, x.SellingPrice, x.OtherPrice ,x.CostPrice , x.TakeBon }).ToList();

            return Ok(Items);
        }
        [HttpPost]
        [Route("Item/GetByListQ")]
        public IActionResult GetByListQ(int Limit, string Sort, int Page, int? Status, string Any)
        {
            var Items = DB.Items.Where(s => (Any != null ? s.Id.ToString().Contains(Any) || s.Name.Contains(Any) || s.MenuItem.Contains(Any) || s.Barcode.Contains(Any) : true) && (Status != null ? s.Status == Status : true))
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
                x.SN,
                x.Type,
                x.Address,
                x.Model,
                x.Barcode,
                x.UnitItem,
                x.Description,
                x.Ingredients,
                x.TakeBon,
                TotalIn =  DB.InventoryMovements.Where(i => i.TypeMove == "In" && i.ItemsId == x.Id).Sum(s => s.Qty),
                TotalOut = DB.InventoryMovements.Where(i => i.TypeMove == "Out" && i.ItemsId == x.Id).Sum(s => s.Qty),
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
            var Items = DB.Items.Search(x => x.Name, x =>x.Barcode , x=> x.Id.ToString() ,x=>x.MenuItem, x=>x.Address , x => x.Model, x => x.SN, x => x.Type).Containing(Any)
                .Select(x => new { x.Id, x.Name,
                    x.SN,
                    x.Type,
                    x.Address,
                    x.Model,
                    x.Barcode, x.SellingPrice, x.OtherPrice, x.CostPrice, x.MenuItem }).ToList();

            return Ok(Items);
        }
        [HttpGet]
        [Route("Item/CheckItemIsExist")]
        public IActionResult CheckItemIsExist(string Name, string BarCode ,string Sn)
        {
            var Items = DB.Items.Where(m => (BarCode != null ? m.Barcode.ToLower() == BarCode.ToLower() : false)
             ||(Name != null ? m.Name.ToLower()== Name.ToLower() : false)
             || (Sn != null ? m.SN.ToLower() == Sn.ToLower() : false)).ToList();

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
                x.SN,
                x.Type,
                x.Address,
                x.Model,
                x.Barcode,
                x.UnitItem,
                x.Description,
                x.Ingredients,
                x.TakeBon,
             //   InventoryQty = CalculateInventoryItemQty(x.Id),
            }).ToList();
                         
          
            return Ok(Items);
        }
        [HttpPost]
        [Route("Item/GetLowOrder")]
        public IActionResult GetLowOrder(int Limit, string Sort, int Page, int? Status, string Any)
        {
            var Items = DB.Items.Where(s => (s.InventoryMovements != null ?
            s.InventoryMovements.Where(d => d.TypeMove == "In").Sum(qc => qc.Qty) - s.InventoryMovements.Where(d => d.TypeMove == "Out").Sum(qc => qc.Qty) < s.LowOrder
            : false) && (Any != null ? s.Id.ToString().Contains(Any) || s.Name.Contains(Any) || s.MenuItem.Contains(Any) || s.Barcode.Contains(Any) : true) && (Status != null ? s.Status == Status : true))
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
            x.SN,
            x.Type,
            x.Address,
            x.Model,
            x.MenuItem,
            x.UnitItem,
          x.Description,
          x.Ingredients,
          x.TakeBon,
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
        [Route("Item/GetEXP")]
        [HttpPost]
        public IActionResult GetEXP(DateTime? DateFrom, DateTime? DateTo, int Limit, string Sort, int Page, int? Status, string Any)
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
                x.Items.MenuItem,
                x.Items.UnitItem,
                x.Description,
                x.Items.Ingredients,
                x.Items.TakeBon,
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
                x.SN,
                x.Type,
                x.Address,
                x.Model,
                x.Barcode,
                x.Description,
                x.MenuItem,
                x.UnitItem,
                x.Ingredients,
                x.TakeBon,
                // InventoryQty = CalculateInventoryItemQty(x.Id),
            }).ToList();

            return Ok(Items);
        }
        [HttpGet]
        [Route("Item/GetItemMove")]
        public IActionResult GetItemMove(long? ItemId, long? MergeItemId, DateTime DateFrom, DateTime DateTo)
        {
            var Movements = DB.InventoryMovements.Where(s => (MergeItemId != null ? s.ItemsId == ItemId || s.ItemsId == MergeItemId : s.ItemsId == ItemId)
                 &&( (s.SalesInvoice.FakeDate >= DateFrom && s.SalesInvoice.FakeDate <= DateTo)
                 || (s.PurchaseInvoice.FakeDate >= DateFrom && s.PurchaseInvoice.FakeDate <= DateTo)
                 || (s.OrderInventory.FakeDate >= DateFrom && s.OrderInventory.FakeDate <= DateTo)
                 || (s.WorkShop.FakeDate >= DateFrom && s.WorkShop.FakeDate <= DateTo)))
                                .Select(x => new { x }).AsEnumerable()
                            .Select(x => new
                            {
                                x.x.Id,
                                In =  x.x.TypeMove == "In" ? x.x.Qty : 0,
                                Out = x.x.TypeMove == "Out" ? x.x.Qty : 0,
                                x.x.ItemsId,
                                x.x.TypeMove,
                                x.x.Description,
                                x.x.Qty,
                                x.x.SellingPrice,
                                x.x.Status,
                               // FakeDate = x.SalesInvoice?.FakeDate +""+ x.OrderInventory?.FakeDate+""+ x.PurchaseInvoice?.FakeDate+""+ x.WorkShop?.FakeDate,
                                TotalRow = 0,
                                FkObject = GetFkObject(x.x)
                            }).ToList();
                double AllTotal = DB.InventoryMovements.Where(s => (MergeItemId != null ? s.ItemsId == ItemId || s.ItemsId == MergeItemId : s.ItemsId == ItemId)).Where(x => x.TypeMove == "In").Sum(s => s.Qty) -
                DB.InventoryMovements.Where(s => (MergeItemId != null ? s.ItemsId == ItemId || s.ItemsId == MergeItemId : s.ItemsId == ItemId)).Where(x=>x.TypeMove=="Out").Sum(s => s.Qty);
                if (AllTotal != (Movements.Sum(s => s.In) - Movements.Sum(s => s.Out)))
                {
                    double Balancecarried = AllTotal - (Movements.Sum(s => s.In) - Movements.Sum(s => s.Out));
                var p = new 
                {
                    Id = Convert.ToInt64(0),
                    In = Balancecarried < 0 ? Balancecarried : 0,
                    Out = Balancecarried > 0 ? Balancecarried : 0,
                    ItemsId = Convert.ToInt64(0),
                    TypeMove = Balancecarried < 0 ? "Out" : "In",
                    Description = "رصيد الفترة السابقة",
                    Qty = Balancecarried,
                    SellingPrice = Convert.ToDouble(0.0),
                    Status = 0,
                    TotalRow = 0,
                    FkObject = GetFkObject(new InventoryMovement())
                };
                Movements.Add(p);
                }
                return Ok(new
                {
                    items = Movements,//.OrderBy(s => s.FakeDate).ToList(),
                    Totals = new
                    {
                        Rows = Movements.Count(),
                    //    Totals = Movements.Sum(s=>s.Qty),
                        In = Movements.Sum(s => s.In),
                        Out = Movements.Sum(s => s.Out),
                        Totals = Movements.Sum(s => s.In) - Movements.Sum(s => s.Out),
                    }
                });
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
                x.SN,
                x.Type,
                x.Address,
                x.Model,
                x.Barcode,
                x.Description,
                x.IsPrime,
                x.Status,
                x.MenuItem,
                x.UnitItem,
                x.Ingredients,
                x.TakeBon,
                //  InventoryQty = CalculateInventoryItemQty(x.Id)
            }).SingleOrDefault();
            return Ok(Item);
        }
        [HttpGet]
        [Route("Item/GetItemByBarcode")]
        public IActionResult GetItemByBarcode(string BarCode )
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
                            x.MenuItem,
                            x.UnitItem,
                            x.Ingredients,
                            x.TakeBon,
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
                try
                {
                Item item = DB.Items.Where(x => x.Id == collection.Id).SingleOrDefault();
                item.Name = collection.Name;
                item.CostPrice = collection.CostPrice;
                item.SellingPrice = collection.SellingPrice;
                item.OtherPrice = collection.OtherPrice;
                item.LowOrder = collection.LowOrder;
                item.Tax = collection.Tax;
                item.Type = collection.Type;
                item.SN = collection.SN;
                item.Address = collection.Address;
                item.Model = collection.Model;
                item.Barcode = collection.Barcode;
                item.IsPrime = collection.IsPrime;
                item.Description = collection.Description;
                item.MenuItem = collection.MenuItem;
                item.UnitItem = collection.UnitItem;
                item.Ingredients = collection.Ingredients;
                item.TakeBon = collection.TakeBon;
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
        [Route("Item/EditIngredient")]
        [HttpPost]
        public IActionResult EditIngredient(long ItemId, string Ingredient)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Item item = DB.Items.Where(x => x.Id == ItemId).SingleOrDefault();
                    item.Ingredients = Ingredient;
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
        [Route("Item/EditTakeBon")]
        [HttpPost]
        public IActionResult EditTakeBon(long ItemId, bool TakeBon)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Item item = DB.Items.Where(x => x.Id == ItemId).SingleOrDefault();
                    item.TakeBon = TakeBon;
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
        
       public dynamic GetFkObject(InventoryMovement OX)
        {
            dynamic Object = null;
            if (OX.SalesInvoiceId != null)
            
                Object = DB.SalesInvoices.Where(x => x.Id == OX.SalesInvoiceId).Select(s => new { s.Id, s.FakeDate,s.Description, Type = "SalesInvoice" }).SingleOrDefault();
            
            if (OX.PurchaseInvoiceId != null)
            
                Object = DB.PurchaseInvoices.Where(x => x.Id == OX.PurchaseInvoiceId).Select(s => new { s.Id, s.FakeDate, s.Description, Type = "PurchaseInvoice" }).SingleOrDefault();
            
            if (OX.OrderInventoryId != null)
            
                Object = DB.OrderInventories.Where(x => x.Id == OX.OrderInventoryId).Select(s=> new { s.Id, s.FakeDate, s.Description, Type = "OrderInventory" }).SingleOrDefault();
            
            if (OX.WorkShopId != null)
            
                Object = DB.WorkShops.Where(x => x.Id == OX.WorkShopId).Select(s => new { s.Id, s.FakeDate, s.Description, Type = "WorkShop" }).SingleOrDefault();
            
            return Object;
        }
    }

}
