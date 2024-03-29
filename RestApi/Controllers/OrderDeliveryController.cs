﻿using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;

namespace RestApi.Controllers;

public class OrderDeliveryController : Controller
{
    private readonly IApplicationDbContext DB;
    public OrderDeliveryController(IApplicationDbContext dbcontext)
    {
        DB = dbcontext;
    }


    [AllowAnonymous]
    [Route("OrderDelivery/Create")]
    [HttpPost]
    public IActionResult Create(OrderDelivery collection)
    {
        if (ModelState.IsValid)
        {
            try
            {

                // TODO: Add insert logic here
                DB.OrderDelivery.Add(collection);
                DB.SaveChanges();
                return Ok(true);
            }
            catch (InvalidCastException e)
            {
                //Console.WriteLine(collection);
                return Ok(e);
            }
        }
        else return Ok(false);
    }
    public int ppt = 0;
    [AllowAnonymous]
    [Route("OrderDelivery/CreateWithDriver")]
    [HttpPost]
    public IActionResult CreateWithDriver(OrderDelivery collection)
    {
        List<long> OrderD = new List<long>();
        var DriverList = DB.Driver.Where(x => x.IsActive == 1)
                  .Select(s => new { s.Id }).ToList();
        if (DriverList.Count > 0)
        {
            foreach (var d in DriverList)
            {
                var LastDriverOrders = DB.OrderDelivery.Where(x => x.DriverId == d.Id).ToList().LastOrDefault().OrderId;
                OrderD.Add(LastDriverOrders);
            }
            OrderD.Sort();
            var smallest = OrderD[0];
            Console.WriteLine(OrderD);

            var lastOrderDriverId = DB.OrderDelivery.Where(x => x.OrderId == smallest).ToList().SingleOrDefault().DriverId;
            collection.Status = 1;
            collection.DriverId = lastOrderDriverId;
            DB.OrderDelivery.Add(collection);
            DB.SaveChanges();
            return Ok(true);

        }
        else
        {
            return Ok(false);
        }
    }

    [Authorize]
    [HttpPost]
    [Route("OrderDelivery/GetByListQ")]
    public IActionResult GetByListQ(int Limit, string Sort, int Page, string User, DateTime? DateFrom, DateTime? DateTo, int? Status, string Any)
    {
        var Deliveries = DB.OrderDelivery.Where(s => (Any == null || s.Id.ToString().Contains(Any) || s.Name.Contains(Any)) && (DateFrom == null || s.FakeDate >= DateFrom)
     && (DateTo == null || s.FakeDate <= DateTo) && (Status == null || s.Status == Status) &&
     (User == null || DB.ActionLog.Where(l => l.TableName == "OrderDelivery" && l.Fktable == s.Id.ToString() && l.UserId == User).SingleOrDefault() != null)).Select(x => new
     {
         x.Id,
         x.OrderId,
         x.Name,
         x.PhoneNumber,
         x.TotalPill,
         x.TotalPrice,
         x.Status,
         x.Content,
         x.Description,
         x.FakeDate,
         x.Region,
         x.DeliveryPrice,
         x.Driver,
     }).ToList();
        Deliveries = (Sort == "+id" ? Deliveries.OrderBy(s => s.Id).ToList() : Deliveries.OrderByDescending(s => s.Id).ToList());
        return Ok(new
        {
            items = Deliveries.Skip((Page - 1) * Limit).Take(Limit).ToList(),
            Totals = new
            {
                Rows = Deliveries.Count(),
                TotalDeliveryPrice = Deliveries.Sum(s => s.DeliveryPrice),
                TotalPrice = Deliveries.Sum(s => s.TotalPrice),
                TotalPill = Deliveries.Sum(s => s.TotalPill),
            }
        });
    }

    [Authorize]
    [Route("OrderDelivery/GetOrderDelivery")]
    [HttpGet]
    public IActionResult GetOrderDelivery(int Limit, string Sort, int Page, int? Status, string Any)

    {
        var Orders = DB.OrderDelivery.Where(s => (s.Status != 4) && (Any == null || s.Id.ToString().Contains(Any) || s.Name.Contains(Any)) && (Status == null || s.Status == Status)).Select(x => new
        {
            x.Id,
            x.OrderId,
            x.Name,
            x.PhoneNumber,
            x.TotalPill,
            x.TotalPrice,
            x.Status,
            x.Content,
            x.Description,
            x.FakeDate,
            x.Region,
            x.DeliveryPrice,
            x.Driver,

        }).ToList();
        Orders = (Sort == "+id" ? Orders.OrderBy(s => s.Id).ToList() : Orders.OrderByDescending(s => s.Id).ToList());

        return Ok(new
        {
            items = Orders.Skip((Page - 1) * Limit).Take(Limit).ToList(),
            Totals = new
            {
                Rows = Orders.Count(),
                TotalDeliveryPrice = Orders.Sum(s => s.DeliveryPrice),
                TotalPrice = Orders.Sum(s => s.TotalPrice),
                TotalPill = Orders.Sum(s => s.TotalPill),
            }
        });
    }

    [Authorize]
    [Route("OrderDelivery/SetDriver")]
    [HttpPost]
    public IActionResult SetDriver(long DriverId, long OrderId)
    {
        if (ModelState.IsValid)
        {
            try
            {
                OrderDelivery Order = DB.OrderDelivery.Where(x => x.Id == OrderId).SingleOrDefault();
                Order.DriverId = DriverId;
                Order.Status = 1;


                DB.SaveChanges();
                return Ok(true);
            }
            catch
            {
                //Console.WriteLine(collection);
                return Ok(false);
            }
        }
        return Ok(false);
    }

