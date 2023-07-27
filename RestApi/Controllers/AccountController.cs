using Application.Common.Interfaces;
using Application.Common.Models;
using Microsoft.AspNetCore.Mvc;
using RestApi.Helper;
using RestApi.Models;

namespace RestApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly JwtSettings _jwtSettings;
    private readonly IIdentityService _identityService;

    public AccountController(
    JwtSettings jwtSettings,
    IIdentityService identityService
    )
    {
        _jwtSettings = jwtSettings;
        _identityService = identityService;
    }

    [HttpPost]
    public async Task<IActionResult> TokenAsync([FromBody] InputUserModel InputModel)
    {
        if (ModelState.IsValid)
        {
            try
            {
                var Token = new UserTokens();
                Result result = await _identityService.PasswordSignInUserForApiAsync(InputModel.Username, InputModel.Password);

                if (result.Succeeded)
                {
                    UserInfoDto user = await _identityService.GetUserInfoAsync(InputModel.Username);
                    Token = JwtHelpers.GenTokenkey(new UserTokens()
                    {
                        EmailId = user.Email,
                        GuidId = Guid.NewGuid(),
                        UserName = user.UserName,
                        Id = user.Id
                    }, _jwtSettings);
                }
                else
                {
                    return BadRequest(result.Errors.FirstOrDefault() ?? $"Invalid login attempt");
                }
                return Ok(Token);
            }
            catch (Exception)
            {
                return BadRequest(ModelState.ToString());
            }
        }
        else return BadRequest(ModelState.ToString());
    }
}
