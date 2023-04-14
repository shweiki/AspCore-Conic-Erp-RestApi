using Application.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Net.Mail;
using System.Net.Mime;

namespace Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _systemConfiguration;


    public EmailService(IConfiguration configuration)
    {
        _systemConfiguration = configuration;

    }

    public async Task<bool> SendMail(string recipientEmailAddress, string subject, string body, bool isBodyHtml)
    {
        string smtpServerAddress = "";
        int port = 0;
        if (string.IsNullOrWhiteSpace(recipientEmailAddress)) return false;
        try
        {
            var smtp = new SMTPConfiguration();
            _systemConfiguration.Bind("SmtpConfiguration", smtp);

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
            mail.To.Add(recipientEmailAddress);
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = isBodyHtml;
            //  mail.Attachments = isBodyHtml;

            smtpClient.Port = port;
            smtpClient.EnableSsl = EnableSsl;
         //   smtpClient.UseDefaultCredentials = true;
            smtpClient.Credentials = new System.Net.NetworkCredential(smtpUsername, smtpPassword);

            smtpClient.Send(mail);
            return true;
        }
        catch (Exception ex)
        {

            return false;
        }
    }
    public async Task<bool> SendMailWithAttachment(string recipientEmailAddress, string subject, string body, bool isBodyHtml, string pathAttachment, string fileName)
    {
        string smtpServerAddress = "";
        int port = 0;

        try
        {

            var smtp = new SMTPConfiguration();
            _systemConfiguration.Bind("SmtpConfiguration", smtp);
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
            mail.To.Add(recipientEmailAddress);
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

            return false;
        }
    }
}
