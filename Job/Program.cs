using Hangfire;
using Jobs;

var builder = WebApplication.CreateBuilder(args);

// Add logging
var logger = builder.CreateLogger(builder.Configuration);

builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddIdentityServices();
builder.Services.AddApplicationServices();

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddHostedService<RecurringJobService>((services => new RecurringJobService(services.GetService<IRecurringJobManager>(), logger)));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseHangfireDashboard();
app.UseHangfireDashboard("/hangfire", new
DashboardOptions
{
    Authorization = new[] { new HangfireAuthorizationFilter() }
});
app.UseEndpoints(endpoints =>
{
    endpoints.MapRazorPages();
    endpoints.MapHangfireDashboard();

});

app.Run();