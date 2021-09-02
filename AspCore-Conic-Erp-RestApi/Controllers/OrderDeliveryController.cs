using System;
using System.Data;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Entities;

namespace AspCore_Conic_Erp_RestApi.Controllers
{
    [Authorize]
    public class OrderDeliveryController : Controller
    {
        private ConicErpContext DB = new ConicErpContext();


        [HttpPost]
        [Route("OrderDelivery/GetOrderDeliveryByListQ")]
        public IActionResult GetOrderDeliveryByListQ(int Limit, string Sort, int Page, string? User, DateTime? DateFrom, DateTime? DateTo, int? Status, string? Any)
        {
            var Deliveries = DB.OrderDeliveries.Where(s => (Any != null ? s.Id.ToString().Contains(Any) || s.Vendor.Name.Contains(Any) : true) && (DateFrom != null ? s.FakeDate >= DateFrom : true)
            && (DateTo != null ? s.FakeDate <= DateTo : true) && (Status != null ? s.Status == Status : true) &&
            (User != null ? DB.ActionLogs.Where(l => l.OrderDeliveryId == s.Id && l.UserId == User).SingleOrDefault() != null : true)).Select(x => new

            {
                x.Id,
                x.Name,
                x.Region,
                x.TotalPill,
                x.TotalPrice,
                x.FakeDate,
                x.DeliveryPrice,
                x.Description,
                x.DriverName,
                x.Status,
            }).ToList();
            Deliveries = (Sort == "+id" ? Deliveries.OrderBy(s => s.Id).ToList() : Deliveries.OrderByDescending(s => s.Id).ToList());
            return Ok(new
            {
                items = Deliveries.Skip((Page - 1) * Limit).Take(Limit).ToList(),
                Totals = new
                {
                    Rows = Deliveries.Count()
                }
                });
        }

        [Route("OrderDelivery/GetOrderDelivery")]
        [HttpGet]
        public IActionResult GetOrderDelivery()

         {
             var Delivery = DB.OrderDeliveries.Select(x => new {
                 x.Id,
                 x.Description,
                 x.FakeDate,
                 x.IsPrime,
                 x.TotalPrice,
                 x.Status,
                 x.VendorId,
                 x.DriverName,
                 x.Name,
                 x.Region
                  
              }).ToList();

              return Ok(Delivery);
           }

     
        [Route("OrderDelivery/CreateDelivery")]
        [HttpPost]
        public IActionResult CreateDelivery(OrderDelivery collection)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // TODO: Add insert logic here
                    collection.Status = 0;
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

       
    }

}