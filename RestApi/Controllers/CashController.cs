using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Linq;

namespace RestApi.Controllers;

[Authorize]
public class CashesController : Controller
{
    private readonly IApplicationDbContext DB;

    public CashesController(IApplicationDbContext dbcontext)
    {
        DB = dbcontext;

    }
    // GET: Cashes
    [Route("Cash/GetCash")]
    [HttpGet]
    public IActionResult GetCashes()
    {
        return Ok(DB.Cash.ToList());
    }
    [Route("Cash/GetActive")]
    [HttpGet]
    public IActionResult GetActive()
    {
        var Cash = DB.Cash.Where(
            x => x.Status != -1).Select(x => new { value = x.AccountId, label = x.Name }).ToList();
        return Ok(Cash);
    }

    // PUT: Cashes/5
    [Route("Cash/Edit")]
    [HttpPost]
    public IActionResult Edit(Cash collection)
    {
        if (ModelState.IsValid)
        {
            Cash cash = DB.Cash.Where(x => x.Id == collection.Id).SingleOrDefault();
            cash.Pcip = collection.Pcip;
            cash.Btcash = collection.Btcash;
            cash.Description = collection.Description;
            cash.Status = collection.Status;
            cash.Name = collection.Name;
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
        else return Ok(false);
    }

    // POST: Cashes
    [Route("Cash/Create")]
    [HttpPost]
    public IActionResult Create(Cash collection)
    {
        if (ModelState.IsValid)
        {
            try
            {
                TreeAccount NewAccount = new TreeAccount
                {
                    Type = "Cash",
                    Name = collection.Name,
                    Description = collection.Description,
                    Status = 0,
                    Code = "",
                    ParentId = DB.TreeAccount.Where(x => x.Type == "Cashes-Main").SingleOrDefault().Code

                };
                DB.TreeAccount.Add(NewAccount);
                DB.SaveChanges();
                // TODO: Add insert logic here
                collection.Status = 0;
                collection.AccountId = NewAccount.Id;
                DB.Cash.Add(collection);
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


}




