using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Entities;
using System.Security.Claims;
using Microsoft.Extensions.Logging;

namespace AspCore_Conic_Erp_RestApi.Controllers
{

    [Authorize]

    public class OprationsysController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<UserController> _logger;
                private ConicErpContext DB;

        public OprationsysController(ConicErpContext dbcontext,UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, ILogger<UserController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
                        DB = dbcontext;


        }
        [Route("Oprationsys/GetOpration")]
        [HttpGet]
        public IActionResult GetOpration()
        {

            var Oprations = DB.Oprationsys.Select(x => new
            {
                x.Id,
                x.OprationName,
                x.TableName,
                x.ControllerName,
                x.RoleName,
                x.OprationDescription,
                x.ArabicOprationDescription,
                x.Status,
                x.ReferenceStatus,
                x.IconClass,
                x.ClassName,
                x.Color,
            }).ToList();
            return Ok(Oprations);

        }
        [Route("Oprationsys/GetOprationByTable")]
        [HttpGet]
        public IActionResult GetOprationByTable(string Name)
        {
            var Oprations = DB.Oprationsys.Where(f => f.TableName == Name).Select(x => new
            {
                x.Id,
                x.OprationName,
                x.TableName,
                x.ControllerName,
                x.RoleName,
                x.OprationDescription,
                x.ArabicOprationDescription,
                x.Status,
                x.ReferenceStatus,
                x.IconClass,
                x.ClassName,
                x.Color,
            }).ToList();
                           
            return Ok(Oprations);

        }
        [Route("Oprationsys/GetOprationByStatusTable")]
        [HttpGet]
        public IActionResult GetOprationByStatusTable(string TableName , int Status)
        {
            var Oprations = DB.Oprationsys.Where(i => i.Status == Status &&i.TableName == TableName ).Select(x => new
            {
                x.Id,
                x.OprationName,
                x.TableName,
                x.ControllerName,
                x.RoleName,
                x.OprationDescription,
                x.ArabicOprationDescription,
                x.Status,
                x.ReferenceStatus,
                x.IconClass,
                x.ClassName,
                x.Color,
            }).SingleOrDefault();

            return Ok(Oprations);

        }
        [Route("Oprationsys/GetNextOprationByStatusTable")]
        [HttpGet]
        public IActionResult GetNextOprationByStatusTable(string TableName, int Status)
        {
            var NextOprations = DB.Oprationsys.Where(i => i.ReferenceStatus == Status && i.TableName == TableName).Select(x => new
            {
                x.Id,
                x.OprationName,
                x.TableName,
                x.ControllerName,
                x.RoleName,
                x.OprationDescription,
                x.ArabicOprationDescription,
                x.Status,
                x.ReferenceStatus,
                x.IconClass,
                x.ClassName,
                x.Color,
            }).ToList();

            return Ok(NextOprations);

        }
        [Route("Oprationsys/ChangeObjStatusByTableName")]
        [HttpPost]
        public IActionResult ChangeObjStatusByTableName(long ObjId, string TableName, int Status, string Description)
        {
            Oprationsy Op = DB.Oprationsys.Where(x => x.TableName == TableName && x.Status == Status).SingleOrDefault();
            if (ChangeStatus(ObjId, Op, Description))
            {
                return Ok(true);
            }
            else return NotFound();
        }
        [Route("Oprationsys/ChangeObjStatus")]
        [HttpPost]
        public IActionResult ChangeObjStatus(long ObjId, long OprationId, string Description)
        {
            Oprationsy Op = DB.Oprationsys.Where(x => x.Id == OprationId).SingleOrDefault();

            if (ChangeStatus(ObjId, Op, Description)){
                return Ok();
            } else return NotFound(true);
        }

        [Route("Oprationsys/ChangeArrObjStatus")]
        [HttpPost]
        public IActionResult ChangeArrObjStatus(List<int> ObjsId,  string TableName, int Status, string Description)
        {
            Oprationsy Op = DB.Oprationsys.Where(x => x.TableName == TableName && x.Status == Status).SingleOrDefault();

            foreach (long O in ObjsId)
            {
                ChangeStatus(O, Op , Description);
            }
            return Ok(true);
        }

