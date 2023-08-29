using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace RestApi.Controllers;

[Authorize]
public class PurchaseInvoiceController : Controller
{
    private readonly IApplicationDbContext DB;
    public PurchaseInvoiceController(IApplicationDbContext dbcontext)
    {
        DB = dbcontext;
    }
    [HttpPost]
    [Route("PurchaseInvoice/GetByListQ")]
    public IActionResult GetByListQ(int Limit, string Sort, int Page, string User, DateTime? DateFrom, DateTime? DateTo, int? Status, string Any)
    {
        var Invoices = DB.PurchaseInvoice.Where(s => (Any != null ? s.Id.ToString().Contains(Any) || s.Vendor.Name.Contains(Any) : true) && (DateFrom != null ? s.FakeDate >= DateFrom : true)
        && (DateTo != null ? s.FakeDate <= DateTo : true) && (Status != null ? s.Status == Status : true) && (User != null ? DB.ActionLog.Where(l => l.TableName == "PurchaseInvoice" && l.Fktable == s.Id.ToString() && l.UserId == User).SingleOrDefault() != null : true)).Select(x => new
        {
            x.Id,
            x.Discount,
            x.Tax,
            Name = (x.Vendor.Name ?? "") + (String.IsNullOrWhiteSpace(x.Name) ? "" : " - " + x.Name),
            x.FakeDate,
            x.PaymentMethod,
            x.Status,
            x.Description,
            x.AccountInvoiceNumber,
            x.InvoicePurchaseDate,
            Total = x.Tax + (x.InventoryMovements.Sum(s => s.SellingPrice * s.Qty) - x.Discount),
            //  Logs = DB.ActionLog.Where(l => l.PurchaseInvoiceId == x.Id).ToList(),
            x.Vendor.AccountId,
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
        var Invoices = DB.PurchaseInvoice.Where(i => i.FakeDate >= DateFrom && i.FakeDate <= DateTo).Select(x => new
        {

            x.Id,
            Name = (x.Vendor.Name ?? "") + (String.IsNullOrWhiteSpace(x.Name) ? "" : " - " + x.Name),
            x.Discount,
            x.Tax,
            x.FakeDate,
            x.PaymentMethod,
            x.Status,
            x.InvoicePurchaseDate,
            x.AccountInvoiceNumber,
            x.Description,
            InventoryMovements = DB.InventoryMovement.Where(i => i.PurchaseInvoiceId == x.Id && i.TypeMove == "In").Select(m => new
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
        var Invoices = DB.InventoryMovement.Where(s => s.PurchaseInvoiceId != null && s.ItemsId == ItemId && (Any != null ? s.Id.ToString().Contains(Any) || s.PurchaseInvoice.Vendor.Name.Contains(Any) || s.Description.Contains(Any) || s.PurchaseInvoice.Description.Contains(Any) || s.PurchaseInvoice.Name.Contains(Any) : true) && (DateFrom != null ? s.PurchaseInvoice.FakeDate >= DateFrom : true)
          && (DateTo != null ? s.PurchaseInvoice.FakeDate <= DateTo : true) && (Status != null ? s.Status == Status : true)
          && (User != null ? DB.ActionLog.Where(l => l.TableName == "PurchaseInvoice" && l.Fktable == s.Id.ToString() && l.UserId == User).SingleOrDefault() != null : true)).Select(x => new
          {
              x.Id,
              x.PurchaseInvoiceId,
              x.PurchaseInvoice.Discount,
              x.Tax,
              Name = (x.PurchaseInvoice.Vendor.Name ?? "") + (String.IsNullOrWhiteSpace(x.PurchaseInvoice.Name) ? "" : " - " + x.PurchaseInvoice.Name),
              x.PurchaseInvoice.FakeDate,
              x.PurchaseInvoice.InvoicePurchaseDate,
              x.PurchaseInvoice.PaymentMethod,
              x.Status,
              x.Description,
              x.PurchaseInvoice.VendorId,
              x.PurchaseInvoice.Vendor,
              Total = x.SellingPrice * x.Qty,
              //     ActionLogs = DB.ActionLog.Where(l=>l.PurchaseInvoiceId == x.Id).ToList(),
              AccountId = DB.Vendor.Where(v => v.Id == x.PurchaseInvoice.VendorId).SingleOrDefault().AccountId.ToString(),
              InventoryMovements = x.PurchaseInvoice.InventoryMovements.Select(imx => new
              {
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
                Tax = Invoices.Sum(s => s.Tax),
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
                collection.InventoryMovements.ToList().ForEach(s => DB.Item.Where(x => x.Id == s.ItemsId).SingleOrDefault().CostPrice = s.SellingPrice);
                DB.PurchaseInvoice.Add(collection);
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
                PurchaseInvoice Invoice = DB.PurchaseInvoice.Where(x => x.Id == collection.Id).SingleOrDefault();

                Invoice.Name = collection.Name;
                Invoice.AccountInvoiceNumber = collection.AccountInvoiceNumber;
                Invoice.Tax = collection.Tax;
                Invoice.Discount = collection.Discount;
                Invoice.Description = collection.Description;
                Invoice.Status = collection.Status;
                Invoice.VendorId = collection.VendorId;
                Invoice.FakeDate = collection.FakeDate;
                Invoice.InvoicePurchaseDate = collection.InvoicePurchaseDate;
                Invoice.PaymentMethod = collection.PaymentMethod;
                DB.InventoryMovement.RemoveRange(DB.InventoryMovement.Where(x => x.PurchaseInvoiceId == Invoice.Id).ToList());
                Invoice.InventoryMovements = collection.InventoryMovements;
                Invoice.InventoryMovements.ToList().ForEach(s => DB.Item.Where(x => x.Id == s.ItemsId).SingleOrDefault().CostPrice = s.SellingPrice);

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
        var Invoices = DB.PurchaseInvoice.Where(i => i.Id == Id).Select(x => new
        {
            x.Id,
            x.Name,
            x.VendorId,
            x.Discount,
            x.Tax,
            x.FakeDate,
            x.InvoicePurchaseDate,
            x.AccountInvoiceNumber,
            x.PaymentMethod,
            x.Status,
            x.Description,
            InventoryMovements = DB.InventoryMovement.Where(i => i.PurchaseInvoiceId == x.Id && i.TypeMove == "In").Select(m => new
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
        var Invoices = DB.PurchaseInvoice.Where(i => i.VendorId == Id).Select(x => new
        {
            x.Id,
            Name = (x.Vendor.Name ?? "") + (String.IsNullOrWhiteSpace(x.Name) ? "" : " - " + x.Name),
            x.VendorId,
            x.Discount,
            x.Tax,
            x.FakeDate,
            x.InvoicePurchaseDate,
            x.AccountInvoiceNumber,
            x.PaymentMethod,
            x.Status,
            x.Description,
            Total = x.Tax + (x.InventoryMovements.Sum(s => s.SellingPrice * s.Qty) - x.Discount),
            InventoryMovements = DB.InventoryMovement.Where(i => i.PurchaseInvoiceId == x.Id).Select(m => new
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