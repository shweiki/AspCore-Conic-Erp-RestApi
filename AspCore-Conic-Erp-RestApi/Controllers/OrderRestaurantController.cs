using System;
using System.Data;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Entities;
using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;

namespace AspCore_Conic_Erp_RestApi.Controllers
{
    public class OrderResaurantController : Controller
    {
                private ConicErpContext DB;
        public OrderResaurantController(ConicErpContext dbcontext)
        {
            DB = dbcontext;
        }


        [AllowAnonymous]
        [Route("OrderResaurant/Create")]
        [HttpPost]
        public IActionResult Create(OrderRestaurant collection, string UserId)
        {
            if (ModelState.IsValid)
            {
                var vendor = DB.Vendors.Where(x => x.UserId == UserId).FirstOrDefault();

                try
                {
                    collection.VendorId = vendor.Id;
                    collection.Name = vendor.Name;
                    collection.PhoneNumber = vendor.PhoneNumber1;
                    // TODO: Add insert logic here
                    DB.OrderRestaurants.Add(collection);
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
 
        [Authorize]
        [HttpPost]
        [Route("OrderRestaurant/GetByListQ")]
        public IActionResult GetByListQ(int Limit, string Sort, int Page, string User, DateTime? DateFrom, DateTime? DateTo, int? Status, string Any)
        {
               var Deliveries = DB.OrderRestaurants.Where(s => (Any != null ? s.Id.ToString().Contains(Any) || s.Name.Contains(Any) : true) && (DateFrom != null ? s.FakeDate >= DateFrom : true)
            && (DateTo != null ? s.FakeDate <= DateTo : true) && (Status != null ? s.Status == Status : true) &&
            (User != null ? DB.ActionLogs.Where(l => l.OrderRestaurantId == s.Id && l.UserId == User).SingleOrDefault() != null : true)).Select(x => new
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
                x.TableNo,
                x.Vendor,
            }).ToList();
            Deliveries = (Sort == "+id" ? Deliveries.OrderBy(s => s.Id).ToList() : Deliveries.OrderByDescending(s => s.Id).ToList());
            return Ok(new
            {
                items = Deliveries.Skip((Page - 1) * Limit).Take(Limit).ToList(),
                Totals = new
                {
                    Rows = Deliveries.Count(),
                    TotalPrice = Deliveries.Sum(s => s.TotalPrice),
                    TotalPill = Deliveries.Sum(s => s.TotalPill),
                }
            });
        }

        [Authorize]
        [Route("OrderRestaurant/GetOrderRestaurant")]
        [HttpGet]
        public IActionResult GetOrderRestaurant(int Limit, string Sort, int Page, int? Status, string Any)

        {
            var Orders = DB.OrderRestaurants.Where(s => (s.Status !=4) && (Any != null ? s.Id.ToString().Contains(Any) || s.Name.Contains(Any) : true) && (Status != null ? s.Status == Status : true)).Select(x => new
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
                x.TableNo,
                x.Vendor,

            }).ToList();
            Orders = (Sort == "+id" ? Orders.OrderBy(s => s.Id).ToList() : Orders.OrderByDescending(s => s.Id).ToList());

            return Ok(new
            {
                items = Orders.Skip((Page - 1) * Limit).Take(Limit).ToList(),
                Totals = new
                {
                    Rows = Orders.Count(),
                    TotalPrice = Orders.Sum(s => s.TotalPrice),
                    TotalPill = Orders.Sum(s => s.TotalPill),
                }
            });
        }

        [Authorize]
        [Route("OrderRestaurant/OrderOnTable")]
        [HttpPost]
        public IActionResult OrderOnTable(long OrderId)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    OrderRestaurant Order = DB.OrderRestaurants.Where(x => x.Id == OrderId).SingleOrDefault();
                    int Status = 1;
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
        [Route("OrderRestaurant/GetCustomerOrder")]
        [HttpGet]
        public IActionResult GetCustomerOrder(string Id, string name, int Limit, string Sort, int Page, int? Status, string Any)

        {
            var Orders = DB.OrderRestaurants.Where(x => (x.Vendor.UserId == Id || name == "Developer") && (x.Status == 1 || x.Status == 2 || x.Status == 3) && (Any != null ? x.Id.ToString().Contains(Any) || x.Name.Contains(Any) : true) && (Status != null ? x.Status == Status : true)).Select(x => new
            // var Orders = DB.OrderRestaurants.Where(x => x.Driver.DriverUserId == Id || name == "Developer").Select(x => new
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
                x.TableNo,
                x.Vendor,
                
            }).ToList();
            Orders = (Sort == "+id" ? Orders.OrderBy(s => s.Id).ToList() : Orders.OrderByDescending(s => s.Id).ToList());

            return Ok(new
            {
                items = Orders.Skip((Page - 1) * Limit).Take(Limit).ToList(),
                Totals = new
                {
                    Rows = Orders.Count(),
                    TotalPrice = Orders.Sum(s => s.TotalPrice),
                    TotalPill = Orders.Sum(s => s.TotalPill),
                }
            });
        

        }

