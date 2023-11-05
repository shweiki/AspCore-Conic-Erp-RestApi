using Microsoft.AspNetCore.SignalR;
using ZktClientService.Hubs;
using ZktClientService.Interfaces;
using ZktClientService.Models;

namespace ZktClientService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IZktServices _zktServices;
        private readonly IConfiguration _configuration;
        private readonly IHubContext<ConicDeviceHub> _conicDeviceHub;

        public Worker(ILogger<Worker> logger, IZktServices zktServices, IConfiguration configuration, IHubContext<ConicDeviceHub> conicDeviceHub)
        {
            _logger = logger;
            _zktServices = zktServices;
            _configuration = configuration;
            _conicDeviceHub = conicDeviceHub;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var deviceIsConnect = _zktServices.IsConnect();
                if (!deviceIsConnect)
                {
                    var devices = new List<DeviceInfo>();
                    _configuration.Bind("Device", devices);
                    foreach (var device in devices.Where(x => x.AutoConnect == true).ToList())
                    {
                        _zktServices.ConnectByIp(device.IP, device.Port);
                    }
                }
                //  _logger.LogInformation("Device Connection is : {deviceIsConnect}  ,  at : {time}", deviceIsConnect, DateTimeOffset.Now);

                await _conicDeviceHub.Clients.All.SendAsync("DeviceState", new { deviceIsConnect, msg = $"The device is " + (deviceIsConnect ? "connected" : "not connected") + "" });
           
                int checkDeviceByMinute = _configuration.GetValue<int>("CheckDeviceByMinute");

                await Task.Delay(checkDeviceByMinute * 10000, stoppingToken);
            }
        }
    }
}