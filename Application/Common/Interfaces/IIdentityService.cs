using Application.Common.Models;

namespace Application.Common.Interfaces;

public interface IIdentityService
{
    Task<(Result Result, string UserId)> CreateUserAsync(string userName, string password, string fullName,
        string email, string phone, string title, string? country, string? state, bool isActive);
    Task<string> GeneratePasswordResetTokenByEmailAsync(string email);
    Task<string> GenerateOTPCodeForEmailAsync(string username);
    Task<string> GenerateOTPCodeForPhoneAsync(string username);
    Task<bool> VerifyOTPPhoneWithCodeAsync(string username, string code);
    Task<bool> VerifyOTPEmailWithCodeAsync(string username, string code);
    Task<string> GenerateUserTokenAsync(string username);
    Task<bool> VerifyUserTokenAsync(string username, string token);
    Task<Result> ResetPasswordWithTokenAsync(string email, string token, string newPassword);
    Task<Result> ChangePasswordAsync(string userName, string currentPassword, string newPassword);
    Task<Result> UpdateUserAsync(string userName, string fullName,
        string email, string phone, string title, string? country, string? state, bool isActive);
    Task<Result> CreateRoleAsync(string roleName);
    Task<Result> UpdateUserStatusAsync(string userName, bool isActive);
    Task<Result> UpdateUserIsReadedConsentAsync(string userName, bool isReaded);
    Task<Result> UpdateUserPhoneAsync(string userName, string phone);
    Task<bool> RoleExistsAsync(string roleName);
    Task<bool> UserExistsByNameAsync(string username);
    Task<Result> AddUserToRoleAsync(string roleName, string username);
    Task<Result> AddUserToRolesAsync(string[] roleNames, string username, bool removeOtherRoles = false);
    Task<Result> PasswordSignInAsync(string username, string password);
    Task<Result> WindowsUserSignInAsync(string username);
    Task<Result> PasswordSignInAdminAsync(string username, string password);
    Task<Result> WindowsSignInAdminAsync(string username);
    Task<Result> PasswordSignInUserAsync(string username, string password);
    Task<Result> PasswordSignInUserForApiAsync(string username, string password);
    Task<Result> WindowsSignInUserAsync(string username);
    Task<List<UserWithRoleDto>> GetUsersWithRolesAsync(int start, int limit, string sortBy, bool isDesc, string userName);
    Task<UserWithRoleDto?> GetUserWithRolesAsync(string username);
    Task<UserWithRoleDto?> GetUserByEmailWithRolesAsync(string username);
    Task<string?> GetUserIdAsync(string username);
    Task<UserInfoDto> GetUserInfoAsync(string username);
    Task<UserInfoDto?> GetUserInfoByIdAsync(string userid);
    Task<UserInfoDto?> GetUserInfoByEmailAsync(string userid);


    Task<int> GetTotalUsersCount();
    Task SignOutAsync();
    Task<UserDto> GetUserByUsername(string username);
    Task<bool> RestReadSignConsentAsync();
}
