using Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;


namespace AspCore_Conic_Erp_RestApi
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var psi = new System.Diagnostics.ProcessStartInfo();
            psi.UseShellExecute = true;
            psi.FileName = "chrome.exe";
            psi.Arguments = "--app=https://localhost/";

            System.Diagnostics.Process.Start(psi);
            CreateHostBuilder(args).Build().MigrateDatabase().Run();

        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();

                });
    }
}
