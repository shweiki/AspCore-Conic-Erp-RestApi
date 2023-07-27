using Microsoft.AspNetCore.Identity;
using Application.Common.Interfaces;
using System.Text;

namespace Infrastructure.Identity;

public class PasswordGenerator : IPasswordGenerator
{
    private readonly UserManager<ApplicationUser> _userManager;

    public PasswordGenerator(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public string GeneratePassword()
    {
        var options = _userManager.Options.Password;

        int length = options.RequiredLength;
        bool nonAlphanumeric = options.RequireNonAlphanumeric;
        bool digit = options.RequireDigit;
        bool lowercase = options.RequireLowercase;
        bool uppercase = options.RequireUppercase;

        StringBuilder password = new();
        Random random = new();

        while (password.Length < length)
        {
            char c = (char)random.Next(32, 126);

            password.Append(c);

            if (char.IsDigit(c))
                digit = false;
            else if (char.IsLower(c))
                lowercase = false;
            else if (char.IsUpper(c))
                uppercase = false;
            else if (!char.IsLetterOrDigit(c))
                nonAlphanumeric = false;
        }

        if (nonAlphanumeric)
            password.Append((char)random.Next(33, 48));
        if (digit)
            password.Append((char)random.Next(48, 58));
        if (lowercase)
            password.Append((char)random.Next(97, 123));
        if (uppercase)
            password.Append((char)random.Next(65, 91));

        return password.ToString();
    }
}
