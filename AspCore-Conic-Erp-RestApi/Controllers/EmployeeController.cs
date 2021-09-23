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
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace AspCore_Conic_Erp_RestApi.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private ConicErpContext DB = new ConicErpContext();
        private readonly UserManager<IdentityUser> _userManager;
        public EmployeeController(UserManager<IdentityUser> userManager)

        {
            _userManager = userManager;

        }
        [Route("Employee/GetReceivablesEmployee")]
        [HttpGet]
        public IActionResult GetReceivablesEmployee()
        {
            var Employees = DB.Employees.Where(f => (f.Account.EntryMovements.Select(d => d.Credit).Sum() - f.Account.EntryMovements.Select(d => d.Debit).Sum()) > 0).Select(x => new
            {
                x.Id,
                x.Name,
                x.Ssn,
                x.PhoneNumber1,
                x.PhoneNumber2,
                x.Status,
                x.Type,
                x.AccountId,
                x.Tag,
                x.Vaccine,
                x.JobTitle,
                TotalDebit = x.Account.EntryMovements.Select(d => d.Debit).Sum(),
                TotalCredit = x.Account.EntryMovements.Select(c => c.Credit).Sum()
            }).ToList();

            return Ok(Employees);
        }
        [Route("Employee/GetPayablesEmployee")]
        [HttpGet]
        public IActionResult GetPayablesEmployee()
        {
            var Employees = DB.Employees.Where(f => (f.Account.EntryMovements.Select(d => d.Credit).Sum() - f.Account.EntryMovements.Select(d => d.Debit).Sum()) < 0).Select(x => new
            {
                x.Id,
                x.Name,
                x.Ssn,
                x.PhoneNumber1,
                x.PhoneNumber2,
                x.Status,
                x.Type,
                x.AccountId,
                x.Tag,
                x.Vaccine,
                x.JobTitle,
                TotalDebit = x.Account.EntryMovements.Select(d => d.Debit).Sum(),
                TotalCredit = x.Account.EntryMovements.Select(c => c.Credit).Sum()
            }).ToList();

            return Ok(Employees);
        }
        [Route("Employee/GetEmployee")]
        [HttpGet]
        public IActionResult GetEmployee()
        {
            var Employees = DB.Employees.Select(x => new { x.Id, x.Name, x.Ssn, x.PhoneNumber1, x.Tag }).ToList();

            return Ok(Employees);
        }

        [Route("Employee/GetEmployeeByAny")]
        [HttpGet]
        public IActionResult GetEmployeeByAny(string Any)
        {
            Any.ToLower();
            var Employees = DB.Employees.Where(m => m.Id.ToString().Contains(Any) || m.Name.ToLower().Contains(Any) || m.Ssn.Contains(Any) || m.PhoneNumber1.Replace("0", "").Replace(" ", "").Contains(Any.Replace("0", "").Replace(" ", "")) || m.PhoneNumber2.Replace("0","").Replace(" ","").Contains(Any.Replace("0", "").Replace(" ", "")))
                .Select(x => new { x.Id, x.Name, x.Ssn, x.PhoneNumber1}).ToList();

            return Ok(Employees);
        }
        [HttpPost]
        [Route("Employee/GetByListQ")]
        public IActionResult GetByListQ(int Limit, string Sort, int Page,int? Status, string Any)
        {
            var Employees = DB.Employees.Where(s => (Any != null ? s.Id.ToString().Contains(Any) || s.Name.Contains(Any) : true) && (Status != null ? s.Status == Status : true)).Select(x => new
            {
                x.Id,
                x.Name,
                x.Ssn,
                x.PhoneNumber1,
                x.PhoneNumber2,
                x.Status,
                x.Type,
                x.AccountId,
                x.Tag,
                x.Vaccine,
                x.JobTitle,
                //lastLogByMember= x.MemberLogs.ToList().OrderBy(o=>o.DateTime).LastOrDefault().DateTime.ToString() + ' ',
               // MembershipsCount = x.MembershipMovements.Count(),
                TotalDebit = x.Account.EntryMovements.Select(d => d.Debit).Sum(),
                TotalCredit = x.Account.EntryMovements.Select(c => c.Credit).Sum(),
            }).ToList();
            Employees = (Sort == "+id" ? Employees.OrderBy(s => s.Id).ToList() : Employees.OrderByDescending(s => s.Id).ToList());
            return Ok(new
            {
                items = Employees.Skip((Page - 1) * Limit).Take(Limit).ToList(),
                Totals = new
                {
                    Rows = Employees.Count(),
                    Totals = Employees.Sum(s => s.TotalCredit - s.TotalDebit),
                    TotalCredit = Employees.Sum(s => s.TotalCredit),
                    TotalDebit = Employees.Sum(s => s.TotalDebit),
                }
            });
        }

        [Route("Employee/CheckEmployeeIsExist")]
        [HttpGet]
        public IActionResult CheckEmployeeIsExist(string Ssn , string PhoneNumber)
        {
            var Employees = DB.Employees.Where(m => m.Ssn == Ssn || m.PhoneNumber1.Replace("0", "") == PhoneNumber.Replace("0", "")).ToList();

            return Ok(Employees.Count() > 0 ? true : false);
        }

        [Route("Employee/GetActiveEmployee")]
        [HttpGet]
        public IActionResult GetActiveEmployee()
        {
            try
            {

                var membershiplist = DB.ActionLogs.Where(l => l.MembershipMovementId != null && l.PostingDateTime >= DateTime.Today).Select(x => x.MembershipMovementId).ToList();

              var Members = DB.MembershipMovements.Where(x => membershiplist.Contains(x.Id)).ToList().Select(x => new
                {
                    x.Id,
                    Name = DB.Members.Where(m => m.Id == x.MemberId).SingleOrDefault().Name,
                    MembershipName = DB.Memberships.Where(m => m.Id == x.MembershipId).SingleOrDefault().Name,
                    x.VisitsUsed,
                    x.Type,
                    x.StartDate,
                    x.EndDate,
                    x.TotalAmmount,
                    x.Description,
                    x.Status,
                    x.Member.Vaccine,
                  // lastLog = DB.MemberLogs.Where(ml => ml.MemberId == x.MemberId).LastOrDefault().DateTime,
                    x.MemberId
                }).ToList();
                return Ok(Members);
            }
            catch
            {
                return Ok("None Active");

            }
        }

        [Route("Employee/GetEmployeeByStatus")]
        [HttpGet]
        public IActionResult GetEmployeeByStatus(int Status)
        {
            var Employees = DB.Employees.Where(f => f.Status == Status).Select(x => new
            {
                x.Id,
                x.Name,
                x.Ssn,
                x.PhoneNumber1,
                x.PhoneNumber2,
                x.Status,
                x.Type,
                x.AccountId,
                x.Tag,
                x.Vaccine,
                x.JobTitle,
                TotalDebit = DB.EntryMovements.Where(l => l.AccountId == x.AccountId).Select(d => d.Debit).Sum(),
                TotalCredit = DB.EntryMovements.Where(l => l.AccountId == x.AccountId).Select(c => c.Credit).Sum()
                // Avatar = Url.Content("~/Images/Member/" + x.Id + ".jpeg").ToString(),
            }).ToList();

            return Ok(Employees);
        }



        [Route("Employee/Create")]
        [HttpPost]
        public async Task<ActionResult> Create(Employee collection)
        {
            var Pass = "123456";
            if (ModelState.IsValid)
            {
              
                var ParentAccount = DB.Accounts.Where(i => i.Description == "Employee").SingleOrDefault();
                ParentAccount = ParentAccount ??= new Account { Id = 0, ParentId = 0, Code = "0" };
                Account NewAccount = new Account
                {
                    Type = "Employee",
                    Description = collection.Description,
                    Status = 0,
                    Code = ParentAccount.Code + '-' + DB.Accounts.Where(i => i.ParentId == ParentAccount.Id).Count() + 1,
                    ParentId = ParentAccount.Id
                };
                
                var NewUser = new IdentityUser()
                {

                    Email = collection.Email,
                    UserName = collection.Name,
                    PhoneNumber = collection.PhoneNumber1,
                    PhoneNumberConfirmed = true,
                    EmailConfirmed = true,
                };
                IdentityResult result = await _userManager.CreateAsync(NewUser, Pass);

                var unlock = await _userManager.SetLockoutEnabledAsync(NewUser, false);
              
                Pass = NewUser.PasswordHash;
                
               

                SalaryPayment NewSalary = new SalaryPayment()
                {
                    
                    GrossSalary = 0,
                    NetSalary = 0,
                    SalaryFrom = new DateTime(),
                    SalaryTo = new DateTime()

                };
                collection.EmployeeUserId = NewUser.Id;
                DB.Accounts.Add(NewAccount);
                DB.SaveChanges();
                collection.AccountId = NewAccount.Id;
                DB.Employees.Add(collection);
                DB.SaveChanges();
                NewSalary.EmployeeId = collection.Id;
                DB.SalaryPayments.Add(NewSalary);
                DB.SaveChanges();


                return Ok(collection.Id);

            }
            return Ok(false);
        }

        [Route("Employee/Edit")]
        [HttpPost]
        public IActionResult Edit(Employee collection)
        {
            if (ModelState.IsValid)
            {
                Employee Employee = DB.Employees.Where(x => x.Id == collection.Id).SingleOrDefault();
                Employee.Name = collection.Name;
                Employee.Ssn = collection.Ssn;
                Employee.Email = collection.Email;
                Employee.PhoneNumber1 = collection.PhoneNumber1;
                Employee.PhoneNumber2 = collection.PhoneNumber2;
                Employee.DateofBirth = collection.DateofBirth;
                Employee.Description = collection.Description;
                Employee.Status = collection.Status;
                Employee.Type = collection.Type;
                Employee.Tag = collection.Tag;
                Employee.Vaccine = collection.Vaccine;
                Employee.JobTitle = collection.JobTitle;
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
        [Route("Employee/GetEmployeeById")]
        [HttpGet]
        public IActionResult GetEmployeeById(long? Id)
        {
            var Employees = DB.Employees.Where(m => m.Id == Id).Select(
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
                    x.JobTitle,
                    x.Status,
                    x.Type,
                    x.Tag,
                    x.Vaccine,
                    Avatar = Url.Content("~/Images/Member/" + x.Id + ".jpeg"),
                    TotalDebit = DB.EntryMovements.Where(l => l.AccountId == x.AccountId).Select(d => d.Debit).Sum(),
                    TotalCredit = DB.EntryMovements.Where(l => l.AccountId == x.AccountId).Select(c => c.Credit).Sum(),
                    x.AccountId,
                }).SingleOrDefault();
            return Ok(Employees);
        }
        [Route("Employee/FixPhoneNumber")]
        [HttpGet]
        public IActionResult FixPhoneNumber()
        {
            DB.Employees.Where(i => i.PhoneNumber1 != null).ToList().ForEach(s => {
                s.PhoneNumber1 = s.PhoneNumber1.Replace(" ", "");
                s.PhoneNumber1 = s.PhoneNumber1.Length == 10 ? s.PhoneNumber1.Substring(1) : s.PhoneNumber1; 
            });

            DB.SaveChanges();
     

            return Ok(true);
        }

        [Route("Employee/CheckEmployees")]
        [HttpGet]
        public IActionResult CheckEmployees()
        {
           var Members = DB.Members?.ToList();
        
            foreach (var M in Members)
            {
                int OStatus = M.Status;

               if (DB.MembershipMovements.Where(x=>x.MemberId == M.Id).Count() <= 0)
               {
                   M.Status = -1;
               }

                var ActiveMemberShip = M.MembershipMovements.Where(m => m.Status == 1).SingleOrDefault();
                if (ActiveMemberShip == null) {
                    M.Status = -1;
                }

            }
            CheckBlackListActionLogEmployees();

            DB.SaveChanges();

            return Ok(true);
        }
       
        [Route("Employee/CheckBlackListActionLogEmployees")]
        [HttpGet]
        public IActionResult CheckBlackListActionLogEmployees()
        {
            var Members = DB.Members?.ToList();

            foreach (var M in Members)
            {
                int OStatus = M.Status;

                var logblacklist = DB.ActionLogs.Where(x => x.MemberId == M.Id  && x.Opration.OprationName== "BlackList").OrderBy(o => o.PostingDateTime).ToList().LastOrDefault();
                if (logblacklist != null)
                {
                    M.Status = DB.Oprationsys.Where(o=>o.Id == logblacklist.OprationId).SingleOrDefault().Status;
                }
                DB.SaveChanges();
            }
            return Ok(true);
        }
    }


}
