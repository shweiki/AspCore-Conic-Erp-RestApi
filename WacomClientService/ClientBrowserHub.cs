using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Net.Sockets;

namespace WacomClientService.Hubs;

public class ClientBrowserHub : Hub
{
    private readonly IHubContext<ClientBrowserHub> _clientBrowserHub;
    private readonly IConfiguration _configuration;
    private readonly IHttpContextAccessor _httpContextAccessor;

    private readonly ConcurrentQueue<(string CallBackMethod, string Action, string Message)> requestQueue = new ConcurrentQueue<(string CallBackMethod, string Action, string Message)>();
    private bool isProcessingQueue = false;


    public ClientBrowserHub(IHubContext<ClientBrowserHub> clientBrowserHub, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
        _clientBrowserHub = clientBrowserHub;
        _configuration = configuration;
    }



    private async Task ProcessRequest(string CallBackMethod, string action, string Response_message)
    {
        Debug.WriteLine("========ProcessRequest====================Start" + action);
        await Clients.Caller.SendAsync(CallBackMethod, action, Response_message);
    }




}