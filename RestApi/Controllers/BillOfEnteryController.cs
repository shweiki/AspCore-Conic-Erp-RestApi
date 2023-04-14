using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RestApi.Controllers;

[Authorize]
public class BillOfEnteryController : Controller
{
    private ConicErpContext DB;
    public BillOfEnteryController(ConicErpContext dbcontext)
    {
        DB = dbcontext;
    }
    [HttpPost]
    [Route("BillOfEntery/GetByListQ")]
    public async Task<IActionResult> GetByListQ(int Limit, string Sort, int Page, bool FromScratch, DateTime? DateFrom, DateTime? DateTo, int? Status, string Any)
    {
        if (FromScratch)
            await CalBillOfEntery(FromScratch);
        else
            await CheckBillOfEntery();

        var Invoices = DB.BillOfEnterys.Where(s => (Any == null || s.Id.ToString().Contains(Any) || s.BonId.Contains(Any) || s.Description.Contains(Any)) && (DateFrom == null || s.FakeDate >= DateFrom)
        && (DateTo == null || s.FakeDate <= DateTo) && (Status == null || s.Status == Status)).Select(x => new
        {
            x.Id,
            x.BonId,
            x.ItemsIds,
            x.FakeDate,
            x.Description,
            x.Status,
            x.PurchaseInvoiceId,
            x.ST9,
            //  Logs = DB.ActionLogs.Where(l => l.BillOfEnteryId == x.Id).ToList(),
            InventoryMovements = DB.InventoryMovements.Where(m => m.PurchaseInvoiceId == x.PurchaseInvoiceId && m.Items.TakeBon == true).Select(imx => new
            {
                imx.Id,
                imx.ItemsId,
                imx.Items.Name,
                imx.Items.Barcode,
                imx.TypeMove,
                imx.InventoryItemId,
                imx.Qty,
                Total = DB.InventoryMovements.Where(m => m.BillOfEnteryId == x.Id && m.ItemsId == imx.ItemsId && m.SalesInvoiceId != null).Sum(s => s.Qty),
                imx.EXP,
                imx.SellingPrice,
                imx.Description,
                Status = DB.InventoryMovements.Where(m => m.BillOfEnteryId == x.Id && m.ItemsId == imx.ItemsId && m.SalesInvoiceId != null).Sum(s => s.Qty) - imx.Qty == 0 ? 1 : 0,
                BillOfEnteryItemMovements = DB.InventoryMovements.Where(m => m.BillOfEnteryId == x.Id && m.ItemsId == imx.ItemsId && m.SalesInvoiceId != null).Select(ibex => new
                {
                    ibex.Id,
                    ibex.ItemsId,
                    //imx.Items,
                    ibex.SalesInvoiceId,
                    SalesInvoiceFakeDate = ibex.SalesInvoice.FakeDate,
                    VendorName = ibex.SalesInvoice.Vendor.Name,
                    ibex.TypeMove,
                    ibex.InventoryItemId,
                    ibex.Qty,
                    Total = Math.Abs(DB.InventoryMovements.Where(m => m.BillOfEnteryId == x.Id && m.ItemsId == imx.ItemsId && m.SalesInvoiceId != null && m.Id <= ibex.Id).Sum(s => s.Qty) - imx.Qty),
                    ibex.EXP,
                    ibex.SellingPrice,
                    ibex.Description,
                    ibex.BillOfEnteryId

                }).ToList(),

            }).ToList(),
        }).ToList();
        Invoices = (Sort == "+id" ? Invoices.OrderBy(o => o.FakeDate).ToList() : Invoices.OrderByDescending(o => o.FakeDate).ToList());
        return Ok(new
        {
            items = Invoices.Skip((Page - 1) * Limit).Take(Limit).ToList(),
            Totals = new
            {
                Open = Invoices.Count(x => x.Status == 0),
                Close = Invoices.Count(x => x.Status == 1),
                Rows = Invoices.Count()
            }
        });
    }

