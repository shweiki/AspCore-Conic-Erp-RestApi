using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity;

public class ApplicationUser : IdentityUser
{
    public string? Title { get; set; }
    public string? FullName { get; set; }
    public string? Country { get; set; }
    public string? State { get; set; }
    public bool Active { get; set; }
    public bool IsReadSignConsent { get; set; }
}