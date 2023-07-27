using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Features.Members.Service;

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
            MembershipMovement MS = await _context.MembershipMovement.Include(x => x.Member).Include(x => x.Membership).Include(x => x.MembershipMovementOrders).SingleOrDefaultAsync(x => x.Id == request.Id);
            var member = MS.Member;
            if (MS is null)
            {
                member.Status = -1;
                return true;

            }

            int OStatus = member.Status;
            double TotalMembershipMovementOrders = MS.MembershipMovementOrders.Where(x => x.Status == -2 || x.Status == -3).ToList().Aggregate(0.0, (acc, x) => acc + (x.EndDate - x.StartDate).TotalDays);
            int MembershipNumberDays = MS.Membership.NumberDays;// DB.Memberships.Where(m => m.Id == MS.MembershipId).FirstOrDefault().NumberDays;
            MS.StartDate = MS.StartDate.Date;
            MS.EndDate = MS.StartDate.AddDays(MembershipNumberDays + TotalMembershipMovementOrders).Date.AddDays(1).AddSeconds(-1);
            if (DateTime.Today >= MS.StartDate.Date && DateTime.Today <= MS.EndDate.Date.AddDays(1).AddSeconds(-1))
            {

                MS.Status = 1;
                member.Status = 0;
                var HowManyDaysLeft = (MS.EndDate - DateTime.Today).TotalDays;
                if (HowManyDaysLeft == 3)
                {
                    Massage msg = new();
                    msg.Body = "عزيزي " + member.Name + " يسعدنا ان تكون متواجد دائماَ معنا في High Fit , نود تذكيرك بان اشتراك الحالي سينتهي بعد 3 ايام وبتاريخ " + MS.EndDate + " وشكرا";
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

                if (MS.StartDate.Date > DateTime.Today)
                {// معلق
                    MS.Status = -2;
                }
                else
                {
                    MS.Status = -1;
                    member.Status = -1;
                }
            }

            foreach (var MSO in MS.MembershipMovementOrders.Where(x => x.Status == 1 || x.Status == 2).ToList())
            {
                if (MSO.Status == 2)
                {
                    MS.EndDate = MS.EndDate.AddDays((MSO.EndDate - MSO.StartDate).TotalDays);
                    MSO.Status = -2;
                    continue;
                }
                if ((DateTime.Today >= MSO.StartDate.Date && DateTime.Today <= MSO.EndDate.Date.AddDays(1).AddSeconds(-1)))
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
                    if (DateTime.Today > MSO.EndDate)
                    {

                        MS.EndDate = MS.EndDate.AddDays((MSO.EndDate - MSO.StartDate).TotalDays);
                        MSO.Status = -3;
                    }

                }
                if ((MS.EndDate.Date.AddDays(1).AddSeconds(-1) > DateTime.Today))
                {


                    MS.Status = 1;
                    member.Status = 1;
                    var DeviceLogs = await _context.DeviceLog.Where(x => x.Fk == member.Id.ToString() && x.TableName == "Member" && (x.DateTime.Date >= MS.StartDate.Date && x.DateTime.Date <= MS.EndDate.Date)).ToListAsync();
                    if (DeviceLogs != null && DeviceLogs.Count > 0)
                    {
                        DeviceLogs = DeviceLogs.GroupBy(a => a.DateTime.Day).Select(g => g.Last()).ToList();
                        MS.VisitsUsed = DeviceLogs.Count();
                        int NumberClass = (int)(MS.Membership.NumberClass == null ? 0 : MS.Membership.NumberClass);

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



                  await _context.SaveChangesAsync(cancellationToken);

            }

            if (OStatus == -2) member.Status = -2;
            //   await DB.SaveChangesAsync();

            return true;

        }
        catch (Exception ex)
        {
            _logger.LogError("Error create Sign Transaction : {Id}  Message : {Message}  StackTrace :{StackTrace} ", request.Id, ex.Message, ex.StackTrace);
            return false;
        }

    }
}