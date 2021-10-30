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
                private ConicErpContext DB;
        public SaleInvoiceController(ConicErpContext dbcontext)
        {
            DB = dbcontext;
        }
        [HttpPost]
        [Route("SaleInvoice/GetByListQ")]
        public IActionResult GetByListQ(int Limit, string Sort, int Page, string User, DateTime? DateFrom, DateTime? DateTo, int? Status, string Any ,string Type)
        {
            
            var Invoices = DB.SalesInvoices.Where(s => (Any != null ? s.Id.ToString().Contains(Any) || s.PaymentMethod.Contains(Any) || s.Vendor.Name.Contains(Any)|| s.Description.Contains(Any)  || s.PhoneNumber.Contains(Any) || s.Name.Contains(Any)|| s.Region.Contains(Any) : true ) && 
            (DateFrom != null ? s.FakeDate >= DateFrom : true) && (DateTo != null ? s.FakeDate <= DateTo : true) && 
            (Status != null ? s.Status == Status : true)&&(Type != null ? s.Type == Type : true) && (User != null ? DB.ActionLogs.Where(l =>l.SalesInvoiceId == s.Id && l.UserId == User).SingleOrDefault() != null : true)).Select(x => new
            {
                x.Id,
                x.Discount,
                x.Tax,
                Name = x.Name + DB.Vendors.Where(v => v.Id == x.VendorId).SingleOrDefault().Name + DB.Members.Where(v => v.Id == x.MemberId).SingleOrDefault().Name,
                x.FakeDate,
                x.PaymentMethod,
                x.Status,
                x.Region,
                x.DeliveryPrice,
                x.Type,
                x.Description,
                AccountId = DB.Vendors.Where(v => v.Id == x.VendorId).SingleOrDefault().AccountId.ToString() + DB.Members.Where(v => v.Id == x.MemberId).SingleOrDefault().AccountId.ToString(),
                x.VendorId,
                x.MemberId,
                x.PhoneNumber,
              //  x.Vendor,
                TotalCost = x.InventoryMovements.Sum(s => s.Items.CostPrice * s.Qty),
                Total = x.InventoryMovements.Sum(s=>s.SellingPrice *s.Qty) - x.Discount,
           //     ActionLogs = DB.ActionLogs.Where(l=>l.SalesInvoiceId == x.Id).ToList(),
                InventoryMovements = x.InventoryMovements.Select(imx => new {
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
                    Total = imx.SellingPrice * imx.Qty,
                    imx.Description
                }).ToList(),
            }).ToList();
            Invoices =  (Sort == "+id" ? Invoices.OrderBy(s => s.Id).ToList() : Invoices.OrderByDescending(s => s.Id).ToList());
            return Ok(new
            {items = Invoices.Skip((Page - 1) * Limit).Take(Limit).ToList(), 
            Totals = new {
            Rows = Invoices.Count(),
            Totals = Invoices.Sum(s => s.Total),
            TotalCost = Invoices.Sum(s => s.TotalCost),
            Profit = Invoices.Sum(s => s.Total) - Invoices.Sum(s => s.TotalCost),
            Cash = Invoices.Where(i=>i.PaymentMethod == "Cash").Sum(s => s.Total),
            Receivables = Invoices.Where(i=>i.PaymentMethod == "Receivables").Sum(s => s.Total),
            Discount = Invoices.Sum(s => s.Discount),
            Visa = Invoices.Where(i=>i.PaymentMethod == "Visa").Sum(s => s.Total)
            } });
    }
        [HttpPost]
        [Route("SaleInvoice/GetByAny")]
        public IActionResult GetByAny(string Any, int Status)
        {
            if (Any == null || Any == "") return Ok();
            DateTime StartToday  = DateTime.Today;
            DateTime EndToday  = StartToday.AddHours(23).AddMinutes(59).AddSeconds(59);
            var Invoices = DB.SalesInvoices.Where(s => (s.Status == Status ||  (s.FakeDate >= StartToday &&  s.FakeDate <= EndToday)) && (s.Id.ToString().Contains(Any)  || s.Vendor.Name.Contains(Any) || s.Description.Contains(Any) || s.PhoneNumber.Contains(Any) || s.Name.Contains(Any) || s.Region.Contains(Any) )
            ).Select(x => new
                {
                    x.Id,
                    x.Discount,
                    x.Tax,
                Name = x.Name + DB.Vendors.Where(v => v.Id == x.VendorId).SingleOrDefault().Name + DB.Members.Where(v => v.Id == x.MemberId).SingleOrDefault().Name,
                x.FakeDate,
                    x.PaymentMethod,
                    x.Status,
                    x.Region,
                    x.DeliveryPrice,
                    x.Type,
                    x.Description,
                    x.VendorId,
                    x.MemberId,
                    x.PhoneNumber,
                    x.Vendor,
                    Total = x.InventoryMovements.Sum(s => s.SellingPrice * s.Qty) - x.Discount,
                    AccountId = DB.Vendors.Where(v => v.Id == x.VendorId).SingleOrDefault().AccountId.ToString() + DB.Members.Where(v => v.Id == x.MemberId).SingleOrDefault().AccountId.ToString(),
                    InventoryMovements = x.InventoryMovements.Select(imx => new {
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
                        Total = imx.SellingPrice * imx.Qty,
                        imx.Description
                    }).ToList(),
                }).ToList();

            return Ok(Invoices);
        }
        [Route("SaleInvoice/GetByItem")]
        [HttpGet]
        public IActionResult GetByItem(long ItemId,int Limit, string Sort, int Page, string User, DateTime? DateFrom, DateTime? DateTo, int? Status, string Any, string Type)
        {
            var Invoices = DB.InventoryMovements.Where(s => s.SalesInvoiceId != null &&  s.ItemsId == ItemId && (Any != null ? s.Id.ToString().Contains(Any) || s.SalesInvoice.Vendor.Name.Contains(Any) || s.Description.Contains(Any) || s.SalesInvoice.PhoneNumber.Contains(Any) || s.SalesInvoice.Name.Contains(Any)  : true) && (DateFrom != null ? s.SalesInvoice.FakeDate >= DateFrom : true)
              && (DateTo != null ? s.SalesInvoice.FakeDate <= DateTo : true) && (Status != null ? s.Status == Status : true) && (Type != null ? s.SalesInvoice.Type == Type : true) && (User != null ? DB.ActionLogs.Where(l => l.InventoryMovementId == s.Id && l.UserId == User).SingleOrDefault() != null : true)).Select(x => new
              {
                  x.Id,
                  x.SalesInvoiceId,
                  x.SalesInvoice.Discount,
                  x.Tax,
                  Name = x.SalesInvoice.Name + DB.Vendors.Where(v => v.Id == x.SalesInvoice.VendorId).SingleOrDefault().Name + DB.Members.Where(v => v.Id == x.SalesInvoice.MemberId).SingleOrDefault().Name,
                  x.SalesInvoice.FakeDate,
                  x.SalesInvoice.PaymentMethod,
                  x.Status,
                  x.SalesInvoice.Type,
                  x.Description,
                  x.SalesInvoice.VendorId,
                  x.SalesInvoice.MemberId,
                  x.SalesInvoice.PhoneNumber,
                  x.SalesInvoice.Vendor,
                  Total = x.SellingPrice * x.Qty,
                //     ActionLogs = DB.ActionLogs.Where(l=>l.SalesInvoiceId == x.Id).ToList(),
                AccountId = DB.Vendors.Where(v => v.Id == x.SalesInvoice.VendorId).SingleOrDefault().AccountId.ToString() + DB.Members.Where(v => v.Id == x.SalesInvoice.MemberId).SingleOrDefault().AccountId.ToString(),
                  InventoryMovements = x.SalesInvoice.InventoryMovements.Select(imx => new {
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
                      Total = imx.SellingPrice * imx.Qty,
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
                x.Region,
                x.DeliveryPrice,
                x.PhoneNumber,
                x.Type,
                x.Description,
                Total = x.InventoryMovements.Sum(s => s.SellingPrice * s.Qty) - x.Discount,
                AccountId =  DB.Vendors.Where(v => v.Id == x.VendorId).SingleOrDefault().AccountId.ToString() + DB.Members.Where(v => v.Id == x.MemberId).SingleOrDefault().AccountId.ToString(),
                InventoryMovements = DB.InventoryMovements.Where(im => im.SalesInvoiceId == x.Id).Select(imx => new {
                    imx.Id,
                    imx.ItemsId,
                    imx.Items.Name,
                    imx.Items.CostPrice,
                    imx.TypeMove,
                    imx.InventoryItemId,
                    imx.EXP,
                    imx.Qty,
                    imx.SellingPrice,
                    Total = imx.SellingPrice * imx.Qty,
                    imx.Description
                }).ToList(),
            }).ToList();


            return Ok(Invoices);
        }
        [HttpPost]
        [Route("SaleInvoice/Create")]

        public IActionResult Create(SalesInvoice collection)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // TODO: Add insert logic here
                  // collection.FakeDate = collection.FakeDate.ToLocalTime();
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
                    Invoice.DeliveryPrice = collection.DeliveryPrice;
                    Invoice.Region = collection.Region;
                    Invoice.Name = collection.Name;
                    Invoice.MemberId = collection.MemberId;
                    Invoice.PhoneNumber = collection.PhoneNumber;
                    Invoice.IsPrime = collection.IsPrime;
                    Invoice.Type = collection.Type;
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
        
        [HttpPost]
        [Route("SaleInvoice/EditPaymentMethod")]
        public IActionResult EditPaymentMethod(long ID , string PaymentMethod)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    SalesInvoice Invoice = DB.SalesInvoices.Where(x => x.Id == ID).SingleOrDefault();
                    Invoice.PaymentMethod = PaymentMethod;
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
        [Route("SaleInvoice/GetSaleInvoiceByListId")]
        public IActionResult GetSaleInvoiceByListId(string listid)
        {
            List<long> list = listid.Split(',').Select(long.Parse).ToList();
            var Invoices = DB.SalesInvoices.Where(s => list.Contains(s.Id)).Select(x => new
            {
                x.Id,
                x.Discount,
                x.Tax,
                Name = DB.Vendors.Where(v => v.Id == x.VendorId).SingleOrDefault().Name + DB.Members.Where(v => v.Id == x.MemberId).SingleOrDefault().Name,
                x.FakeDate,
                x.PaymentMethod,
                x.Status,
                x.Region,
                x.DeliveryPrice,
                x.PhoneNumber,
                x.Type,
                x.Description,
                TotalCost = x.InventoryMovements.Sum(s => s.Items.CostPrice * s.Qty),
                Total = x.InventoryMovements.Sum(s => s.SellingPrice * s.Qty) - x.Discount,
                AccountId = DB.Vendors.Where(v => v.Id == x.VendorId).SingleOrDefault().AccountId.ToString() + DB.Members.Where(v => v.Id == x.MemberId).SingleOrDefault().AccountId.ToString(),
                InventoryMovements = DB.InventoryMovements.Where(im => im.SalesInvoiceId == x.Id).Select(imx => new {
                    imx.Id,
                    imx.ItemsId,
                    imx.Items.Name,
                    imx.Items.CostPrice,
                    imx.Items.Ingredients,
                    imx.TypeMove,
                    imx.InventoryItemId,
                    imx.EXP,
                    imx.Qty,
                    imx.SellingPrice,
                    Total = imx.SellingPrice * imx.Qty,
                    imx.Description
                }).ToList(),
            }).ToList();
            return Ok(new
            {
                items = Invoices.ToList(),
                Totals = new
                {
                    Rows = Invoices.Count(),
                    Totals = Invoices.Sum(s => s.Total),
                    TotalCost = Invoices.Sum(s => s.TotalCost),
                    Profit = Invoices.Sum(s => s.Total) - Invoices.Sum(s => s.TotalCost),
                    Cash = Invoices.Where(i => i.PaymentMethod == "Cash").Sum(s => s.Total),
                    Receivables = Invoices.Where(i => i.PaymentMethod == "Receivables").Sum(s => s.Total),
                    Discount = Invoices.Sum(s => s.Discount),
                    Visa = Invoices.Where(i => i.PaymentMethod == "Visa").Sum(s => s.Total)
                }
            });
        }
        [Route("SaleInvoice/GetSaleInvoiceById")]
        [HttpGet]
        public IActionResult GetSaleInvoiceById(long? Id)
        {
            var Invoices = DB.SalesInvoices.Where(x => x.Id == Id).Select(x => new {
                x.Id,
                x.VendorId,
                x.Name,
                x.Discount,
                x.Tax,
                x.FakeDate,
                x.PaymentMethod,
                x.Region,
                x.DeliveryPrice,
                x.Status,
                x.Type,
                x.Description,
                x.PhoneNumber,
                Total = x.InventoryMovements.Sum(s => s.SellingPrice * s.Qty) - x.Discount,
                InventoryMovements = DB.InventoryMovements.Where(Im => Im.SalesInvoiceId == x.Id).Select(m => new {
                    m.Id,
                    m.ItemsId,
                    m.TypeMove,
                    m.Status,
                    m.Qty,
                    m.Items.Name,
                    m.SalesInvoiceId,
                    m.InventoryItemId,
                    m.SellingPrice,
                    m.Description,
                    m.EXP,
                    Total = m.SellingPrice * m.Qty,
                }).ToList()
            }).SingleOrDefault();

            return Ok(Invoices);
        }
        [Route("SaleInvoice/GetSaleInvoiceByVendorId")]
        [HttpGet]
        public IActionResult GetSaleInvoiceByVendorId(long? Id)
        {
            var Invoices = DB.SalesInvoices.Where(f => f.VendorId != null && f.VendorId == Id ).Select(x => new
            {
                x.Id,
                Name = x.Vendor.Name,
                x.Status,
                x.Type,
                x.Description,
                x.PaymentMethod,
                x.Discount,
                x.FakeDate,
                x.VendorId,
                Total = x.InventoryMovements.Sum(s => s.SellingPrice * s.Qty) - x.Discount,
                InventoryMovements = x.InventoryMovements.Select(m => new
                {
                    m.Id,
                    m.Status,
                    m.Items.Name,
                    m.Qty,
                    m.SellingPrice,
                    m.Description,
                    m.EXP,
                    Total = m.SellingPrice * m.Qty,
                }).ToList(),

            }).ToList();
            return Ok(Invoices);
        }
        [Route("SaleInvoice/GetSaleInvoiceByMemberId")]
        [HttpGet]
        public IActionResult GetSaleInvoiceByMemberId(long? Id)
        {
            var Invoices = DB.SalesInvoices.Where(f => f.MemberId !=null && f.MemberId == Id && f.IsPrime == true).Select(x => new
            {
                x.Id,
                Name =  x.Vendor.Name + x.Member.Name,
                x.Status,
                x.Type,
                x.Description,
                x.Discount,
                x.PaymentMethod,
                x.FakeDate,
                x.MemberId,
                Total = x.InventoryMovements.Sum(s => s.SellingPrice * s.Qty) - x.Discount,
                InventoryMovements = x.InventoryMovements.Select(m => new
                {
                    m.Id,
                    m.Status,
                    m.Items.Name,
                    m.Qty,
                    m.SellingPrice,
                    m.Description,
                    m.EXP,
                    Total = m.SellingPrice * m.Qty,
                }).ToList(),

            }).ToList();
            return Ok(Invoices);
        }

    }
}
