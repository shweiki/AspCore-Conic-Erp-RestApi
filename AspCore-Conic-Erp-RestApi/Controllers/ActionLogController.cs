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
        private ConicErpContext DB = new ConicErpContext();
        public Boolean Create(ActionLog LogOpratio)
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
        public IActionResult GetLogByObjTable(string TableName, int ID)
        {
            List<ActionLog> ActionLogs = DB.ActionLogs.ToList(); 
            switch (TableName)
            {
                case "InventoryItem":
                      ActionLogs = DB.ActionLogs.Where(l => l.InventoryItemId == ID).ToList();
                    break;
                case "OrderInventory":
                     ActionLogs = DB.ActionLogs.Where(l => l.OrderInventoryId == ID).ToList();
                    break;
                case "StocktakingInventory":
                    ActionLogs = DB.ActionLogs.Where(l => l.StocktakingInventoryId == ID).ToList();
                    break;
                case "UnitItem":
                    ActionLogs = DB.ActionLogs.Where(l => l.UnitId == ID).ToList();
                    break;
                case "MenuItem":
                    ActionLogs = DB.ActionLogs.Where(l => l.MenuId == ID).ToList();
                    break;
                case "OriginItem":
                    ActionLogs = DB.ActionLogs.Where(l => l.OriginId == ID).ToList();
                    break;
                case "Item":
                    ActionLogs = DB.ActionLogs.Where(l => l.ItemsId == ID).ToList();
                    break;
                case "Vendor":
                    ActionLogs = DB.ActionLogs.Where(l => l.VendorId == ID).ToList();
                    break;
                case "InventoryMovement":
                    ActionLogs = DB.ActionLogs.Where(l => l.InventoryMovementId == ID).ToList();
                    break;
                case "StockMovement":
                    ActionLogs = DB.ActionLogs.Where(l => l.StockMovementId == ID).ToList();
                    break;
                case "SalesInvoice":
                    ActionLogs = DB.ActionLogs.Where(l => l.SalesInvoiceId == ID).ToList();
                    break;
                case "PurchaseInvoice":
                    ActionLogs = DB.ActionLogs.Where(l => l.PurchaseInvoiceId == ID).ToList();
                    break;
                case "Account":
                    ActionLogs = DB.ActionLogs.Where(l => l.AccountId == ID).ToList();
                    break;
                case "EntryAccounting":
                    ActionLogs = DB.ActionLogs.Where(l => l.EntryId == ID).ToList();
                    break;
                case "Payment":
                    ActionLogs = DB.ActionLogs.Where(l => l.PaymentId == ID).ToList();
                    break;
                case "Member":
                    ActionLogs = DB.ActionLogs.Where(l => l.MemberId == ID).ToList();
                    break;
                case "Membership":
                    ActionLogs = DB.ActionLogs.Where(l => l.MembershipId == ID).ToList();
                    break;
                case "MembershipMovement":
                    ActionLogs = DB.ActionLogs.Where(l => l.MembershipMovementId == ID).ToList();
                    break;
                case "MembershipMovementOrder":
                    ActionLogs = DB.ActionLogs.Where(l => l.MembershipMovementOrderId == ID).ToList();
                    break;
                case "Bank":
                    ActionLogs = DB.ActionLogs.Where(l => l.BankId == ID).ToList();
                    break;
                case "Cheque":
                    ActionLogs = DB.ActionLogs.Where(l => l.ChequeId == ID).ToList();
                    break;
                case "Cash":
                    ActionLogs = DB.ActionLogs.Where(l => l.CashId == ID).ToList();
                    break;
                case "Discount":
                    ActionLogs = DB.ActionLogs.Where(l => l.DiscountId == ID).ToList();
                    break;
                case "Service":
                    ActionLogs = DB.ActionLogs.Where(l => l.ServiceId == ID).ToList();
                    break;
            };
       
            return Ok(ActionLogs);


        }


    }
}
