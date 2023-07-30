using Serilog;
using Serilog.Core;

namespace Jobs;

public static class ConfigureServices
{

    public static Logger CreateLogger(this WebApplicationBuilder builder, IConfiguration configuration)
    {

        Serilog.Debugging.SelfLog.Enable(msg => Console.WriteLine(msg));

        var logger = new LoggerConfiguration()
          .ReadFrom.Configuration(configuration)
          .Enrich.FromLogContext()
          .CreateLogger();

        builder.Logging.ClearProviders();
        builder.Logging.AddSerilog(logger);

        return logger;
    }
}
