using System;
using System.Data;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Entities;
using Microsoft.AspNetCore.Mvc;

namespace AspCore_Conic_Erp_RestApi.Controllers
{
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
            && (DateTo != null ? s.FakeDate <= DateTo : true) && (Status != null ? s.Status == Status : true) ).Select(x => new
            {
                x.Id,
                x.BonId,
                x.ItemsIds,
                x.FakeDate,
                x.Description,
                x.Status,
                x.PurchaseInvoiceId,
                //  Logs = DB.ActionLogs.Where(l => l.BillOfEnteryId == x.Id).ToList(),
                InventoryMovements = DB.InventoryMovements.Where(m=>m.PurchaseInvoiceId == x.PurchaseInvoiceId && m.Items.TakeBon == true).Select(imx => new
                {
                    imx.Id,
                    imx.ItemsId,
                    imx.Items.Name,
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


        [Route("BillOfEntery/Create")]
        [HttpPost]
        public IActionResult Create(BillOfEntery collection)
        {
            if (ModelState.IsValid)
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
                //  Logs = DB.ActionLogs.Where(l => l.BillOfEnteryId == x.Id).ToList(),
                InventoryMovements = DB.InventoryMovements.Where(m => m.PurchaseInvoiceId == x.PurchaseInvoiceId && m.Items.TakeBon == true).Select(imx => new
                {
                    imx.Id,
                    imx.ItemsId,
                    imx.Items.Name,
                    imx.TypeMove,
                    imx.InventoryItemId,
                    imx.Qty,
                    imx.EXP,
                    imx.SellingPrice,
                    imx.Description
                }).ToList()

            }).SingleOrDefault();


            return Ok(Invoices);
        }


    }
}