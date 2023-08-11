using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Application.Common.Interfaces;
using Application.Features.SystemConfiguration.Queries.GetSystemConfiguration;

namespace Application.Services.SystemConfiguration;

public class SystemConfigurationService : ISystemConfigurationService
{
    private readonly ISender _mediator;
    private readonly ILogger<SystemConfigurationService> _logger;
    private IMemoryCache _cache;
    private static readonly SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);

    private Features.SystemConfiguration.Queries.GetSystemConfiguration.SystemConfiguration? SystemConfiguration { get; set; }

    public SystemConfigurationService(ISender mediator,
        ILogger<SystemConfigurationService> logger,
        IMemoryCache cache)
    {
        _mediator = mediator;
        _logger = logger;
        _cache = cache ?? throw new ArgumentNullException(nameof(cache));
    }

    public async Task<Features.SystemConfiguration.Queries.GetSystemConfiguration.SystemConfiguration?> GetSystemConfiguration()
    {
        if (SystemConfiguration is null)
        {
            await TryingFetchFromCache();

            if (SystemConfiguration is null)
            {
                _cache.Remove("SystemConfiguration");

                return null;
            }
        }
        return SystemConfiguration;
    }
    public async Task<bool> RefrechSystemConfiguration()
    {
        SystemConfiguration = null;
        _cache.Remove("SystemConfiguration");

        await TryingFetchFromCache();

        return true;
    }
    private async Task TryingFetchFromCache()
    {
        if (_cache.TryGetValue("SystemConfiguration", out Features.SystemConfiguration.Queries.GetSystemConfiguration.SystemConfiguration systemConfiguration))
        {
            if (systemConfiguration is null)
            {
                _cache.Remove("SystemConfiguration");
                await TryingFetchFromCache();
            }
            else
            {
                SystemConfiguration = systemConfiguration;
            }
            return;
        }
        else
        {
            try
            {
                await semaphore.WaitAsync();

                if (_cache.TryGetValue("SystemConfiguration", out systemConfiguration))
                {
                    SystemConfiguration = systemConfiguration;

                    return;
                }
                else
                {
                    await GetSystemConfigurationFromDataBase();
                    if (systemConfiguration is null)
                    {
                        _cache.Remove("SystemConfiguration");
                    }
                    else
                    {
                        var cacheEntryOptions = new MemoryCacheEntryOptions()
                            .SetSlidingExpiration(TimeSpan.FromMinutes(10))
                            .SetAbsoluteExpiration(TimeSpan.FromHours(2))
                            .SetPriority(CacheItemPriority.Normal);

                        _cache.Set("SystemConfiguration", SystemConfiguration, cacheEntryOptions);
                    }

                }
            }
            finally
            {
                semaphore.Release();
            }
        }
    }
    private async Task GetSystemConfigurationFromDataBase()
    {
        SystemConfiguration = null;
        var query = new GetSystemConfigurationQuery();
        var systemConfiguration = await _mediator.Send(query);
        if (systemConfiguration is null)
        {
            return;
        }
        try
        {
            SystemConfiguration = systemConfiguration;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error get System Configuration From DataBase : {SystemConfiguration}", SystemConfiguration);
        }
    }

}
