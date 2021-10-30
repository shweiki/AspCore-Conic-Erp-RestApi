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
                private ConicErpContext DB;
        public MembershipMovementController(ConicErpContext dbcontext)
        {
            DB = dbcontext;
        }

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
                    return Ok(collection.Id);

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
                    return Ok(collection.Id);

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
            DateTime MaxDate = new DateTime(2021, 1, 1);
            IList<MembershipMovement>  MembershipMovements = DB.MembershipMovements.Where(x=>  x.EndDate  >= MaxDate)?.OrderBy(s => s.Id).ToList();
         
            foreach (MembershipMovement MS in MembershipMovements)
            {
              
                var member = DB.Members.Where(x => x.Id == MS.MemberId).SingleOrDefault();
                int OStatus = member.Status;
                if (MS.Status == 0) continue;

                if ((DateTime.Now >= MS.StartDate && DateTime.Now <= MS.EndDate))
                {
                 
                        MS.Status = 1;
                        member.Status = 0;
                    var HowManyDaysLeft = (MS.EndDate - DateTime.Now).TotalDays;
                    if (HowManyDaysLeft == 3)
                    {
                        Massage msg = new Massage();
                        msg.Body = "عزيزي " + member.Name + " يسعدنا ان تكون متواجد دائماَ معنا في High Fit , نود تذكيرك بان اشتراك الحالي سينتهي بعد 3 ايام وبتاريخ " + MS.EndDate + " وشكرا";
                        msg.Status = 0;
                        msg.TableName = "Member";
                        msg.Fktable = member.Id;
                        msg.PhoneNumber = member.PhoneNumber1;
                        msg.SendDate = DateTime.Now;
                        msg.Type = "رسالة تذكير";
                        DB.Massages.Add(msg);
                    }

                }
                else
                {

                    if ( MS.StartDate > DateTime.Now ) {// معلق
                        MS.Status = -2;
                    }
                    else
                    {

                        MS.Status = -1;
                        member.Status = -1;
                    }
                }
             
                foreach (MembershipMovementOrder MSO in DB.MembershipMovementOrders.Where(x => x.MemberShipMovementId == MS.Id &&( x.Status == 1 || x.Status == 2)).ToList())
                {
                    if (MSO.Status == 2)
                    {
                        MS.EndDate = MS.EndDate.AddDays((MSO.EndDate - MSO.StartDate).TotalDays);
                        MSO.Status = -2;
                        continue;
                    }
                    if ((DateTime.Now >= MSO.StartDate && DateTime.Now <= MSO.EndDate))
                    {
                        if (MSO.Type == "Freeze")
                        {
                            MS.Status = 2;
                            member.Status = 1;
                        }
                        if (MSO.Type == "Extra")
                        {
                            MS.Status = 3;
                           member.Status = 2;

                        }
                    }
                    else
                    {
                        if (MSO.Type == "Extra")
                        {
                            MS.EndDate = MS.EndDate.AddDays((MSO.EndDate - MSO.StartDate).TotalDays);
                            MSO.Status = -3;
                        }
                        if (DateTime.Now > MSO.EndDate)
                        {

                            MS.EndDate =  MS.EndDate.AddDays((MSO.EndDate - MSO.StartDate).TotalDays);
                            MSO.Status = -3;
                        }

                }
                if ((MS.EndDate > DateTime.Now))
                {
                   
                  
                    MS.Status = 1;
                        member.Status = 1;

                }
                if (MS == null )
                {
                        member.Status = -1;
                    }

                }

                if (OStatus == -2) member.Status = -2;

                    DB.SaveChanges();

            }
            return Ok(true);
        }

        [Route("MembershipMovement/GetMembershipMovementByMemberId")]
        [HttpGet]
        public IActionResult GetMembershipMovementByMemberId(long? MemberId)
        {
            var MembershipMovements = DB.MembershipMovements.Where(z => z.MemberId == MemberId).Select(MS => new
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
                MS.Status,
                MS.Tax,
                MembershipMovementOrders = DB.MembershipMovementOrders.Where(f => f.MemberShipMovementId == MS.Id).Select(MSO => new
                {
                    MSO.Id,
                    MSO.Type,
                    MSO.StartDate,
                    MSO.EndDate,
                    MSO.Status,
                    MSO.Description,
                }).ToList(),
            }).ToList();
                                    
                                         
                 
            return Ok(MembershipMovements);
        }
        [Route("MembershipMovement/GetMembershipMovementById")]
        [HttpGet]
        public IActionResult GetMembershipMovementById(long? Id)
        {
            var MembershipMovements = DB.MembershipMovements.Where(z => z.Id == Id).Select(x => new {
                x.Id,
                x.VisitsUsed,
                x.Type,
                x.TotalAmmount,
                x.MemberId,
                x.MembershipId,
                x.DiscountDescription,
                x.Description,
                x.StartDate,
                x.EndDate,
                x.Discount,
                x.Tax,
                x.Status,
                x.EditorName,
            }).SingleOrDefault();


            return Ok(MembershipMovements);
        }
        [Route("MembershipMovement/GetMembershipMovementByStatus")]
        [HttpGet]
        public IActionResult GetMembershipMovementByStatus(int? Status)
        {
            var MembershipMovements = DB.MembershipMovements.Where(z => z.Status == Status).Select(x => new {
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
                MemberName = DB.Members.Where(m => m.Id == x.MemberId).SingleOrDefault().Name,
                MembershipName = DB.Memberships.Where(m => m.Id == x.MembershipId).SingleOrDefault().Name,
            }).ToList();
                         
                              
            return Ok(MembershipMovements);
        }     
        
        [Route("MembershipMovement/GetMembershipMovementByDateIn")]
        [HttpGet]
        public IActionResult GetMembershipMovementByDateIn(DateTime DateIn)
        {

         
            var MembershipMovements = DB.MembershipMovements.Where(z => DateIn >= z.StartDate && z.EndDate  >= DateIn).Select(x => new {
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
                MemberName = DB.Members.Where(m => m.Id == x.MemberId).SingleOrDefault().Name,
                MembershipName = DB.Memberships.Where(m => m.Id == x.MembershipId).SingleOrDefault().Name,
            }).ToList();
     
                              
          return Ok(MembershipMovements);
        }
    }
}