using System.Data;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Entities; 
using Microsoft.AspNetCore.Mvc;

namespace AspCore_Conic_Erp_RestApi.Controllers
{
    [Authorize]
    public class VendorController : Controller
    {
        private ConicErpContext DB = new ConicErpContext();
        [Route("Vendor/GetVendor")]
        [HttpGet]
        public IActionResult GetVendor()
        {
            var Vendors = DB.Vendors.Select(x => new
            {
                x.Id,
                x.Name,
                x.Address,
                x.Email,
                x.PhoneNumber1,
                x.PhoneNumber2,
                x.Fax,
                x.Description,
                x.CreditLimit,
                x.IsPrime,
                x.Type,
                x.AccountId,
                x.Status,
                TotalDebit = DB.EntryMovements.Where(l => l.AccountId == x.AccountId).Select(d => d.Debit).Sum(),
                TotalCredit = DB.EntryMovements.Where(l => l.AccountId == x.AccountId).Select(c => c.Credit).Sum(),
            }).ToList();

            return Ok(Vendors);
        }
        [Route("Vendor/GetActiveVendor")]
        [HttpGet]
        public IActionResult GetActiveVendor()
        {
            var Vendor = DB.Vendors.Where(x => x.Status == 0).Select(x => new { value = x.Id, label = x.Name }).ToList();
            return Ok(Vendor);
        }
        [Route("Vendor/Create")]
        [HttpPost]
        public IActionResult Create(Vendor collection)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Account NewAccount = new Account
                    {
                        Type = "Vendor",
                        Description = collection.Description,
                        Status = 0,
                        Code = ""
                    };
                    DB.Accounts.Add(NewAccount);
                    DB.SaveChanges();
                    collection.Status = 0;
                    collection.AccountId = NewAccount.Id;
                    DB.Vendors.Add(collection);
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

        [Route("Vendor/Edit")]
        [HttpPost]
        public IActionResult Edit(Vendor collection)
        {
            if (ModelState.IsValid)
            {
                Vendor vendor = DB.Vendors.Where(x => x.Id == collection.Id).SingleOrDefault();
                vendor.Name = collection.Name;
                vendor.Address = collection.Address;
                vendor.Email = collection.Email;
                vendor.PhoneNumber1 = collection.PhoneNumber1;
                vendor.PhoneNumber2 = collection.PhoneNumber2;
                vendor.Fax = collection.Fax;
                vendor.Description = collection.Description;
                vendor.CreditLimit = collection.CreditLimit;
                vendor.Status = collection.Status;
                vendor.IsPrime = collection.IsPrime;
                vendor.Type = collection.Type;
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
         [Route("Vendor/GetVendorCheque")]
        [HttpGet]
        public IActionResult GetVendorCheque()
        {
            var Vendor = DB.Vendors.Where(x => x.Status == 0).Select(x => new { value = x.Id, label = x.Name }).ToList();
            return Ok(Vendor);
        }

    }
}