    private async Task<bool> CalBillOfEntery(bool fromScratch)
    {
        if (fromScratch)
        {
            DB.InventoryMovements.ToList().ForEach(x => x.BillOfEnteryId = null);
        }
        var billOfEnterys = await DB.BillOfEnterys.ToListAsync();
        billOfEnterys.ForEach(x => x.Status = 0);
        DB.SaveChanges();

        var BillOfEnterys = billOfEnterys.OrderBy(o => o.FakeDate).Select(x => new
        {
            x.Id,
            x.BonId,
            x.ItemsIds,
            x.FakeDate,
            x.Description,
            x.Status,
            x.PurchaseInvoiceId,
            //  Logs = DB.ActionLogs.Where(l => l.BillOfEnteryId == x.Id).ToList(),
            InventoryMovements = DB.InventoryMovements.Where(m => m.PurchaseInvoiceId == x.PurchaseInvoiceId && m.Items.TakeBon == true).Select(imx => new
            {
                imx.Id,
                imx.ItemsId,
                imx.Items.Name,
                imx.Items.Barcode,
                imx.TypeMove,
                imx.InventoryItemId,
                imx.Qty,
                // Total = DB.InventoryMovements.Where(m => m.BillOfEnteryId == x.Id && m.ItemsId == imx.ItemsId && m.SalesInvoiceId != null).Sum(s=>s.Qty),
                imx.EXP,
                imx.SellingPrice,
                imx.Description,
            }).ToList()
        }).ToList();
        foreach (var billOfEntery in BillOfEnterys)
        {
            int HowWillBeClose = billOfEntery.InventoryMovements.Count(); // when be 0 mean all item is close 
            foreach (var inventoryMovement in billOfEntery.InventoryMovements)
            {
                double QtyBillEnteryItem = inventoryMovement.Qty;
                var SaleItemMoves = DB.InventoryMovements.Where(x => x.SalesInvoice != null
                && x.ItemsId == inventoryMovement.ItemsId
                && x.BillOfEnteryId == null).OrderBy(o => o.SalesInvoice.FakeDate).ToList();
                foreach (var saleItemMove in SaleItemMoves)
                {

                    if (saleItemMove.Qty <= QtyBillEnteryItem)
                    {
                        QtyBillEnteryItem -= saleItemMove.Qty;
                        saleItemMove.BillOfEnteryId = billOfEntery.Id;
                        DB.SaveChanges();
                        //    SaleItemMove.Remove(SIM);
                        if (QtyBillEnteryItem == 0) { break; } else continue;
                    }
                    if (saleItemMove.Qty > QtyBillEnteryItem)
                    {
                        double ModQty = saleItemMove.Qty - QtyBillEnteryItem;
                        QtyBillEnteryItem -= QtyBillEnteryItem; // mean zero
                        saleItemMove.Qty -= ModQty;
                        saleItemMove.BillOfEnteryId = billOfEntery.Id;
                        DB.InventoryMovements.Add(new InventoryMovement
                        {
                            ItemsId = saleItemMove.ItemsId,
                            TypeMove = saleItemMove.TypeMove,
                            Qty = ModQty,
                            SellingPrice = saleItemMove.SellingPrice,
                            Tax = saleItemMove.Tax,
                            Description = "تجزئة للبيان الجمركي",
                            Status = saleItemMove.Status,
                            InventoryItemId = saleItemMove.InventoryItemId,
                            SalesInvoiceId = saleItemMove.SalesInvoiceId,
                            PurchaseInvoiceId = saleItemMove.PurchaseInvoiceId,
                            OrderInventoryId = saleItemMove.OrderInventoryId,
                            WorkShopId = saleItemMove.WorkShopId,
                            BillOfEnteryId = null,
                            EXP = saleItemMove.EXP
                        });
                        DB.SaveChanges();
                        break;
                    }
                }
                if (DB.InventoryMovements.Where(m => m.BillOfEnteryId == billOfEntery.Id && m.ItemsId == inventoryMovement.ItemsId && m.SalesInvoiceId != null).Sum(s => s.Qty) - inventoryMovement.Qty == 0)
                    HowWillBeClose -= 1;
            }
            if (HowWillBeClose == 0)
                DB.BillOfEnterys.Find(billOfEntery.Id).Status = 1; // Bill Of Entery Is Close
            DB.SaveChanges();

        }
        return true;
    }
    private async Task<bool> CheckBillOfEntery()
    {

        var billOfEnterys = await DB.BillOfEnterys.ToListAsync();
        billOfEnterys.ForEach(x => x.Status = 0);
        DB.SaveChanges();

        var BillOfEnterys = billOfEnterys.OrderBy(o => o.FakeDate).Select(x => new
        {
            x.Id,
            x.BonId,
            x.ItemsIds,
            x.FakeDate,
            x.Description,
            x.Status,
            x.PurchaseInvoiceId,
            InventoryMovements = DB.InventoryMovements.Where(m => m.PurchaseInvoiceId == x.PurchaseInvoiceId && m.Items.TakeBon == true).Select(imx => new
            {
                imx.Id,
                imx.ItemsId,
                imx.Qty,
            }).ToList()
        }).ToList();
        foreach (var billOfEntery in BillOfEnterys)
        {
            int HowWillBeClose = billOfEntery.InventoryMovements.Count(); // when be 0 mean all item is close 
            foreach (var inventoryMovement in billOfEntery.InventoryMovements)
            {
                var SaleItemMoves = DB.InventoryMovements.Where(x => x.SalesInvoice != null
                             && x.ItemsId == inventoryMovement.ItemsId
                             && x.BillOfEnteryId == billOfEntery.Id).OrderBy(o => o.SalesInvoice.FakeDate).ToList();

                if (SaleItemMoves.Sum(s => s.Qty) - inventoryMovement.Qty == 0)
                    HowWillBeClose -= 1;
            }
            if (HowWillBeClose == 0)
                DB.BillOfEnterys.Find(billOfEntery.Id).Status = 1; // Bill Of Entery Is Close
            DB.SaveChanges();
        }
        return true;
    }


