using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
namespace RestApi.Controllers;

[Authorize]
public class VendorController : Controller
{
    private readonly IApplicationDbContext DB;
    public VendorController(IApplicationDbContext dbcontext)
    {
        DB = dbcontext;

    }
    [Route("Vendor/GetVendor")]
    [HttpGet]
    public IActionResult GetVendor()
    {
        var Vendors = DB.Vendor.Select(x => new
        {
            x.Id,
            x.Name,
            x.Region,
            x.Email,
            x.PhoneNumber1,
            x.PhoneNumber2,
            x.Fax,
            x.Description,
            x.CreditLimit,
            x.IsPrime,
            x.Type,
            x.Ssn,
            x.AccountId,
            x.Status,
            TotalDebit = DB.EntryMovement.Where(l => l.AccountId == x.AccountId).Select(d => d.Debit).Sum(),
            TotalCredit = DB.EntryMovement.Where(l => l.AccountId == x.AccountId).Select(c => c.Credit).Sum(),
        }).ToList();

        return Ok(Vendors);
    }
    [Route("Vendor/GetActiveVendor")]
    [HttpGet]
    public IActionResult GetActiveVendor()
    {
        var Vendor = DB.Vendor.Where(x => x.Status == 0).Select(x => new { value = x.Id, label = x.Name }).ToList();
        return Ok(Vendor);
    }
    [Route("Vendor/GetVendorByAny")]
    [HttpGet]
    public IActionResult GetVendorByAny(string Any)
    {
        Any.ToLower();
        var Vendors = DB.Vendor.Where(m => m.Id.ToString().Contains(Any) || m.Name.ToLower().Contains(Any) || m.PhoneNumber1.Replace("0", "").Replace(" ", "").Contains(Any.Replace("0", "").Replace(" ", "")) || m.PhoneNumber2.Replace("0", "").Replace(" ", "").Contains(Any.Replace("0", "").Replace(" ", "")))
            .Select(x => new { x.Id, x.Name, x.PhoneNumber1, x.Ssn, x.AccountId }).ToList();

        return Ok(Vendors);
    }
    [HttpPost]
    [Route("Vendor/GetByListQ")]
    public IActionResult GetByListQ(int Limit, string Sort, int Page, int? Status, string Any)
    {
        var Vendors = DB.Vendor.Include(x => x.Account.EntryMovements).Where(s => (Any == null || s.Id.ToString().Contains(Any) || s.Name.Contains(Any) || s.PhoneNumber1.Replace("0", "").Replace(" ", "").Contains(Any.Replace("0", "").Replace(" ", "")) || s.PhoneNumber2.Replace("0", "").Replace(" ", "").Contains(Any.Replace("0", "").Replace(" ", "")))
        && (Status == null || s.Status == Status)).Select(x => new
        {
            x.Id,
            x.Name,
            x.PhoneNumber1,
            x.PhoneNumber2,
            x.Status,
            x.Type,
            x.Region,
            x.Ssn,
            x.AccountId,
            TotalDebit = x.Account.EntryMovements.Select(d => d.Debit).Sum(),
            TotalCredit = x.Account.EntryMovements.Select(c => c.Credit).Sum(),
        }).ToList();
        Vendors = (Sort == "+id" ? Vendors.OrderBy(s => s.Id).ToList() : Vendors.OrderByDescending(s => s.Id).ToList());
        return Ok(new
        {
            items = Vendors.Skip((Page - 1) * Limit).Take(Limit).ToList(),
            Totals = new
            {
                Rows = Vendors.Count(),
                Totals = Vendors.Sum(s => s.TotalCredit - s.TotalDebit),
                TotalCredit = Vendors.Sum(s => s.TotalCredit),
                TotalDebit = Vendors.Sum(s => s.TotalDebit),
            }
        });
    }
    [Route("Vendor/CheckIsExist")]
    [HttpGet]
    public IActionResult CheckIsExist(string Name, string PhoneNumber, string Ssn)
    {
        var Vendor = DB.Vendor.Where(m => (Name != null && m.Name == Name) || (Ssn != null && m.Ssn == Ssn) || (PhoneNumber != null && m.PhoneNumber1.Replace("0", "") == PhoneNumber.Replace("0", ""))).ToList();

        return Ok(Vendor.Count() > 0);
    }
    [Route("Vendor/Create")]
    [HttpPost]
    public IActionResult Create(Vendor collection)
    {
        if (ModelState.IsValid)
        {
            try
            {
                TreeAccount NewAccount = new TreeAccount
                {
                    Name = collection.Name,
                    Type = "Vendor",
                    Description = collection.Description,
                    Status = 0,
                    Code = "",
                    ParentId = DB.TreeAccount.Where(x => x.Type == collection.Type + "s-Main").SingleOrDefault().Code
                };
                DB.TreeAccount.Add(NewAccount);
                DB.SaveChanges();
                collection.Status = 0;
                collection.AccountId = NewAccount.Id;
                DB.Vendor.Add(collection);
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
    [Route("Vendor/GetById")]
    [HttpGet]
    public async Task<IActionResult> GetById(long? Id)
    {
        var Vendor = await DB.Vendor.Include(x => x.Account.EntryMovements).Where(m => m.Id == Id).Select(
            x => new
            {
                x.Id,
                x.Name,
                x.Ssn,
                x.Email,
                x.PhoneNumber1,
                x.PhoneNumber2,
                x.Description,
                x.Status,
                x.Region,
                x.Type,
                TotalDebit = x.Account.EntryMovements.Sum(d => d.Debit),
                TotalCredit = x.Account.EntryMovements.Sum(c => c.Credit),
                x.AccountId,

            }).SingleOrDefaultAsync();
        return Ok(Vendor);
    }
    [Route("Vendor/Edit")]
    [HttpPost]
    public IActionResult Edit(Vendor collection)
    {
        if (ModelState.IsValid)
        {
            try
            {
                Vendor vendor = DB.Vendor.Where(x => x.Id == collection.Id).SingleOrDefault();
                vendor.Name = collection.Name;
                vendor.Ssn = collection.Ssn;
                vendor.Region = collection.Region;
                vendor.Email = collection.Email;
                vendor.PhoneNumber1 = collection.PhoneNumber1;
                vendor.PhoneNumber2 = collection.PhoneNumber2;
                vendor.Fax = collection.Fax;
                vendor.Description = collection.Description;
                vendor.CreditLimit = collection.CreditLimit;
                vendor.Status = collection.Status;
                vendor.IsPrime = collection.IsPrime;
                vendor.Type = collection.Type;

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
        var Vendor = DB.Vendor.Where(x => x.Status == 0).Select(x => new { value = x.Id, label = x.Name }).ToList();
        return Ok(Vendor);
    }

    [Route("Vendor/GetVendorByUserId")]
    [HttpGet]
    public IActionResult GetVendorByUserId(String Id)
    {

        var Vendors = DB.Vendor.Where(m => m.UserId == Id)
            .Select(x => new { x.Id, x.Name, x.PhoneNumber1 }).ToList();

        return Ok(Vendors);

        //if (ModelState.IsValid)
        //{
        //    try
        //    {
        //        var vendors = DB.Vendor.Where(m => m.UserId == Id)
        //            .Select(x => new {
        //                x.Id
        //            }).SingleOrDefault();
        //        return Ok(vendors);
        //    }
        //    catch
        //    {
        //        //Console.WriteLine(collection);
        //        return Ok(false);
        //    }
        //}
        //return Ok(true);
    }


}
