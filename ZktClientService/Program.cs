var configuration = new ConfigurationBuilder()
.AddEnvironmentVariables()
.AddCommandLine(args)
.AddJsonFile("appsettings.json")
.Build();

var host = Host.CreateDefaultBuilder(args).UseWindowsService(options =>
                {
                    options.ServiceName = "ZktClientService";
                }).ConfigureWebHostDefaults(webBuilder =>
               {
                   webBuilder.UseConfiguration(configuration);
                   webBuilder.UseStartup<Startup>();
                   webBuilder.UseUrls(configuration.GetValue<string>("UseUrls") ?? "http://localhost:9989/");
               }).Build();


await host.RunAsync();
