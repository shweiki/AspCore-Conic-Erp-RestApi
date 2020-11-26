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
        private ConicErpContext DB = new ConicErpContext();
        [Route("Discount/GetDiscount")]
        [HttpGet]
        public IActionResult GetDiscount()
        {
            var Discounts = (from x in DB.Discounts.ToList()
                             select new
                             {
                                 x.Id,
                                 x.Name,
                                 x.Type,
                                 x.Value,
                                 x.ValueOfDays,
                                 x.IsPrime,
                                 x.Status,
                                 x.Description,
                           
                             });
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
                    collection.Status = 0;
                    DB.Discounts.Add(collection);
                    DB.SaveChanges();
                    Oprationsy Opx = DB.Oprationsys.Where(d => d.Status == collection.Status && d.TableName == "Discount").SingleOrDefault();
                    OprationsysController Op = new OprationsysController();
                    if (Op.ChangeStatus(collection.Id, Opx.Id, "<!" + collection.Id + "!>"))
                    {
                        return Ok(true);
                    }
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
