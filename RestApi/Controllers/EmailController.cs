using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MailApp.Controllers;

[Authorize]
public class EmailController : Controller
{
    private readonly IEmailService _emailService;

    public EmailController(IEmailService emailService)
    {
        _emailService = emailService;
    }

    [Route("Email/SendTo")]
    [HttpPost]
    public async Task<IActionResult> SendTo(string to, string subject, string body)
    {
       
        return Ok(await _emailService.SendMail(to, subject, body, true));
    }

}
