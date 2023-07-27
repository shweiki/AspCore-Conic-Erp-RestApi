using MediatR;
using Microsoft.EntityFrameworkCore;
using Application.Common.Interfaces;

namespace Application.Features.SystemConfiguration.Command.AddDefaultSystemConfiguration;

public class AddDefaultSystemConfigurationCommand : IRequest<int>
{
}
public class AddDefaultSystemConfigurationCommandHandler : IRequestHandler<AddDefaultSystemConfigurationCommand, int>
{

    private readonly IApplicationDbContext _context;

    public AddDefaultSystemConfigurationCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<int> Handle(AddDefaultSystemConfigurationCommand request, CancellationToken cancellationToken)
    {
        var defaultConfig = new List<Domain.Entities.SystemConfiguration> {
                new Domain.Entities.SystemConfiguration {  Key = "System.DefaultFilesPath",  Value = "C:/ConicFiles/" },
                new Domain.Entities.SystemConfiguration {  Key = "System.EmailNotificationEnabled" ,  Value ="true" },
                new Domain.Entities.SystemConfiguration {  Key = "System.LicenseExpiryNotificationPeriodInDays" ,  Value = "7" },
                new Domain.Entities.SystemConfiguration {  Key = "System.DSSExpiryNotificationPeriodInDays" ,  Value = "7" },
                new Domain.Entities.SystemConfiguration {  Key = "System.DigitalCertificateExpiryNotificationPeriodInDays" ,  Value = "7" },
                new Domain.Entities.SystemConfiguration {  Key = "System.SignatureQuotaLimitNotificationPeriodInPercentage" ,  Value = "85" },
                new Domain.Entities.SystemConfiguration {  Key = "System.OTPExpiryPeriodInMinute" ,  Value = "5" },
                new Domain.Entities.SystemConfiguration {  Key = "System.OTPResendLimit" ,  Value = "3" },
                new Domain.Entities.SystemConfiguration {  Key = "System.OTPSessionTimeOutInMinute" ,  Value = "30" },
                new Domain.Entities.SystemConfiguration {  Key = "System.TerminatedSignedSignaturePeriodInDay" ,  Value = "7" },
                new Domain.Entities.SystemConfiguration {  Key = "System.UsingWinAuth" ,  Value ="false" },
                new Domain.Entities.SystemConfiguration {  Key = "System.HomePageUrl" ,  Value = "https://localhost:7041/" },
                new Domain.Entities.SystemConfiguration {  Key = "System.LocationServiceUrl" ,  Value = "https://api.bigdatacloud.net/data/reverse-geocode-client?latitude={latitude}&longitude={longitude}" },

                new Domain.Entities.SystemConfiguration {  Key = "SMTP.SmtpServer",  Value = "" },
                new Domain.Entities.SystemConfiguration {  Key = "SMTP.SmtpUsername" ,  Value ="" },
                new Domain.Entities.SystemConfiguration {  Key = "SMTP.SmtpPassword" ,  Value = "" },
                new Domain.Entities.SystemConfiguration {  Key = "SMTP.SmtpPort" ,  Value = "" },
                new Domain.Entities.SystemConfiguration {  Key = "SMTP.SmtpTLS" ,  Value ="false" },
                new Domain.Entities.SystemConfiguration {  Key = "SMTP.SmtpFromAddress" ,  Value = "" },

                new Domain.Entities.SystemConfiguration {  Key = "SMS.BaseURL",  Value = "" },
                new Domain.Entities.SystemConfiguration {  Key = "SMS.BaseURLBulkMessages" ,  Value ="" },
                new Domain.Entities.SystemConfiguration {  Key = "SMS.SenderId" ,  Value = "" },
                new Domain.Entities.SystemConfiguration {  Key = "SMS.AccName" ,  Value = "" },
                new Domain.Entities.SystemConfiguration {  Key = "SMS.AccPassword" ,  Value = "" },

            };
        foreach (Domain.Entities.SystemConfiguration systemConfiguration in defaultConfig)
        {
            var configExist = await _context.SystemConfiguration.SingleOrDefaultAsync(x => x.Key == systemConfiguration.Key) is not null;

            if (!configExist)
            {
                await _context.SystemConfiguration.AddAsync(systemConfiguration);
            }
        }
        await _context.SaveChangesAsync(cancellationToken, "");

        return 0;
    }
}