using Application.Common.Interfaces;
using Application.Features.SystemConfiguration.Queries.GetSMTPConfiguration;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Net.Mail;
using System.Net.Mime;

namespace Infrastructure.Common.Services;

public class EmailService : IEmailService
{
    private readonly ISystemConfigurationService _systemConfiguration;
    private readonly ILogger<EmailService> _logger;
    private readonly ISender _mediator;

    public EmailService(ISystemConfigurationService systemConfiguration, ILogger<EmailService> logger, ISender mediator)
    {
        _systemConfiguration = systemConfiguration;
        _logger = logger;
        _mediator = mediator;
    }

    public async Task<bool> SendMail(string emailAddress, string subject, string body, bool isBodyHtml)
    {
        string smtpServerAddress = "";
        int port = 0;
        if (string.IsNullOrWhiteSpace(emailAddress)) return false;
        try
        {
            var configuration = await _systemConfiguration.GetSystemConfiguration();
            bool enabled = configuration.EmailNotificationEnabled;
            if (!enabled)
            {
                return false;
            }
            var smtp = await _mediator.Send(new GetSMTPConfigurationQuery());
            if (smtp is null)
            {
                return false;
            }

            smtpServerAddress = smtp.SmtpServer;
            port = smtp.SmtpPort;

            string smtpUsername = smtp.SmtpUsername;
            string smtpPassword = smtp.SmtpPassword;
            string smtpFromAddress = smtp.SmtpFromAddress;
            bool EnableSsl = smtp.SmtpTLS;

            var smtpClient = new SmtpClient(smtpServerAddress);
            var mail = new MailMessage();

            mail.From = new MailAddress(smtpFromAddress);
            mail.To.Add(emailAddress);
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = isBodyHtml;
            // mail.AlternateViews = new AlternateView()
            //  mail.Attachments = isBodyHtml;

            smtpClient.Port = port;
            //   smtpClient.Timeout = 100;
            smtpClient.Credentials = new System.Net.NetworkCredential(smtpUsername, smtpPassword);

            //   smtpClient.Credentials = CredentialCache.DefaultNetworkCredentials;

            smtpClient.EnableSsl = EnableSsl;

            smtpClient.Send(mail);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending email to {emailAddress}. SmtpServer: [{smtpServerAddress}] smtpPort: [{port}]",
                emailAddress, smtpServerAddress, port);
            return false;
        }
    }
    public async Task<bool> SendMailWithAttachment(string emailAddress, string subject, string body, bool isBodyHtml, string pathAttachment, string fileName)
    {
        string smtpServerAddress = "";
        int port = 0;

        try
        {
            var configuration = await _systemConfiguration.GetSystemConfiguration();
            bool enabled = configuration.EmailNotificationEnabled;
            if (!enabled)
            {
                return false;
            }

            var smtp = await _mediator.Send(new GetSMTPConfigurationQuery());
            if (smtp is null)
            {
                return false;
            }
            smtpServerAddress = smtp.SmtpServer;
            port = smtp.SmtpPort;

            string smtpUsername = smtp.SmtpUsername;
            string smtpPassword = smtp.SmtpPassword;
            string smtpFromAddress = smtp.SmtpFromAddress;
            bool EnableSsl = smtp.SmtpTLS;

            var smtpClient = new SmtpClient(smtpServerAddress);
            var mail = new MailMessage();

            mail.From = new MailAddress(smtpFromAddress);
            mail.To.Add(emailAddress);
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = isBodyHtml;
            Attachment attachment = new Attachment(pathAttachment, MediaTypeNames.Application.Pdf);
            attachment.Name = fileName;
            mail.Attachments.Add(attachment);

            smtpClient.Port = port;
            smtpClient.Credentials = new System.Net.NetworkCredential(smtpUsername, smtpPassword);
            smtpClient.EnableSsl = EnableSsl;

            smtpClient.Send(mail);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending email to {emailAddress}. SmtpServer: [{smtpServerAddress}] smtpPort: [{port}]",
                emailAddress, smtpServerAddress, port);
            return false;
        }
    }
}
