using Domain.Entities; using Application.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestApi.Controllers;

[Authorize]

public class MembershipMovementOrderController : Controller
{
    private readonly IApplicationDbContext DB;
    public MembershipMovementOrderController(IApplicationDbContext dbcontext)
    {
        DB = dbcontext;
    }
    [Route("MembershipMovementOrder/GetById")]
    [HttpGet]
    public IActionResult GetById(long? Id)
    {
        var MembershipMovementOrder = DB.MembershipMovementOrder.Where(i => i.Id == Id).Select(x => new
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
        var MembershipMovementOrder = DB.MembershipMovementOrder.Where(i => i.MemberShipMovementId == MemberShipId).Select(x => new
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
        var MembershipMovementOrders = DB.MembershipMovementOrder.Where(i => i.Status == Status).Select(x => new
        {
            x.Id,
            x.Type,
            x.StartDate,
            x.EndDate,
            x.Status,
            x.EditorName,
            x.MemberShipMovementId,
            x.Description,
            MembershipMovementType = DB.MembershipMovement.Where(m => m.Id == x.MemberShipMovementId).SingleOrDefault().Type,
            MemberId = DB.MembershipMovement.Where(m => m.Id == x.MemberShipMovementId).SingleOrDefault().MemberId,
            Name = DB.Member.Where(m => m.Id == DB.MembershipMovement.Where(m => m.Id == x.MemberShipMovementId).SingleOrDefault().MemberId).SingleOrDefault().Name,
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

                DB.MembershipMovementOrder.Add(collection);
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
                MembershipMovementOrder membershipmovementorder = DB.MembershipMovementOrder.Where(x => x.Id == collection.Id).SingleOrDefault();
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
            MembershipMovementOrder membershipmovementorder = await DB.MembershipMovementOrder.FindAsync(Id);
            DB.MembershipMovementOrder.Remove(membershipmovementorder);

            await DB.SaveChangesAsync();
            return Ok(true);
        }
        catch (Exception ex)
        {
            return Ok(ex.Message);
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

                DB.MembershipMovementOrder.AddRange(collection);
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