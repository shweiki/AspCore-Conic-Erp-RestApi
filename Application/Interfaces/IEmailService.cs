using System.Threading.Tasks;

namespace Application.Interfaces;

public interface IEmailService
{
    Task<bool> SendMail(string recipientEmailAddress, string subject, string body, bool isBodyHtml);
    Task<bool> SendMailWithAttachment(string recipientEmailAddress, string subject, string body, bool isBodyHtml, string pathAttachment, string fileName);
}