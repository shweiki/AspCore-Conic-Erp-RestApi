using Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace AspCore_Conic_Erp_RestApi
{
    public class Program
    {

        public static void Main(string[] args)
        {
    /*        RegistryKey rk = Registry.CurrentUser.OpenSubKey(@"Control Panel\International", true);
            rk.SetValue("sTimeFormat", "HH:mm:ss"); // HH for 24hrs, hh for 12 hrs
     
            RegistryKey rk = Registry.CurrentUser.OpenSubKey(@"Control Panel\International", true);
            rk.SetValue("sTimeFormat", "dd-MM-yyyy");*/
           OpenBrowser();
          CreateHostBuilder(args).Build().MigrateDatabase().Run();
         //  CreateHostBuilder(args).Build().Run();

         
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                }).UseWindowsService();
        public static void OpenBrowser()
        {


            string rootUrl = "http://localhost:5000";
                //_httpContextAccessor.HttpContext.Request.Scheme.ToString();// host;
            ProcessStartInfo psi = new ProcessStartInfo("chrome", "--app=\"" + rootUrl + "\"") { UseShellExecute = true };
            try
            {
                Process.Start(psi);
            }
            catch
            {
                try
                {
                    psi.FileName = "firefox";
                    psi.Arguments = rootUrl;
                    Process.Start(psi);
                }
                catch
                {
                    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    {
                        psi.FileName = "edge";
                    }
                    else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                    {
                        psi.FileName = "safari";
                    }
                    try
                    {
                        Process.Start(psi);
                    }
                    catch
                    {
                    }
                }
            }
        }
 

}
}
