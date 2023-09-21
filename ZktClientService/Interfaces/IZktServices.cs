using ZktClientService.Models;

namespace ZktClientService.Interfaces;

public interface IZktServices
{
    public bool IsConnect();
    (bool, string) ConnectByIp(string IPAdd, int Port);
    (bool, string) DisconnectByIp(string IPAdd, int Port);
    bool EnrollUser(string IP, int Port, string UserId);
    bool PutUser(string IP, int Port, string UserId, string Name);
    bool EnableUser(string IP, int Port, string UserId, string Name, bool Enable);
    bool ClearLog(string IP, int Port);
    bool ClearAdministrators(string IP, int Port);
    bool Restart(string IP, int Port);
    bool TurnOff(string IP, int Port);
    IList<ZktLogRecord>? GetLogData(string IP, int Port);
}
