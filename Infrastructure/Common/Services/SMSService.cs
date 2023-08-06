using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Application.Common.Interfaces;
using Application.Features.SystemConfiguration.Queries.GetSMSConfiguration;
using System.Text.RegularExpressions;

namespace Infrastructure.Common.Services;

public class SMSService : ISMSService
{
    private readonly ILogger<SMSService> _logger;
    private readonly ISender _mediator;

    public SMSService(ILogger<SMSService> logger, ISender mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    public async Task<bool> SendSMSAsync(string tonumber, string message)
    {
        var SMSconfig = await _mediator.Send(new GetSMSConfigurationQuery());
        if (SMSconfig is null) return false;

        string baseURL = SMSconfig.BaseURL;
        try
        {
            var regexBaseURL = Regex.Replace(baseURL, @"\{msg\}", message);
            regexBaseURL = Regex.Replace(regexBaseURL, @"\{number\}", tonumber.Trim());
            Uri sendingOnebyOneURL = new Uri(regexBaseURL);

            //Uri sendingOnebyOneURL = new Uri(baseURL
            //    + "?senderid=" + senderId
            //    + "&numbers=" + tonumber.Replace(" ", string.Empty)
            //    + "&accname=" + accName
            //    + "&AccPass=" + accPassword
            //    + "&msg=" + message
            //    + "&id=" + string.Empty);

            using (var httpClient = new HttpClient())
            {
                var req = new HttpRequestMessage(HttpMethod.Get, sendingOnebyOneURL);
                var res = await httpClient.SendAsync(req);
                if (res.StatusCode.ToString() == "OK")
                {
                    return true;
                }
                else
                {
                    var result = JObject.Parse(await res.Content.ReadAsStringAsync());

                    _logger.LogError("Error sending sms to {tonumber}. SmsServer: {result}", result);
                    return false;

                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending sms to {tonumber}. SmsServer: [{senderId}]",
                tonumber, baseURL);
            return false;
        }
    }
}

