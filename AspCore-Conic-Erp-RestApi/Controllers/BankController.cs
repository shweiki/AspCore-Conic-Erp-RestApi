using System.Data;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Entities; 

namespace AspCore_Conic_Erp_RestApi.Controllers
{
    [Authorize]
    public class BankController : Controller
    {
        private ConicErpContext DB;
              private readonly IUnitOfWork UW;

        public BankController(ConicErpContext dbcontext,IUnitOfWork unitOfWork)
        {
            UW = unitOfWork;
            DB = dbcontext;

        }
        // GET: Banks
        [Route("Bank/Get")]
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(UW.Banks.Get());
            //var Banks = DB.Banks.Select(x => new {
            //    x.Id,
            //    x.Iban,
            //    x.AccountId,
            //    x.Name,
            //    x.Description,
            //    x.AccountNumber,
            //    x.AccountType,
            //    x.BranchName,
            //    x.Currency,
            //    x.Status
            //}).ToList();

            //return Ok(Banks);
        }
        [Route("Bank/GetActive")]
        [HttpGet]
        public IActionResult GetActive()
        {
            var Banks = DB.Banks.Select(x => new { value = x.AccountId, label = x.Name }).ToList();
            return Ok(Banks);
        }
        // POST: Banks
        [Route("Bank/Create")]
        [HttpPost]

        public IActionResult Create(Bank collection)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Account NewAccount = new Account
                    {
                        Type = "Bank",
                        Name = collection.Name,
                        Description = collection.Description,
                        Status = 0,
                        Code = ""
                    };
                    DB.Accounts.Add(NewAccount);
                    DB.SaveChanges();
                    // TODO: Add insert logic here
                    collection.Status = 0;
                    collection.AccountId = NewAccount.Id;
                    UW.Banks.Create(collection);
                    UW.Complete();
                    //DB.Banks.Add(collection);
                    //DB.SaveChanges();


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
        public IActionResult Edit(Bank collection)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Bank bank = DB.Banks.Where(x => x.Id == collection.Id).SingleOrDefault();
                bank.Name = collection.Name;
                bank.Description = collection.Description;
                bank.Iban = collection.Iban;
                bank.AccountType = collection.AccountType;
                bank.BranchName = collection.BranchName;
                bank.AccountNumber = collection.AccountNumber;
           
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