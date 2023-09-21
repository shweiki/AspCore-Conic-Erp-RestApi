using Microsoft.Extensions.DependencyInjection.Extensions;
using ZktClientService;
using ZktClientService.Hubs;
using ZktClientService.Interfaces;
using ZktClientService.Services;

public class Startup
{
    public IConfiguration _configuration { get; }
    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSignalR();

        services.AddHostedService<Worker>();
        services.TryAddSingleton<IZktServices, ZktServices>();
        services.TryAddSingleton<IServerServices, ServerServices>();
        services.AddControllers();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseCors(x => x
            .AllowAnyMethod()
            .AllowAnyHeader()
            .SetIsOriginAllowed(origin => true) // allow any origin
            .AllowCredentials()
            ); // allow credentials
        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapHub<ClientBrowserHub>("/hubs/clientbrowser");
            endpoints.MapControllers();
        });
    }
}
