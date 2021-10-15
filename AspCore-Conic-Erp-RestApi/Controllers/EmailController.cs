using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmailService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MailApp.Controllers
{
    [Authorize]
    public class EmailController : Controller
    {
    

        private readonly IEmailSender _emailSender;

        public EmailController(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }
        [Route("Email/Send")]
        [HttpPost]
        public async Task<IActionResult> Send(string to ,string subject , string body)
        {

            var message = new Message(new string[] { to }, subject, body, null);
            await _emailSender.SendEmailAsync(message);

            return Ok(true);
        }
    
     
    }
}
