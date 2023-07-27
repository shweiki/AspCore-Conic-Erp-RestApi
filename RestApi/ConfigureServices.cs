using Api.Filters;
using Api.Models;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Serilog;
using Serilog.Core;
using System;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        services.AddControllers().AddNewtonsoftJson(options =>
        {
            //  options.SerializerSettings.ContractResolver = new DefaultContractResolver();
            //   options.SerializerSettings.DateFormatHandling = DateFormatHandling.MicrosoftDateFormat;
            options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Local;
            options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

        });

        // Authorization Handler with Custom
        services.AddSingleton<IAuthorizationHandler, CustomAuthorizationHandler>();


        services.Configure<ForwardedHeadersOptions>(options => { options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto; });

        services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();


        return services;
    }

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
    public static IServiceCollection AddJWTAuthenticationServices(this IServiceCollection services, IConfiguration Configuration)
    {
        // Add Jwt Setings
        var bindJwtSettings = new JwtSettings();
        Configuration.Bind("JsonWebTokenKeys", bindJwtSettings);
        services.AddSingleton(bindJwtSettings);


        services.AddAuthentication(
        options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
        .AddJwtBearer(options =>
        {
            options.SaveToken = true;
            options.RequireHttpsMetadata = false;
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = bindJwtSettings.ValidIssuer,
                ValidAudience = bindJwtSettings.ValidAudience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(bindJwtSettings.IssuerSigningKey))
            };
        });
        return services;
    }
}
