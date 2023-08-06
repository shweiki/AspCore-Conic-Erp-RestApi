using Microsoft.AspNetCore.Http;
using Application.Common.Interfaces;
using System.Security.Claims;

namespace Infrastructure.Common.Services;
#nullable disable

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    public string Username => _httpContextAccessor.HttpContext?.User.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
    
    //public string Username
    //{
    //    get
    //    {
    //        var currentUser = _httpContextAccessor.HttpContext?.User.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
    //        if (!string.IsNullOrWhiteSpace(currentUser.Split('\\')[1]))
    //        {
    //            currentUser = currentUser.Split('\\')[1];
    //        }
    //        return currentUser;
    //    }
    //}

}
