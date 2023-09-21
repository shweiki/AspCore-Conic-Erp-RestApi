using ZktClientService.Interfaces;
using ZktClientService.Models;

namespace ZktClientService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IZktServices _zktServices;
        private readonly IConfiguration _configuration;

        public Worker(ILogger<Worker> logger, IZktServices zktServices, IConfiguration configuration)
        {
            _logger = logger;
            _zktServices = zktServices;
            _configuration = configuration;
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
                _logger.LogInformation("Device Connection is : {deviceIsConnect}  ,  at : {time}", deviceIsConnect, DateTimeOffset.Now);

                await Task.Delay(60000, stoppingToken);
            }
        }
    }
}