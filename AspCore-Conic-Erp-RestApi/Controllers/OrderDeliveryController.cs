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
                    Rows = Deliveries.Count()
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
    }

}