using Application.Common.Models;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Common.Services;

internal class IdntityUserServices

{
    private readonly UserManager<ApplicationUser> _userManager;

    public IdntityUserServices(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }
    public async Task<UserDto> GetUserInfo(string UserId)
    {
        var userR = await _userManager.FindByIdAsync(UserId);
        return new UserDto
        {
            UserName = userR.UserName,
            Active = userR.Active,
            Email = userR.Email,
            FullName = userR.FullName,
            PhoneNumber = userR.PhoneNumber,
            Title = userR.Title
        };
    }
}
