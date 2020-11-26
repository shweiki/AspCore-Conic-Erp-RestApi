using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Entities;

namespace AspCore_Conic_Erp_RestApi.Controllers
{
    [Authorize]

    public class MembershipMovementOrderController : Controller
    {
        private ConicErpContext DB = new ConicErpContext();
        [Route("MembershipMovementOrder/GetMembershipMovementOrderByMemberShipID")]
        [HttpGet]
        public IActionResult GetMembershipMovementOrderByMemberShipID(long? MemberShipID)
        {
            var MembershipMovementOrder = (from x in DB.MembershipMovementOrders.ToList()
                            where (x.MemberShipMovementId == MemberShipID)
                            let p = new
                            {
                                x.Id,
                                x.EditorName,
                                x.Type,
                                x.StartDate,
                                x.EndDate,
                                x.Status,
                                x.MemberShipMovementId,
                                x.Description
                            }
                            select p);

            return Ok(MembershipMovementOrder);
        }

        [Route("MembershipMovementOrder/GetMembershipMovementOrderByStatus")]
        [HttpGet]
        public IActionResult GetMembershipMovementOrderByStatus(int? Status)
        {
            var MembershipMovementOrders = (from x in DB.MembershipMovementOrders.ToList()
                            where (x.Status == Status)
                            let p = new
                            {
                                x.Id,
                                x.Type,
                                x.StartDate,
                                x.EndDate,
                                x.Status,
                                x.EditorName,
                                x.MemberShipMovementId,
                                x.Description,
                                MembershipMovementType =DB.MembershipMovements.Where(m=>m.Id == x.MemberShipMovementId).SingleOrDefault().Type,
                                MemberId = DB.MembershipMovements.Where(m=>m.Id == x.MemberShipMovementId).SingleOrDefault().MemberId,
                                Name = DB.Members.Where(m=>m.Id == DB.MembershipMovements.Where(m => m.Id == x.MemberShipMovementId).SingleOrDefault().MemberId).SingleOrDefault().Name,
                          
                            }
                            select p);

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
                    Oprationsy Opx = DB.Oprationsys.Where(d => d.Status == collection.Status && d.TableName == "MembershipMovementOrder").SingleOrDefault();
                    OprationsysController Op = new OprationsysController();
                    if (Op.ChangeStatus(collection.Id, Opx.Id, "<!" + collection.Id + "!>"))
                    {
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
            return Ok(false);
        }

   


    }
}