namespace Application.Common.Models;

public class UserDto
{
    public string? UserName { get; set; }
    public string? FullName { get; set; }
    public string? Title { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Country { get; set; }
    public string? Type { get; set; }
    public string? State { get; set; }
    public bool Active { get; set; }
}
