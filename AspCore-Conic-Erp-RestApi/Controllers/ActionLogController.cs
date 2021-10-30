using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Entities;
using Microsoft.AspNetCore.Authorization;

namespace AspCore_Conic_Erp_RestApi.Controllers
{
    [Authorize]
    public class ActionLogController : Controller
    {
        private ConicErpContext DB;
        public ActionLogController(ConicErpContext dbcontext)
        {
            DB = dbcontext;
        }        public Boolean Create(ActionLog LogOpratio)
        {
            LogOpratio.PostingDateTime = DateTime.Now;
            try
            {
                // TODO: Add insert logic here
                DB.ActionLogs.Add(LogOpratio);
                DB.SaveChanges();
                //  Console.WriteLine(collection);
                //return Json(collection);
                return (true);
            }
            catch
            {
                //Console.WriteLine(collection);
                return (false);
            }
        }
        [Route("ActionLog/GetLogByObjTable")]
        [HttpGet]
        public IActionResult GetLogByObjTable(string TableName, int Id)
        {
            List<ActionLog> ActionLogs = new List<ActionLog>() ; 
            switch (TableName)
            {
                case "InventoryItem":
                      ActionLogs = DB.ActionLogs.Where(l => l.InventoryItemId == Id).ToList();
                    break;
                case "OrderInventory":
                     ActionLogs = DB.ActionLogs.Where(l => l.OrderInventoryId == Id).ToList();
                    break;
                case "StocktakingInventory":
                    ActionLogs = DB.ActionLogs.Where(l => l.StocktakingInventoryId == Id).ToList();
                    break;
                case "UnitItem":
                    ActionLogs = DB.ActionLogs.Where(l => l.UnitId == Id).ToList();
                    break;
                case "MenuItem":
                    ActionLogs = DB.ActionLogs.Where(l => l.MenuId == Id).ToList();
                    break;
                case "OriginItem":
                    ActionLogs = DB.ActionLogs.Where(l => l.OriginId == Id).ToList();
                    break;
                case "Item":
                    ActionLogs = DB.ActionLogs.Where(l => l.ItemsId == Id).ToList();
                    break;
                case "Vendor":
                    ActionLogs = DB.ActionLogs.Where(l => l.VendorId == Id).ToList();
                    break;
                case "InventoryMovement":
                    ActionLogs = DB.ActionLogs.Where(l => l.InventoryMovementId == Id).ToList();
                    break;
                case "StockMovement":
                    ActionLogs = DB.ActionLogs.Where(l => l.StockMovementId == Id).ToList();
                    break;
                case "SalesInvoice":
                    ActionLogs = DB.ActionLogs.Where(l => l.SalesInvoiceId == Id).ToList();
                    break;
                case "PurchaseInvoice":
                    ActionLogs = DB.ActionLogs.Where(l => l.PurchaseInvoiceId == Id).ToList();
                    break;
                case "Account":
                    ActionLogs = DB.ActionLogs.Where(l => l.AccountId == Id).ToList();
                    break;
                case "EntryAccounting":
                    ActionLogs = DB.ActionLogs.Where(l => l.EntryId == Id).ToList();
                    break;
                case "Payment":
                    ActionLogs = DB.ActionLogs.Where(l => l.PaymentId == Id).ToList();
                    break;
                case "Member":
                    ActionLogs = DB.ActionLogs.Where(l => l.MemberId == Id).ToList();
                    break;
                case "Membership":
                    ActionLogs = DB.ActionLogs.Where(l => l.MembershipId == Id).ToList();
                    break;
                case "MembershipMovement":
                    ActionLogs = DB.ActionLogs.Where(l => l.MembershipMovementId == Id).ToList();
                    break;
                case "MembershipMovementOrder":
                    ActionLogs = DB.ActionLogs.Where(l => l.MembershipMovementOrderId == Id).ToList();
                    break;
                case "Bank":
                    ActionLogs = DB.ActionLogs.Where(l => l.BankId == Id).ToList();
                    break;
                case "Cheque":
                    ActionLogs = DB.ActionLogs.Where(l => l.ChequeId == Id).ToList();
                    break;
                case "Cash":
                    ActionLogs = DB.ActionLogs.Where(l => l.CashId == Id).ToList();
                    break;
                case "Discount":
                    ActionLogs = DB.ActionLogs.Where(l => l.DiscountId == Id).ToList();
                    break;
                case "Service":
                    ActionLogs = DB.ActionLogs.Where(l => l.ServiceId == Id).ToList();
                    break;
                case "OrderDelivery":
                    ActionLogs = DB.ActionLogs.Where(l => l.OrderDeliveryId == Id).ToList();
                    break;
                case "Area":
                    ActionLogs = DB.ActionLogs.Where(l => l.AreaId == Id).ToList();
                    break;
                case "Driver":
                    ActionLogs = DB.ActionLogs.Where(l => l.DriverId == Id).ToList();
                    break;
            };
       
            return Ok(ActionLogs);


        }


    }
}
