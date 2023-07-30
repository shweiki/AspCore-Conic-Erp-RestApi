using Hangfire.Dashboard;

public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context)
    {
        // Retrieve the HttpContext from the DashboardContext
        var httpContext = context.GetHttpContext();


        // Check if the user is authenticated and in the required role (e.g., "SystemAdmin")
        //  return httpContext.User.Identity.IsAuthenticated && httpContext.User.IsInRole(SystemAdmin.Name);
        return true;
    }
}
