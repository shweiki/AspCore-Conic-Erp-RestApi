using ZktClientService.Interfaces;

namespace ZktClientService.Models
{
    public class DeviceInfo
    {

        public string Ip { get; set; }
        public string MAC { get; set; }
        public int Port { get; set; }
        public string Type { get; set; }
        public bool AutoConnect { get; set; }
        public IZktServices ZktServices { get; set; }


    }
}
