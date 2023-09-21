
namespace Application.Common.Models;

public class UserWithRoleDto
{
    public UserInfoDto User { get; set; } = new();
    public List<string> Roles { get; set; } = new();
    public string HighestRole { get; set; } = "";
}