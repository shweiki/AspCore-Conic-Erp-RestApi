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
            var Invoices = DB.SalesInvoices.Where(i => i.FakeDate >= DateFrom && i.FakeDate <= DateTo).Select(x => new {

                x.Id,
                x.Discount,
                x.Tax,
                Name = x.Vendor.Name + " " + x.Member.Name + " - " + x.Name,
                FakeDate = x.FakeDate.ToString("dd/MM/yyyy"),
                x.PaymentMethod,
                x.Status,
                x.Description,
                AccountID = (x.Vendor == null) ? x.Member.AccountId : x.Vendor.AccountId,
                InventoryMovements = DB.InventoryMovements.Where(i => i.SalesInvoiceId == x.Id && i.TypeMove == "Out").Select(m => new {
                    m.Id,
                    Name = DB.Items.Where(x => x.Id == m.ItemsId).SingleOrDefault().Name,
                    m.Qty,
                    InventoryName = DB.InventoryItems.Where(x => x.Id == m.InventoryItemId).SingleOrDefault().Name,
                    m.SellingPrice,
                    m.Description

                }).ToList()

            }).ToList();

            return Ok(Invoices);
    }
        [Route("SaleInvoice/GetSaleItem")]
        [HttpGet]
        public IActionResult GetSaleItem(long ItemID, DateTime DateFrom, DateTime DateTo )
        {
            var Invoices =  DB.InventoryMovements.Where(i => i.SalesInvoiceId != null && i.ItemsId == ItemID && i.SalesInvoice.FakeDate >= DateFrom && i.SalesInvoice.FakeDate <= DateTo).Select(x => new
            {
                x.Id,
                x.SellingPrice,
                x.Qty,
                x.Status,
                x.Tax,
                x.TypeMove,
                x.SalesInvoice.Vendor.Name,
                FakeDate = x.SalesInvoice.FakeDate.ToString("dd/MM/yyyy"),
                x.Description,
                x.SalesInvoiceId,
                x.ItemsId,
                Type = "مبيعات"
            }).ToList();

            return Ok(Invoices);
        }
        [Route("SaleInvoice/GetSaleInvoiceByStatus")]
        [HttpGet]
        public IActionResult GetSaleInvoiceByStatus(int? Status)
        {
            var Invoices = DB.SalesInvoices.Where(s => s.Status == Status).Select(x => new
            {
                x.Id,
                x.Discount,
                x.Tax,
                Name =DB.Vendors.Where(v=>v.Id == x.VendorId).SingleOrDefault().Name+ DB.Members.Where(v => v.Id == x.MemberId).SingleOrDefault().Name,
                FakeDate = x.FakeDate.ToString("dd/MM/yyyy"),
                x.PaymentMethod,
                x.Status,
                x.Description,
              //  AccountID = ' ' + DB.Vendors.Where(v => v.Id == x.VendorId).SingleOrDefault().AccountId + DB.Members.Where(v => v.Id == x.MemberId).SingleOrDefault().AccountId,
                InventoryMovements =  DB.InventoryMovements.Where(im =>im.SalesInvoiceId == x.Id &&im.TypeMove == "Out").Select(imx => new {
                    imx.Id,
                   imx.ItemsId,
                   imx.InventoryItemId,
                    imx.Qty,
                    imx.SellingPrice,
                    imx.Description
                }).ToList(),

            }).ToList();


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

    }
}