        [Authorize]
        [Route("OrderRestaurant/VendorDone")]
        [HttpPost]
        public IActionResult VendorDone(long id)
        {
            
           

            if (ModelState.IsValid)
            {
                try
                {
                    OrderRestaurant Order = DB.OrderRestaurants.Where(x => x.Id == id).SingleOrDefault();
                    int Status = 2;
                    string Description = "Received";
                    if (ChangeStatus( Order, Description, Status ))
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
        [Route("OrderRestaurant/OrdrerCheckout")]
        [HttpPost]
        public IActionResult OrderCheckOut(long id)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    OrderRestaurant Order = DB.OrderRestaurants.Where(x => x.Id == id).SingleOrDefault();
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
        [Route("OrderRestaurant/OrderDone")]
        [HttpPost]
        public IActionResult OrderDone(long id)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    OrderRestaurant Order = DB.OrderRestaurants.Where(x => x.Id == id).SingleOrDefault();
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
        [Route("OrderRestaurant/GetByListQByVendor")]
        public IActionResult GetByListQByDriver(string Id, string name, int Limit, string Sort, int Page, string User, DateTime? DateFrom, DateTime? DateTo, int? Status, string Any)
        {
            var Deliveries = DB.OrderRestaurants.Where(s => (s.Vendor.UserId == Id || name == "Developer") && (Any != null ? s.Id.ToString().Contains(Any) || s.Name.Contains(Any) : true) && (DateFrom != null ? s.FakeDate >= DateFrom : true)
            && (DateTo != null ? s.FakeDate <= DateTo : true) && (Status != null ? s.Status == Status : true) &&
            (User != null ? DB.ActionLogs.Where(l => l.OrderDeliveryId == s.Id && l.UserId == User).SingleOrDefault() != null : true)).Select(x => new
            {
                x.Id,
                x.OrderId,
                x.Name,
                x.Status,
                x.Content,
                x.FakeDate,
                x.TableNo,
                x.Vendor,
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


       
        public Boolean ChangeStatus(OrderRestaurant Order, string Description, int Status)
        {
            ActionLog log = new ActionLog();
            log.PostingDateTime = DateTime.Now;
            log.OprationId = (int)Order.Id;
           // log.DriverId = Order.DriverId;
            log.UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            log.Description = Description;
            log.Fktable = "{Vendor";
            log.TableName = "OrederRestaurant";
            log.OrderRestaurantId = (int)Order.Id;
            DB.OrderRestaurants.Where(x => x.Id == log.OrderRestaurantId).SingleOrDefault().Status = Status;


            // ActionLogController logCon = new ActionLogController(ConicErpContext Db);
            DB.ActionLogs.Add(log);
            
                DB.SaveChanges();
                return true;
        

        }
    }
}