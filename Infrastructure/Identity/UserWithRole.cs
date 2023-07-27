namespace Infrastructure.Identity;

public class UserWithRole
{
    public ApplicationUser User { get; set; } = new();
    public List<string> Roles { get; set; } = new();
}
