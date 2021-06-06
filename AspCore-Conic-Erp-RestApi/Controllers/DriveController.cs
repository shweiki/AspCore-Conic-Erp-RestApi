using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web;
using Microsoft.AspNetCore.Authorization;
using Entities;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace AspCore_Conic_Erp_RestApi.Controllers
{
    [Authorize]
    public class DriverController : Controller
    {
        private ConicErpContext DB = new ConicErpContext();

        [Route("Driver/GetDriver")]
        [HttpGet]
        public IActionResult GetDriver()
        {
            var Drivers = DB.Drivers.Select(x => new { x.Id, x.Name, x.Ssn, x.PhoneNumber1, x.Tag , x.Company}).ToList();

            return Ok(Drivers);
        }
        [Route("Driver/GetActiveDriver")]
        [HttpGet]
        public IActionResult GetActiveDriver()
        {
            var Drivers = DB.Drivers.Where(x => x.Status == 0).Select(x => new { 
                value = x.Id, label = x.Name , phone = x.PhoneNumber1 , x.Company}).ToList();
            return Ok(Drivers);
        }
        [Route("Driver/GetDriverByAny")]
        [HttpGet]
        public IActionResult GetDriverByAny(string Any)
        {
            Any.ToLower();
            var Drivers = DB.Drivers.Where(m => m.Id.ToString().Contains(Any) || m.Name.ToLower().Contains(Any) || m.Ssn.Contains(Any) || m.PhoneNumber1.Replace("0", "").Replace(" ", "").Contains(Any.Replace("0", "").Replace(" ", "")) || m.PhoneNumber2.Replace("0","").Replace(" ","").Contains(Any.Replace("0", "").Replace(" ", "")) || m.Tag.Contains(Any))
                .Select(x => new { x.Id, x.Name, x.Ssn, x.PhoneNumber1, x.Tag }).ToList();

            return Ok(Drivers);
        }
        [HttpPost]
        [Route("Driver/GetByListQ")]
        public IActionResult GetByListQ(int Limit, string Sort, int Page,int? Status, string Any)
        {
            var Drivers = DB.Drivers.Where(s => (Any != null ? s.Id.ToString().Contains(Any) || s.Name.Contains(Any) : true) && (Status != null ? s.Status == Status : true)).Select(x => new
            {
                x.Id,
                x.Name,
                x.Ssn,
                x.PhoneNumber1,
                x.PhoneNumber2,
                x.Status,
                x.Type,
                x.Company,
                x.Tag,
            }).ToList();
            Drivers = (Sort == "+id" ? Drivers.OrderBy(s => s.Id).ToList() : Drivers.OrderByDescending(s => s.Id).ToList());
            return Ok(new
            {
                items = Drivers.Skip((Page - 1) * Limit).Take(Limit).ToList(),
                Totals = new
                {
                    Rows = Drivers.Count(),
              
                }
            });
        }

        [Route("Driver/CheckDriverIsExist")]
        [HttpGet]
        public IActionResult CheckDriverIsExist(string Ssn , string PhoneNumber)
        {
            var Drivers = DB.Drivers.Where(m => m.Ssn == Ssn || m.PhoneNumber1.Replace("0", "") == PhoneNumber.Replace("0", "")).ToList();

            return Ok(Drivers.Count() > 0 ? true : false);
        }

  
        [Route("Driver/GetDriverByStatus")]
        [HttpGet]
        public IActionResult GetDriverByStatus(int Status)
        {
            var Drivers = DB.Drivers.Where(f => f.Status == Status).Select(x => new
            {
                x.Id,
                x.Name,
                x.Ssn,
                x.PhoneNumber1,
                x.PhoneNumber2,
                x.Status,
                x.Type,
                x.Company,
                x.Tag,
                // Avatar = Url.Content("~/Images/Driver/" + x.Id + ".jpeg").ToString(),
            }).ToList();

            return Ok(Drivers);
        }

        [Route("Driver/Create")]
        [HttpPost]
        public IActionResult Create(Driver collection)
        {
            if (ModelState.IsValid)
            {
                try
                {
               
                    DB.Drivers.Add(collection);
                    DB.SaveChanges();
                    return Ok(collection.Id);
                }
                catch
                {
                    //Console.WriteLine(collection);
                    return Ok(false);
                }
            }
            return Ok(false);
        }

        [Route("Driver/Edit")]
        [HttpPost]
        public IActionResult Edit(Driver collection)
        {
            if (ModelState.IsValid)
            {
                Driver Driver = DB.Drivers.Where(x => x.Id == collection.Id).SingleOrDefault();
                Driver.Name = collection.Name;
                Driver.Ssn = collection.Ssn;
                Driver.Email = collection.Email;
                Driver.PhoneNumber1 = collection.PhoneNumber1;
                Driver.PhoneNumber2 = collection.PhoneNumber2;
                Driver.DateofBirth = collection.DateofBirth;
                Driver.Company = collection.Company;
                Driver.Description = collection.Description;
                Driver.Status = collection.Status;
                Driver.Type = collection.Type;
                Driver.Tag = collection.Tag;
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
        [Route("Driver/GetDriverById")]
        [HttpGet]
        public IActionResult GetDriverById(long? Id)
        {
            var Drivers = DB.Drivers.Where(m => m.Id == Id).Select(
                x => new
                {
                    x.Id,
                    x.Name,
                    x.Ssn,
                    x.DateofBirth,
                    x.Email,
                    x.PhoneNumber1,
                    x.PhoneNumber2,
                    x.Description,
                    x.Status,
                    x.Type,
                    x.Tag,
                    x.Company,
                 
                }).SingleOrDefault();
            return Ok(Drivers);
        }
        [Route("Driver/FixPhoneNumber")]
        [HttpGet]
        public IActionResult FixPhoneNumber()
        {
            DB.Drivers.Where(i => i.PhoneNumber1 != null).ToList().ForEach(s => {
                s.PhoneNumber1 = s.PhoneNumber1.Replace(" ", "");
                s.PhoneNumber1 = s.PhoneNumber1.Length == 10 ? s.PhoneNumber1.Substring(1) : s.PhoneNumber1; 
            });

            DB.SaveChanges();
     

            return Ok(true);
        }

 
    }

}
