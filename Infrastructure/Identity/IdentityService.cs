using Application.Common.Interfaces;
using Application.Common.Models;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static Application.Common.Enums.RolesEnum;

namespace Infrastructure.Identity;

public class IdentityService : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public IdentityService(UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        SignInManager<ApplicationUser> signInManager,
        ISender sender,
        IMapper mapper)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _signInManager = signInManager;
        _sender = sender;
        _mapper = mapper;
    }

    public async Task<Result> CreateRoleAsync(string roleName)
    {

        var result = await _roleManager.CreateAsync(new IdentityRole(roleName));
        return result.ToApplicationResult();
    }

    public async Task<(Result Result, string UserId)> CreateUserAsync(string userName, string password, string fullName,
        string email, string phone, string title, string? country, string? state, bool isActive)
    {
        var user = new ApplicationUser
        {
            UserName = userName,
            Email = email,
            PhoneNumber = phone,
            Title = title,
            FullName = fullName,
            Active = isActive,
            Country = country,
            State = state,
            IsReadSignConsent = false,
        };

        var result = await _userManager.CreateAsync(user, password);

        return (result.ToApplicationResult(), user.Id);
    }

    public async Task<string> GeneratePasswordResetTokenByEmailAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        string token = await _userManager.GeneratePasswordResetTokenAsync(user);
        return token;
    }
    public async Task<string> GenerateOTPCodeForEmailAsync(string username)
    {
        var user = await _userManager.FindByNameAsync(username);
        var code = await _userManager.GenerateTwoFactorTokenAsync(user, TokenOptions.DefaultEmailProvider);
        return code;
    }
    public async Task<string> GenerateOTPCodeForPhoneAsync(string username)
    {
        var user = await _userManager.FindByNameAsync(username);
        var code = await _userManager.GenerateTwoFactorTokenAsync(user, TokenOptions.DefaultPhoneProvider);
        return code;
    }

    public async Task<bool> VerifyOTPPhoneWithCodeAsync(string username, string code)
    {
        var user = await _userManager.FindByNameAsync(username);
        var result = await _userManager.VerifyTwoFactorTokenAsync(user, TokenOptions.DefaultPhoneProvider, code);
        return result;
    }
    public async Task<bool> VerifyOTPEmailWithCodeAsync(string username, string code)
    {
        var user = await _userManager.FindByNameAsync(username);
        var result = await _userManager.VerifyTwoFactorTokenAsync(user, TokenOptions.DefaultEmailProvider, code);
        return result;
    }
    public async Task<Result> ResetPasswordWithTokenAsync(string email, string token, string newPassword)
    {
        var user = await _userManager.FindByEmailAsync(email);
        var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
        return result.ToApplicationResult();
    }
    public async Task<Result> ChangePasswordAsync(string userName, string currentPassword, string newPassword)
    {
        var user = await _userManager.FindByNameAsync(userName);
        var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
        return result.ToApplicationResult();
    }
    public async Task<string> GenerateUserTokenAsync(string username)
    {
        var user = await _userManager.FindByNameAsync(username);
        var token = await _userManager.GenerateUserTokenAsync(user, TokenOptions.DefaultProvider, username);
        return token;
    }
    public async Task<bool> VerifyUserTokenAsync(string username, string token)
    {
        var user = await _userManager.FindByNameAsync(username);
        var result = await _userManager.VerifyUserTokenAsync(user, TokenOptions.DefaultProvider, username, token);
        return result;
    }
    public async Task<bool> RemoveUserTokenAsync(string username, string token)
    {
        var user = await _userManager.FindByNameAsync(username);
        var result = await _userManager.RemoveAuthenticationTokenAsync(user, TokenOptions.DefaultProvider, token);
        return result.Succeeded;
    }
    public async Task<Result> UpdateUserAsync(string userName, string fullName,
        string email, string phone, string title, string? country, string? state, bool isActive)
    {
        var user = await _userManager.FindByNameAsync(userName);
        user.Email = email;
        user.PhoneNumber = phone;
        user.Title = title;
        user.Country = country;
        user.State = state;
        user.FullName = fullName;
        user.Active = isActive;

        var result = await _userManager.UpdateAsync(user);
        return result.ToApplicationResult();
    }

    public async Task<Result> UpdateUserStatusAsync(string userName, bool isActive)
    {
        ApplicationUser? user = await _userManager.FindByNameAsync(userName);
        if (user is null)
        {
            return Result.Failure(new List<string> { $"Couldn't find user {userName}." });
        }
        user.Active = isActive;
        var result = await _userManager.UpdateAsync(user);
        return (result.ToApplicationResult());
    }
    public async Task<Result> UpdateUserIsReadedConsentAsync(string userName, bool isReaded)
    {
        ApplicationUser? user = await _userManager.FindByNameAsync(userName);
        if (user is null)
        {
            return Result.Failure(new List<string> { $"Couldn't find user {userName}." });
        }
        user.IsReadSignConsent = isReaded;
        var result = await _userManager.UpdateAsync(user);
        return (result.ToApplicationResult());
    }
    public async Task<Result> UpdateUserPhoneAsync(string userName, string phone)
    {
        ApplicationUser? user = await _userManager.FindByNameAsync(userName);
        if (user is null)
        {
            return Result.Failure(new List<string> { $"Couldn't find user {userName}." });
        }
        user.PhoneNumber = phone;
        var result = await _userManager.UpdateAsync(user);
        return (result.ToApplicationResult());
    }

    public async Task<bool> RoleExistsAsync(string roleName)
    {
        return await _roleManager.RoleExistsAsync(roleName);
    }

    public async Task<bool> UserExistsByNameAsync(string username)
    {
        ApplicationUser? user = await _userManager.FindByNameAsync(username);
        return user != null;
    }

    public async Task<Result> AddUserToRoleAsync(string roleName, string username)
    {
        ApplicationUser? user = await _userManager.FindByNameAsync(username);
        if (user is not null)
        {
            var userWithRoles = await GetUserWithRolesAsync(username);

            if (userWithRoles is null)
            {
                return Result.Failure(new List<string> { $"Couldn't find user {username}." });
            }

            if (userWithRoles.Roles.Contains(roleName))
            {
                return Result.Success();
            }

            var result = await _userManager.AddToRoleAsync(user, roleName);
            return (result.ToApplicationResult());
        }
        return Result.Failure(new List<string> { $"Couldn't find user {username}." });
    }

    public async Task<Result> AddUserToRolesAsync(string[] roleNames, string username, bool removeOtherRoles = false)
    {
        ApplicationUser? user = await _userManager.FindByNameAsync(username);
        if (user is not null)
        {
            var userWithRoles = await GetUserWithRolesAsync(username);

            if (userWithRoles is null)
            {
                return Result.Failure(new List<string> { $"Couldn't find user {username}." });
            }

            var rolesToAdd = roleNames.Except(userWithRoles.Roles);

            var result = await _userManager.AddToRolesAsync(user, rolesToAdd);
            if (removeOtherRoles)
            {
                var rolesToRemove = userWithRoles.Roles.Except(roleNames);
                await _userManager.RemoveFromRolesAsync(user, rolesToRemove); // TODO: Add error handling
            }
            return (result.ToApplicationResult());
        }
        return Result.Failure(new List<string> { $"Couldn't find user {username}." });
    }

    public async Task<Result> PasswordSignInAsync(string username, string password)
    {
        var user = await _userManager.FindByNameAsync(username);
        if (user is null)
        {
            return Result.Failure(new string[] { "Wrong username or password, please try again." });
        }

        var validationResult = await SignInValidation(user!);

        if (!validationResult.Succeeded)
        {
            return validationResult;
        }

        var result = await _signInManager.PasswordSignInAsync(username, password, false, false);

        if (!result.Succeeded)
        {
            return Result.Failure(new string[] { "Wrong username or password, please try again." });
        }
        else
        {
            return result.ToApplicationResult();
        }
    }

    public async Task<Result> WindowsUserSignInAsync(string username)
    {
        var user = await _userManager.FindByNameAsync(username);
        if (user is null)
        {
            return Result.Failure(new string[] { "Wrong username or password, please try again." });
        }

        var validationResult = await SignInValidation(user!);

        if (!validationResult.Succeeded)
        {
            return validationResult;
        }

        await _signInManager.SignInAsync(user, false);

        return Result.Success();
    }
    public async Task<Result> PasswordSignInAdminAsync(string username, string password)
    {
        var user = await _userManager.FindByNameAsync(username);
        if (user is null)
        {
            return Result.Failure(new string[] { "Wrong username or password, please try again." });
        }

        var validationResult = await SignInAdminValidation(user!);

        if (!validationResult.Succeeded)
        {
            return validationResult;
        }

        var result = await _signInManager.PasswordSignInAsync(username, password, false, false);

        if (!result.Succeeded)
        {
            return Result.Failure(new string[] { "Wrong username or password, please try again." });
        }
        else
        {
            return result.ToApplicationResult();
        }
    }
    public async Task<Result> PasswordSignInUserAsync(string username, string password)
    {
        var user = await _userManager.FindByNameAsync(username);
        if (user is null)
        {
            return Result.Failure(new string[] { "Wrong username or password, please try again." });
        }

        var validationResult = await SignInUserValidation(user!);

        if (!validationResult.Succeeded)
        {
            return validationResult;
        }

        var result = await _signInManager.PasswordSignInAsync(username, password, false, false);

        if (!result.Succeeded)
        {
            return Result.Failure(new string[] { "Wrong username or password, please try again." });
        }
        else
        {
            return result.ToApplicationResult();

        }
    }
    public async Task<Result> PasswordSignInUserForApiAsync(string username, string password)
    {
        var user = await _userManager.FindByNameAsync(username);
        if (user is null)
        {
            return Result.Failure(new string[] { "Wrong username or password, please try again." });
        }

        var validationResult = await SignInUserValidation(user!);

        if (!validationResult.Succeeded)
        {
            return validationResult;
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);

        if (!result.Succeeded)
        {
            return Result.Failure(new string[] { "Wrong username or password, please try again." });
        }
        else
        {
            return result.ToApplicationResult();

        }
    }

    public async Task<Result> WindowsSignInAdminAsync(string username)
    {
        var user = await _userManager.FindByNameAsync(username);
        if (user is null)
        {
            return Result.Failure(new string[] { "Wrong username or password, please try again." });
        }

        var validationResult = await SignInAdminValidation(user!);

        if (!validationResult.Succeeded)
        {
            return validationResult;
        }

        await _signInManager.SignInAsync(user, false);

        return Result.Success();
    }

    public async Task<Result> WindowsSignInUserAsync(string username)
    {
        var user = await _userManager.FindByNameAsync(username);
        if (user is null)
        {
            return Result.Failure(new string[] { "Wrong username , please try again." });
        }

        var validationResult = await SignInUserValidation(user!);

        if (!validationResult.Succeeded)
        {
            return validationResult;
        }

        await _signInManager.SignInAsync(user, false);

        return Result.Success();
    }

    private async Task<Result> SignInValidation(ApplicationUser user)
    {
        if (!user.Active)
        {
            return Result.Failure(new string[] { "Your account is deactivated." });
        }

        bool isSystemAdmin = await _userManager.IsInRoleAsync(user, SystemAdmin.Name);

        if (!isSystemAdmin)
        {
            //var licenseInfo = await _sender.Send(new GetLicenseInfoQuery());

            //if (licenseInfo is null)
            //{
            //    return Result.Failure(new string[] { "Invalid system license." });
            //}
            //else if (licenseInfo.ExpiryDate.HasValue && licenseInfo.ExpiryDate < DateTime.Now)
            //{
            //    return Result.Failure(new string[] { "You can’t login, the system license has expired. Please contact the system administrator." });
            //}
        }

        return Result.Success();
    }
    private async Task<Result> SignInAdminValidation(ApplicationUser user)
    {
        if (!user.Active)
        {
            return Result.Failure(new string[] { "Your account is deactivated." });
        }

        bool isSystemAdmin = await _userManager.IsInRoleAsync(user, SystemAdmin.Name);

        if (!isSystemAdmin)
        {
            bool isAdmin = await _userManager.IsInRoleAsync(user, Admin.Name);

            if (!isAdmin)
            {
                return Result.Failure(new string[] { "Your account is not admin." });
            }
            //var licenseInfo = await _sender.Send(new GetLicenseInfoQuery());

            //if (licenseInfo is null)
            //{
            //    return Result.Failure(new string[] { "Invalid system license." });
            //}
            //else if (licenseInfo.ExpiryDate.HasValue && licenseInfo.ExpiryDate < DateTime.Now)
            //{
            //    return Result.Failure(new string[] { "You can’t login, the system license has expired. Please contact the system administrator." });
            //}
        }

        return Result.Success();
    }
    private async Task<Result> SignInUserValidation(ApplicationUser user)
    {
        if (!user.Active)
        {
            return Result.Failure(new string[] { "Your account is deactivated." });
        }
        bool isUser = await _userManager.IsInRoleAsync(user, User.Name);

        if (!isUser)
        {
            return Result.Failure(new string[] { "Your account is not user." });
        }
        //var licenseInfo = await _sender.Send(new GetLicenseInfoQuery());

        //if (licenseInfo is null)
        //{
        //    return Result.Failure(new string[] { "Invalid system license." });
        //}
        //else if (licenseInfo.ExpiryDate.HasValue && licenseInfo.ExpiryDate < DateTime.Now)
        //{
        //    return Result.Failure(new string[] { "You can’t login, the system license has expired. Please contact the system administrator." });
        //}

        return Result.Success();
    }

    public async Task SignOutAsync()
    {
        await _signInManager.SignOutAsync();
    }

    public async Task<List<UserWithRoleDto>> GetUsersWithRolesAsync(int start, int limit, string sortBy, bool isDesc, string userName)
    {
        var users = await _userManager.Users
            .Select(u => new UserWithRole { User = u, Roles = new() })
        .ToListAsync();

        if (!string.IsNullOrWhiteSpace(userName))
        {
            userName = userName.Trim().ToLower();
            users = users.Where(x => x.User.UserName.ToLower().Contains(userName) || x.User.FullName.ToLower().Contains(userName)).ToList();
        }

        var roleNames = await _roleManager.Roles.Select(r => r.Name).ToListAsync();

        foreach (var roleName in roleNames)
        {
            //For each role, fetch the users
            var usersInRole = await _userManager.GetUsersInRoleAsync(roleName);

            //Populate the roles for each user in memory
            var toUpdate = users.Where(u => usersInRole.Any(ur => ur.Id == u.User.Id));
            foreach (var user in toUpdate)
            {
                user.Roles.Add(roleName);
            }
        }


        sortBy = sortBy ?? "username";

        var usersOrdered = isDesc ? sortBy.ToLower() switch
        {
            "username" => users.OrderByDescending(x => x.User.UserName),
            "fullname" => users.OrderByDescending(x => x.User.FullName),
            "country" => users.OrderByDescending(x => x.User.Country),
            "state" => users.OrderByDescending(x => x.User.State),
            "email" => users.OrderByDescending(x => x.User.Email),
            "usertype" => users.OrderByDescending(x => x.Roles.Count > 0 ? x.Roles[0] : x.User.UserName),
            _ => users.OrderBy(x => x.User.UserName),
        } : sortBy switch
        {
            "username" => users.OrderBy(x => x.User.UserName),
            "fullname" => users.OrderBy(x => x.User.FullName),
            "country" => users.OrderBy(x => x.User.Country),
            "state" => users.OrderBy(x => x.User.State),
            "email" => users.OrderBy(x => x.User.Email),
            "usertype" => users.OrderBy(x => x.Roles.Count > 0 ? x.Roles[0] : x.User.UserName),
            _ => users.OrderBy(x => x.User.UserName),
        };

        var usersResult = usersOrdered.Skip(start).Take(limit);

        var userResultDto = _mapper.Map<List<UserWithRoleDto>>(usersResult);
        return userResultDto;
    }

    public async Task<UserWithRoleDto?> GetUserWithRolesAsync(string username)
    {
        var user = await _userManager.Users
            .Where(u => u.UserName == username)
            .Select(u => new UserWithRole { User = u, Roles = new() })
            .FirstOrDefaultAsync();

        if (user is null)
        {
            return null;
        }

        UserWithRoleDto userResultDto = await GetUserWithRoles(user);
        return userResultDto;
    }

    public async Task<UserWithRoleDto?> GetUserByEmailWithRolesAsync(string email)
    {
        var user = await _userManager.Users
            .Where(u => u.Email.ToLower() == email.ToLower())
            .Select(u => new UserWithRole { User = u, Roles = new() })
            .FirstOrDefaultAsync();

        if (user is null)
        {
            return null;
        }

        UserWithRoleDto userResultDto = await GetUserWithRoles(user);
        return userResultDto;
    }

    public async Task<UserDto> GetUserByUsername(string username)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == username);
        return _mapper.Map<UserDto>(user);
    }
    private async Task<UserWithRoleDto> GetUserWithRoles(UserWithRole? user)
    {
        var roleNames = await _roleManager.Roles.Select(r => r.Name).ToListAsync();

        var roles = await _userManager.GetRolesAsync(user.User);

        user.Roles.AddRange(roles);

        var userResultDto = _mapper.Map<UserWithRoleDto>(user);
        return userResultDto;
    }

    public async Task<int> GetTotalUsersCount()
    {
        var count = await _userManager.Users.CountAsync();
        return count;
    }



    public async Task<string?> GetUserIdAsync(string username)
    {
        var users = await _userManager.FindByNameAsync(username);
        return users.Id;
    }
    public async Task<UserInfoDto> GetUserInfoAsync(string username)
    {
        var users = await _userManager.FindByNameAsync(username);
        var userResultDto = _mapper.Map<UserInfoDto>(users);

        return userResultDto;
    }
    public async Task<UserInfoDto?> GetUserInfoByIdAsync(string userid)
    {
        var users = await _userManager.FindByIdAsync(userid);
        var userResultDto = _mapper.Map<UserInfoDto>(users);

        return userResultDto;
    }
    public async Task<UserInfoDto?> GetUserInfoByEmailAsync(string useremail)
    {
        var users = await _userManager.FindByEmailAsync(useremail);
        var userResultDto = _mapper.Map<UserInfoDto>(users);

        return userResultDto;
    }

    public async Task<bool> RestReadSignConsentAsync()
    {
        var users = await _userManager.Users.ToListAsync();
        foreach (var user in users)
        {
            user.IsReadSignConsent = false;
            await _userManager.UpdateAsync(user);
        }

        return true;
    }
}