        [Route("Oprationsys/Create")]
        [HttpPost]
        public IActionResult Create(Oprationsy collection)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // TODO: Add insert logic here
                    DB.Oprationsys.Add(collection);
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

        [Route("Oprationsys/Edit")]
        [HttpPost]
        public IActionResult Edit(Oprationsy collection)
        {
            if (ModelState.IsValid)
            {
                Oprationsy Opration = DB.Oprationsys.Where(x => x.Id == collection.Id).SingleOrDefault();
                Opration.OprationName = collection.OprationName;
                Opration.Status = collection.Status;
                Opration.ReferenceStatus = collection.ReferenceStatus;
                Opration.OprationDescription = collection.OprationDescription;
                Opration.ArabicOprationDescription = collection.ArabicOprationDescription;
                Opration.ControllerName = collection.ControllerName;
                Opration.ClassName = collection.ControllerName;
                Opration.IconClass = collection.IconClass;
                Opration.TableName = collection.TableName;
                Opration.RoleName = collection.RoleName;
                Opration.Color = collection.Color;
                try
                {

                    DB.SaveChanges();
                    return Ok(true);
                }
                catch
                {
                    return Ok(false);

                }
                // TODO: Add update logic here

            }
            return Ok(false);

        }

        [Route("Oprationsys/ChangeStatus")]
        [HttpPost]
        public Boolean ChangeStatus(long? ObjId, Oprationsy Oprationsys, string Description)
        {
            ActionLog log = new ActionLog();
            // var claimsIdentity = (ClaimsIdentity)HttpContext.User.Identity;
            //  var claim = claimsIdentity.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            log.PostingDateTime = DateTime.Now;
            log.OprationId = Oprationsys.Id;
            log.UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            log.Description = Description;
            log.TableName = Oprationsys.TableName;

            switch (Oprationsys.TableName)
            {
                case "InventoryItem":
                    log.InventoryItemId = (int)ObjId;
                    DB.InventoryItems.Where(x => x.Id == log.InventoryItemId).SingleOrDefault().Status = Oprationsys.Status;
                    break;
                case "OrderInventory":
                    log.OrderInventoryId = ObjId;
                    DB.OrderInventories.Where(x => x.Id == log.OrderInventoryId).SingleOrDefault().Status = Oprationsys.Status;
                    DB.InventoryMovements.Where(x => x.OrderInventoryId == ObjId).ToList().ForEach(b => b.Status = Oprationsys.Status);

                    break;
                case "StocktakingInventory":
                    log.StocktakingInventoryId = ObjId;
                    DB.StocktakingInventories.Where(x => x.Id == log.StocktakingInventoryId).SingleOrDefault().Status = Oprationsys.Status;
                    break;
                case "UnitItem":
                    log.UnitId = (int)ObjId;
                    DB.UnitItems.Where(x => x.Id == log.UnitId).SingleOrDefault().Status = Oprationsys.Status;
                    break;
                case "MenuItem":
                    log.MenuId = (int)ObjId;
                    DB.MenuItems.Where(x => x.Id == log.MenuId).SingleOrDefault().Status = Oprationsys.Status;
                    break;
                case "OriginItem":
                    log.OriginId = (int)ObjId;
                    DB.OriginItems.Where(x => x.Id == log.OriginId).SingleOrDefault().Status = Oprationsys.Status;
                    break;
                case "Item":
                    log.ItemsId = ObjId;
                    DB.Items.Where(x => x.Id == log.ItemsId).SingleOrDefault().Status = Oprationsys.Status;
                    break;
                case "Vendor":
                    log.VendorId = ObjId;
                    DB.Vendors.Where(x => x.Id == log.VendorId).SingleOrDefault().Status = Oprationsys.Status;
                    break;
                case "InventoryMovement":
                    log.InventoryMovementId = ObjId;
                    DB.InventoryMovements.Where(x => x.Id == log.InventoryMovementId).SingleOrDefault().Status = Oprationsys.Status;
                    break;
                case "StockMovement":
                    log.StockMovementId = ObjId;
                    DB.StockMovements.Where(x => x.Id == log.StockMovementId).SingleOrDefault().Status = Oprationsys.Status;
                    break;
                case "SalesInvoice":
                    log.SalesInvoiceId = ObjId;
                    DB.SalesInvoices.Where(x => x.Id == log.SalesInvoiceId).SingleOrDefault().Status = Oprationsys.Status;
                    DB.InventoryMovements.Where(x => x.SalesInvoiceId == ObjId).ToList().ForEach(b => b.Status = Oprationsys.Status);
                    break;
                case "PurchaseInvoice":
                    log.PurchaseInvoiceId = ObjId;
                    DB.PurchaseInvoices.Where(x => x.Id == log.PurchaseInvoiceId).SingleOrDefault().Status = Oprationsys.Status;
                    DB.InventoryMovements.Where(x => x.PurchaseInvoiceId == ObjId).ToList().ForEach(b => b.Status = Oprationsys.Status);
                    break;
                case "Account":
                    log.AccountId = ObjId;
                    DB.Accounts.Where(x => x.Id == log.AccountId).SingleOrDefault().Status = Oprationsys.Status;
                    break;
                case "EntryAccounting":
                    log.EntryId = ObjId;
                    DB.EntryAccountings.Where(x => x.Id == log.EntryId).SingleOrDefault().Status = Oprationsys.Status;
                    break;
                case "Payment":
                    log.PaymentId = ObjId;
                    DB.Payments.Where(x => x.Id == log.PaymentId).SingleOrDefault().Status = Oprationsys.Status;
                    break;
                case "Member":
                    log.MemberId = ObjId;
                    DB.Members.Where(x => x.Id == log.MemberId).SingleOrDefault().Status = Oprationsys.Status;
                    break;
                case "Membership":
                    log.MembershipId = (int)ObjId;
                    DB.Memberships.Where(x => x.Id == log.MembershipId).SingleOrDefault().Status = Oprationsys.Status;
                    break; 
                case "MembershipMovement":
                    log.MembershipMovementId = ObjId;
                    DB.MembershipMovements.Where(x => x.Id == log.MembershipMovementId).SingleOrDefault().Status = Oprationsys.Status;
                    break;
                case "MembershipMovementOrder":
                    log.MembershipMovementOrderId = (int)ObjId;
                    DB.MembershipMovementOrders.Where(x => x.Id == log.MembershipMovementOrderId).SingleOrDefault().Status = Oprationsys.Status;
                    break;
                case "Bank":
                    log.BankId = (int)ObjId;
                    DB.Banks.Where(x => x.Id == log.BankId).SingleOrDefault().Status = Oprationsys.Status;
                    break;
                case "Cheque":
                    log.ChequeId = (int)ObjId;
                    DB.Cheques.Where(x => x.Id == log.ChequeId).SingleOrDefault().Status = Oprationsys.Status;
                    break;
                case "Cash":
                    log.CashId = (int)ObjId;
                    DB.Cashes.Where(x => x.Id == log.CashId).SingleOrDefault().Status = Oprationsys.Status;
                    break;
                case "Discount":
                    log.DiscountId = (int)ObjId;
                    DB.Discounts.Where(x => x.Id == log.DiscountId).SingleOrDefault().Status = Oprationsys.Status;
                    break;
                case "Service":
                    log.ServiceId = (int)ObjId;
                    DB.Services.Where(x => x.Id == log.ServiceId).SingleOrDefault().Status = Oprationsys.Status;
                    break;
                case "Area":
                    log.AreaId = (int)ObjId;
                    DB.Areas.Where(x => x.Id == log.AreaId).SingleOrDefault().Status = Oprationsys.Status;
                    break;
                case "Driver":
                    log.DriverId = (int)ObjId;
                    DB.Drivers.Where(x => x.Id == log.DriverId).SingleOrDefault().Status = Oprationsys.Status;
                    break;
                case "OrderDelivery":
                    log.OrderDeliveryId = (int)ObjId;
                    DB.OrderDeliveries.Where(x => x.Id == log.OrderDeliveryId).SingleOrDefault().Status = Oprationsys.Status;
                    break;
                case "Visit":
                //    log.OrderDeliveryId = (int)ObjId;
                    DB.Visits.Where(x => x.Id == ObjId).SingleOrDefault().Status = Oprationsys.Status;
                    break;

                default:
                    log.Fktable = ObjId.ToString();                    
                    break;

            }
            log.Fktable = ObjId.ToString();
            DB.ActionLogs.Add(log);
                DB.SaveChanges();
                return true;
          
         //   return false;

        }


    }
}
