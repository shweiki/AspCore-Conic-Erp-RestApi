using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using System.Security.Claims;

namespace Infrastructure.Identity;
public class UserClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser, IdentityRole>
{
    private IHttpContextAccessor _httpContext;
    private readonly ISender _mediator;

    public UserClaimsPrincipalFactory(
        IHttpContextAccessor httpContext,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        ISender sender,
        IOptions<IdentityOptions> optionsAccessor) : base(userManager, roleManager, optionsAccessor)
    {
        _httpContext = httpContext;
        _mediator = sender;

    }

    public async override Task<ClaimsPrincipal> CreateAsync(ApplicationUser user)
    {
        //   string realClientIP = GetHeaderValueAs("X-Forwarded-For") ?? "";

        //  string ip = _httpContext.HttpContext.Connection.RemoteIpAddress.ToString();

        string FromPath = _httpContext.HttpContext.Request.Query["FromPath"];
        var principal = await base.CreateAsync(user);

        ((ClaimsIdentity)principal.Identity).AddClaims(new[] {
           // new Claim(ClaimTypes.Sid,licenseInfo?.LicenseLevel.ToString() ?? ""),
            new Claim(ClaimTypes.GivenName, user.FullName),
            new Claim(ClaimTypes.Webpage,FromPath ?? ""),
        });

        return principal;
    }
    public T GetHeaderValueAs<T>(string headerName)
    {
        StringValues values;

        _httpContext.HttpContext.Request.Headers.TryGetValue(headerName, out values);
        if (!StringValues.IsNullOrEmpty(values))
        {
            var rawValues = values.ToString();

            if (!string.IsNullOrEmpty(rawValues))
                return (T)Convert.ChangeType(values.ToString(), typeof(T));
        }

        return default(T);
    }
}