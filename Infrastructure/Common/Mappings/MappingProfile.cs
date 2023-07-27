using AutoMapper;
using Application.Common.Enums;
using Application.Common.Models;
using Infrastructure.Identity;

namespace Infrastructure.Common.Mappings;

public class MappingProfile : Profile
{
    public string RolesToString(List<string> roles)
    {
        if (roles.Contains(RolesEnum.SystemAdmin.Name)) // TODO: use constants for role names
        {
            return RolesEnum.SystemAdmin.Display;
        }
        else if (roles.Contains(RolesEnum.Admin.Name))
        {
            return RolesEnum.Admin.Display;
        }
        else if (roles.Contains(RolesEnum.User.Name))
        {
            return RolesEnum.User.Display;
        }
        else // Ignore the Signer role
        {
            return "";
        }
    }

    public MappingProfile()
    {
        CreateMap<UserWithRole, UserWithRoleDto>()
            .ForMember(x => x.HighestRole, options => options
            .MapFrom(y => RolesToString(y.Roles)));
        CreateMap<ApplicationUser, UserDto>().ReverseMap();


        CreateMap<ApplicationUser, UserInfoDto>().ReverseMap();
    }
}
