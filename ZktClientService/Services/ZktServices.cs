using Microsoft.AspNetCore.SignalR;
using System.Text.Json;
using ZktClientService.Hubs;
using ZktClientService.Interfaces;
using ZktClientService.Models;

namespace ZktClientService.Services;

internal class ZktServices : IZktServices
{
    private readonly IServerServices _serverServices;
    private readonly IHubContext<ConicDeviceHub> _conicDeviceHub;
    private readonly ILogger<ZktServices> _logger;

    public static ZkemClient _service;
    private int machineNumber = 1;

    public ZktServices(IServerServices serverServices, IHubContext<ConicDeviceHub> conicDeviceHub, ILogger<ZktServices> logger)
    {
        _serverServices = serverServices;
        _conicDeviceHub = conicDeviceHub;
        _logger = logger;
        if (_service is null)
        {
            _service = new ZkemClient(RaiseDeviceEvent);
        }
    }
    public bool IsConnect()
    {
        return _service.isConnected;
    }
    public (bool, string) ConnectByIp(string IP, int Port)
    {
        bool isValidIpA = UniversalStatic.ValidateIP(IP);

        if (!isValidIpA)
            return (false, "The Device IP is invalid !!");

        isValidIpA = UniversalStatic.PingTheDevice(IP);

        if (!isValidIpA)
            return (false, $"The device at {IP} : {Port} did not respond!!");

        bool isDeviceConnected = _service.Connect_Net(IP, Port);

        //  _service.SetDeviceTime2(1, DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

        return (isDeviceConnected, $"Is Device Connected at {IP} : {Port}");
    }
    public (bool, string) DisconnectByIp(string IP, int Port)
    {
        bool isValidIpA = UniversalStatic.ValidateIP(IP);

        if (!isValidIpA)
            return (false, "The Device IP is invalid !!");

        isValidIpA = UniversalStatic.PingTheDevice(IP);

        if (!isValidIpA)
            return (false, $"The device at {IP} : {Port} did not respond!!");

        _service.Disconnect();

        return (true, $"Disconnect device at {IP} : {Port}");
    }
    public bool EnrollUser(string IP, int Port, string UserId)
    {
        string currentIP = "";
        _service.GetDeviceIP(1, ref currentIP);
        if (!_service.isConnected || !string.Equals(currentIP, IP))
        {
            (bool isDeviceConnected, string description) = ConnectByIp(IP, Port);
            if (!isDeviceConnected) return false;
        }
        int iFingerIndex = 111;
        int idwErrorCode = 0;
        bool startenroll_retult = false;
        _service.CancelOperation();
        //   objZkeeper.SSR_DeleteEnrollData((int)DeviceId,sUserID, 0);//If the specified index of user's templates has existed ,delete it first.(SSR_DelUserTmp is also available sometimes)
        //     objZkeeper.RefreshData(1);//the data in the device should be refreshed

        if (_service.StartEnrollEx(UserId, iFingerIndex, 0))
        {
            //  int iCanSaveTmp = 1;

            _service.StartIdentify();//After enrolling templates,you should let the device into the 1:N verification condition
            _service.RefreshData(1);//the data in the device should be refreshed
            startenroll_retult = true;

        }
        else
        {
            _service.GetLastError(ref idwErrorCode);
            startenroll_retult = false;
        }
        return startenroll_retult;
    }
    public bool PutUser(string IP, int Port, string UserId, string Name)
    {
        string currentIP = "";
        _service.GetDeviceIP(1, ref currentIP);
        if (!_service.isConnected || !string.Equals(currentIP, IP))
        {
            (bool isDeviceConnected, string description) = ConnectByIp(IP, Port);
            if (!isDeviceConnected) return false;
        }

        _service.EnableDevice(1, false);

        bool isDone = _service.SSR_SetUserInfo(1, UserId, Name, "", 0, true);
        if (isDone)
        {
            _service.RefreshData(1);
        }
        _service.EnableDevice(1, true);

        return isDone;
    }
    public bool EnableUser(string IP, int Port, string UserId, string Name, bool Enable)
    {
        string currentIP = "";
        _service.GetDeviceIP(1, ref currentIP);
        if (!_service.isConnected || !string.Equals(currentIP, IP))
        {
            (bool isDeviceConnected, string description) = ConnectByIp(IP, Port);
            if (!isDeviceConnected) return false;
        }

        _service.EnableDevice(1, false);

        bool isDone = _service.SSR_SetUserInfo(1, UserId, Name, "", 0, Enable);
        if (isDone)
        {
            _service.RefreshData(1);
        }
        _service.EnableDevice(1, true);

        return isDone;
    }
    public IList<ZktLogRecord>? GetLogData(string IP, int Port)
    {
        string currentIP = "";
        _service.GetDeviceIP(1, ref currentIP);
        if (!_service.isConnected || !string.Equals(currentIP, IP))
        {
            (bool isDeviceConnected, string description) = ConnectByIp(IP, Port);
            if (!isDeviceConnected) return null;
        }

        string dwEnrollNumber1 = "";
        int dwVerifyMode = 0;
        int dwInOutMode = 0;
        int dwYear = 0;
        int dwMonth = 0;
        int dwDay = 0;
        int dwHour = 0;
        int dwMinute = 0;
        int dwSecond = 0;
        int dwWorkCode = 0;

        IList<ZktLogRecord> lstEnrollData = new List<ZktLogRecord>();

        _service.ReadAllGLogData(machineNumber);

        while (_service.SSR_GetGeneralLogData(machineNumber, out dwEnrollNumber1, out dwVerifyMode, out dwInOutMode, out dwYear, out dwMonth, out dwDay, out dwHour, out dwMinute, out dwSecond, ref dwWorkCode))
        {
            string inputDate = new DateTime(dwYear, dwMonth, dwDay, dwHour, dwMinute, dwSecond).ToString();

            ZktLogRecord record = new ZktLogRecord();
            record.IndRegID = int.Parse(dwEnrollNumber1);
            record.DateTimeRecord = inputDate;

            lstEnrollData.Add(record);
        }

        return lstEnrollData;
    }
    public bool ClearLog(string IP, int Port)
    {
        bool ret = false;

        string currentIP = "";
        _service.GetDeviceIP(1, ref currentIP);
        if (!_service.isConnected || !string.Equals(currentIP, IP))
        {
            (bool isDeviceConnected, string description) = ConnectByIp(IP, Port);
            if (!isDeviceConnected) return false;
        }

        _service.EnableDevice(machineNumber, false);//disable the device

        if (_service.ClearGLog(machineNumber))
        {
            _service.RefreshData(machineNumber);
            ret = true;
        }
        _service.EnableDevice(machineNumber, true);//enable the device

        //  bool ClearKeeperData = _service.ClearKeeperData(machineNumber);
        //  bool ClearSLog = _service.ClearSLog(machineNumber);

        return ret;
    }
    public bool ClearAdministrators(string IP, int Port)
    {
        string currentIP = "";
        _service.GetDeviceIP(1, ref currentIP);
        if (!_service.isConnected || !string.Equals(currentIP, IP))
        {
            (bool isDeviceConnected, string description) = ConnectByIp(IP, Port);
            if (!isDeviceConnected) return false;
        }

        bool clearAdministrators = _service.ClearAdministrators(1);

        return clearAdministrators;
    }
    public bool Restart(string IP, int Port)
    {
        string currentIP = "";
        _service.GetDeviceIP(1, ref currentIP);
        if (!_service.isConnected || !string.Equals(currentIP, IP))
        {
            (bool isDeviceConnected, string description) = ConnectByIp(IP, Port);
            if (!isDeviceConnected) return false;
        }

        bool restartDevice = _service.RestartDevice(1);

        return restartDevice;
    }
    public bool TurnOff(string IP, int Port)
    {
        string currentIP = "";
        _service.GetDeviceIP(1, ref currentIP);
        if (!_service.isConnected || !string.Equals(currentIP, IP))
        {
            (bool isDeviceConnected, string description) = ConnectByIp(IP, Port);
            if (!isDeviceConnected) return false;
        }

        bool powerOffDevice = _service.PowerOffDevice(1);

        return powerOffDevice;
    }
    public void RaiseDeviceEvent(object sender, EventFlagEnum eventType)
    {
        switch (eventType)
        {
            case EventFlagEnum.Connect:
                {

                    break;
                }
            case EventFlagEnum.Disconnect:
                {

                    break;
                }
            case EventFlagEnum.OnAttTransactionExEvent:
                {

                    AttTransactionLog attTransactionLog = JsonSerializer.Deserialize<AttTransactionLog>(JsonSerializer.Serialize(sender));
                    _logger.LogInformation($"On Att Transaction Ex Event :{JsonSerializer.Serialize(attTransactionLog)}  ");

                    if (attTransactionLog is null) break;
                    var log = _serverServices.PostLogToServer(attTransactionLog);
                    if (log is null) break;
                    _conicDeviceHub.Clients.All.SendAsync("SendEventLog", log).Wait();
                    break;
                }

            default:
                break;
        }

    }

}
