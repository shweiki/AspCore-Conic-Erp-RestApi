using System;
using System.Data;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Entities; 
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace AspCore_Conic_Erp_RestApi.Controllers
{
    [Authorize]
    public class SaleInvoiceController : Controller
    {
        private ConicErpContext DB = new ConicErpContext();
        [HttpPost]
        [Route("SaleInvoice/GetByListQ")]
        public IActionResult GetByListQ(int Limit ,string Sort,int Page, string? User, DateTime? DateFrom, DateTime? DateTo, int? Status ,string? Any)
        {
            var Invoices = DB.SalesInvoices.Where(s =>(Any != null?  s.Id.ToString().Contains(Any)||s.Vendor.Name.Contains(Any) : true) && (DateFrom != null ? s.FakeDate >= DateFrom : true)
            && (DateTo != null ? s.FakeDate <= DateTo : true) && (Status != null ? s.Status == Status : true) &&(User != null ? DB.ActionLogs.Where(l =>l.SalesInvoiceId == s.Id && l.UserId == User).SingleOrDefault() != null : true)).Select(x => new
            {
                x.Id,
                x.Discount,
                x.Tax,
                Name = DB.Vendors.Where(v => v.Id == x.VendorId).SingleOrDefault().Name + DB.Members.Where(v => v.Id == x.MemberId).SingleOrDefault().Name,
                x.FakeDate,
                x.PaymentMethod,
                x.Status,
                x.Description,
                Total = x.InventoryMovements.Sum(s=>s.SellingPrice *s.Qty) - x.Discount,
                Logs= DB.ActionLogs.Where(l=>l.SalesInvoiceId == x.Id).ToList(),
                AccountId = DB.Vendors.Where(v => v.Id == x.VendorId).SingleOrDefault().AccountId.ToString() + DB.Members.Where(v => v.Id == x.MemberId).SingleOrDefault().AccountId.ToString(),
                InventoryMovements = x.InventoryMovements.Select(imx => new {
                    imx.Id,
                    imx.ItemsId,
                    imx.Items.Name,
                    imx.TypeMove,
                    imx.InventoryItemId,
                    imx.Qty,
                    imx.SellingPrice,
                    imx.Description
                }).ToList(),
            }).ToList();
            Invoices =  (Sort == "+id" ? Invoices.OrderBy(s => s.Id).ToList() : Invoices.OrderByDescending(s => s.Id).ToList());
            return Ok(new {items = Invoices.Skip((Page - 1) * Limit).Take(Limit).ToList(), 
            Totals = new {
            Rows = Invoices.Count(),
            Totals = Invoices.Sum(s => s.Total),
            Cash = Invoices.Where(i=>i.PaymentMethod == "Cash").Sum(s => s.Total),
            Receivables = Invoices.Where(i=>i.PaymentMethod == "Receivables").Sum(s => s.Total),
            Visa = Invoices.Where(i=>i.PaymentMethod == "Visa").Sum(s => s.Total)
            } });
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
                Name = x.SalesInvoice.Vendor.Name + x.SalesInvoice.Member.Name,
                x.SalesInvoice.FakeDate,
                x.Description,
                x.SalesInvoiceId,
                x.ItemsId,
                Type = "مبيعات"
            }).ToList();

            return Ok(Invoices);
        }
        [Route("SaleInvoice/GetSaleInvoiceByStatus")]
        [HttpGet]
        public IActionResult GetSaleInvoiceByStatus(DateTime? DateFrom , DateTime? DateTo, int? Status)
        {
            var Invoices = DB.SalesInvoices.Where(s =>(DateFrom !=null ? s.FakeDate >= DateFrom : true ) 
            && (DateTo != null ?  s.FakeDate <= DateTo : true) && (Status != null ? s.Status == Status :true)).Select(x => new
            {
                x.Id,
                x.Discount,
                x.Tax,
                Name =DB.Vendors.Where(v=>v.Id == x.VendorId).SingleOrDefault().Name+ DB.Members.Where(v => v.Id == x.MemberId).SingleOrDefault().Name,
                x.FakeDate,
                x.PaymentMethod,
                x.Status,
                x.Description,
                AccountId =  DB.Vendors.Where(v => v.Id == x.VendorId).SingleOrDefault().AccountId.ToString() + DB.Members.Where(v => v.Id == x.MemberId).SingleOrDefault().AccountId.ToString(),
                InventoryMovements = DB.InventoryMovements.Where(im => im.SalesInvoiceId == x.Id).Select(imx => new {
                    imx.Id,
                    imx.ItemsId,
                    imx.Items.Name,
                    imx.TypeMove,
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
                 //   collection.FakeDate = collection.FakeDate.ToLocalTime();
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
        [Route("SaleInvoice/Edit")]
        [HttpPost]
        public IActionResult Edit(SalesInvoice collection)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    SalesInvoice Invoice = DB.SalesInvoices.Where(x => x.Id == collection.Id).SingleOrDefault();

                    Invoice.Type = collection.Type;
                    Invoice.Tax = collection.Tax;
                    Invoice.Discount = collection.Discount;
                    Invoice.Description = collection.Description;
                    Invoice.Status = collection.Status;
                    Invoice.VendorId = collection.VendorId;
                    Invoice.FakeDate = collection.FakeDate;
                    Invoice.PaymentMethod = collection.PaymentMethod;
                    Invoice.Name = collection.Name;
                    Invoice.MemberId = collection.MemberId;
                    Invoice.IsPrime = collection.IsPrime;
                    DB.InventoryMovements.RemoveRange(DB.InventoryMovements.Where(m=>m.SalesInvoiceId ==Invoice.Id).ToList());
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
        [Route("SaleInvoice/GetSaleInvoiceByID")]
        [HttpGet]
        public IActionResult GetSaleInvoiceByID(long? ID)
        {
            var Invoices = DB.SalesInvoices.Where(x => x.Id == ID).Select(x => new {
                x.Id,
                x.VendorId,
                x.Name,
                x.Discount,
                x.Tax,
                x.FakeDate,
                x.PaymentMethod,
                x.Status,
                x.Description,
                InventoryMovements = DB.InventoryMovements.Where(Im => Im.SalesInvoiceId == x.Id).Select(m => new {
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
                    m.SalesInvoiceId,
                    m.InventoryItemId,
                    m.SellingPrice,
                    m.Description

                }).ToList()
            }).SingleOrDefault();

            return Ok(Invoices);
        }

    }
}
