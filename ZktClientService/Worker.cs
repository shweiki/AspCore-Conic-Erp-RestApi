using Microsoft.AspNetCore.SignalR;
using ZktClientService.Hubs;
using ZktClientService.Interfaces;

namespace ZktClientService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IZktServices _zktServices;
        private readonly IServerServices _serverServices;
        private readonly IConfiguration _configuration;
        private readonly IHubContext<ConicDeviceHub> _conicDeviceHub;

        public Worker(ILogger<Worker> logger, IServerServices serverServices, IZktServices zktServices, IConfiguration configuration, IHubContext<ConicDeviceHub> conicDeviceHub)
        {
            _logger = logger;
            _zktServices = zktServices;
            _serverServices = serverServices;
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

                    foreach (var device in _serverServices.Devices.Where(x => x.AutoConnect == true).ToList())
                    {
                        (bool isDone, string status) = _zktServices.ConnectByIp(device.Ip, device.Port);
                        _logger.LogInformation("Device Connection is {status}: {deviceIsConnect}  ,  at : {time}", status, deviceIsConnect, DateTimeOffset.Now);

                    }
                }
                 
                await _conicDeviceHub.Clients.All.SendAsync("DeviceState", new { deviceIsConnect, msg = $"The device is " + (deviceIsConnect ? "connected" : "not connected") + "" });

                int checkDeviceByMinute = _configuration.GetValue<int>("CheckDeviceByMinute");

                await Task.Delay(checkDeviceByMinute * 10000, stoppingToken);
            }
        }
    }
}