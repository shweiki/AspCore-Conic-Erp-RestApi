using Application.Common.Interfaces;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ZktecoIntegration.Services;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddZktecoIntegrationServices(this IServiceCollection services)
    {
        services.TryAddSingleton<IZktServices, ZktServices>();

        return services;
    }
}
