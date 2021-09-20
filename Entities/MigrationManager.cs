using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    public static class MigrationManager
    {
        public static IHost MigrateDatabase(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                using (var appContext = scope.ServiceProvider.GetRequiredService<ConicErpContext>())
                {
                    try
                    {
                        appContext.Database.Migrate();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                       // appContext.Database.EnsureDeleted();
                        //appContext.Database.EnsureDeleted();
                        //Log errors or do anything you think it's needed
                        throw;
                    }
                }
            }

            return host;
        }
    }
}
