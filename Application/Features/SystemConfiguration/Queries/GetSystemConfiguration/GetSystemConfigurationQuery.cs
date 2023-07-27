using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Application.Common.Interfaces;


namespace SignTEC.Application.Features.SystemConfiguration.Queries.GetSystemConfiguration;

public class GetSystemConfigurationQuery : IRequest<SystemConfiguration>
{
}
public class GetSystemConfigurationQueryHandler : IRequestHandler<GetSystemConfigurationQuery, SystemConfiguration?>
{
    private readonly IApplicationDbContext _context;

    public GetSystemConfigurationQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<SystemConfiguration?> Handle(GetSystemConfigurationQuery request, CancellationToken cancellationToken)
    {
        var systemConfiguration = await _context.SystemConfiguration.Where(x => x.Key.Contains("System.")).ToListAsync();
        if (systemConfiguration.Count > 0)
        {
            string homePageUrl = systemConfiguration.SingleOrDefault(x => x.Key == "System.HomePageUrl")?.Value ?? "https://localhost:7041/";
            if (homePageUrl.LastOrDefault() != '/')
            {
                homePageUrl += "/";
            }
            return new SystemConfiguration
            {
                DefaultFilesPath = systemConfiguration.SingleOrDefault(x => x.Key == "System.DefaultFilesPath")?.Value ?? "C:/SignTecFiles/",
                EmailNotificationEnabled = bool.Parse(systemConfiguration.SingleOrDefault(x => x.Key == "System.EmailNotificationEnabled")?.Value ?? "true"),
                LicenseExpiryNotificationPeriodInDays = int.Parse(systemConfiguration.SingleOrDefault(x => x.Key == "System.LicenseExpiryNotificationPeriodInDays")?.Value ?? "7"),
                DSSExpiryNotificationPeriodInDays = int.Parse(systemConfiguration.SingleOrDefault(x => x.Key == "System.DSSExpiryNotificationPeriodInDays")?.Value ?? "7"),
                DigitalCertificateExpiryNotificationPeriodInDays = int.Parse(systemConfiguration.SingleOrDefault(x => x.Key == "System.DigitalCertificateExpiryNotificationPeriodInDays")?.Value ?? "7"),
                SignatureQuotaLimitNotificationPeriodInPercentage = double.Parse(systemConfiguration.SingleOrDefault(x => x.Key == "System.SignatureQuotaLimitNotificationPeriodInPercentage")?.Value ?? "85"),
                OTPExpiryPeriodInMinute = int.Parse(systemConfiguration.SingleOrDefault(x => x.Key == "System.OTPExpiryPeriodInMinute")?.Value ?? "5"),
                OTPResendLimit = int.Parse(systemConfiguration.SingleOrDefault(x => x.Key == "System.OTPResendLimit")?.Value ?? "3"),
                OTPSessionTimeOutInMinute = int.Parse(systemConfiguration.SingleOrDefault(x => x.Key == "System.OTPSessionTimeOutInMinute")?.Value ?? "30"),
                TerminatedSignedSignaturePeriodInDay = int.Parse(systemConfiguration.SingleOrDefault(x => x.Key == "System.TerminatedSignedSignaturePeriodInDay")?.Value ?? "30"),
                UsingWinAuth = bool.Parse(systemConfiguration.SingleOrDefault(x => x.Key == "System.UsingWinAuth")?.Value ?? "false"),
                HomePageUrl = homePageUrl,
                LocationServiceUrl = systemConfiguration.SingleOrDefault(x => x.Key == "System.LocationServiceUrl")?.Value ?? "",
            };
        }
        return new SystemConfiguration();
    }
}