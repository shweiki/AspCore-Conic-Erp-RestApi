﻿using Microsoft.AspNetCore.SignalR;
using System.Text.Json;
using ZktClientService.Interfaces;
using ZktClientService.Models;

namespace ZktClientService.Hubs
{
    public class ConicDeviceHub : Hub
    {
        private readonly IZktServices _zktServices;
        private readonly IServerServices _serverServices;
        private readonly IConfiguration _configuration;

        public ConicDeviceHub(IZktServices zktServices, IServerServices serverServices, IConfiguration configuration)
        {
            _zktServices = zktServices;
            _serverServices = serverServices;
            _configuration = configuration;

        }
        public async Task SendMessage(string message)
        {

            await Clients.Caller.SendAsync("SendMessage", message);

        }
        public async Task SendEventLog(object log)
        {
            await Clients.Caller.SendAsync("SendEventLog", log);
        } 
        public async Task DeviceState(object msg)
        {
            await Clients.Caller.SendAsync("DeviceState", msg);
        }

        public async Task EnrollUser(string iP, string objectId, string name)
        {
            try
            {
                var devices = new List<DeviceInfo>();
                _configuration.Bind("Device", devices);
                var device = devices.Where(x => x.IP == iP).SingleOrDefault();
                if (device is null)
                {
                    await Clients.Caller.SendAsync("SendMessage", "The device is not register");
                    return;
                }

                var deviceIsConnect = _zktServices.IsConnect();

                if (!deviceIsConnect)
                {
                    (deviceIsConnect, string result) = _zktServices.ConnectByIp(device.IP, device.Port);
                    if (!deviceIsConnect)
                        await Clients.Caller.SendAsync("SendMessage", $"The device is not connect reason : {result}");

                }
                await Clients.Caller.SendAsync("SendMessage", $"Enroll User return {_zktServices.PutUser(device.IP, device.Port, objectId, name)} ");
                return;
            }
            catch (Exception ex)
            {
                await Clients.Caller.SendAsync("SendMessage", $"Catch Message : {ex.Message}  StackTrace : {ex.StackTrace}");
                return;
            }

        }
        public async Task EnrollServerUsers(string iP, string accessToken)
        {
            try
            {

                var devices = new List<DeviceInfo>();
                _configuration.Bind("Device", devices);
                var device = devices.Where(x => x.IP == iP).SingleOrDefault();
                if (device is null)
                {
                    await Clients.Caller.SendAsync("SendMessage", "The device is not register");
                    return;
                }

                var deviceIsConnect = _zktServices.IsConnect();

                if (!deviceIsConnect)
                {
                    (deviceIsConnect, string result) = _zktServices.ConnectByIp(device.IP, device.Port);
                    if (!deviceIsConnect)
                        await Clients.Caller.SendAsync("SendMessage", $"The device is not connect reason : {result}");

                }
                var users = _serverServices.GetUsers(accessToken);
                foreach (var user in users)
                {
                    _zktServices.PutUser(device.IP, device.Port, user.Id, user.Name);
                }
                await Clients.Caller.SendAsync("SendMessage", $"Enroll Server Users is Done ");

                return;
            }
            catch (Exception ex)
            {
                await Clients.Caller.SendAsync("SendMessage", $"Catch Message : {ex.Message}  StackTrace : {ex.StackTrace}");
                return;
            }

        }
        public async Task ConnectDevice(string iP)
        {
            try
            {
                var devices = new List<DeviceInfo>();
                _configuration.Bind("Device", devices);
                var device = devices.Where(x => x.IP == iP).SingleOrDefault();
                if (device is null)
                {
                    await Clients.Caller.SendAsync("SendMessage", "The device is not register");
                    return;
                }

                var deviceIsConnect = _zktServices.IsConnect();

                if (!deviceIsConnect)
                {
                    (deviceIsConnect, string result) = _zktServices.ConnectByIp(device.IP, device.Port);

                    await Clients.Caller.SendAsync("SendMessage", $"The device is connected {deviceIsConnect} reason : {result}");

                }
                await Clients.Caller.SendAsync("SendMessage", $"The device is connected {deviceIsConnect}");

                return;
            }
            catch (Exception ex)
            {
                await Clients.Caller.SendAsync("SendMessage", $"Catch Message : {ex.Message}  StackTrace : {ex.StackTrace}");
                return;
            }

        }
        public async Task ClearLog(string iP)
        {
            try
            {
                var devices = new List<DeviceInfo>();
                _configuration.Bind("Device", devices);
                var device = devices.Where(x => x.IP == iP).SingleOrDefault();
                if (device is null)
                {
                    await Clients.Caller.SendAsync("SendMessage", "The device is not register");
                    return;
                }

                var deviceIsConnect = _zktServices.IsConnect();

                if (!deviceIsConnect)
                {
                    (deviceIsConnect, string result) = _zktServices.ConnectByIp(device.IP, device.Port);
                    if (!deviceIsConnect)
                        await Clients.Caller.SendAsync("SendMessage", $"The device is not connect reason : {result}");

                }

                await Clients.Caller.SendAsync("SendMessage", $"Clear Log device return {_zktServices.ClearLog(device.IP, device.Port)} ");
                return;
            }
            catch (Exception ex)
            {
                await Clients.Caller.SendAsync("SendMessage", $"Catch Message : {ex.Message}  StackTrace : {ex.StackTrace}");
                return;
            }

        }
        public async Task GetLogData(string iP)
        {
            try
            {
                var devices = new List<DeviceInfo>();
                _configuration.Bind("Device", devices);
                var device = devices.Where(x => x.IP == iP).SingleOrDefault();
                if (device is null)
                {
                    await Clients.Caller.SendAsync("SendMessage", "The device is not register");
                    return;
                }

                var deviceIsConnect = _zktServices.IsConnect();

                if (!deviceIsConnect)
                {
                    (deviceIsConnect, string result) = _zktServices.ConnectByIp(device.IP, device.Port);
                    if (!deviceIsConnect)
                        await Clients.Caller.SendAsync("SendMessage", $"The device is not connect reason : {result}");

                }
                IList<ZktLogRecord> zktLogRecord = _zktServices.GetLogData(device.IP, device.Port);
                if (zktLogRecord is not null && zktLogRecord.Count > 0)
                {
                    foreach (var record in zktLogRecord)
                    {
                        var log = _serverServices.PostLogToServer(new AttTransactionLog
                        {
                            Datetime = record.DateTime,
                            Id = record.IndRegID.ToString(),
                            Ip = device.IP
                        });
                    }
                }
                await Clients.Caller.SendAsync("SendMessage", $"Get Log Data return {JsonSerializer.Serialize<IList<ZktLogRecord>>(zktLogRecord)} ");
                return;
            }
            catch (Exception ex)
            {
                await Clients.Caller.SendAsync("SendMessage", $"Catch Message : {ex.Message}  StackTrace : {ex.StackTrace}");
                return;
            }

        }
        public async Task ClearAdministrators(string iP)
        {
            try
            {
                var devices = new List<DeviceInfo>();
                _configuration.Bind("Device", devices);
                var device = devices.Where(x => x.IP == iP).SingleOrDefault();
                if (device is null)
                {
                    await Clients.Caller.SendAsync("SendMessage", "The device is not register");
                    return;
                }

                var deviceIsConnect = _zktServices.IsConnect();

                if (!deviceIsConnect)
                {
                    (deviceIsConnect, string result) = _zktServices.ConnectByIp(device.IP, device.Port);
                    if (!deviceIsConnect)
                        await Clients.Caller.SendAsync("SendMessage", $"The device is not connect reason : {result}");

                }

                await Clients.Caller.SendAsync("SendMessage", $"Clear Administrators return {_zktServices.ClearAdministrators(device.IP, device.Port)} ");
                return;
            }
            catch (Exception ex)
            {
                await Clients.Caller.SendAsync("SendMessage", $"Catch Message : {ex.Message}  StackTrace : {ex.StackTrace}");
                return;
            }

        }
        public async Task RestartDevice(string iP)
        {
            try
            {
                var devices = new List<DeviceInfo>();
                _configuration.Bind("Device", devices);
                var device = devices.Where(x => x.IP == iP).SingleOrDefault();
                if (device is null)
                {
                    await Clients.Caller.SendAsync("SendMessage", "The device is not register");
                    return;
                }

                var deviceIsConnect = _zktServices.IsConnect();

                if (!deviceIsConnect)
                {
                    (deviceIsConnect, string result) = _zktServices.ConnectByIp(device.IP, device.Port);
                    if (!deviceIsConnect)
                        await Clients.Caller.SendAsync("SendMessage", $"The device is not connect reason : {result}");

                }

                await Clients.Caller.SendAsync("SendMessage", $"Restart device return {_zktServices.ClearAdministrators(device.IP, device.Port)} ");
                return;
            }
            catch (Exception ex)
            {
                await Clients.Caller.SendAsync("SendMessage", $"Catch Message : {ex.Message}  StackTrace : {ex.StackTrace}");
                return;
            }

        }
        public async Task TurnOffDevice(string iP)
        {
            try
            {
                var devices = new List<DeviceInfo>();
                _configuration.Bind("Device", devices);
                var device = devices.Where(x => x.IP == iP).SingleOrDefault();
                if (device is null)
                {
                    await Clients.Caller.SendAsync("SendMessage", "The device is not register");
                    return;
                }

                var deviceIsConnect = _zktServices.IsConnect();

                if (!deviceIsConnect)
                {
                    (deviceIsConnect, string result) = _zktServices.ConnectByIp(device.IP, device.Port);
                    if (!deviceIsConnect)
                        await Clients.Caller.SendAsync("SendMessage", $"The device is not connect reason : {result}");

                }

                await Clients.Caller.SendAsync("SendMessage", $"Turn Off device return {_zktServices.TurnOff(device.IP, device.Port)} ");
                return;
            }
            catch (Exception ex)
            {
                await Clients.Caller.SendAsync("SendMessage", $"Catch Message : {ex.Message}  StackTrace : {ex.StackTrace}");
                return;
            }

        }
    }
}