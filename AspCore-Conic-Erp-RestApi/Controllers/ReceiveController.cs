using System;
using System.Data;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace AspCore_Conic_Erp_RestApi.Controllers
{
    [Authorize]
    public class ReceiveController : Controller
    {
                private ConicErpContext DB;
        public ReceiveController(ConicErpContext dbcontext)
        {
            DB = dbcontext;
        }
        [HttpPost]
        [Route("Receive/GetByListQ")]
        public IActionResult GetByListQ(int Limit, string Sort, int Page, string User, DateTime? DateFrom, DateTime? DateTo, int? Status, string Any)
        {
            var Invoices = DB.Receives.Where(s => (Any != null ? s.Id.ToString().Contains(Any) || s.Vendor.Name.Contains(Any) : true) && (DateFrom != null ? s.FakeDate >= DateFrom : true)
            && (DateTo != null ? s.FakeDate <= DateTo : true) && (Status != null ? s.Status == Status : true) &&
            (User != null ? DB.ActionLogs.Where(l => l.ReceiveId == s.Id && l.UserId == User).SingleOrDefault() != null : true)).Select(x => new

            {
                x.Id,
                x.TotalAmmount,
                x.Type,
                x.EditorName,
                Name = x.Vendor.Name + " " + x.Member.Name + " - " + x.Name,
                x.FakeDate,
                x.ReceiveMethod,
                x.Status,
                x.Description,
                x.VendorId,
                x.MemberId,
                ObjectId = x.VendorId == null ? x.MemberId : x.VendorId,
                AccountId = (x.Vendor == null) ? x.Member.AccountId : x.Vendor.AccountId,
            }).ToList();
            Invoices = (Sort == "+id" ? Invoices.OrderBy(s => s.Id).ToList() : Invoices.OrderByDescending(s => s.Id).ToList());
            return Ok(new
            {
                items = Invoices.Skip((Page - 1) * Limit).Take(Limit).ToList(),
                Totals = new
                {
                    Rows = Invoices.Count(),
                    Totals = Invoices.Sum(s => s.TotalAmmount),
                    Cash = Invoices.Where(i => i.ReceiveMethod == "Cash").Sum(s => s.TotalAmmount),
                    Cheque = Invoices.Where(i => i.ReceiveMethod == "Cheque").Sum(s => s.TotalAmmount),
                    Visa = Invoices.Where(i => i.ReceiveMethod == "Visa").Sum(s => s.TotalAmmount)
                }
            });
        }

        [Route("Receive/GetReceive")]
        [HttpGet]
        public ActionResult GetReceive(DateTime DateFrom, DateTime DateTo, int Status)
        {
            var Receives = DB.Receives.Where(i => i.FakeDate >= DateFrom && i.FakeDate <= DateTo && i.Status == Status).Select(x => new
            {
                x.Id,
                x.TotalAmmount,
                x.Type,
                x.EditorName,
                Name = x.Vendor.Name + " " + x.Member.Name + " - " + x.Name,
                x.FakeDate,
                x.ReceiveMethod,
                x.Status,
                x.Description,
                x.VendorId,
                x.MemberId,
                ObjectId = x.VendorId == null ? x.MemberId : x.VendorId,
                AccountId = (x.Vendor == null) ? x.Member.AccountId : x.Vendor.AccountId,
            }).ToList();


            return Ok(Receives.ToList());
        }
        [Route("Receive/GetReceivesByMemberId")]
        [HttpGet]
        public IActionResult GetReceivesByMemberId(long? MemberId)
        {
            var Receives = DB.Receives.Where(i => i.MemberId == MemberId).Select(x => new {
                x.Id,
                x.TotalAmmount,
                x.Type,
                x.EditorName,
                Name = x.Vendor.Name + " " + x.Member.Name + " - " + x.Name,
                x.FakeDate,
                x.ReceiveMethod,
                ObjectId = x.VendorId == null ? x.MemberId : x.VendorId,
                x.Description,
                x.VendorId,
                x.MemberId,
                AccountId = (x.Vendor == null) ? x.Member.AccountId : x.Vendor.AccountId,
                x.Status
            }).ToList();

            return Ok(Receives);
        }
        [Route("Receive/GetReceivesByVendorId")]
        [HttpGet]
        public IActionResult GetReceivesByVendorId(long? VendorId)
        {
            var Receives = DB.Receives.Where(i => i.VendorId == VendorId).Select(x => new {
                x.Id,
                x.TotalAmmount,
                x.Type,
                x.EditorName,
                Name = x.Vendor.Name + " " + x.Member.Name + " - " + x.Name,
                x.FakeDate,
                x.ReceiveMethod,
                ObjectId = x.VendorId == null ? x.MemberId : x.VendorId,
                x.Description,
                x.VendorId,
                x.MemberId,
                AccountId = (x.Vendor == null) ? x.Member.AccountId : x.Vendor.AccountId,
                x.Status
            }).ToList();

            return Ok(Receives);
        }
        [Route("Receive/GetById")]
        [HttpGet]
        public IActionResult GetById(long? Id)
        {
            var Receive = DB.Receives.Where(i => i.Id == Id).Select(x => new {
                x.Id,
                x.TotalAmmount,
                x.Type,
                x.EditorName,
                Name = x.Vendor.Name + " " + x.Member.Name + " - " + x.Name,
                x.FakeDate,
                x.ReceiveMethod,
                ObjectId = x.VendorId == null ? x.MemberId : x.VendorId,
                x.Description,
                x.MemberId,
                x.VendorId,
                AccountId = (x.Vendor == null) ? x.Member.AccountId : x.Vendor.AccountId,
                x.Status
            }).SingleOrDefault();

            return Ok(Receive);
        }
        [Route("Receive/GetReceiveByStatus")]
        [HttpGet]
        public IActionResult GetReceiveByStatus(int Limit, string Sort, int Page, int? Status)
        {
            var Receives = DB.Receives.Where(i => i.Status == Status).Select(x => new
            {
                x.Id,
                x.TotalAmmount,
                x.Type,
                x.EditorName,
                Name = x.Vendor.Name + " " + x.Member.Name + " - " + x.Name,
                x.FakeDate,
                x.ReceiveMethod,
                x.Status,
                x.Description,
                ObjectId = x.VendorId == null ? x.MemberId : x.VendorId,
                x.VendorId,
                x.MemberId,
                AccountId = (x.Vendor == null) ? x.Member.AccountId : x.Vendor.AccountId,
            }).ToList();
            Receives = (Sort == "+id" ? Receives.OrderBy(s => s.Id).ToList() : Receives.OrderByDescending(s => s.Id).ToList());
            return Ok(new
            {
                items = Receives.Skip((Page - 1) * Limit).Take(Limit).ToList(),
                Totals = new
                {
                    Rows = Receives.Count(),
                    Totals = Receives.Sum(s => s.TotalAmmount),
                    Cash = Receives.Where(i => i.ReceiveMethod == "Cash").Sum(s => s.TotalAmmount),
                    Cheque = Receives.Where(i => i.ReceiveMethod == "Cheque").Sum(s => s.TotalAmmount),
                    Visa = Receives.Where(i => i.ReceiveMethod == "Visa").Sum(s => s.TotalAmmount)
                }
            });


        }
        [Route("Receive/GetReceiveByListId")]
        [HttpGet]
        public IActionResult GetReceiveByListId(string listid)
        {
            List<long> list = listid.Split(',').Select(long.Parse).ToList();
            var Receives = DB.Receives.Where(s => list.Contains(s.Id)).Select(x => new
            {
                x.Id,
                x.TotalAmmount,
                x.Type,
                x.EditorName,
                Name = x.Vendor.Name + " " + x.Member.Name + " - " + x.Name,
                x.FakeDate,
                x.ReceiveMethod,
                x.Status,
                x.Description,
                x.VendorId,
                x.MemberId,
                ObjectId = x.VendorId == null ? x.MemberId : x.VendorId,
                AccountId = (x.Vendor == null) ? x.Member.AccountId : x.Vendor.AccountId,
            }).ToList();
            return Ok(new
            {
                items = Receives.ToList(),
                Totals = new
                {
                    Rows = Receives.Count(),
                    Totals = Receives.Sum(s => s.TotalAmmount),
                    Cash = Receives.Where(i => i.ReceiveMethod == "Cash").Sum(s => s.TotalAmmount),
                    Cheque = Receives.Where(i => i.ReceiveMethod == "Cheque").Sum(s => s.TotalAmmount),
                    Visa = Receives.Where(i => i.ReceiveMethod == "Visa").Sum(s => s.TotalAmmount)
                }
            });
        }
        [HttpPost]
        [Route("Receive/Create")]
        public IActionResult Create(Receive collection)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // TODO: Add insert logic here
                    DB.Receives.Add(collection);
                    DB.SaveChanges();
                    return Ok(collection.Id);

                }
                catch
                {
                    //Console.WriteLine(collection);
                    return Ok(false);
                }
            }
            else return Ok(false);
        }
        [Route("Receive/Edit")]
        [HttpPost]
        public IActionResult Edit(Receive collection)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Receive Receive = DB.Receives.Where(x => x.Id == collection.Id).SingleOrDefault();
                    Receive.Name = collection.Name;
                    Receive.FakeDate = collection.FakeDate;
                    Receive.ReceiveMethod = collection.ReceiveMethod;
                    Receive.TotalAmmount = collection.TotalAmmount;
                    Receive.Description = collection.Description;
                    Receive.VendorId = collection.VendorId;
                    Receive.IsPrime = collection.IsPrime;
                    Receive.MemberId = collection.MemberId;
                    Receive.Type = collection.Type;
                    Receive.EditorName = collection.EditorName;

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
        [HttpPost]
        [Route("Receive/EditReceiveMethod")]
        public IActionResult EditReceiveMethod(long ID, string ReceiveMethod)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Receive Receive = DB.Receives.Where(x => x.Id == ID).SingleOrDefault();
                    Receive.ReceiveMethod = ReceiveMethod;
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


    }
}
