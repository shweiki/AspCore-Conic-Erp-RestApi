namespace Application.Common.Models;

public class UsersListDto
{
    public UsersListDto()
    {
        Items = new List<UserWithRoleDto>();
    }

    public IList<UserWithRoleDto> Items { get; set; }
    public int TotalCount { get; set; }
}
