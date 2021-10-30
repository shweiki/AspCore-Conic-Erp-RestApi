using System;
using System.Data;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Entities;
using Microsoft.AspNetCore.Mvc;

namespace AspCore_Conic_Erp_RestApi.Controllers
{
    [Authorize]
    public class PurchaseInvoiceController : Controller
    {
                private ConicErpContext DB;
        public PurchaseInvoiceController(ConicErpContext dbcontext)
        {
            DB = dbcontext;
        }
        [HttpPost]
        [Route("PurchaseInvoice/GetByListQ")]
        public IActionResult GetByListQ(int Limit, string Sort, int Page, string User, DateTime? DateFrom, DateTime? DateTo, int? Status, string Any)
        {
            var Invoices = DB.PurchaseInvoices.Where(s => (Any != null ? s.Id.ToString().Contains(Any) || s.Vendor.Name.Contains(Any) : true) && (DateFrom != null ? s.FakeDate >= DateFrom : true)
            && (DateTo != null ? s.FakeDate <= DateTo : true) && (Status != null ? s.Status == Status : true) && (User != null ? DB.ActionLogs.Where(l => l.PurchaseInvoiceId == s.Id && l.UserId == User).SingleOrDefault() != null : true)).Select(x => new
            {
                x.Id,
                x.Discount,
                x.Tax,
                Name = x.Vendor.Name,
                x.FakeDate,
                x.PaymentMethod,
                x.Status,
                x.Description,
                x.AccountInvoiceNumber,
                x.InvoicePurchaseDate,
                Total = x.InventoryMovements.Sum(s => s.SellingPrice * s.Qty) - x.Discount,
              //  Logs = DB.ActionLogs.Where(l => l.PurchaseInvoiceId == x.Id).ToList(),
                AccountId = x.Vendor.AccountId,
                InventoryMovements = x.InventoryMovements.Select(imx => new
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
                    Totals = Invoices.Sum(s => s.Total),
                    Cash = Invoices.Where(i => i.PaymentMethod == "Cash").Sum(s => s.Total),
                    Receivables = Invoices.Where(i => i.PaymentMethod == "Receivables").Sum(s => s.Total),
                    Visa = Invoices.Where(i => i.PaymentMethod == "Visa").Sum(s => s.Total)
                }
            });
        }

