using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data;
using System.Linq;

namespace AspCore_Conic_Erp_RestApi.Controllers;

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
    public IActionResult GetByListQ(int Limit, string Sort, int Page, string User, DateTime? DateFrom, DateTime? DateTo, int? Status, string Any)
    {
        var Invoices = DB.BillOfEnterys.Where(s => (Any != null ? s.Id.ToString().Contains(Any) || s.BonId.Contains(Any) || s.Description.Contains(Any) : true) && (DateFrom != null ? s.FakeDate >= DateFrom : true)
        && (DateTo != null ? s.FakeDate <= DateTo : true) && (Status != null ? s.Status == Status : true)).Select(x => new
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
                Status = DB.InventoryMovements.Where(m => m.BillOfEnteryId == x.Id && m.ItemsId == imx.ItemsId && m.SalesInvoiceId != null).Sum(s => s.Qty) - imx.Qty == 0 ? "مغلق" : "مفتوح",
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
                Rows = Invoices.Count(),
            }
        });
    }
    [HttpGet]
    [Route("BillOfEntery/CalBillOfEntery")]
    public IActionResult CalBillOfEntery()
    {
        DB.InventoryMovements.ToList().ForEach(x => x.BillOfEnteryId = null);
        DB.BillOfEnterys.ToList().ForEach(x => x.Status = 0);
        DB.SaveChanges();

        var BillOfEnterys = DB.BillOfEnterys.OrderBy(o => o.FakeDate).Select(x => new
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
        foreach (var boe in BillOfEnterys)
        {
            int HowWillBeClose = boe.InventoryMovements.Count(); // when be 0 mean all item is close 
            foreach (var boei in boe.InventoryMovements)
            {
                double QtyBillEnteryItem = boei.Qty;
                var SaleItemMove = DB.InventoryMovements.Where(x => x.SalesInvoice != null && x.ItemsId == boei.ItemsId && x.BillOfEnteryId == null).OrderBy(o => o.SalesInvoice.FakeDate).ToList();
                foreach (var SIM in SaleItemMove)
                {

                    if (SIM.Qty <= QtyBillEnteryItem)
                    {
                        QtyBillEnteryItem -= SIM.Qty;
                        SIM.BillOfEnteryId = boe.Id;
                        DB.SaveChanges();
                        //    SaleItemMove.Remove(SIM);
                        if (QtyBillEnteryItem == 0) { break; } else continue;
                    }
                    if (SIM.Qty > QtyBillEnteryItem)
                    {
                        double ModQty = SIM.Qty - QtyBillEnteryItem;
                        QtyBillEnteryItem -= QtyBillEnteryItem; // mean zero
                        SIM.Qty -= ModQty;
                        SIM.BillOfEnteryId = boe.Id;
                        DB.InventoryMovements.Add(new InventoryMovement
                        {
                            ItemsId = SIM.ItemsId,
                            TypeMove = SIM.TypeMove,
                            Qty = ModQty,
                            SellingPrice = SIM.SellingPrice,
                            Tax = SIM.Tax,
                            Description = "تجزئة للبيان الجمركي",
                            Status = SIM.Status,
                            InventoryItemId = SIM.InventoryItemId,
                            SalesInvoiceId = SIM.SalesInvoiceId,
                            PurchaseInvoiceId = SIM.PurchaseInvoiceId,
                            OrderInventoryId = SIM.OrderInventoryId,
                            WorkShopId = SIM.WorkShopId,
                            BillOfEnteryId = null,
                            EXP = SIM.EXP
                        });
                        DB.SaveChanges();
                        break;
                    }
                }
                if (DB.InventoryMovements.Where(m => m.BillOfEnteryId == boe.Id && m.ItemsId == boei.ItemsId && m.SalesInvoiceId != null).Sum(s => s.Qty) - boei.Qty == 0)
                    HowWillBeClose -= 1;
            }
            if (HowWillBeClose == 0)
                DB.BillOfEnterys.Find(boe.Id).Status = 1; // Bon Is Close
            DB.SaveChanges();

        }
        return Ok(true);
    }


    [Route("BillOfEntery/Create")]
    [HttpPost]
    public IActionResult Create(BillOfEntery collection)
    {
        if (ModelState.IsValid && collection.BonId is not null)
        {
            try
            {
                // TODO: Add insert logic here
                //  collection.InventoryMovements.ToList().ForEach(s => DB.Items.Where(x => x.Id == s.ItemsId).SingleOrDefault().CostPrice = s.SellingPrice);
                DB.BillOfEnterys.Add(collection);
                DB.SaveChanges();
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
    public IActionResult Edit(BillOfEntery collection)
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
    public IActionResult PinBillOfEntery(long InventoryMovementsId, long? BillOfEnteryId, bool Pin)
    {
        if (ModelState.IsValid)
        {
            try
            {
                InventoryMovement Movement = DB.InventoryMovements.Where(x => x.Id == InventoryMovementsId).SingleOrDefault();
                Movement.BillOfEnteryId = Pin == true ? BillOfEnteryId : null;
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
    [HttpPost]
    [Route("BillOfEntery/PinST9BillOfEntery")]
    public IActionResult PinST9BillOfEntery(long BillOfEnteryId, string St9)
    {
        if (ModelState.IsValid)
        {
            try
            {
                BillOfEntery Bill = DB.BillOfEnterys.Where(x => x.Id == BillOfEnteryId).SingleOrDefault();
                Bill.ST9 = St9;
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