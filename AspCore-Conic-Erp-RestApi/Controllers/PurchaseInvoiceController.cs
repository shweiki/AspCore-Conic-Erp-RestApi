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
        private ConicErpContext DB = new ConicErpContext();

        [Route("PurchaseInvoice/GetPurchaseInvoice")]
        [HttpGet]
        public IActionResult GetPurchaseInvoice(DateTimeOffset DateFrom, DateTimeOffset DateTo)
        {
            var Invoices = DB.PurchaseInvoices.Where(i => i.FakeDate >= DateFrom && i.FakeDate <= DateTo).Select(x => new
            {

                x.Id,
                Name = x.Vendor.Name + " - " + x.Name,
                x.Discount,
                x.Tax,
                FakeDate = x.FakeDate.Value.ToString("dd/MM/yyyy"),
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
                    m.SellingPrice,
                    m.Description
                }).ToList()
            }).ToList();
                            

            return Ok(Invoices);
        }
        [Route("PurchaseInvoice/GetPurchaseItem")]
        [HttpGet]
        public IActionResult GetPurchaseItem(long? ItemID, DateTimeOffset DateFrom, DateTimeOffset DateTo)
        {
            var Invoices = DB.InventoryMovements.Where(i => i.PurchaseInvoiceId != null && i.ItemsId == ItemID && i.PurchaseInvoice.FakeDate >= DateFrom && i.PurchaseInvoice.FakeDate <= DateTo).Select(x => new
            {
                x.Id,
                x.SellingPrice,
                x.Qty,
                x.Status,
                x.Tax,
                FakeDate = x.PurchaseInvoice.FakeDate.Value.ToString("dd/MM/yyyy"),
                x.Description,

            }).ToList();

            return Ok(Invoices);
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
                    DB.InventoryMovements.RemoveRange(Invoice.InventoryMovements);
                    Invoice.InventoryMovements = collection.InventoryMovements;
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
        [Route("PurchaseInvoice/GetPurchaseInvoiceByID")]
        [HttpGet]
        public IActionResult GetPurchaseInvoiceByID(long? ID)
        {
            var Invoices = DB.PurchaseInvoices.Where(i => i.Id == ID).Select(x => new {
                x.Id,
                Name = x.Vendor.Name + " - " + x.Name,
                x.VendorId,
                x.Discount,
                x.Tax,
                x.FakeDate,
                x.InvoicePurchaseDate,
                x.PaymentMethod,
                x.Status,
                x.Description,
                InventoryMovements = DB.InventoryMovements.Where(i => i.PurchaseInvoiceId == x.Id && i.TypeMove == "In").Select(m => new {
                    m.Id,
                    m.ItemsId,
                    m.TypeMove,
                    m.Status,
                    m.Qty,
                    Itemx = new
                    {
                        m.SellingPrice,
                        m.Items.Name
                    },
                    m.PurchaseInvoiceId,
                    m.InventoryItemId,
                    m.SellingPrice,
                    m.Description
                }).ToList()
                                
            }).SingleOrDefault();
                       
                        

            return Ok(Invoices);
        }

    }
}