    [Authorize]
    [Route("OrderDelivery/GetDriverOrder")]
    [HttpGet]
    public IActionResult GetDriverOrder(string Id, string name, int Limit, string Sort, int Page, int? Status, string Any)

    {
        var Orders = DB.OrderDelivery.Where(x => (x.Driver.DriverUserId == Id || name == "Developer") && (x.Status == 1 || x.Status == 2 || x.Status == 3) && (Any == null || x.Id.ToString().Contains(Any) || x.Name.Contains(Any)) && (Status == null || x.Status == Status)).Select(x => new
        // var Orders = DB.OrderDelivery.Where(x => x.Driver.DriverUserId == Id || name == "Developer").Select(x => new
        {
            x.Id,
            x.OrderId,
            x.Name,
            x.PhoneNumber,
            x.TotalPill,
            x.TotalPrice,
            x.Status,
            x.Content,
            x.Description,
            x.FakeDate,
            x.Region,
            x.DeliveryPrice,
            x.Driver,

        }).ToList();
        Orders = (Sort == "+id" ? Orders.OrderBy(s => s.Id).ToList() : Orders.OrderByDescending(s => s.Id).ToList());

        return Ok(new
        {
            items = Orders.Skip((Page - 1) * Limit).Take(Limit).ToList(),
            Totals = new
            {
                Rows = Orders.Count(),
                TotalDeliveryPrice = Orders.Sum(s => s.DeliveryPrice),
                TotalPrice = Orders.Sum(s => s.TotalPrice),
                TotalPill = Orders.Sum(s => s.TotalPill),
            }
        });


    }

    [Authorize]
    [Route("OrderDelivery/OrderReceived")]
    [HttpPost]
    public IActionResult OrderReceived(long id)
    {



        if (ModelState.IsValid)
        {
            try
            {
                OrderDelivery Order = DB.OrderDelivery.Where(x => x.Id == id).SingleOrDefault();
                int Status = 2;
                string Description = "Received";
                if (ChangeStatus(Order, Description, Status))
                {
                    return Ok(true);
                }
                else return NotFound();
            }
            catch
            {
                return Ok(false);
            }
        }
        return Ok(false);
    }

    [Authorize]
    [Route("OrderDelivery/OrderDelivered")]
    [HttpPost]
    public IActionResult OrderDelivered(long id)
    {
        if (ModelState.IsValid)
        {
            try
            {
                OrderDelivery Order = DB.OrderDelivery.Where(x => x.Id == id).SingleOrDefault();
                int Status = 3;
                string Description = "Delivered";
                if (ChangeStatus(Order, Description, Status))
                {
                    return Ok(true);
                }
                else return NotFound();
            }
            catch
            {
                //Console.WriteLine(collection);
                return Ok(false);
            }
        }
        return Ok(false);
    }

    [Authorize]
    [Route("OrderDelivery/OrderDone")]
    [HttpPost]
    public IActionResult OrderDone(long id)
    {
        if (ModelState.IsValid)
        {
            try
            {
                OrderDelivery Order = DB.OrderDelivery.Where(x => x.Id == id).SingleOrDefault();
                int Status = 4;
                string Description = "Done";
                if (ChangeStatus(Order, Description, Status))
                {
                    return Ok(true);
                }
                else return NotFound();
            }
            catch
            {
                //Console.WriteLine(collection);
                return Ok(false);
            }
        }
        return Ok(false);
    }

    [Authorize]
    [HttpPost]
    [Route("OrderDelivery/GetByListQByDriver")]
    public IActionResult GetByListQByDriver(string Id, string name, int Limit, string Sort, int Page, string User, DateTime? DateFrom, DateTime? DateTo, int? Status, string Any)
    {
        var Deliveries = DB.OrderDelivery.Where(s => (s.Driver.DriverUserId == Id || name == "Developer") && (Any == null || s.Id.ToString().Contains(Any) || s.Name.Contains(Any)) && (DateFrom == null || s.FakeDate >= DateFrom)
        && (DateTo == null || s.FakeDate <= DateTo) && (Status == null || s.Status == Status) &&
        (User == null || DB.ActionLog.Where(l => l.TableName == "OrderDelivery" && l.Fktable == s.Id.ToString() && l.UserId == User).SingleOrDefault() != null)).Select(x => new
        {
            x.Id,
            x.OrderId,
            x.Name,
            x.Status,
            x.Content,
            x.FakeDate,
            x.Region,
            x.Driver,
        }).ToList();
        Deliveries = (Sort == "+id" ? Deliveries.OrderBy(s => s.Id).ToList() : Deliveries.OrderByDescending(s => s.Id).ToList());
        return Ok(new
        {
            items = Deliveries.Skip((Page - 1) * Limit).Take(Limit).ToList(),
            Totals = new
            {
                Rows = Deliveries.Count(),
            }
        });
    }



    public Boolean ChangeStatus(OrderDelivery Order, string Description, int Status)
    {
        ActionLog log = new ActionLog();
        log.PostingDateTime = DateTime.Now;
        log.OprationId = (int)Order.Id;
        log.Fktable = Order.DriverId.ToString();
        log.UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
        log.Description = Description;
        log.TableName = "OrderDelivery";
        DB.OrderDelivery.Where(x => x.Id == Order.DriverId).SingleOrDefault().Status = Status;


        // ActionLogController logCon = new ActionLogController(IApplicationDbContext Db);
        DB.ActionLog.Add(log);

        DB.SaveChanges();
        return true;


    }
}