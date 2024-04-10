using Application.Common.Interfaces;
using Domain.Common.Enum;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Features.MemberShips.Services;

public class ScanMembershipMovementByIdService : IRequest<MemberStatus>
{
    public long Id { get; set; }
}

public class ScanMembershipMovementByIdServiceHandler : IRequestHandler<ScanMembershipMovementByIdService, MemberStatus>
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

    public async Task<MemberStatus> Handle(ScanMembershipMovementByIdService request, CancellationToken cancellationToken)
    {
        try
        {
            var membershipMovement = await _context.MembershipMovement.Include(x => x.Member).Include(x => x.Membership).Include(x => x.MembershipMovementOrders).SingleOrDefaultAsync(x => x.Id == request.Id);
            var member = membershipMovement.Member;

            if (membershipMovement is null)
            {
                member.Status = (int)MemberStatus.Deactivate;
                return MemberStatus.Deactivate;
            }

            int OStatus = member.Status;
            double TotalMembershipMovementOrders = Math.Ceiling(membershipMovement.MembershipMovementOrders.Where(x => x.Status == (int)MembershipMovementOrderStatus.InProgress || x.Status == (int)MembershipMovementOrderStatus.Calculated).ToList().Aggregate(0.0, (acc, x) => acc + (x.EndDate - x.StartDate).TotalDays));
            int MembershipNumberDays = membershipMovement.Membership.NumberDays;// DB.Memberships.Where(m => m.Id == MS.MembershipId).FirstOrDefault().NumberDays;
            membershipMovement.StartDate = membershipMovement.StartDate.Date;
            membershipMovement.EndDate = membershipMovement.StartDate.AddDays(MembershipNumberDays + TotalMembershipMovementOrders);
            if (DateTime.Today >= membershipMovement.StartDate.Date && DateTime.Today <= membershipMovement.EndDate.Date.AddDays(1).AddSeconds(-1))
            {

                membershipMovement.Status = (int)MembershipMovementStatus.InProgress;
                member.Status = (int)MemberStatus.Active;

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
                    membershipMovement.Status = (int)MembershipMovementStatus.Suspense;
                }
                else
                {
                    membershipMovement.Status = (int)MembershipMovementStatus.Terminated;
                    member.Status = (int)MemberStatus.Deactivate;
                }
            }

            foreach (var membershipMovementOrder in membershipMovement.MembershipMovementOrders.Where(x => x.Status == 1 || x.Status == 2).ToList())
            {
                if (membershipMovementOrder.Status == (int)MembershipMovementOrderStatus.Calculated)
                {
                    continue;
                }
                if (DateTime.Today >= membershipMovementOrder.StartDate.Date && DateTime.Today <= membershipMovementOrder.EndDate.Date.AddDays(1).AddSeconds(-1))
                {

                    membershipMovement.Status = (int)MembershipMovementStatus.InProgress;
                    member.Status = (int)MemberStatus.Active;

                }
                else
                {

                    if (DateTime.Today > membershipMovementOrder.EndDate)
                    {

                        membershipMovement.EndDate = membershipMovement.EndDate.AddDays((membershipMovementOrder.EndDate - membershipMovementOrder.StartDate).TotalDays);
                        membershipMovementOrder.Status = (int)MembershipMovementOrderStatus.Calculated;
                    }

                }
                if (membershipMovement.EndDate.Date.AddDays(1).AddSeconds(-1) > DateTime.Today)
                {
                    membershipMovement.Status = (int)MembershipMovementStatus.InProgress;
                    member.Status = (int)MemberStatus.Active;
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

            if (OStatus == (int)MemberStatus.BlackList) member.Status = (int)MemberStatus.BlackList;


            await _context.SaveChangesAsync(cancellationToken);

            return (MemberStatus)member.Status;

        }
        catch (Exception ex)
        {
            _logger.LogError("Error create Sign Transaction : {Id}  Message : {Message}  StackTrace :{StackTrace} ", request.Id, ex.Message, ex.StackTrace);
            return MemberStatus.Deactivate;
        }

    }
}