using Microsoft.AspNetCore.Authorization;

namespace RestApi.Filters;

public class CustomAuthorizationHandler : IAuthorizationHandler
{
    private readonly ILogger<CustomAuthorizationHandler> _logger;

    public CustomAuthorizationHandler(ILogger<CustomAuthorizationHandler> logger)
    {
        _logger = logger;
    }

    public Task HandleAsync(AuthorizationHandlerContext context)
    {
        foreach (var requirement in context.PendingRequirements)
        {
            _logger.LogInformation($"Authorization requirement '{requirement.GetType().Name}' evaluated.");

            // Log any additional information about the requirement or decision
            // ...
        }

        if (context.HasSucceeded)
        {
            _logger.LogInformation("Authorization successful.");
        }
        else
        {
            _logger.LogWarning("Authorization denied.");

            // Log the reasons for denial
            foreach (var failure in context.FailureReasons)
            {
                _logger.LogInformation($"Authorization failure: {failure}");
            }
        }

        return Task.CompletedTask;
    }
}