using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Features.MemberShips.Services;

public class ScanMembershipMovementByIdService : IRequest<bool>
{
    public long Id { get; set; }
}

public class ScanMembershipMovementByIdServiceHandler : IRequestHandler<ScanMembershipMovementByIdService, bool>
{
    private readonly IApplicationDbContext _context;
    private readonly ILogger _logger;
    private readonly ISender _mediator;

    public ScanMembershipMovementByIdServiceHandler(
        IApplicationDbContext context,
        ILogger<ScanMembershipMovementByIdService> logger,
         ISender sender
        )
    {
        _context = context;
        _logger = logger;
        _mediator = sender;
    }

    public async Task<bool> Handle(ScanMembershipMovementByIdService request, CancellationToken cancellationToken)
    {
        try
        {
            MembershipMovement membershipMovement = await _context.MembershipMovement.Include(x => x.Member).Include(x => x.Membership).Include(x => x.MembershipMovementOrders).SingleOrDefaultAsync(x => x.Id == request.Id);
            var member = membershipMovement.Member;

            if (membershipMovement is null)
            {
                member.Status = -1;
                return true;
            }

            int OStatus = member.Status;
            double TotalMembershipMovementOrders = membershipMovement.MembershipMovementOrders.Where(x => x.Status == -2 || x.Status == -3).ToList().Aggregate(0.0, (acc, x) => acc + (x.EndDate - x.StartDate).TotalDays);
            int MembershipNumberDays = membershipMovement.Membership.NumberDays;// DB.Memberships.Where(m => m.Id == MS.MembershipId).FirstOrDefault().NumberDays;
            membershipMovement.StartDate = membershipMovement.StartDate.Date;
            membershipMovement.EndDate = membershipMovement.StartDate.AddDays(MembershipNumberDays + TotalMembershipMovementOrders).Date.AddDays(1).AddSeconds(-1);
            if (DateTime.Today >= membershipMovement.StartDate.Date && DateTime.Today <= membershipMovement.EndDate.Date.AddDays(1).AddSeconds(-1))
            {

                membershipMovement.Status = 1;
                member.Status = 0;
                var HowManyDaysLeft = (membershipMovement.EndDate - DateTime.Today).TotalDays;
                if (HowManyDaysLeft == 3)
                {
                    Massage msg = new();
                    msg.Body = "عزيزي " + member.Name + " يسعدنا ان تكون متواجد دائماَ معنا في High Fit , نود تذكيرك بان اشتراك الحالي سينتهي بعد 3 ايام وبتاريخ " + membershipMovement.EndDate + " وشكرا";
                    msg.Status = 0;
                    msg.TableName = "Member";
                    msg.Fktable = member.Id;
                    msg.PhoneNumber = member.PhoneNumber1;
                    msg.SendDate = DateTime.Now;
                    msg.Type = "رسالة تذكير";
                    _context.Massage.Add(msg);
                }

            }
            else
            {

                if (membershipMovement.StartDate.Date > DateTime.Today)
                {// معلق
                    membershipMovement.Status = -2;
                }
                else
                {
                    membershipMovement.Status = -1;
                    member.Status = -1;
                }
            }

            foreach (var membershipMovementOrder in membershipMovement.MembershipMovementOrders.Where(x => x.Status == 1 || x.Status == 2).ToList())
            {
                if (membershipMovementOrder.Status == 2)
                {
                    membershipMovement.EndDate = membershipMovement.EndDate.AddDays((membershipMovementOrder.EndDate - membershipMovementOrder.StartDate).TotalDays);
                    membershipMovementOrder.Status = -2;
                    continue;
                }
                if (DateTime.Today >= membershipMovementOrder.StartDate.Date && DateTime.Today <= membershipMovementOrder.EndDate.Date.AddDays(1).AddSeconds(-1))
                {
                    if (membershipMovementOrder.Type == "Freeze")
                    {
                        membershipMovement.Status = 2;
                        member.Status = 1;
                    }
                    if (membershipMovementOrder.Type == "Extra")
                    {
                        membershipMovement.Status = 3;
                        member.Status = 2;

                    }
                }
                else
                {
                    if (membershipMovementOrder.Type == "Extra")
                    {
                        membershipMovement.EndDate = membershipMovement.EndDate.AddDays((membershipMovementOrder.EndDate - membershipMovementOrder.StartDate).TotalDays);
                        membershipMovementOrder.Status = -3;
                    }
                    if (DateTime.Today > membershipMovementOrder.EndDate)
                    {

                        membershipMovement.EndDate = membershipMovement.EndDate.AddDays((membershipMovementOrder.EndDate - membershipMovementOrder.StartDate).TotalDays);
                        membershipMovementOrder.Status = -3;
                    }

                }
                if (membershipMovement.EndDate.Date.AddDays(1).AddSeconds(-1) > DateTime.Today)
                {


                    membershipMovement.Status = 1;
                    member.Status = 1;
                    var DeviceLogs = await _context.DeviceLog.Where(x => x.Fk == member.Id.ToString() && x.TableName == "Member" && x.DateTime.Date >= membershipMovement.StartDate.Date && x.DateTime.Date <= membershipMovement.EndDate.Date).ToListAsync();
                    if (DeviceLogs != null && DeviceLogs.Count > 0)
                    {
                        DeviceLogs = DeviceLogs.GroupBy(a => a.DateTime.Day).Select(g => g.Last()).ToList();
                        membershipMovement.VisitsUsed = DeviceLogs.Count();
                        int NumberClass = (int)(membershipMovement.Membership.NumberClass == null ? 0 : membershipMovement.Membership.NumberClass);

                        //var WhenFinishNumberOfClassUnActiveMemberShip = Configuration["GymConfiguration:WhenFinishNumberOfClassUnActiveMemberShip"];
                        //if (WhenFinishNumberOfClassUnActiveMemberShip == "True")
                        //{
                        //    if (MS.Membership.NumberClass <= MS.VisitsUsed)
                        //    {
                        //        MS.EndDate = DeviceLogs.LastOrDefault().DateTime;
                        //        MS.Status = -1;
                        //        member.Status = -1;
                        //    }
                        //}
                    }
                }


            }

            if (OStatus == -2) member.Status = -2;


            await _context.SaveChangesAsync(cancellationToken);

            return true;

        }
        catch (Exception ex)
        {
            _logger.LogError("Error create Sign Transaction : {Id}  Message : {Message}  StackTrace :{StackTrace} ", request.Id, ex.Message, ex.StackTrace);
            return false;
        }

    }
}