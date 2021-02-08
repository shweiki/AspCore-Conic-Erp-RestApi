using Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace AspCore_Conic_Erp_RestApi
{
    public class Program
    {

        public static void Main(string[] args)
        {
            OpenBrowser();
            CreateHostBuilder(args).Build().MigrateDatabase().Run();
           // CreateHostBuilder(args).Build().Run();

         
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                                   
                    webBuilder.UseStartup<Startup>();
                });
        public static void OpenBrowser()
        {


            string rootUrl = "http://localhost";
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
