using Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;

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



            string Con = "Server=" + Environment.MachineName + "//SQLEXPRESS;Database=Conic_Erp;Trusted_Connection=True;MultipleActiveResultSets=true";
         //   string Con = "Server=(localdb)\\mssqllocaldb;Database=Conic_Erp;Trusted_Connection=True;MultipleActiveResultSets=true";
            services.AddDbContext<ConicErpContext>(options =>
                options.UseSqlServer(
                 //   Configuration.GetConnectionString("DefaultConnection"),
                 Con,
                         options => options.MigrationsAssembly("AspCore-Conic-Erp-RestApi")));
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
               // options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Local; 
                options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Local; // this should be set if you always expect UTC dates in method bodies, if not, you can use RoundTrip instead.
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
