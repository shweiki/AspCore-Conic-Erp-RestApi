using MediatR;
using Microsoft.EntityFrameworkCore;
using Application.Common.Interfaces;


namespace SignTEC.Application.Features.SystemConfiguration.Queries.GetSMSConfiguration;

public class GetSMSConfigurationQuery : IRequest<SMSConfiguration>
{
}
public class GetSMSConfigurationQueryHandler : IRequestHandler<GetSMSConfigurationQuery, SMSConfiguration>
{
    private readonly IApplicationDbContext _context;

    public GetSMSConfigurationQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<SMSConfiguration> Handle(GetSMSConfigurationQuery request, CancellationToken cancellationToken)
    {
        var sMSConfiguration = await _context.SystemConfiguration.Where(x => x.Key.Contains("SMS.")).ToListAsync();
        if (!string.IsNullOrWhiteSpace(sMSConfiguration.SingleOrDefault(x => x.Key == "SMS.BaseURL")?.Value))
        {
            return new SMSConfiguration
            {
                BaseURL = sMSConfiguration.SingleOrDefault(x => x.Key == "SMS.BaseURL")?.Value ?? "",
                BaseURLBulkMessages = sMSConfiguration.SingleOrDefault(x => x.Key == "SMS.BaseURLBulkMessages")?.Value ?? "",
                SenderId = sMSConfiguration.SingleOrDefault(x => x.Key == "SMS.SenderId")?.Value ?? "",
                AccName = sMSConfiguration.SingleOrDefault(x => x.Key == "SMS.AccName")?.Value ?? "",
                AccPassword = sMSConfiguration.SingleOrDefault(x => x.Key == "SMS.AccPassword")?.Value ?? "",
            };
        }
        return null;
    }
}
