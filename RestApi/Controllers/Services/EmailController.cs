using Application.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RestApi.Controllers.Services;

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
