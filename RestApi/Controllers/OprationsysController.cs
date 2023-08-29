using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace RestApi.Controllers;


[Authorize]

public class OprationsysController : Controller
{

    private readonly ILogger<OprationsysController> _logger;
    private readonly IApplicationDbContext DB;

    public OprationsysController(IApplicationDbContext dbcontext, ILogger<OprationsysController> logger)
    {
        _logger = logger;
        DB = dbcontext;
    }
    [Route("Oprationsys/GetOpration")]
    [HttpGet]
    public IActionResult GetOpration()
    {

        var Oprations = DB.Oprationsy.Select(x => new
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
        var Oprations = DB.Oprationsy.Where(f => f.TableName == Name).Select(x => new
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
    public IActionResult GetOprationByStatusTable(string TableName, int Status)
    {
        var Oprations = DB.Oprationsy.Where(i => i.Status == Status && i.TableName == TableName).Select(x => new
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
        var NextOprations = DB.Oprationsy.Where(i => i.ReferenceStatus == Status && i.TableName == TableName).Select(x => new
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
        Oprationsy Op = DB.Oprationsy.Where(x => x.TableName == TableName && x.Status == Status).SingleOrDefault();
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
        Oprationsy Op = DB.Oprationsy.Where(x => x.Id == OprationId).SingleOrDefault();

        if (ChangeStatus(ObjId, Op, Description))
        {
            return Ok();
        }
        else return NotFound(true);
    }

    [Route("Oprationsys/ChangeArrObjStatus")]
    [HttpPost]
    public IActionResult ChangeArrObjStatus(List<int> ObjsId, string TableName, int Status, string Description)
    {
        Oprationsy Op = DB.Oprationsy.Where(x => x.TableName == TableName && x.Status == Status).SingleOrDefault();

        foreach (long O in ObjsId)
        {
            ChangeStatus(O, Op, Description);
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
                DB.Oprationsy.Add(collection);
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
            Oprationsy Opration = DB.Oprationsy.Where(x => x.Id == collection.Id).SingleOrDefault();
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
                DB.InventoryItem.Where(x => x.Id == ObjId).SingleOrDefault().Status = Oprationsys.Status;
                break;
            case "OrderInventory":
                DB.OrderInventory.Where(x => x.Id == ObjId).SingleOrDefault().Status = Oprationsys.Status;
                DB.InventoryMovement.Where(x => x.OrderInventoryId == ObjId).ToList().ForEach(b => b.Status = Oprationsys.Status);
                break;
            case "StocktakingInventory":
                DB.StocktakingInventory.Where(x => x.Id == ObjId).SingleOrDefault().Status = Oprationsys.Status;
                break;
            case "UnitItem":
                DB.UnitItem.Where(x => x.Id == ObjId).SingleOrDefault().Status = Oprationsys.Status;
                break;
            case "MenuItem":
                DB.MenuItem.Where(x => x.Id == ObjId).SingleOrDefault().Status = Oprationsys.Status;
                break;
            case "OriginItem":
                DB.OriginItem.Where(x => x.Id == ObjId).SingleOrDefault().Status = Oprationsys.Status;
                break;
            case "Item":
                DB.Item.Where(x => x.Id == ObjId).SingleOrDefault().Status = Oprationsys.Status;
                break;
            case "Vendor":
                DB.Vendor.Where(x => x.Id == ObjId).SingleOrDefault().Status = Oprationsys.Status;
                break;
            case "InventoryMovement":
                DB.InventoryMovement.Where(x => x.Id == ObjId).SingleOrDefault().Status = Oprationsys.Status;
                break;
            case "StockMovement":
                DB.StockMovement.Where(x => x.Id == ObjId).SingleOrDefault().Status = Oprationsys.Status;
                break;
            case "SalesInvoice":
                DB.SalesInvoice.Where(x => x.Id == ObjId).SingleOrDefault().Status = Oprationsys.Status;
                DB.InventoryMovement.Where(x => x.SalesInvoiceId == ObjId).ToList().ForEach(b => b.Status = Oprationsys.Status);
                break;
            case "PurchaseInvoice":
                DB.PurchaseInvoice.Where(x => x.Id == ObjId).SingleOrDefault().Status = Oprationsys.Status;
                DB.InventoryMovement.Where(x => x.PurchaseInvoiceId == ObjId).ToList().ForEach(b => b.Status = Oprationsys.Status);
                break;
            case "Account":
                DB.TreeAccount.Where(x => x.Id == ObjId).SingleOrDefault().Status = Oprationsys.Status;
                break;
            case "EntryAccounting":
                DB.EntryAccounting.Where(x => x.Id == ObjId).SingleOrDefault().Status = Oprationsys.Status;
                break;
            case "Payment":
                DB.Payment.Where(x => x.Id == ObjId).SingleOrDefault().Status = Oprationsys.Status;
                break;
            case "Member":
                DB.Member.Where(x => x.Id == ObjId).SingleOrDefault().Status = Oprationsys.Status;
                break;
            case "Membership":
                DB.Membership.Where(x => x.Id == ObjId).SingleOrDefault().Status = Oprationsys.Status;
                break;
            case "MembershipMovement":
                DB.MembershipMovement.Where(x => x.Id == ObjId).SingleOrDefault().Status = Oprationsys.Status;
                break;
            case "MembershipMovementOrder":
                DB.MembershipMovementOrder.Where(x => x.Id == ObjId).SingleOrDefault().Status = Oprationsys.Status;
                break;
            case "Bank":
                DB.Bank.Where(x => x.Id == ObjId).SingleOrDefault().Status = Oprationsys.Status;
                break;
            case "Cheque":
                DB.Cheque.Where(x => x.Id == ObjId).SingleOrDefault().Status = Oprationsys.Status;
                break;
            case "Cash":
                DB.Cash.Where(x => x.Id == ObjId).SingleOrDefault().Status = Oprationsys.Status;
                break;
            case "Discount":
                DB.Discount.Where(x => x.Id == ObjId).SingleOrDefault().Status = Oprationsys.Status;
                break;
            case "Service":
                DB.Service.Where(x => x.Id == ObjId).SingleOrDefault().Status = Oprationsys.Status;
                break;
            case "Area":
                DB.Area.Where(x => x.Id == ObjId).SingleOrDefault().Status = Oprationsys.Status;
                break;
            case "Driver":
                DB.Driver.Where(x => x.Id == ObjId).SingleOrDefault().Status = Oprationsys.Status;
                break;
            case "OrderDelivery":
                DB.OrderDelivery.Where(x => x.Id == ObjId).SingleOrDefault().Status = Oprationsys.Status;
                break;
            case "Visit":
                DB.Visit.Where(x => x.Id == ObjId).SingleOrDefault().Status = Oprationsys.Status;
                break;
            case "OrderRestaurant":
                DB.OrderRestaurant.Where(x => x.Id == ObjId).SingleOrDefault().Status = Oprationsys.Status;
                break;
            case "Employee":
                DB.Employee.Where(x => x.Id == ObjId).SingleOrDefault().Status = Oprationsys.Status;
                break;
            default:
                log.Fktable = ObjId.ToString();
                break;

        }
        log.Fktable = ObjId.ToString();
        DB.ActionLog.Add(log);
        DB.SaveChanges();
        return true;

        //   return false;

    }


}
