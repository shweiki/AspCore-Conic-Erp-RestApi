using Application.Common.Models;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity;

public static class IdentityResultExtensions
{
    public static Result ToApplicationResult(this IdentityResult result)
    {
        return result.Succeeded
            ? Result.Success()
            : Result.Failure(result.Errors.Select(e => e.Description));
    }

    public static Result ToApplicationResult(this SignInResult result)
    {
        return result.Succeeded
            ? Result.Success()
            : Result.Failure(new List<string>());
    }
}
