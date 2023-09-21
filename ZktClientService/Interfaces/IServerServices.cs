using ZktClientService.Models;

namespace ZktClientService.Interfaces
{
    public interface IServerServices
    {
        public object? PostLogToServer(AttTransactionLog attTransactionLog);
        public List<UserInfo> GetUsers(string accessToken);
    }
}
