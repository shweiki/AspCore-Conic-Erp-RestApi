using System;
using System.Data;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Entities; 
using Microsoft.AspNetCore.Mvc;


namespace AspCore_Conic_Erp_RestApi.Controllers
{
    [Authorize]
    public class SaleInvoiceController : Controller
    {
        private ConicErpContext DB = new ConicErpContext();

        [Route("SaleInvoice/GetSaleInvoice")]
        [HttpGet]
        public IActionResult GetSaleInvoice(DateTime DateFrom, DateTime DateTo)
        {
            var Invoices = (from x in DB.SalesInvoices.ToList()
                            where (x.FakeDate >= DateFrom) && (x.FakeDate <= DateTo)
                            let p = new 
                         {
                                x.Id,
                                x.Discount,
                                x.Tax,
                                Name = x.Vendor?.Name + " " + x.Member?.Name + " - " + x.Name,
                                FakeDate = x.FakeDate.ToString("dd/MM/yyyy"),
                                x.PaymentMethod,
                                x.Status,
                                x.Description,
                                AccountID = (x.Vendor?.AccountId != null) ? x.Vendor?.AccountId :   x.Member?.AccountId,
                                InventoryMovements = (from m in DB.InventoryMovements.ToList()
                                                     where (m.SalesInvoiceId == x.Id) && (m.TypeMove == "Out")
                                                     select new
                                                     {
                                                         m.Id,
                                                        Name = DB.Items.Where(x => x.Id == m.ItemsId).SingleOrDefault().Name,
                                                         m.Qty,
                                                         InventoryName = DB.InventoryItems.Where(x=>x.Id == m.InventoryItemId).SingleOrDefault().Name,
                                                         m.SellingPrice,
                                                         m.Description
                                                     })
                         }
                            select p);
            
            return Ok(Invoices);
    }
        [Route("SaleInvoice/GetSaleItem")]
        [HttpGet]
        public IActionResult GetSaleItem(long ItemID, DateTime DateFrom, DateTime DateTo )
        {
            var Invoices = (from x in DB.InventoryMovements.Where(i=>i.SalesInvoiceId !=null).ToList()
                            where (x.ItemsId== ItemID) &&(DB.SalesInvoices.Where(s=>s.Id == x.SalesInvoiceId).Single().FakeDate >= DateFrom) && (DB.SalesInvoices.Where(s => s.Id == x.SalesInvoiceId).Single().FakeDate  <= DateTo) 
                            let p = new
                            {
                                x.Id,
                                x.SellingPrice,
                                x.Qty,
                                x.Status,
                                x.Tax,
                                Name = x.SalesInvoice?.Vendor?.Name + " " + x.SalesInvoice?.Member?.Name + " - " + x.SalesInvoice.Name,
                                FakeDate = x.SalesInvoice.FakeDate.ToString("dd/MM/yyyy"),
                                x.Description,
                      
                            
                            }
                            select p);

            return Ok(Invoices);
        }
        [Route("SaleInvoice/GetSaleInvoiceByStatus")]
        [HttpGet]
        public IActionResult GetSaleInvoiceByStatus(int? Status)
        {
            var Invoices = (from x in DB.SalesInvoices.ToList()
                            where (x.Status == Status)
                            let p = new
                            {
                                x.Id,
                                x.Discount,
                                x.Tax,
                                Name = x.Vendor?.Name + " " + x.Member?.Name + " - " + x.Name,
                                FakeDate = x.FakeDate.ToString("dd/MM/yyyy"),
                                x.PaymentMethod,
                                x.Status,
                                x.Description,
                                AccountID = (x.Vendor == null) ? x.Member.AccountId : x.Vendor.AccountId,
                                InventoryMovements = (from m in DB.InventoryMovements.ToList()
                                                      where (m.SalesInvoiceId == x.Id) && (m.TypeMove == "Out")
                                                      select new
                                                      {
                                                          m.Id,
                                                          m.Items.Name,
                                                          m.Qty,
                                                          m.SellingPrice,
                                                          m.Description
                                                      }),
                          
                            }
                            select p);

            return Ok(Invoices);
        }
        [HttpPost]
        public IActionResult Create(SalesInvoice collection)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // TODO: Add insert logic here
                    DB.SalesInvoices.Add(collection);
                    DB.SaveChanges();
                    Oprationsy Opx = DB.Oprationsys.Where(d => d.Status == collection.Status && d.TableName == "SalesInvoice").SingleOrDefault();
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

    }
}
