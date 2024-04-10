using ZktClientService.Models;

namespace ZktClientService.Interfaces
{
    public interface IServerServices
    {
        public List<DeviceInfo> Devices { get; }
        public void RefreshDevices();
        public object? PostLogToServer(AttTransactionLog attTransactionLog);
        public List<UserInfo> GetUsers(string accessToken);
        public List<DeviceInfo> GetDevices(string accessToken);
    }
}
