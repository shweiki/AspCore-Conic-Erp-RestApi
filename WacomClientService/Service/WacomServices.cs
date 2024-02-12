using System.Diagnostics;
using System.Management;

namespace WacomClientService.Service
{
    public class WacomServices
    {
        public static bool IsUsbDeviceConnected()
        {
            using (var searcher = new ManagementObjectSearcher(@"Select * From Win32_USBHub"))
            {
                using (var collection = searcher.Get())
                {
                    foreach (var device in collection)
                    {
                        var usbDevice = Convert.ToString(device);

                        Debug.WriteLine(usbDevice);
                        // return true;
                    }
                }
            }
            return true;
        }
        public bool IsUsbDeviceConnected(string pid, string vid)
        {
            using (var searcher = new ManagementObjectSearcher(@"Select * From Win32_USBControllerDevice"))
            {
                using (var collection = searcher.Get())
                {
                    foreach (var device in collection)
                    {
                        var usbDevice = Convert.ToString(device);

                        if (usbDevice.Contains(pid) && usbDevice.Contains(vid))
                            return true;
                    }
                }
            }
            return false;
        }
    }
}
