using System.Windows;
using WacomClientService.Service;

namespace WacomClientService
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IHost _host;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Create and configure the host for the Worker Service
            var configuration = new ConfigurationBuilder().AddEnvironmentVariables()
            //.AddCommandLine()
            .AddJsonFile("appsettings.json")
            .Build();

            _host = Host.CreateDefaultBuilder().
             ConfigureWebHostDefaults(webBuilder =>
             {
                 webBuilder.UseConfiguration(configuration);
                 webBuilder.UseStartup<Startup>();
                 webBuilder.UseUrls(configuration.GetValue<string>("UseUrls") ?? "http://localhost:9995/");
             }).Build();

            WacomServices.IsUsbDeviceConnected();

            // Start the host
            _host.Start();

        }


        protected override void OnExit(ExitEventArgs e)
        {
            // Stop the host when the application exits
            _host?.StopAsync(TimeSpan.FromSeconds(5));
            _host?.Dispose();

            base.OnExit(e);
        }


    }

}
