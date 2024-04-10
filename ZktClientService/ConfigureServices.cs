using Microsoft.Extensions.DependencyInjection.Extensions;
using ZktClientService.Interfaces;
using ZktClientService.Services;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddZktecoIntegrationServices(this IServiceCollection services)
    {
        services.TryAddSingleton<IServerServices, ServerServices>();
      //  services.TryAddSingleton<IZktServices, ZktServices>();

        return services;
    }
}
