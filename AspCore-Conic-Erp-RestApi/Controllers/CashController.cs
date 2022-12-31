using System.Data;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Entities; 

namespace AspCore_Conic_Erp_RestApi.Controllers;

[Authorize]
public class CashesController : Controller
{
    private ConicErpContext DB;

    public CashesController(ConicErpContext dbcontext)
    {
        DB = dbcontext;

    }
    // GET: Cashes
    [Route("Cash/GetCash")]
    [HttpGet]
    public IActionResult GetCashes()
    {
        return Ok(DB.Cashes.ToList());
    }
    [Route("Cash/GetActive")]
    [HttpGet]
    public IActionResult GetActive()
    {
        var Cash = DB.Cashes.Where(
            x => x.Status != -1 ).Select(x => new { value = x.AccountId, label = x.Name }).ToList();
        return Ok(Cash);
    }

    // PUT: Cashes/5
    [Route("Cash/Edit")]
    [HttpPost]
    public IActionResult Edit(Cash collection)
    {
        if (ModelState.IsValid)
        {
            Cash cash = DB.Cashes.Where(x => x.Id == collection.Id).SingleOrDefault();
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
                    ParentId = DB.TreeAccounts.Where(x => x.Type == "Cashes-Main").SingleOrDefault().Code

                };
                DB.TreeAccounts.Add(NewAccount);
                DB.SaveChanges();
                // TODO: Add insert logic here
                collection.Status = 0;
                collection.AccountId = NewAccount.Id;
                DB.Cashes.Add(collection);
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




