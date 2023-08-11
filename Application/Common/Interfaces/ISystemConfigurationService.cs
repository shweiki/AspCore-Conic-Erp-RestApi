using Application.Features.SystemConfiguration.Queries.GetSystemConfiguration;

namespace Application.Common.Interfaces;

public interface ISystemConfigurationService
{
    public Task<SystemConfiguration?> GetSystemConfiguration();
    public Task<bool> RefrechSystemConfiguration();
}
