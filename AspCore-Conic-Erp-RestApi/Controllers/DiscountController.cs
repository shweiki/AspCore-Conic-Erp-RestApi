using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Entities; 
using Microsoft.AspNetCore.Mvc;

namespace AspCore_Conic_Erp_RestApi.Controllers
{
    [Authorize]
    public class DiscountController : Controller
    {
        private ConicErpContext DB;
        public DiscountController(ConicErpContext dbcontext)
        {
            DB = dbcontext;
        }        [Route("Discount/GetDiscount")]
        [HttpGet]
        public IActionResult GetDiscount()
        {
            var Discounts = DB.Discounts.Select(x => new {
                x.Id,
                x.Name,
                x.Type,
                x.Value,
                x.ValueOfDays,
                x.IsPrime,
                x.Status,
                x.Description,
            }).ToList();
    
            return Ok(Discounts);
        }
        [Route("Discount/GetActiveDiscount")]
        [HttpGet]
        public IActionResult GetActiveDiscount()
        {
            var Discounts = DB.Discounts.Select(x => new { value = x.Value, type = x.Type, label = x.Name ,x.ValueOfDays }).ToList();

            return Ok(Discounts);
        }
        [Route("Discount/Create")]
        [HttpPost]
        public IActionResult Create(Discount collection)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    DB.Discounts.Add(collection);
                    DB.SaveChanges();
                }
                catch
                {
                    //Console.WriteLine(collection);
                    return Ok(false);
                }
            }
            return Ok(false);
        }
        [Route("Discount/Edit")]
        [HttpPost]
        public IActionResult Edit(Discount collection)
        {
            if (ModelState.IsValid)
            {
                    Discount Discount = DB.Discounts.Where(x => x.Id == collection.Id).SingleOrDefault();
                    Discount.Name = collection.Name;
                    Discount.Value = collection.Value;
                    Discount.ValueOfDays = collection.ValueOfDays;
                    Discount.Type = collection.Type;
                    Discount.Description = collection.Description;
                try
                {
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

    }
}
