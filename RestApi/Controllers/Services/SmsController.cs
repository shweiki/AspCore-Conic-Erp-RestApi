using Application.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MailApp.Controllers;

[Authorize]
public class SmsController : Controller
{
    private readonly ISMSService _smsService;

    public SmsController(ISMSService smsService)
    {
        _smsService = smsService;
    }

    [Route("Sms/SendTo")]
    [HttpPost]
    public async Task<IActionResult> SendTo(string tonumber, string message)
    {

        return Ok(await _smsService.SendSMSAsync(tonumber, message));
    }
    [Route("Sms/SendToMulti")]
    [HttpPost]
    public async Task<IActionResult> SendToMulti(List<string> tonumbers, string message)
    {
        return Ok(await _smsService.SendMultiSMSAsync(tonumbers, message));
    }

}
