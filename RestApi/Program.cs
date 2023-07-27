using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;

  string ImagesPath = "C:\\ConicImages";

var builder = WebApplication.CreateBuilder(args);
ImagesPath = builder.Configuration.GetValue<string>("ImagesPath") ?? "C:\\ConicImages";
if (!Directory.Exists(ImagesPath))
{
    Directory.CreateDirectory(ImagesPath);
}
// Add logging
var logger = builder.CreateLogger(builder.Configuration);

// Add services to the container.
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddIdentityServices();
builder.Services.AddApplicationServices();
// Add services to the container.

builder.Services.AddApiServices();
builder.Services.AddJWTAuthenticationServices(builder.Configuration);

builder.Services.AddMemoryCache();


builder.Services.AddControllersWithViews();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// TODO: Move this code to a better place
try
{
    // var provider = builder.Services.BuildServiceProvider();

    //  var mediator = provider.GetService<ISender>()!;
    //  await mediator.Send(new AddDefaultRolesCommand());
    //  await mediator.Send(new AddDefaultSystemConfigurationCommand());
}
catch (Exception ex)
{
    logger.Error(ex, "Error adding default roles.");
}
var app = builder.Build();
// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI(c =>
//    {
//        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API v1");
//        c.DocExpansion(DocExpansion.List);
//    });
//}
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API v1");
    c.DocExpansion(DocExpansion.List);
});
app.UseForwardedHeaders();

//app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true) // allow any origin
    .AllowCredentials()
    ); // allow credentials
app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles(new StaticFileOptions()
{
    FileProvider = new PhysicalFileProvider(
    Path.GetFullPath(ImagesPath)),
    RequestPath = new PathString("/images"),
    DefaultContentType = "application/octet-stream"
});
app.MapControllers();

// Run app
try
{
    app.Run();
}
finally
{
    logger.Dispose();
}
