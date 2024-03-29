﻿using Application.Common.Helpers;
using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NinjaNye.SearchExtensions;

namespace RestApi.Controllers;

[Authorize]
public class ItemController : Controller
{
    private readonly IApplicationDbContext DB;
    public ItemController(IApplicationDbContext dbcontext)
    {
        DB = dbcontext;
    }
    [HttpGet]
    [Route("Item/GetItem")]
    public IActionResult GetItem()
    {
        var Items = DB.Item.Select(x => new { x.Id, x.Name, x.Address, x.Model, x.Type, x.SN, x.Barcode, x.SellingPrice, x.OtherPrice, x.CostPrice, x.TakeBon }).ToList();

        return Ok(Items);
    }
    [HttpPost]
    [Route("Item/GetByListQ")]
    public IActionResult GetByListQ(int Limit, string Sort, int Page, int? Status, string Any)
    {
        var Items = DB.Item.Where(s => (Any != null ? s.Id.ToString().Contains(Any) || s.Name.Contains(Any) || s.MenuItem.Contains(Any) ||
        s.Barcode.Contains(Any) : true) && (Status == null || s.Status == Status))
          .Select(x => new
          {
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
              x.MenuItem,
              x.Description,
              x.Ingredients,
              x.TakeBon,
              TotalIn = Utility.toFixed(DB.InventoryMovement.Where(i => i.TypeMove == "In" && i.ItemsId == x.Id).Sum(s => s.Qty), 2),
              TotalOut = Utility.toFixed(DB.InventoryMovement.Where(i => i.TypeMove == "Out" && i.ItemsId == x.Id).Sum(s => s.Qty), 2),
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
    public IActionResult GetItemByAny(string Any, bool IsDisplay = false)
    {
        Any = Any.ToLower();
        var Items = DB.Item.Search(x => x.Name.ToLower(), x => x.Barcode.ToLower(), x => x.Id.ToString(), x => x.MenuItem.ToLower(), x => x.Address.ToLower(), x => x.Model.ToLower(), x => x.SN.ToLower(), x => x.Type.ToLower()).Containing(Any)
            .Select(x => new
            {
                x.Id,
                x.Name,
                x.SN,
                x.Type,
                x.Address,
                x.Model,
                x.Barcode,
                x.SellingPrice,
                x.OtherPrice,
                x.CostPrice,
                x.MenuItem,
                TotalIn = DB.InventoryMovement.Where(i => i.TypeMove == "In" && i.ItemsId == x.Id).Sum(s => s.Qty),
                TotalOut = DB.InventoryMovement.Where(i => i.TypeMove == "Out" && i.ItemsId == x.Id).Sum(s => s.Qty)
            }).ToList();

        return Ok(Items);
    }
    [HttpGet]
    [Route("Item/CheckItemIsExist")]
    public IActionResult CheckItemIsExist(string Name, string BarCode, string Sn)
    {
        var Items = DB.Item.Where(m => (BarCode != null ? m.Barcode.ToLower() == BarCode.ToLower() : false)
         || (Name != null ? m.Name.ToLower() == Name.ToLower() : false)
         || (Sn != null ? m.SN.ToLower() == Sn.ToLower() : false)).ToList();

        return Ok(Items.Count() > 0 ? true : false);
    }
    [HttpGet]
    [Route("Item/GetIsPrimeItem")]
    public IActionResult GetIsPrimeItem()
    {
        var Items = DB.Item.Where(i => i.IsPrime == true).Select(x => new
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
            x.SN,
            x.Type,
            x.Address,
            x.Model,
            x.Barcode,
            x.UnitItem,
            x.Description,
            x.Ingredients,
            x.TakeBon,
            x.MenuItem,
            //   InventoryQty = CalculateInventoryItemQty(x.Id),
        }).ToList();


        return Ok(Items);
    }
    [HttpPost]
    [Route("Item/GetLowOrder")]
    public IActionResult GetLowOrder(int Limit, string Sort, int Page, int? Status, string Any)
    {
        var Items = DB.Item.Where(s => (s.InventoryMovements != null ?
        s.InventoryMovements.Where(d => d.TypeMove == "In").Sum(qc => qc.Qty) - s.InventoryMovements.Where(d => d.TypeMove == "Out").Sum(qc => qc.Qty) < s.LowOrder
        : false) && (Any != null ? s.Id.ToString().Contains(Any) || s.Name.Contains(Any) || s.MenuItem.Contains(Any) || s.Barcode.Contains(Any) : true) && (Status != null ? s.Status == Status : true))
         .Select(x => new
         {
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
    public async Task<IActionResult> GetEXP(DateTime? DateFrom, DateTime? DateTo, int Limit, string Sort, int Page, int? Status, string Any)
    {
        var itemsQuery = DB.InventoryMovement.Where(s => (DateTo != null ? s.EXP <= DateTo : true) && (DateFrom != null ? s.EXP >= DateFrom : true)).AsQueryable();
        int totalCountBeforeFilter = await itemsQuery.CountAsync();

        var itemsQueryList = await itemsQuery.ToListAsync();

        var items = itemsQueryList.Select(x => new
        {
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
            TotalIn = x.Items.InventoryMovements.Where(x => x.TypeMove == "In").Sum(s => s.Qty),
            TotalOut = x.Items.InventoryMovements.Where(x => x.TypeMove == "Out").Sum(s => s.Qty),
        }).ToList();

        items = (Sort == "+id" ? items.OrderBy(s => s.Id).ToList() : items.OrderByDescending(s => s.Id).ToList());

        var itemsTaken = items.Skip((Page - 1) * Limit).Take(Limit).ToList();

        return Ok(new
        {
            items = itemsTaken,
            Totals = new
            {
                Rows = totalCountBeforeFilter,
                TotalIn = items.Sum(s => s.TotalIn),
                TotalOut = items.Sum(s => s.TotalOut),
                Totals = items.Sum(s => s.TotalIn) - items.Sum(s => s.TotalOut),

            }
        });

    }
    [HttpGet]
    [Route("Item/GetActiveItem")]
    public IActionResult GetActiveItem()
    {
        var Items = DB.Item.Where(i => i.Status == 0).Select(x => new
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
        var Movements = DB.InventoryMovement.Where(s => (MergeItemId != null ? s.ItemsId == ItemId || s.ItemsId == MergeItemId : s.ItemsId == ItemId)
             && ((s.SalesInvoice.FakeDate >= DateFrom && s.SalesInvoice.FakeDate <= DateTo)
             || (s.PurchaseInvoice.FakeDate >= DateFrom && s.PurchaseInvoice.FakeDate <= DateTo)
             || (s.OrderInventory.FakeDate >= DateFrom && s.OrderInventory.FakeDate <= DateTo)
             || (s.WorkShop.FakeDate >= DateFrom && s.WorkShop.FakeDate <= DateTo)))
                            .Select(x => new { x }).AsEnumerable()
                        .Select(x => new
                        {
                            x.x.Id,
                            In = x.x.TypeMove == "In" ? x.x.Qty : 0,
                            Out = x.x.TypeMove == "Out" ? x.x.Qty : 0,
                            x.x.ItemsId,
                            x.x.TypeMove,
                            x.x.Description,
                            x.x.Qty,
                            x.x.SellingPrice,
                            x.x.Status,
                            // FakeDate = x.SalesInvoice?.FakeDate +""+ x.OrderInventory?.FakeDate+""+ x.PurchaseInvoice?.FakeDate+""+ x.WorkShop?.FakeDate,
                            TotalRow = (x.x.TypeMove == "In" ? x.x.Qty : 0) - (x.x.TypeMove == "Out" ? x.x.Qty : 0),
                            FkObject = GetFkObject(x.x)
                        }).ToList();
        double AllTotal = DB.InventoryMovement.Where(s => (MergeItemId != null ? s.ItemsId == ItemId || s.ItemsId == MergeItemId : s.ItemsId == ItemId)).Where(x => x.TypeMove == "In").Sum(s => s.Qty) -
        DB.InventoryMovement.Where(s => (MergeItemId != null ? s.ItemsId == ItemId || s.ItemsId == MergeItemId : s.ItemsId == ItemId)).Where(x => x.TypeMove == "Out").Sum(s => s.Qty);
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
                TotalRow = Convert.ToDouble(0.0),
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
        var Item = DB.Item.Where(x => x.Id == Id).Select(x => new
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
    public IActionResult GetItemByBarcode(string BarCode)
    {
        var Item = DB.Item.Where(x => x.Barcode == BarCode).Select(x => new
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
            TotalIn = DB.InventoryMovement.Where(i => i.TypeMove == "In" && i.ItemsId == x.Id).Sum(s => s.Qty),
            TotalOut = DB.InventoryMovement.Where(i => i.TypeMove == "Out" && i.ItemsId == x.Id).Sum(s => s.Qty)
            //  InventoryQty = CalculateInventoryItemQty(x.Id)
        }).FirstOrDefault();
        if (Item != null)
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
                DB.Item.Add(collection);
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
                Item item = DB.Item.Where(x => x.Id == collection.Id).SingleOrDefault();
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
    [Route("Item/Delete")]
    [HttpPost]
    public IActionResult Delete(long ItemId)
    {

        try
        {
            var item = DB.Item.Where(x => x.Id == ItemId).SingleOrDefault();
            DB.Item.Remove(item);
            DB.SaveChanges();
            return Ok(true);
        }
        catch
        {
            //Console.WriteLine(collection);
            return Ok(false);
        }
    }
    [Route("Item/EditIngredient")]
    [HttpPost]
    public IActionResult EditIngredient(long ItemId, string Ingredient)
    {
        if (ModelState.IsValid)
        {
            try
            {
                Item item = DB.Item.Where(x => x.Id == ItemId).SingleOrDefault();
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
                Item item = DB.Item.Where(x => x.Id == ItemId).SingleOrDefault();
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
        var InventoryItemsQty = from x in DB.InventoryMovement.Where(i => i.ItemsId == Id && i.Status >= 0).ToList()
                                group x by x.InventoryItemId into g
                                select new
                                {
                                    InventoryItemId = g.Key,
                                    InventoryName = DB.InventoryItem.Where(a => a.Id == g.Key).Select(c => c.Name).FirstOrDefault(),
                                    QtyIn = g.Where(d => d.TypeMove == "In").Sum(qc => qc.Qty),
                                    QtyOut = g.Where(d => d.TypeMove == "Out").Sum(qc => qc.Qty)
                                };

        return Ok(InventoryItemsQty);
    }
    public object CalculateInventoryItemQtyById(long Id)
    {
        var InventoryItemsQty = from x in DB.InventoryMovement.Where(i => i.ItemsId == Id && i.Status >= 0).ToList()
                                group x by x.InventoryItemId into g
                                select new
                                {
                                    InventoryItemId = g.Key,
                                    InventoryName = DB.InventoryItem.Where(a => a.Id == g.Key).Select(c => c.Name).FirstOrDefault(),
                                    QtyIn = g.Where(d => d.TypeMove == "In").Sum(qc => qc.Qty),
                                    QtyOut = g.Where(d => d.TypeMove == "Out").Sum(qc => qc.Qty)
                                };

        return InventoryItemsQty;
    }
    [Route("Item/GetInventoryItemEXP")]
    [HttpPost]
    public IActionResult GetInventoryItemEXP(long Id)
    {
        var InventoryItemsExp = from x in DB.InventoryMovement.Where(i => i.ItemsId == Id && i.Status == 0).ToList()
                                group x by x.EXP into g
                                select new
                                {
                                    Exp = g.Key,
                                };
        return Ok(InventoryItemsExp);
    }

    [HttpGet]
    [Route("Item/CalculateCostPrice")]
    public IActionResult CalculateCostPrice()
    {
        DB.InventoryMovement.Where(i => i.PurchaseInvoiceId != null).ToList().ForEach(s => DB.Item.Where(x => x.Id == s.ItemsId).SingleOrDefault().CostPrice = s.SellingPrice);
        DB.SaveChanges();
        return Ok(true);
    }

    public dynamic GetFkObject(InventoryMovement OX)
    {
        dynamic Object = null;

        if (OX.SalesInvoiceId != null)

            Object = DB.SalesInvoice.Where(x => x.Id == OX.SalesInvoiceId).Select(s => new { s.Id, s.FakeDate, s.Description, Type = "SalesInvoice" }).SingleOrDefault();

        if (OX.PurchaseInvoiceId != null)

            Object = DB.PurchaseInvoice.Where(x => x.Id == OX.PurchaseInvoiceId).Select(s => new { s.Id, s.FakeDate, s.Description, Type = "PurchaseInvoice" }).SingleOrDefault();

        if (OX.OrderInventoryId != null)

            Object = DB.OrderInventory.Where(x => x.Id == OX.OrderInventoryId).Select(s => new { s.Id, s.FakeDate, s.Description, Type = "OrderInventory" }).SingleOrDefault();

        if (OX.WorkShopId != null)

            Object = DB.WorkShop.Where(x => x.Id == OX.WorkShopId).Select(s => new { s.Id, s.FakeDate, s.Description, Type = "WorkShop" }).SingleOrDefault();
        if (Object is null)
        {
            Object = new { Id = 0, FakeDate = DateTime.Now.Year };
        }
        return Object;
    }
}
