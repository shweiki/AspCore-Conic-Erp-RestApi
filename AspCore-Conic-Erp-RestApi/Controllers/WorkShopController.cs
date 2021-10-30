using System;
using System.Data;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Entities;
using Microsoft.AspNetCore.Mvc;

namespace AspCore_Conic_Erp_RestApi.Controllers
{
    [Authorize]
    public class WorkShopController : Controller
    {
                private ConicErpContext DB;
        public WorkShopController(ConicErpContext dbcontext)
        {
            DB = dbcontext;
        }
        [HttpPost]
        [Route("WorkShop/GetByListQ")]
        public IActionResult GetByListQ(int Limit, string Sort, int Page, string User, DateTime? DateFrom, DateTime? DateTo, int? Status, string Any)
        {
            var Invoices = DB.WorkShops.Where(s => (Any != null ? s.Id.ToString().Contains(Any) || s.Vendor.Name.Contains(Any) : true) && (DateFrom != null ? s.FakeDate >= DateFrom : true)
            && (DateTo != null ? s.FakeDate <= DateTo : true) && (Status != null ? s.Status == Status : true) && (User != null ? DB.ActionLogs.Where(l => l.WorkShopId == s.Id && l.UserId == User).SingleOrDefault() != null : true)).Select(x => new
            {
                x.Id,
                x.Discount,
                x.Tax,
                Name = x.Vendor.Name,
                x.FakeDate,
                x.PaymentMethod,
                x.Status,
                x.Description,
                x.DeliveryDate,
                x.TotalAmmount,
                x.LowCost,
                Total = x.InventoryMovements.Sum(s => s.SellingPrice * s.Qty) - x.Discount,
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

        [Route("WorkShop/GetWorkShop")]
        [HttpGet]
        public IActionResult GetWorkShop(DateTime DateFrom, DateTime DateTo)
        {
            var Invoices = DB.WorkShops.Where(i => i.FakeDate >= DateFrom && i.FakeDate <= DateTo).Select(x => new
            {

                x.Id,
                Name = x.Vendor.Name + " - " + x.Name,
                x.Discount,
                x.Tax,
                x.FakeDate,
                x.PaymentMethod,
                x.Status,
                x.DeliveryDate,
                x.LowCost,
                x.Description,
                InventoryMovements = DB.InventoryMovements.Where(i => i.WorkShopId == x.Id && i.TypeMove == "In").Select(m => new
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
        [Route("WorkShop/GetByItem")]
        [HttpGet]
        public IActionResult GetByItem(long ItemId, int Limit, string Sort, int Page, string User, DateTime? DateFrom, DateTime? DateTo, int? Status, string Any, string Type)
        {
            var Invoices = DB.InventoryMovements.Where(s =>s.WorkShopId !=null && s.ItemsId == ItemId && (Any != null ? s.Id.ToString().Contains(Any) || s.WorkShop.Vendor.Name.Contains(Any) || s.Description.Contains(Any) || s.WorkShop.Description.Contains(Any) || s.WorkShop.Name.Contains(Any) : true) && (DateFrom != null ? s.WorkShop.FakeDate >= DateFrom : true)
              && (DateTo != null ? s.WorkShop.FakeDate <= DateTo : true) && (Status != null ? s.Status == Status : true) &&  (User != null ? DB.ActionLogs.Where(l => l.InventoryMovementId == s.Id && l.UserId == User).SingleOrDefault() != null : true)).Select(x => new
              {
                  x.Id,
                  x.WorkShopId,
                  x.WorkShop.Discount,
                  x.Tax,
                  Name = x.WorkShop.Name, //+ DB.Vendors.Where(v => v.Id == x.VendorId).SingleOrDefault().Name + DB.Members.Where(v => v.Id == x.MemberId).SingleOrDefault().Name,
                  x.WorkShop.FakeDate,
                  x.WorkShop.DeliveryDate,
                  x.WorkShop.PaymentMethod,
                  x.Status,
                  x.Description,
                  x.WorkShop.VendorId,
                  x.WorkShop.Vendor,
                  Total = x.SellingPrice * x.Qty,
                  //     ActionLogs = DB.ActionLogs.Where(l=>l.WorkShopId == x.Id).ToList(),
                  AccountId = DB.Vendors.Where(v => v.Id == x.WorkShop.VendorId).SingleOrDefault().AccountId.ToString(),
                  InventoryMovements = x.WorkShop.InventoryMovements.Select(imx => new {
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
        [Route("WorkShop/Create")]
        [HttpPost]
        public IActionResult Create(WorkShop collection)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // TODO: Add insert logic here
                    collection.InventoryMovements.ToList().ForEach(s => DB.Items.Where(x => x.Id == s.ItemsId).SingleOrDefault().CostPrice = s.SellingPrice);
                    DB.WorkShops.Add(collection);
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
        [Route("WorkShop/Edit")]
        [HttpPost]
        public IActionResult Edit(WorkShop collection)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    WorkShop Invoice = DB.WorkShops.Where(x => x.Id == collection.Id).SingleOrDefault();

                    Invoice.Name = collection.Name;
                    Invoice.Tax = collection.Tax;
                    Invoice.Discount = collection.Discount;
                    Invoice.Description = collection.Description;
                    Invoice.Status = collection.Status;
                    Invoice.VendorId = collection.VendorId;
                    Invoice.FakeDate = collection.FakeDate;
                    Invoice.TotalAmmount = collection.TotalAmmount;
                    Invoice.DeliveryDate = collection.DeliveryDate;
                    Invoice.LowCost = collection.LowCost;
                    Invoice.PaymentMethod = collection.PaymentMethod;
                    DB.InventoryMovements.RemoveRange(DB.InventoryMovements.Where(x => x.WorkShopId == Invoice.Id).ToList());
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
        [Route("WorkShop/GetWorkShopById")]
        [HttpGet]
        public IActionResult GetWorkShopById(long? Id)
        {
            var Invoices = DB.WorkShops.Where(i => i.Id == Id).Select(x => new
            {
                x.Id,
                Name = x.Vendor.Name + " - " + x.Name,
                x.VendorId,
                x.Discount,
                x.Tax,
                x.FakeDate,
                x.DeliveryDate,
                x.LowCost,
                x.TotalAmmount,
                x.PaymentMethod,
                x.Status,
                x.Description,
                InventoryMovements = DB.InventoryMovements.Where(i => i.WorkShopId == x.Id && i.TypeMove == "In").Select(m => new
                {
                    m.Id,
                    m.ItemsId,
                    m.TypeMove,
                    m.Status,
                    m.Qty,
                    m.SellingPrice,
                    m.Items.Name,
                    m.WorkShopId,
                    m.InventoryItemId,
                    m.Description,
                    m.EXP
                }).ToList()

            }).SingleOrDefault();


            return Ok(Invoices);
        }

    }
}