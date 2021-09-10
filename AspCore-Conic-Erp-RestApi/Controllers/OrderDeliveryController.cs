using System;
using System.Data;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace AspCore_Conic_Erp_RestApi.Controllers
{
    [Authorize]
    public class OrderDeliveryController : Controller
    {
        private ConicErpContext DB = new ConicErpContext();
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<UserController> _logger;

        public OrderDeliveryController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, ILogger<UserController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;

        }

        [HttpPost]
        [Route("OrderDelivery/GetByListQ")]
        public IActionResult GetByListQ(int Limit, string Sort, int Page, string? User, DateTime? DateFrom, DateTime? DateTo, int? Status, string? Any)
        {
            var Deliveries = DB.OrderDeliveries.Where(s => (Any != null ? s.Id.ToString().Contains(Any) || s.Name.Contains(Any) : true) && (DateFrom != null ? s.FakeDate >= DateFrom : true)
            && (DateTo != null ? s.FakeDate <= DateTo : true) && (Status != null ? s.Status == Status : true) &&
            (User != null ? DB.ActionLogs.Where(l => l.OrderDeliveryId == s.Id && l.UserId == User).SingleOrDefault() != null : true)).Select(x => new
            {
                x.Id,
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
        [Route("OrderDelivery/GetOrderDelivery")]
        [HttpGet]
        public IActionResult GetOrderDelivery(int Limit, string Sort, int Page, int? Status, string? Any)

        {
            var Orders = DB.OrderDeliveries.Where(s => (Any != null ? s.Id.ToString().Contains(Any) || s.Name.Contains(Any) : true) && (Status != null ? s.Status == Status : true)).Select(x => new
            {
                x.Id,
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
        [Route("OrderDelivery/Create")]
        [HttpPost]
        public IActionResult Create(OrderDelivery collection)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // TODO: Add insert logic here
                    DB.OrderDeliveries.Add(collection);
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

        [Route("OrderDelivery/SetDriver")]
        [HttpPost]
        public IActionResult SetDriver(long DriverId, long OrderId)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    OrderDelivery Order = DB.OrderDeliveries.Where(x => x.Id == OrderId).SingleOrDefault();
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

        [Route("OrderDelivery/GetDriverOrder")]
        [HttpGet]
        public IActionResult GetDriverOrder(string Id, string name, int Limit, string Sort, int Page, int? Status, string? Any)

        {
            var Orders = DB.OrderDeliveries.Where(x => (x.Driver.DriverUserId == Id || name == "Developer") && (x.Status == 1 || x.Status == 2) && (Any != null ? x.Id.ToString().Contains(Any) || x.Name.Contains(Any) : true) && (Status != null ? x.Status == Status : true)).Select(x => new
            // var Orders = DB.OrderDeliveries.Where(x => x.Driver.DriverUserId == Id || name == "Developer").Select(x => new
            {
                
                x.Id,
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

        [Route("OrderDelivery/OrderReceived")]
        [HttpPost]
        public IActionResult OrderReceived(long id)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    OrderDelivery Order = DB.OrderDeliveries.Where(x => x.Id == id).SingleOrDefault();
                    Order.Status = 2;
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
        
             [Route("OrderDelivery/OrderDelivered")]
        [HttpPost]
        public IActionResult OrderDelivered(long id)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    OrderDelivery Order = DB.OrderDeliveries.Where(x => x.Id == id).SingleOrDefault();
                    Order.Status = 3;
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
        [HttpPost]
        [Route("OrderDelivery/GetByListQByDriver")]
        public IActionResult GetByListQByDriver(string Id, string name, int Limit, string Sort, int Page, string? User, DateTime? DateFrom, DateTime? DateTo, int? Status, string? Any)
        {
            var Deliveries = DB.OrderDeliveries.Where(s => (s.Driver.DriverUserId == Id || name == "Developer") && (Any != null ? s.Id.ToString().Contains(Any) || s.Name.Contains(Any) : true) && (DateFrom != null ? s.FakeDate >= DateFrom : true)
            && (DateTo != null ? s.FakeDate <= DateTo : true) && (Status != null ? s.Status == Status : true) &&
            (User != null ? DB.ActionLogs.Where(l => l.OrderDeliveryId == s.Id && l.UserId == User).SingleOrDefault() != null : true)).Select(x => new
            {
                x.Id,
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
    }
}