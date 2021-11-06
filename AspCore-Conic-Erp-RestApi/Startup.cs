using Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using EmailService;
using Microsoft.AspNetCore.Http.Features;
using System;

namespace AspCore_Conic_Erp_RestApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            int lat = Environment.CurrentDirectory.LastIndexOf("\\") + 1;
            string Name = Environment.CurrentDirectory.Substring(lat, (Environment.CurrentDirectory.Length - lat));
            Name = Name.Replace("-", "").ToUpper();
            
            var emailConfig = Configuration.GetSection("EmailConfiguration:" + Name + "").Get<EmailConfiguration>();
           if(emailConfig ==null) emailConfig = Configuration.GetSection("EmailConfiguration:Default").Get<EmailConfiguration>();
            services.AddSingleton(emailConfig);
            services.AddScoped<IEmailSender, EmailSender>();
            services.Configure<FormOptions>(o => {
                o.ValueLengthLimit = int.MaxValue;
                o.MultipartBodyLengthLimit = int.MaxValue;
                o.MemoryBufferThreshold = int.MaxValue;
            });
            var ConnectionString = Configuration.GetConnectionString(Name);
            if (ConnectionString == null)
                ConnectionString = Configuration.GetConnectionString("Default");

            services.AddDbContext<ConicErpContext>(options =>
            {
                options.UseSqlServer(ConnectionString
                   , options => options.MigrationsAssembly("AspCore-Conic-Erp-RestApi")
                    );
            });
         
            services.AddTransient<IUnitOfWork, UnitOfWork>();
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
            services.AddAuthentication("esvlogin")
          .AddCookie("esvlogin", act => {
              act.LoginPath = "/home/login";
              act.AccessDeniedPath = "/home/login";
              act.SlidingExpiration = true;
          });
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
 
            app.UseHttpsRedirection();
            app.UseDefaultFiles();
            app.UseStaticFiles();
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
}