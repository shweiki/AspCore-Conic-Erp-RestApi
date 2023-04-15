using Application.Interfaces;
using Domain;
using Infrastructure.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.IO;

namespace RestApi;

public class Startup
{
    public IConfiguration Configuration { get; }
    public string ImagesPath { get; set; } = "C:\\ConicImages";

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
        ImagesPath = configuration.GetValue<string>("ImagesPath") ?? "C:\\ConicImages";
        if (!Directory.Exists(ImagesPath))
        {
            Directory.CreateDirectory(ImagesPath);
        }

    }


    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddMemoryCache();
        services.Configure<FormOptions>(o =>
        {
            o.ValueLengthLimit = int.MaxValue;
            o.MultipartBodyLengthLimit = int.MaxValue;
            o.MemoryBufferThreshold = int.MaxValue;
        });

        string ConnectionString = Configuration.GetConnectionString("Default");
        services.AddDbContext<ConicErpContext>(options =>
        {
            options.UseSqlServer(ConnectionString, sqlServerOptionsAction: options =>
            {
                options.MigrationsAssembly("RestApi");
            });
         //   options.EnableSensitiveDataLogging(true);
        });
        // services.AddScoped<ConicErpContext>(provider => provider.GetRequiredService<ConicErpContext>());
        services.AddScoped<ConicErpContext>();

        services.AddTransient<IUnitOfWork, UnitOfWork>();
        services.AddTransient<IEmailService, EmailService>();
        services.AddDatabaseDeveloperPageExceptionFilter();
        services.Configure<IdentityOptions>(options =>
        {
            // Default Password settings.
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequiredLength = 6;
            options.Password.RequiredUniqueChars = 0;
        });
        services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ConicErpContext>()
            .AddRoleManager<RoleManager<IdentityRole>>()
            .AddUserManager<UserManager<IdentityUser>>();
      //  services.AddAuthentication("esvlogin")
      //.AddCookie("esvlogin", act =>
      //{
      //    act.LoginPath = "/home/login";
      //    act.AccessDeniedPath = "/home/login";
      //    act.SlidingExpiration = true;
      //});
        services.AddCors();
        services.AddHttpContextAccessor();
        services.AddControllers().AddNewtonsoftJson(options =>
         {
             options.SerializerSettings.ContractResolver = new DefaultContractResolver();
             //   options.SerializerSettings.DateFormatHandling = DateFormatHandling.MicrosoftDateFormat;
             options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Local;
             options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

         });

    }
    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseMigrationsEndPoint();
        }
        if (!env.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }
        app.UseHttpsRedirection();
        app.UseDefaultFiles();
        app.UseStaticFiles();
        app.UseStaticFiles(new StaticFileOptions()
        {
            FileProvider = new PhysicalFileProvider(
            Path.GetFullPath(ImagesPath)),
            RequestPath = new PathString("/images"),
            DefaultContentType = "application/octet-stream"
        });
        app.UseRouting();
        app.UseCors(x => x
            .AllowAnyMethod()
            .AllowAnyHeader()
            .SetIsOriginAllowed(origin => true) // allow any origin
            .AllowCredentials()
            ); // allow credentials
        app.UseCookiePolicy();
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapDefaultControllerRoute();

        });
    }
}