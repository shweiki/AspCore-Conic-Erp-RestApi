using Microsoft.AspNetCore.SignalR;

namespace RestApi.SignalR;

public enum ProcessTypes
{
    Deposit = 0,
    Withdrawal,
    WithdrawalUsingCheque
}
public class SignalRHub : Hub
{
    IUsersService _usersService;

    public SignalRHub(IUsersService usersService)
    {
        _usersService = usersService;
    }


    public void RegisterUsername(string username)
    {
        if (_usersService.Users.ContainsKey(username))
        {
            _usersService.Users.Remove(username);
        }
        _usersService.Users.Add(username, GetConnectionId());
    }

    public async Task SendDeviceLogEvent(string userId, object deviceLog)
    {
        string conId = _usersService.Users[userId];
        await Clients.Client(conId).SendAsync("SendDeviceLogEvent", new object());
    }
    public string GetConnectionId()
    {
        return Context.ConnectionId;
    }
}

public interface IUsersService
{
    Dictionary<string, string> Users { get; set; }
}
public class UsersService : IUsersService
{
    public Dictionary<string, string> Users { get; set; }

    public UsersService()
    {
        Users = new Dictionary<string, string>();
    }


}
