using System.Threading.Tasks;

namespace Application.Common.Interfaces;

public interface IEmailService
{
    Task<bool> SendMail(string emailAddress, string subject, string body, bool isBodyHtml);
    Task<bool> SendMailWithAttachment(string emailAddress, string subject, string body, bool isBodyHtml, string pathAttachment, string fileName);
}