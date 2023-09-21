using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using ZktClientService.Interfaces;
using ZktClientService.Models;

namespace ZktClientService.Services
{
    public class ServerServices : IServerServices
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        private readonly ILogger<ServerServices> _logger;

        public ServerServices(IConfiguration configuration, ILogger<ServerServices> logger)
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.UseCookies = true;
            handler.UseDefaultCredentials = true;
            _httpClient = new HttpClient(handler);

            _httpClient.BaseAddress = new Uri(configuration.GetValue<string>("ServerBaseUrl"));
            _httpClient.DefaultRequestHeaders.Accept.Add(
               new MediaTypeWithQualityHeaderValue("application/json"));
            _logger = logger;
        }
        public string GetDevice()
        {
            return "";
        }
        public object? PostLogToServer(AttTransactionLog attTransactionLog)
        {
            try
            {
                // Wrap our JSON inside a StringContent object
                var response = _httpClient.PostAsJsonAsync("/DeviceLog/CreateFromDeviceService", attTransactionLog).Result;
                HttpStatusCode statusCode = response.StatusCode;
                var responseContent = response.Content.ReadAsStringAsync().Result;

               // _logger.LogInformation($"Post Log To Server responseContent :   {JsonSerializer.Serialize<Object>(responseContent)},  Status Code: {statusCode}  , {JsonSerializer.Serialize<AttTransactionLog>(attTransactionLog)} ");
                if (statusCode == HttpStatusCode.OK)
                {
                    return responseContent;
                }
                return null;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Post Log To Server Message : {ex.Message} , StackTrace:  {ex.StackTrace} ");
            }

            return null;
        }
        public List<UserInfo> GetUsers(string accessToken)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = _httpClient.GetAsync("/Device/GetUsersForDevice").Result;
            HttpStatusCode statusCode = response.StatusCode;

            if (statusCode == HttpStatusCode.OK)
            {
                var responseContent = response.Content.ReadAsStringAsync().Result;
                List<UserInfo> userInfos = JsonSerializer.Deserialize<List<UserInfo>>(responseContent);

                return userInfos;
            }
            return null;
        }
    }
}
