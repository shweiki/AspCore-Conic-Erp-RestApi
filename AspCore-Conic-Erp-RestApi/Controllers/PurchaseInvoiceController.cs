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
        public IActionResult GetPurchaseInvoice(DateTime DateFrom, DateTime DateTo)
        {
            var Invoices = (from x in DB.PurchaseInvoices.ToList()
                            where (x.FakeDate >= DateFrom) && (x.FakeDate <= DateTo)
                            let p = new
                            {
                                x.Id,
                                Name = x.Vendor?.Name + " - " + x.Name,
                                x.Discount,
                                x.Tax,
                                FakeDate = x.FakeDate.Value.ToString("dd/MM/yyyy"),
                                x.PaymentMethod,
                                x.Status,
                                x.Description,
                                InventoryMovements = (from m in DB.InventoryMovements.ToList()
                                                      where (m.PurchaseInvoiceId == x.Id) && (m.TypeMove == "In")
                                                      select new
                                                      {
                                                          m.Id,
                                                          m.Items.Name,
                                                          InventoryName = m.InventoryItem.Name,
                                                          m.Qty,
                                                          m.SellingPrice,
                                                          m.Description
                                                      }),
                 
                            }
                            select p);

            return Ok(Invoices);
        }
        [Route("PurchaseInvoice/GetPurchaseItem")]
        [HttpGet]
        public IActionResult GetPurchaseItem(long? ItemID, DateTime DateFrom, DateTime DateTo)
        {
            var Invoices = (from x in DB.InventoryMovements.Where(i => i.PurchaseInvoiceId != null).ToList()
                            where (x.ItemsId == ItemID) && (x.PurchaseInvoice.FakeDate >= DateFrom) && (x.PurchaseInvoice.FakeDate <= DateTo)
                            let p = new
                            {
                                x.Id,
                                x.SellingPrice,
                                x.Qty,
                                x.Status,
                                x.Tax,
                                FakeDate = x.PurchaseInvoice.FakeDate.Value.ToString("dd/MM/yyyy"),
                                x.Description,
                
                            }
                            select p);

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
                    DB.PurchaseInvoices.Add(collection);
                    DB.SaveChanges();
                    Oprationsy Opx = DB.Oprationsys.Where(d => d.Status == collection.Status && d.TableName == "PurchaseInvoice").SingleOrDefault();
                    OprationsysController Op = new OprationsysController();
                    if (Op.ChangeStatus(collection.Id, Opx.Id, "<!" + collection.Id + "!>"))
                    {
                        return Ok(collection.Id);
                    }
                    else return Ok(false);
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
            var Invoices = (from x in DB.PurchaseInvoices.ToList()
                            where (x.Id == ID)
                            let p = new
                            {
                                x.Id,
                                Name = x.Vendor?.Name + " - " + x.Name,
                                x.VendorId,
                                x.Discount,
                                x.Tax,
                                x.FakeDate,
                                x.InvoicePurchaseDate,
                                x.PaymentMethod,
                                x.Status,
                                x.Description,
                                InventoryMovements = (from m in DB.InventoryMovements.ToList()
                                                      where (m.PurchaseInvoiceId == x.Id) && (m.TypeMove == "In")
                                                      select new
                                                      {
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
                                                      }),
                    
                            }
                            select p).SingleOrDefault();

            return Ok(Invoices);
        }

    }
}