    [Route("BillOfEntery/Create")]
    [HttpPost]
    public async Task<IActionResult> Create(BillOfEntery collection)
    {
        if (ModelState.IsValid && collection.BonId is not null)
        {
            try
            {
                // TODO: Add insert logic here
                //  collection.InventoryMovements.ToList().ForEach(s => DB.Items.Where(x => x.Id == s.ItemsId).SingleOrDefault().CostPrice = s.SellingPrice);
                DB.BillOfEnterys.Add(collection);
                await DB.SaveChangesAsync(new CancellationToken(), User.Identity.Name);
                return Ok(collection.Id);

            }
            catch
            {
                //Console.WriteLine(collection);
                return Ok(false);
            }
        }
        else return Ok(false);
    }
    [Route("BillOfEntery/Edit")]
    [HttpPost]
    public async Task<IActionResult> Edit(BillOfEntery collection)
    {
        if (ModelState.IsValid)
        {
            try
            {
                BillOfEntery Invoice = DB.BillOfEnterys.Where(x => x.Id == collection.Id).SingleOrDefault();

                Invoice.BonId = collection.BonId;
                Invoice.ItemsIds = collection.ItemsIds;
                Invoice.FakeDate = collection.FakeDate;
                Invoice.Description = collection.Description;
                Invoice.Status = collection.Status;
                Invoice.PurchaseInvoiceId = collection.PurchaseInvoiceId;
                Invoice.ST9 = collection.ST9;

                await DB.SaveChangesAsync(new CancellationToken(), User.Identity.Name);

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
    [Route("BillOfEntery/GetBillOfEnteryById")]
    [HttpGet]
    public IActionResult GetBillOfEnteryById(long? Id)
    {
        var Invoices = DB.BillOfEnterys.Where(i => i.Id == Id).Select(x => new
        {
            x.Id,
            x.BonId,
            x.ItemsIds,
            x.FakeDate,
            x.Description,
            x.Status,
            x.PurchaseInvoiceId,
            x.ST9,
            //  Logs = DB.ActionLogs.Where(l => l.BillOfEnteryId == x.Id).ToList(),
            InventoryMovements = DB.InventoryMovements.Where(m => m.PurchaseInvoiceId == x.PurchaseInvoiceId && m.Items.TakeBon == true).Select(imx => new
            {
                imx.Id,
                imx.ItemsId,
                imx.Items,
                imx.TypeMove,
                imx.InventoryItemId,
                imx.Qty,
                imx.EXP,
                imx.SellingPrice,
                SalesItemMovements = DB.InventoryMovements.Where(m => m.SalesInvoiceId != null && m.ItemsId == imx.ItemsId).Select(imx => new
                {
                    imx.Id,
                    imx.ItemsId,
                    //imx.Items,
                    imx.SalesInvoice,
                    imx.TypeMove,
                    imx.InventoryItemId,
                    imx.Qty,
                    imx.EXP,
                    imx.SellingPrice,
                    imx.Description
                }).ToList(),
                imx.Description
            }).ToList()

        }).SingleOrDefault();


        return Ok(Invoices);
    }
    [Route("BillOfEntery/GetActiveBillOfEnteryForItemId")]
    [HttpGet]
    public IActionResult GetActiveBillOfEnteryForItemId(long ItemId)
    {
        var Invoices = DB.BillOfEnterys.Where(i => i.Status == 0
        && DB.InventoryMovements.Where(m => m.PurchaseInvoiceId == i.PurchaseInvoiceId && m.ItemsId == ItemId).Count() > 0).Select(x => new
        {
            x.Id,
            x.BonId,
            x.ItemsIds,
            x.FakeDate,
            x.Description,
            x.Status,
            x.PurchaseInvoiceId,
            x.ST9,
        }).ToList();
        return Ok(Invoices);
    }
    [Route("BillOfEntery/GetBillOfEnteryByPurchaseId")]
    [HttpGet]
    public IActionResult GetBillOfEnteryByPurchaseId(long? Id)
    {
        var Invoices = DB.BillOfEnterys.Where(i => i.PurchaseInvoiceId == Id).Select(x => new
        {
            x.Id,
            x.BonId,
            x.ItemsIds,
            x.FakeDate,
            x.Description,
            x.Status,
            x.PurchaseInvoiceId,
            x.ST9,
        }).SingleOrDefault();
        return Ok(Invoices);
    }
    [HttpPost]
    [Route("BillOfEntery/PinBillOfEntery")]
    public async Task<IActionResult> PinBillOfEntery(long InventoryMovementsId, long? BillOfEnteryId, bool Pin)
    {
        if (ModelState.IsValid)
        {
            try
            {
                InventoryMovement Movement = DB.InventoryMovements.Where(x => x.Id == InventoryMovementsId).SingleOrDefault();
                Movement.BillOfEnteryId = Pin == true ? BillOfEnteryId : null;
                await DB.SaveChangesAsync(new CancellationToken(), User.Identity.Name);
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
    [HttpPost]
    [Route("BillOfEntery/PinST9BillOfEntery")]
    public async Task<IActionResult> PinST9BillOfEntery(long BillOfEnteryId, string St9)
    {
        if (ModelState.IsValid)
        {
            try
            {
                BillOfEntery Bill = DB.BillOfEnterys.Where(x => x.Id == BillOfEnteryId).SingleOrDefault();
                Bill.ST9 = St9;
                await DB.SaveChangesAsync(new CancellationToken(), User.Identity.Name);
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