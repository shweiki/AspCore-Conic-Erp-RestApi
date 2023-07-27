using MediatR;
using Application.Common.Enums;
using Application.Common.Interfaces;

namespace Application.Features.Role.Commands.AddDefaultRoles;

public class AddDefaultRolesCommand : IRequest<int>
{
}

public class AddUserCommandHandler : IRequestHandler<AddDefaultRolesCommand, int>
{
    private readonly IIdentityService _identityService;

    public AddUserCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<int> Handle(AddDefaultRolesCommand request, CancellationToken cancellationToken)
    {
        // SystemAdmin: The default user which will be added automatically.
        // Admin: User added by another user and given admin privileges, and also has normal user privileges.
        // User: User that has priviliges for the workspace but not the admninistration.
        // Signer: User that has priviliges to sign documents.
        string[] roleNames = { RolesEnum.SystemAdmin.Name, RolesEnum.Admin.Name, RolesEnum.User.Name };

        string defaultAdminUsername = "Developer";
        string defaultAdminFullNname = "Developer";
        string defaultAdminTitle = "System Administrator";
        string defaultAdminPassword = "Taha123456++"; // TODO: Should be removed from code, and the hash added by sql.

        foreach (string roleName in roleNames)
        {
            var roleExist = await _identityService.RoleExistsAsync(roleName);
            if (!roleExist)
            {
                await _identityService.CreateRoleAsync(roleName); // TODO: Add error checking and handling.
            }
        }

        bool userExists = await _identityService.UserExistsByNameAsync(defaultAdminUsername);

        if (!userExists)
        {
            (var result, _) = await _identityService.CreateUserAsync(defaultAdminUsername, defaultAdminPassword,
                defaultAdminFullNname, "test@example.com", "", defaultAdminTitle, "", "", true);

            if (result.Succeeded)  // TODO: Add error checking and handling.
            {
                await _identityService.AddUserToRoleAsync("SystemAdmin", defaultAdminUsername);  // TODO: Add error checking and handling.
            }
        }

        return 0;
    }
}