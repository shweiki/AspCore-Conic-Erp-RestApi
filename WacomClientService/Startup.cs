using Microsoft.Extensions.FileProviders;
using System.IO;
using WacomClientService;
using WacomClientService.Hubs;

public class Startup
{

    public IConfiguration _configuration { get; }
    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton(typeof(MainWindow));

        // services.AddTransient (typeof(MainWindow));
        services.AddHttpContextAccessor();

        services.AddSignalR();
        services.AddControllers();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        string ImagesPath = _configuration.GetValue<string>("ImagesPath") ?? "C:\\WacomClientService\\";

        if (!Directory.Exists(ImagesPath))
        {
            Directory.CreateDirectory(ImagesPath);
        }

        app.UseCors(x => x
            .AllowAnyMethod()
            .AllowAnyHeader()
            .SetIsOriginAllowed(origin => true) // allow any origin
            .AllowCredentials()
            ); // allow credentials
        app.UseRouting();
        app.UseStaticFiles();
        app.UseStaticFiles(new StaticFileOptions()
        {
            FileProvider = new PhysicalFileProvider(
            Path.GetFullPath(ImagesPath)),
            RequestPath = new PathString("/images"),
            DefaultContentType = "application/octet-stream"
        });
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapHub<ClientBrowserHub>("/hubs/clientbrowser");
        });
    }
}