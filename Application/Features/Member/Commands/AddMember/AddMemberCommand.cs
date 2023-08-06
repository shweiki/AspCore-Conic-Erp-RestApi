using Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;


namespace Application.Features.Member.Commands.AddMember;

public class AddMemberCommand : IRequest<string>
{
    public string Name { get; set; }
    public DateTime? DateofBirth { get; set; }
    public string Email { get; set; }
    public string PhoneNumber1 { get; set; }
    public string PhoneNumber2 { get; set; }
    public string Description { get; set; }
    public int Status { get; set; }
    public string Type { get; set; }
    public long AccountId { get; set; }
    public string Ssn { get; set; }
    public string Tag { get; set; }
    public string Vaccine { get; set; }


}

public class AddUserCommandHandler : IRequestHandler<AddMemberCommand, string>
{
    private readonly IApplicationDbContext _context;
    private readonly ILogger<AddMemberCommand> _logger;
    private readonly ICurrentUserService _currentUserService;
    private readonly ISMSService _smsService;

    public AddUserCommandHandler(IApplicationDbContext context, ILogger<AddMemberCommand> logger, ICurrentUserService currentUserService, ISMSService smsService)
    {
        _context = context;
        _logger = logger;
        _currentUserService = currentUserService;
        _smsService = smsService;
    }

    public async Task<string> Handle(AddMemberCommand request, CancellationToken cancellationToken)
    {
        return "";
        //    var result = await _context.ExternalUser.AddAsync(new Domain.Entities.ExternalUser
        //    {
        //        FullName = request.FullName,
        //        Organization = request.Organization,
        //        Email = request.Email,
        //        EmailConfirmed = false,
        //        PhoneNumber = request.Phone,
        //        PhoneNumberConfirmed = false,
        //        Country = request.Country,
        //        State = request.State,
        //        ExternalUserStatusLookUpId = (int)ExternalUserStatusLookupEnum.Pending
        //    });
        //    if (result is not null)
        //    {
        //        try
        //        {
        //            AddActionLog(result.Entity.FullName);
        //            await _context.SaveChangesAsync(cancellationToken, _currentUserService.Username);
        //            _smsService.SendExternalVerificationAccountEmail(result.Entity.Id.ToString());
        //            return result.Entity.Id.ToString();
        //        }
        //        catch (Exception ex)
        //        {
        //            _logger.LogError(ex, "Error Add Member Command : {FullName}", request.FullName);
        //            return null;
        //        }

        //    }
        //    else
        //    {
        //        _logger.LogError("Error Add Member Command : {FullName}", request.FullName);
        //        return null;
        //    }

        //}
        //public void AddActionLog(string fullName)
        //{
        //    var actionLog = new Domain.Entities.ActionLog
        //    {
        //        Username = _currentUserService.Username,
        //        ActionDate = DateTime.Now,
        //        ActionTypeLookupId = (int)ActionTypeLookupEnum.CreateExternalUser, // TODO: Add ActionTypeEnum
        //        Detail = fullName,
        //    };
        //    _context.ActionLog.Add(actionLog);
        //}
    }
}