        [Route("PurchaseInvoice/GetPurchaseInvoice")]
        [HttpGet]
        public IActionResult GetPurchaseInvoice(DateTime DateFrom, DateTime DateTo)
        {
            var Invoices = DB.PurchaseInvoices.Where(i => i.FakeDate >= DateFrom && i.FakeDate <= DateTo).Select(x => new
            {

                x.Id,
                Name = x.Vendor.Name + " - " + x.Name,
                x.Discount,
                x.Tax,
                x.FakeDate,
                x.PaymentMethod,
                x.Status,
                x.InvoicePurchaseDate,
                x.AccountInvoiceNumber,
                x.Description,
                InventoryMovements = DB.InventoryMovements.Where(i => i.PurchaseInvoiceId == x.Id && i.TypeMove == "In").Select(m => new
                {
                    m.Id,
                    m.Items.Name,
                    InventoryName = m.InventoryItem.Name,
                    m.Qty,
                    m.EXP,
                    m.SellingPrice,
                    m.Description
                }).ToList()
            }).ToList();


            return Ok(Invoices);
        }
        [Route("PurchaseInvoice/GetByItem")]
        [HttpGet]
        public IActionResult GetByItem(long ItemId, int Limit, string Sort, int Page, string User, DateTime? DateFrom, DateTime? DateTo, int? Status, string Any, string Type)
        {
            var Invoices = DB.InventoryMovements.Where(s =>s.PurchaseInvoiceId !=null && s.ItemsId == ItemId && (Any != null ? s.Id.ToString().Contains(Any) || s.PurchaseInvoice.Vendor.Name.Contains(Any) || s.Description.Contains(Any) || s.PurchaseInvoice.Description.Contains(Any) || s.PurchaseInvoice.Name.Contains(Any) : true) && (DateFrom != null ? s.PurchaseInvoice.FakeDate >= DateFrom : true)
              && (DateTo != null ? s.PurchaseInvoice.FakeDate <= DateTo : true) && (Status != null ? s.Status == Status : true) &&  (User != null ? DB.ActionLogs.Where(l => l.InventoryMovementId == s.Id && l.UserId == User).SingleOrDefault() != null : true)).Select(x => new
              {
                  x.Id,
                  x.PurchaseInvoiceId,
                  x.PurchaseInvoice.Discount,
                  x.Tax,
                  Name = x.PurchaseInvoice.Name, //+ DB.Vendors.Where(v => v.Id == x.VendorId).SingleOrDefault().Name + DB.Members.Where(v => v.Id == x.MemberId).SingleOrDefault().Name,
                  x.PurchaseInvoice.FakeDate,
                  x.PurchaseInvoice.InvoicePurchaseDate,
                  x.PurchaseInvoice.PaymentMethod,
                  x.Status,
                  x.Description,
                  x.PurchaseInvoice.VendorId,
                  x.PurchaseInvoice.Vendor,
                  Total = x.SellingPrice * x.Qty,
                  //     ActionLogs = DB.ActionLogs.Where(l=>l.PurchaseInvoiceId == x.Id).ToList(),
                  AccountId = DB.Vendors.Where(v => v.Id == x.PurchaseInvoice.VendorId).SingleOrDefault().AccountId.ToString(),
                  InventoryMovements = x.PurchaseInvoice.InventoryMovements.Select(imx => new {
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
                    Totals = Invoices.Sum(s => s.Total),
                    Cash = Invoices.Where(i => i.PaymentMethod == "Cash").Sum(s => s.Total),
                    Receivables = Invoices.Where(i => i.PaymentMethod == "Receivables").Sum(s => s.Total),
                    Discount = Invoices.Sum(s => s.Discount),
                    Visa = Invoices.Where(i => i.PaymentMethod == "Visa").Sum(s => s.Total)
                }
            });


        }
        [Route("PurchaseInvoice/Create")]
        [HttpPost]
        public IActionResult Create(PurchaseInvoice collection)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // TODO: Add insert logic here
                    collection.InventoryMovements.ToList().ForEach(s => DB.Items.Where(x => x.Id == s.ItemsId).SingleOrDefault().CostPrice = s.SellingPrice);
                    DB.PurchaseInvoices.Add(collection);
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
        [Route("PurchaseInvoice/Edit")]
        [HttpPost]
        public IActionResult Edit(PurchaseInvoice collection)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    PurchaseInvoice Invoice = DB.PurchaseInvoices.Where(x => x.Id == collection.Id).SingleOrDefault();

                    Invoice.AccountInvoiceNumber = collection.AccountInvoiceNumber;
                    Invoice.Tax = collection.Tax;
                    Invoice.Discount = collection.Discount;
                    Invoice.Description = collection.Description;
                    Invoice.Status = collection.Status;
                    Invoice.VendorId = collection.VendorId;
                    Invoice.FakeDate = collection.FakeDate;
                    Invoice.InvoicePurchaseDate = collection.InvoicePurchaseDate;
                    Invoice.PaymentMethod = collection.PaymentMethod;
                    DB.InventoryMovements.RemoveRange(DB.InventoryMovements.Where(x => x.PurchaseInvoiceId == Invoice.Id).ToList());
                    Invoice.InventoryMovements = collection.InventoryMovements;
                    Invoice.InventoryMovements.ToList().ForEach(s => DB.Items.Where(x => x.Id == s.ItemsId).SingleOrDefault().CostPrice = s.SellingPrice);

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
        [Route("PurchaseInvoice/GetPurchaseInvoiceById")]
        [HttpGet]
        public IActionResult GetPurchaseInvoiceById(long? Id)
        {
            var Invoices = DB.PurchaseInvoices.Where(i => i.Id == Id).Select(x => new
            {
                x.Id,
                Name = x.Vendor.Name + " - " + x.Name,
                x.VendorId,
                x.Discount,
                x.Tax,
                x.FakeDate,
                x.InvoicePurchaseDate,
                x.AccountInvoiceNumber,
                x.PaymentMethod,
                x.Status,
                x.Description,
                InventoryMovements = DB.InventoryMovements.Where(i => i.PurchaseInvoiceId == x.Id && i.TypeMove == "In").Select(m => new
                {
                    m.Id,
                    m.ItemsId,
                    m.TypeMove,
                    m.Status,
                    m.Qty,
                    m.SellingPrice,
                    m.Items.Name,
                    m.PurchaseInvoiceId,
                    m.InventoryItemId,
                    m.Description,
                    m.EXP
                }).ToList()

            }).SingleOrDefault();


            return Ok(Invoices);
        }
        [Route("PurchaseInvoice/GetPurchaseInvoiceByVendorId")]
        [HttpGet]
        public IActionResult GetPurchaseInvoiceByVendorId(long? Id)
        {
            var Invoices = DB.PurchaseInvoices.Where(i => i.VendorId == Id).Select(x => new
            {
                x.Id,
                Name = x.Vendor.Name + " - " + x.Name,
                x.VendorId,
                x.Discount,
                x.Tax,
                x.FakeDate,
                x.InvoicePurchaseDate,
                x.AccountInvoiceNumber,
                x.PaymentMethod,
                x.Status,
                x.Description,
                Total = x.InventoryMovements.Sum(s => s.SellingPrice * s.Qty) - x.Discount,
                InventoryMovements = DB.InventoryMovements.Where(i => i.PurchaseInvoiceId == x.Id ).Select(m => new
                {
                    m.Id,
                    m.ItemsId,
                    m.TypeMove,
                    m.Status,
                    m.Qty,
                    m.SellingPrice,
                    m.Items.Name,
                    m.PurchaseInvoiceId,
                    m.InventoryItemId,
                    m.Description,
                    m.EXP
                }).ToList()

            }).ToList();


            return Ok(Invoices);
        }
        


    }
}