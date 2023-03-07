using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspCore_Conic_Erp_RestApi.Controllers;

[Authorize]

public class MembershipMovementOrderController : Controller
{
    private ConicErpContext DB;
    public MembershipMovementOrderController(ConicErpContext dbcontext)
    {
        DB = dbcontext;
    }
    [Route("MembershipMovementOrder/GetById")]
    [HttpGet]
    public IActionResult GetById(long? Id)
    {
        var MembershipMovementOrder = DB.MembershipMovementOrders.Where(i => i.Id == Id).Select(x => new
        {
            x.Id,
            x.EditorName,
            x.Type,
            x.StartDate,
            x.EndDate,
            x.Status,
            x.MemberShipMovementId,
            x.Description
        }).SingleOrDefault();

        return Ok(MembershipMovementOrder);
    }
    [Route("MembershipMovementOrder/GetMembershipMovementOrderByMemberShipId")]
    [HttpGet]
    public IActionResult GetMembershipMovementOrderByMemberShipId(long? MemberShipId)
    {
        var MembershipMovementOrder = DB.MembershipMovementOrders.Where(i => i.MemberShipMovementId == MemberShipId).Select(x => new
        {
            x.Id,
            x.EditorName,
            x.Type,
            x.StartDate,
            x.EndDate,
            x.Status,
            x.MemberShipMovementId,
            x.Description
        }).ToList();

        return Ok(MembershipMovementOrder);
    }

    [Route("MembershipMovementOrder/GetMembershipMovementOrderByStatus")]
    [HttpGet]
    public IActionResult GetMembershipMovementOrderByStatus(int? Status)
    {
        var MembershipMovementOrders = DB.MembershipMovementOrders.Where(i => i.Status == Status).Select(x => new
        {
            x.Id,
            x.Type,
            x.StartDate,
            x.EndDate,
            x.Status,
            x.EditorName,
            x.MemberShipMovementId,
            x.Description,
            MembershipMovementType = DB.MembershipMovements.Where(m => m.Id == x.MemberShipMovementId).SingleOrDefault().Type,
            MemberId = DB.MembershipMovements.Where(m => m.Id == x.MemberShipMovementId).SingleOrDefault().MemberId,
            Name = DB.Members.Where(m => m.Id == DB.MembershipMovements.Where(m => m.Id == x.MemberShipMovementId).SingleOrDefault().MemberId).SingleOrDefault().Name,
        }).ToList();


        return Ok(MembershipMovementOrders);
    }

    [Route("MembershipMovementOrder/Create")]
    [HttpPost]
    public IActionResult Create(MembershipMovementOrder collection)
    {
        if (ModelState.IsValid)
        {
            try
            {

                DB.MembershipMovementOrders.Add(collection);
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
    [Route("MembershipMovementOrder/Edit")]
    [HttpPost]
    public IActionResult Edit(MembershipMovementOrder collection)
    {
        if (ModelState.IsValid)
        {
            try
            {
                MembershipMovementOrder membershipmovementorder = DB.MembershipMovementOrders.Where(x => x.Id == collection.Id).SingleOrDefault();
                membershipmovementorder.Type = collection.Type;
                membershipmovementorder.StartDate = collection.StartDate;
                membershipmovementorder.EndDate = collection.EndDate;
                membershipmovementorder.Status = collection.Status;
                membershipmovementorder.Description = collection.Description;
                membershipmovementorder.EditorName = collection.EditorName;


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
    [Route("MembershipMovementOrder/Delete")]
    [HttpPost]
    public async Task<IActionResult> Delete(int Id)
    {

        try
        {
            var entryAccount = DB.EntryMovements.Where(x => x.TableName == "MembershipMovement" && x.Fktable == Id).LastOrDefault();
            if (entryAccount is not null)
            {
                DB.EntryAccountings.Remove(await DB.EntryAccountings.FindAsync(entryAccount.EntryId));
                MembershipMovementOrder membershipmovementorder = await DB.MembershipMovementOrders.FindAsync(Id);
                DB.MembershipMovementOrders.Remove(membershipmovementorder);

                await DB.SaveChangesAsync();
                return Ok(true);
            }
            else return Ok(false);

        }
        catch
        {
            //Console.WriteLine(collection);
            return Ok(false);
        }

    }
    [Route("MembershipMovementOrder/CreateMulti")]
    [HttpPost]
    public IActionResult CreateMulti(List<MembershipMovementOrder> collection)
    {
        if (ModelState.IsValid)
        {
            try
            {

                DB.MembershipMovementOrders.AddRange(collection);
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