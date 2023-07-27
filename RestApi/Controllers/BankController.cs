
using Domain;
using Domain.Entities; using Application.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RestApi.Controllers;

[Authorize]
public class BankController : Controller
{
    private readonly IApplicationDbContext DB;

    public BankController(IApplicationDbContext dbcontext)
    {
        DB = dbcontext;

    }
    // GET: Banks
    [Route("Bank/Get")]
    [HttpGet]
    public IActionResult Get()
    {
        // return Ok(UW.Bank.Get());
        var Banks = DB.Bank.Select(x => new
        {
            x.Id,
            x.Iban,
            x.AccountId,
            x.Name,
            x.Description,
            x.AccountNumber,
            x.AccountType,
            x.BranchName,
            x.Currency,
            x.Status
        }).ToList();

        return Ok(Banks);
    }
    [Route("Bank/GetActive")]
    [HttpGet]
    public IActionResult GetActive()
    {
        var Banks = DB.Bank.Select(x => new { value = x.AccountId, label = x.Name }).ToList();
        return Ok(Banks);
    }
    // POST: Banks
    [Route("Bank/Create")]
    [HttpPost]

    public async Task<IActionResult> Create(Bank collection)
    {
        if (ModelState.IsValid)
        {
            try
            {

                // TODO: Add insert logic here
                collection.Status = 0;
                collection.Account = new TreeAccount
                {
                    Type = "Bank",
                    Name = collection.Name,
                    Description = collection.Description,
                    Status = 0,
                    Code = "",
                    ParentId = DB.TreeAccount.Where(x => x.Type == "Banks-Main").SingleOrDefault().Code
                };
                DB.Bank.Add(collection);
                await DB.SaveChangesAsync(new CancellationToken(), User.Identity.Name);


            }
            catch
            {
                //Console.WriteLine(collection);
                return Ok(false);
            }
        }
        return Ok(false);
    }

    // DELETE: Banks/5
    [Route("Banks/Edit")]
    [HttpPost]
    public async Task<IActionResult> Edit(Bank collection)
    {
        if (ModelState.IsValid)
        {
            try
            {
                Bank bank = DB.Bank.Where(x => x.Id == collection.Id).SingleOrDefault();
                bank.Name = collection.Name;
                bank.Description = collection.Description;
                bank.Iban = collection.Iban;
                bank.AccountType = collection.AccountType;
                bank.BranchName = collection.BranchName;
                bank.AccountNumber = collection.AccountNumber;

                await DB.SaveChangesAsync(new CancellationToken(), User.Identity.Name);
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