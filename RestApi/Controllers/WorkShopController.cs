﻿using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace RestApi.Controllers;

[Authorize]
public class WorkShopController : Controller
{
    private readonly IApplicationDbContext DB;
    public WorkShopController(IApplicationDbContext dbcontext)
    {
        DB = dbcontext;
    }
    [HttpPost]
    [Route("WorkShop/GetByListQ")]
    public IActionResult GetByListQ(int Limit, string Sort, int Page, string User, DateTime? DateFrom, DateTime? DateTo, int? Status, string Any)
    {
        var Invoices = DB.WorkShop.Where(s => (Any == null || s.Id.ToString().Contains(Any) || s.Vendor.Name.Contains(Any)) && (DateFrom == null || s.DeliveryDate >= DateFrom)
        && (DateTo == null || s.DeliveryDate <= DateTo) && (Status == null || s.Status == Status) && (User == null || DB.ActionLog.Where(l => l.TableName == "WorkShop" && l.Fktable == s.Id.ToString() && l.UserId == User).SingleOrDefault() != null)).Select(x => new
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
        var Invoices = DB.WorkShop.Where(i => i.FakeDate >= DateFrom && i.FakeDate <= DateTo).Select(x => new
        {

            x.Id,
            x.Name,
            x.Discount,
            x.Tax,
            x.FakeDate,
            x.PaymentMethod,
            x.Status,
            x.DeliveryDate,
            x.LowCost,
            x.Description,
            InventoryMovements = DB.InventoryMovement.Where(i => i.WorkShopId == x.Id && i.TypeMove == "In").Select(m => new
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
        var Invoices = DB.InventoryMovement.Where(s => s.WorkShopId != null && s.ItemsId == ItemId && (Any == null || s.Id.ToString().Contains(Any) || s.WorkShop.Vendor.Name.Contains(Any) || s.Description.Contains(Any) || s.WorkShop.Description.Contains(Any) || s.WorkShop.Name.Contains(Any)) && (DateFrom == null || s.WorkShop.FakeDate >= DateFrom)
          && (DateTo == null || s.WorkShop.FakeDate <= DateTo) && (Status == null || s.Status == Status)
          && (User == null || DB.ActionLog.Where(l => l.TableName == "InventoryMovement" && l.Fktable == s.Id.ToString() && l.UserId == User).SingleOrDefault() != null)).Select(x => new
          {
              x.Id,
              x.WorkShopId,
              x.WorkShop.Discount,
              x.Tax,
              Name = x.WorkShop.Name, //+ DB.Vendor.Where(v => v.Id == x.VendorId).SingleOrDefault().Name + DB.Member.Where(v => v.Id == x.MemberId).SingleOrDefault().Name,
              x.WorkShop.FakeDate,
              x.WorkShop.DeliveryDate,
              x.WorkShop.PaymentMethod,
              x.Status,
              x.Description,
              x.WorkShop.VendorId,
              x.WorkShop.Vendor,
              Total = x.SellingPrice * x.Qty,
              //     ActionLogs = DB.ActionLog.Where(l=>l.WorkShopId == x.Id).ToList(),
              AccountId = DB.Vendor.Where(v => v.Id == x.WorkShop.VendorId).SingleOrDefault().AccountId.ToString(),
              InventoryMovements = x.WorkShop.InventoryMovements.Select(imx => new
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
                collection.InventoryMovements.ToList().ForEach(s => DB.Item.Where(x => x.Id == s.ItemsId).SingleOrDefault().CostPrice = s.SellingPrice);
                DB.WorkShop.Add(collection);
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
                WorkShop Invoice = DB.WorkShop.Where(x => x.Id == collection.Id).SingleOrDefault();

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
                DB.InventoryMovement.RemoveRange(DB.InventoryMovement.Where(x => x.WorkShopId == Invoice.Id).ToList());
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
    [Route("WorkShop/GetWorkShopById")]
    [HttpGet]
    public IActionResult GetWorkShopById(long? Id)
    {
        var Invoices = DB.WorkShop.Where(i => i.Id == Id).Select(x => new
        {
            x.Id,
            x.Name,
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
            InventoryMovements = DB.InventoryMovement.Where(i => i.WorkShopId == x.Id).Select(m => new
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