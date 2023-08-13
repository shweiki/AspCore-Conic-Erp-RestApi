 using Microsoft.AspNetCore.SignalR.Client;

namespace RestApi.SignalR;

public class SignalRChatService
{
    private readonly HubConnection _connection;

    public SignalRChatService(HubConnection connection)
    {
        _connection = connection;
    }
    public async Task Connect()
    {
        await _connection.StartAsync();
    }
    public async Task SendDeviceLogEvent(string trxReference)
    {
        await _connection.SendAsync("SendDeviceLogEvent", trxReference, new object());
    }
}
