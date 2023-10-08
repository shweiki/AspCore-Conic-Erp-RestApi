using Application.Common.Interfaces;
using Hangfire;
using Infrastructure.Common.Services;
using Infrastructure.Identity;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;


namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        if (configuration.GetValue("UseInMemoryDatabase", false))
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase("CleanArchitectureDb"));
        }
        else
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseLazyLoadingProxies().UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                    builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
        }

        services.TryAddTransient<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

        services.TryAddScoped<ICurrentUserService, CurrentUserService>();
        services.TryAddScoped<ISMSService, SMSService>();
        services.TryAddScoped<IEmailService, EmailService>();

        services.AddHangfire(x => x.UseSqlServerStorage(configuration.GetConnectionString("HangfireConnection")));
        services.AddHangfireServer();
        services.AddSignalR();

        return services;
    }

    public static IServiceCollection AddInfrastructureServicesForApi(this IServiceCollection services)
    {
        services.AddIdentityServices();

        services.ConfigureApplicationCookie(options =>
        {
            options.Cookie.HttpOnly = true;
            options.ExpireTimeSpan = TimeSpan.FromMinutes(10);
            options.LoginPath = "/Account/Login";
            options.LogoutPath = "/Account/Login";
            options.AccessDeniedPath = "/Account/AccessDenied";
            options.SlidingExpiration = true;
        });

        return services;
    }
    public static IServiceCollection AddIdentityServices(this IServiceCollection services)
    {

        // For Api and Jobs

        // TODO: Add Identity Services for API: (without authentication)
        services.AddIdentity<ApplicationUser, IdentityRole>()
         .AddEntityFrameworkStores<ApplicationDbContext>()
         .AddDefaultTokenProviders();
        services.AddHttpContextAccessor();

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
        // Identity services
        //   services.TryAddScoped<IUserValidator<ApplicationUser>, UserValidator<ApplicationUser>>();
        //     services.TryAddScoped<IPasswordValidator<ApplicationUser>, PasswordValidator<ApplicationUser>>();
        //      services.TryAddScoped<IPasswordHasher<ApplicationUser>, PasswordHasher<ApplicationUser>>();
        //      services.TryAddScoped<ILookupNormalizer, UpperInvariantLookupNormalizer>();
        //     services.TryAddScoped<IRoleValidator<IdentityRole>, RoleValidator<IdentityRole>>();
        // No interface for the error describer so we can add errors without rev'ing the interface
        //        services.TryAddScoped<IdentityErrorDescriber>();
        //      services.TryAddScoped<ISecurityStampValidator, SecurityStampValidator<ApplicationUser>>();
        //      services.TryAddScoped<ITwoFactorSecurityStampValidator, TwoFactorSecurityStampValidator<ApplicationUser>>();
        //       services.TryAddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, UserClaimsPrincipalFactory<ApplicationUser, IdentityRole>>();
        //    services.TryAddScoped<UserManager<ApplicationUser>>();
        //         services.TryAddScoped<SignInManager<ApplicationUser>>();
        //         services.TryAddScoped<RoleManager<IdentityRole>>();
        //       services.TryAddScoped<IUserStore<ApplicationUser>, UserStore<ApplicationUser, IdentityRole, ApplicationDbContext>>();
        //       services.TryAddScoped<IRoleStore<IdentityRole>, RoleStore<IdentityRole, ApplicationDbContext>>();
        ///       services.TryAddScoped<IUserConfirmation<ApplicationUser>, DefaultUserConfirmation<ApplicationUser>>();
        //      services.TryAddScoped<ISystemClock, SystemClock>();

        services.AddScoped<IIdentityService, IdentityService>();
        services.AddScoped<IPasswordGenerator, PasswordGenerator>();
        services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, UserClaimsPrincipalFactory>();



        return services;
    }

    public static IApplicationBuilder UseCookiePolicyForWeb(this IApplicationBuilder app, bool secure)
    {
        var cookiePolicyOptions = new CookiePolicyOptions
        {
            MinimumSameSitePolicy = SameSiteMode.Strict,
            HttpOnly = AspNetCore.CookiePolicy.HttpOnlyPolicy.Always,
            Secure = secure ? CookieSecurePolicy.Always : CookieSecurePolicy.None,
        };

        app.UseCookiePolicy(cookiePolicyOptions);

        return app;
    }
}
