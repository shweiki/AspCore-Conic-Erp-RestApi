using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Entities; 
using Microsoft.AspNetCore.Mvc;

namespace AspCore_Conic_Erp_RestApi.Controllers
{
    [Authorize]
    public class MembershipMovementController : Controller
    {
        private ConicErpContext DB = new ConicErpContext();

        [Route("MembershipMovement/Create")]
        [HttpPost]
        public IActionResult Create(MembershipMovement collection)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    DB.MembershipMovements.Add(collection);
                    DB.SaveChanges();
                    Oprationsy Opx = DB.Oprationsys.Where(d => d.Status == collection.Status && d.TableName == "MembershipMovement").SingleOrDefault();
                    OprationsysController Op = new OprationsysController();
                    if (Op.ChangeStatus(collection.Id, Opx.Id, "<!" + collection.Id + "!>"))
                    {
                        return Ok(collection.Id);
                    }
                    else return Ok("False Op");
                }
                catch
                {
                    //Console.WriteLine(collection);
                    return Ok("catch");
                }
            }
            return Ok("False Valid");
        }
        [Route("MembershipMovement/Edit")]
        [HttpPost]
        public IActionResult Edit(MembershipMovement collection)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var MemberShipMovement = DB.MembershipMovements.Where(x => x.Id == collection.Id).SingleOrDefault();

                    MemberShipMovement.StartDate = collection.StartDate;
                    MemberShipMovement.EndDate = collection.EndDate;
                    MemberShipMovement.Tax = collection.Tax;
                    MemberShipMovement.TotalAmmount = collection.TotalAmmount;
                    MemberShipMovement.Type = collection.Type;
                    MemberShipMovement.Discount = collection.Discount;
                    MemberShipMovement.Description = collection.Description;
                    MemberShipMovement.DiscountDescription = collection.DiscountDescription;
                    MemberShipMovement.MembershipId = collection.MembershipId;
                    MemberShipMovement.EditorName = collection.EditorName;

                    DB.SaveChanges();
                    Oprationsy Opx = DB.Oprationsys.Where(d => d.Status == MemberShipMovement.Status && d.TableName == "MembershipMovement").SingleOrDefault();
                    OprationsysController Op = new OprationsysController();
                    if (Op.ChangeStatus(collection.Id, Opx.Id, "<!" + collection.Id + "!>"))
                    {
                        return Ok(collection.Id);
                    }
                    else return Ok("False Op");
                }
                catch
                {
                    //Console.WriteLine(collection);
                    return Ok("catch");
                }
            }
            return Ok("False Valid");
        }

        [Route("MembershipMovement/CheckMembershipMovement")]
        [HttpGet]
        public IActionResult CheckMembershipMovement()
        {
           IList<MembershipMovement>  MembershipMovements = DB.MembershipMovements.Where(x=>x.Status >0 || x.Status == -2 )?.ToList();
            foreach (MembershipMovement MS in MembershipMovements)
            {
                var member = DB.Members.Where(x => x.Id == MS.MemberId).SingleOrDefault();
                if ((DateTime.Today >= MS.StartDate && DateTime.Today <= MS.EndDate))
                {
                        MS.Status = 1;
                    if (member.Status == -2) member.Status = -2;
                    else
                        member.Status = 0;

                }
                else
                {

                    if ( MS.StartDate > DateTime.Today ) {// معلق
                        MS.Status = -2;
                    }
                    else
                    {

                        MS.Status = -1;
                        if (member.Status == -2) member.Status = -2;
                        else
                            member.Status = -1;
                    }
                }
                foreach (MembershipMovementOrder MSO in MS.MembershipMovementOrders.Where(x => x.Status == 1).ToList())
                {
                    if ((DateTime.Today >= MSO.StartDate && DateTime.Today <= MSO.EndDate))
                    {
                        if (MSO.Type == "Freeze")
                        {
                            MS.Status = 2;
                            if (member.Status == -2) member.Status = -2;
                            else member.Status = 1;
                        }
                        if (MSO.Type == "Extra")
                        {
                            MS.Status = 3;
                            if (member.Status == -2) member.Status = -2;
                            else member.Status = 2;

                        }
                    }
                    else
                    {
                        if (MSO.Type == "Extra")
                        {
                            MS.EndDate = MS.EndDate.AddDays((MSO.EndDate - MSO.StartDate).TotalDays);
                            MSO.Status = -3;
                        }
                        if (DateTime.Today > MSO.EndDate)
                        {

                            MS.EndDate =  MS.EndDate.AddDays((MSO.EndDate - MSO.StartDate).TotalDays);
                            MSO.Status = -3;
                        }

                }
                if ((MS.EndDate > DateTime.Today))
                {
                   
                  
                    MS.Status = 1;
                        if (member.Status == -2) member.Status = -2;
                        else member.Status = 1;

                }
                if (MS == null )
                {
                        if (member.Status == -2) member.Status = -2;
                        else member.Status = -1;
                    }

                }


                DB.SaveChanges();

            }
            return Ok(true);
        }

        [Route("MembershipMovement/GetMembershipMovementByMemberId")]
        [HttpGet]
        public IActionResult GetMembershipMovementByMemberId(long? MemberId)
        {
            var MembershipMovements = (from MS in DB.MembershipMovements?.Where(z => z.MemberId == MemberId)?.ToList()
                                       let p = new
                                       {
                                           MS.Id,
                                           MS.Membership.Name,
                                           MS.Membership.MinFreezeLimitDays,
                                           MS.Membership.MaxFreezeLimitDays,
                                           MS.VisitsUsed,
                                           MS.Type,
                                           MS.TotalAmmount,
                                           MS.MemberId,
                                           MS.MembershipId,
                                           MS.DiscountDescription,
                                           MS.Description,
                                           MS.StartDate,
                                           MS.EndDate,
                                           MS.Discount,
                                           MS.EditorName,
                                           MembershipMovementOrders = (from MSO in DB.MembershipMovementOrders.Where(f => f.MemberShipMovementId == MS.Id).ToList()
                                                                       select new
                                                                       {
                                                                           MSO.Id,
                                                                           MSO.Type,
                                                                           MSO.StartDate,
                                                                           MSO.EndDate,
                                                                           MSO.Description,
                                                                           Status = (from a in DB.Oprationsys.ToList()
                                                                                     where (a.Status == MSO.Status) && (a.TableName == "MembershipMovementOrder")
                                                                                     select new
                                                                                     {
                                                                                         a.Id,
                                                                                         a.OprationName,
                                                                                         a.Status,
                                                                                         a.OprationDescription,
                                                                                         a.ArabicOprationDescription,
                                                                                         a.IconClass,
                                                                                         a.ClassName
                                                                                     }).FirstOrDefault(),
                                                                       }).ToList(),

                                           MS.Tax,
                                           Status = (from a in DB.Oprationsys.ToList()
                                                     where (a.Status == MS.Status) && (a.TableName == "MembershipMovement")
                                                     select new
                                                     {
                                                         a.Id,
                                                         a.OprationName,
                                                         a.Status,
                                                         a.OprationDescription,
                                                         a.ArabicOprationDescription,
                                                         a.IconClass,
                                                         a.ClassName
                                                     }).FirstOrDefault(),
                                  
                                       }
                                       select p);

            return Ok(MembershipMovements);
        }
        [Route("MembershipMovement/GetMembershipMovementByID")]
        [HttpGet]
        public IActionResult GetMembershipMovementByID(long? ID)
        {
            var MembershipMovements = (from MS in DB.MembershipMovements?.Where(z => z.Id == ID)?.ToList()
                                       let p = new
                                       {
                                           MS.Id,
                                           MS.VisitsUsed,
                                           MS.Type,
                                           MS.TotalAmmount,
                                           MS.MemberId,
                                           MS.MembershipId,
                                           MS.DiscountDescription,
                                           MS.Description,
                                           MS.StartDate,
                                           MS.EndDate,
                                           MS.Discount,
                                           MS.Tax,
                                           MS.Status,                                   
                                           MS.EditorName,                                   

                                       }
                                       select p).SingleOrDefault();

            return Ok(MembershipMovements);
        }
        [Route("MembershipMovement/GetMembershipMovementByStatus")]
        [HttpGet]
        public IActionResult GetMembershipMovementByStatus(int? Status)
        {
            var MembershipMovements = (from x in DB.MembershipMovements?.Where(z=> z.Status == Status)?.ToList()
                            let p = new
                            {
                                x.Id,
                                x.TotalAmmount,
                                x.Tax,
                                x.StartDate,
                                x.EndDate,
                                x.Type,
                                x.VisitsUsed,
                                x.Discount,
                                x.DiscountDescription,
                                x.Description,
                                x.Status,
                                x.EditorName,
                                x.MemberId,
                                x.Member.AccountId,
                                MemberName= DB.Members.Where(m=>m.Id == x.MemberId).SingleOrDefault().Name,
                                MembershipName= DB.Memberships.Where(m=>m.Id == x.MembershipId).SingleOrDefault().Name,
                            }
                            select p);

            return Ok(MembershipMovements);
        }
    }
}