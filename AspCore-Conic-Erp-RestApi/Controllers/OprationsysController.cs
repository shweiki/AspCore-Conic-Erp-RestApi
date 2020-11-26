using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Entities;

namespace AspCore_Conic_Erp_RestApi.Controllers
{
    [Authorize]

    public class OprationsysController : Controller
    {
        private ConicErpContext DB = new ConicErpContext();

        [Route("Oprationsys/GetOpration")]
        [HttpGet]
        public IActionResult GetOpration()
        {
            var Oprations = from x in DB.Oprationsys.ToList()
                            select new
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
                            };
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
                x.ClassName
            }).ToList();
                           
            return Ok(Oprations);

        }
        [Route("Oprationsys/ChangeObjStatusByTableName")]
        [HttpPost]
        public IActionResult ChangeObjStatusByTableName(long ObjID, string TableName, int Status, string Description)
        {
            Oprationsy Op = DB.Oprationsys.Where(x => x.TableName == TableName && x.Status == Status).SingleOrDefault();
            if (ChangeStatus(ObjID, Op.Id, Description))
            {
                return Ok();
            }
            else return NotFound();
        }
        [Route("Oprationsys/ChangeObjStatus")]
        [HttpPost]
        public IActionResult ChangeObjStatus(long ObjID, long OprationID, string Description)
        {
            if (ChangeStatus(ObjID, OprationID, Description)){
                return Ok();
            } else return NotFound();
        }

        [Route("Oprationsys/ChangeArrObjStatus")]
        [HttpPost]
        public IActionResult ChangeArrObjStatus(List<long> ObjsID, long? OprationID, string Description)
        {
            foreach (long O in ObjsID)
            {
                this.ChangeStatus(O, OprationID, Description);
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
        public Boolean ChangeStatus(long? ObjID, long? OprationID, string Description)
        {
            Oprationsy Oprationsys = DB.Oprationsys.Where(x => x.Id == OprationID).SingleOrDefault();
            ActionLog log = new ActionLog();
            log.PostingDateTime = DateTime.Now;
            log.OprationId = Oprationsys.Id;
            log.UserId = User.Identity.Name;
            log.Description = Description;

            switch (Oprationsys.TableName)
            {
                case "InventoryItem":
                    log.InventoryItemId = (int)ObjID;
                    DB.InventoryItems.Where(x => x.Id == log.InventoryItemId).SingleOrDefault().Status = Oprationsys.Status;
                    break;
                case "OrderInventory":
                    log.OrderInventoryId = ObjID;
                    DB.OrderInventories.Where(x => x.Id == log.OrderInventoryId).SingleOrDefault().Status = Oprationsys.Status;
                    break;
                case "StocktakingInventory":
                    log.StocktakingInventoryId = ObjID;
                    DB.StocktakingInventories.Where(x => x.Id == log.StocktakingInventoryId).SingleOrDefault().Status = Oprationsys.Status;
                    break;
                case "UnitItem":
                    log.UnitId = (int)ObjID;
                    DB.UnitItems.Where(x => x.Id == log.UnitId).SingleOrDefault().Status = Oprationsys.Status;
                    break;
                case "MenuItem":
                    log.MenuId = (int)ObjID;
                    DB.MenuItems.Where(x => x.Id == log.MenuId).SingleOrDefault().Status = Oprationsys.Status;
                    break;
                case "OriginItem":
                    log.OriginId = (int)ObjID;
                    DB.OriginItems.Where(x => x.Id == log.OriginId).SingleOrDefault().Status = Oprationsys.Status;
                    break;
                case "Item":
                    log.ItemsId = ObjID;
                    DB.Items.Where(x => x.Id == log.ItemsId).SingleOrDefault().Status = Oprationsys.Status;
                    break;
                case "Vendor":
                    log.VendorId = ObjID;
                    DB.Vendors.Where(x => x.Id == log.VendorId).SingleOrDefault().Status = Oprationsys.Status;
                    break;
                case "InventoryMovement":
                    log.InventoryMovementId = ObjID;
                    DB.InventoryMovements.Where(x => x.Id == log.InventoryMovementId).SingleOrDefault().Status = Oprationsys.Status;
                    break;
                case "StockMovement":
                    log.StockMovementId = ObjID;
                    DB.StockMovements.Where(x => x.Id == log.StockMovementId).SingleOrDefault().Status = Oprationsys.Status;
                    break;
                case "SalesInvoice":
                    log.SalesInvoiceId = ObjID;
                    DB.SalesInvoices.Where(x => x.Id == log.SalesInvoiceId).SingleOrDefault().Status = Oprationsys.Status;
                    break;
                case "PurchaseInvoice":
                    log.PurchaseInvoiceId = ObjID;
                    DB.PurchaseInvoices.Where(x => x.Id == log.PurchaseInvoiceId).SingleOrDefault().Status = Oprationsys.Status;
                    break;
                case "Account":
                    log.AccountId = ObjID;
                    DB.Accounts.Where(x => x.Id == log.AccountId).SingleOrDefault().Status = Oprationsys.Status;
                    break;
                case "EntryAccounting":
                    log.EntryId = ObjID;
                    DB.EntryAccountings.Where(x => x.Id == log.EntryId).SingleOrDefault().Status = Oprationsys.Status;
                    break;
                case "Payment":
                    log.PaymentId = ObjID;
                    DB.Payments.Where(x => x.Id == log.PaymentId).SingleOrDefault().Status = Oprationsys.Status;
                    break;
                case "Member":
                    log.MemberId = ObjID;
                    DB.Members.Where(x => x.Id == log.MemberId).SingleOrDefault().Status = Oprationsys.Status;
                    break;
                case "Membership":
                    log.MembershipId = (int)ObjID;
                    DB.Memberships.Where(x => x.Id == log.MembershipId).SingleOrDefault().Status = Oprationsys.Status;
                    break; 
                case "MembershipMovement":
                    log.MembershipMovementId = ObjID;
                    DB.MembershipMovements.Where(x => x.Id == log.MembershipMovementId).SingleOrDefault().Status = Oprationsys.Status;
                    break;
                case "MembershipMovementOrder":
                    log.MembershipMovementOrderId = (int)ObjID;
                    DB.MembershipMovementOrders.Where(x => x.Id == log.MembershipMovementOrderId).SingleOrDefault().Status = Oprationsys.Status;
                    break;
                case "Bank":
                    log.BankId = (int)ObjID;
                    DB.Banks.Where(x => x.Id == log.BankId).SingleOrDefault().Status = Oprationsys.Status;
                    break;
                case "Cheque":
                    log.ChequeId = (int)ObjID;
                    DB.Cheques.Where(x => x.Id == log.ChequeId).SingleOrDefault().Status = Oprationsys.Status;
                    break;
                case "Cash":
                    log.CashId = (int)ObjID;
                    DB.Cashes.Where(x => x.Id == log.CashId).SingleOrDefault().Status = Oprationsys.Status;
                    break;
                case "Discount":
                    log.DiscountId = (int)ObjID;
                    DB.Discounts.Where(x => x.Id == log.DiscountId).SingleOrDefault().Status = Oprationsys.Status;
                    break;
                case "Service":
                    log.ServiceId = (int)ObjID;
                    DB.Services.Where(x => x.Id == log.ServiceId).SingleOrDefault().Status = Oprationsys.Status;
                    break;
            }
            ActionLogController logCon = new ActionLogController();
            if (logCon.Create(log))
            {
                DB.SaveChanges();
                return true;
            }
            return false;

        }

    }
}
