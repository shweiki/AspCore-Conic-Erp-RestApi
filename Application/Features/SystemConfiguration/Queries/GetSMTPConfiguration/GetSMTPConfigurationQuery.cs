using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.SystemConfiguration.Queries.GetSMTPConfiguration;

public class GetSMTPConfigurationQuery : IRequest<SMTPConfiguration?>
{
}
public class GetSMTPConfigurationQueryHandler : IRequestHandler<GetSMTPConfigurationQuery, SMTPConfiguration?>
{
    private readonly IApplicationDbContext _context;

    public GetSMTPConfigurationQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<SMTPConfiguration?> Handle(GetSMTPConfigurationQuery request, CancellationToken cancellationToken)
    {
        var sMTPConfiguration = await _context.SystemConfiguration.Where(x => x.Key.Contains("SMTP.")).ToListAsync();
        if (!string.IsNullOrWhiteSpace(sMTPConfiguration.SingleOrDefault(x => x.Key == "SMTP.SmtpServer")?.Value))
        {
            var SMTP = new SMTPConfiguration
            {
                SmtpServer = sMTPConfiguration.SingleOrDefault(x => x.Key == "SMTP.SmtpServer")?.Value ?? "",
                SmtpUsername = sMTPConfiguration.SingleOrDefault(x => x.Key == "SMTP.SmtpUsername")?.Value ?? "",
                SmtpPassword = sMTPConfiguration.SingleOrDefault(x => x.Key == "SMTP.SmtpPassword")?.Value ?? "",
                SmtpPort = int.Parse(sMTPConfiguration.SingleOrDefault(x => x.Key == "SMTP.SmtpPort")?.Value ?? "0"),
                SmtpTLS = bool.Parse(sMTPConfiguration.SingleOrDefault(x => x.Key == "SMTP.SmtpTLS")?.Value ?? "false"),
                SmtpFromAddress = sMTPConfiguration.SingleOrDefault(x => x.Key == "SMTP.SmtpFromAddress")?.Value ?? ""
            };
            return SMTP;
        }
        return null;
    }
}
