using Application.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace RestApi.Controllers;

public class UserController : Controller
{
    private readonly ILogger<UserController> _logger;
    private readonly IApplicationDbContext DB;
    private readonly IIdentityService _identityService;

    public UserController(IApplicationDbContext dbcontext, ILogger<UserController> logger, IIdentityService identityService)
    {

        _logger = logger;
        DB = dbcontext;
        _identityService = identityService;

    }



    // POST User/Login
    [AllowAnonymous]
    [HttpPost]
    [Route("User/Login")]
    public async Task<IActionResult> Login(Userlogin model)
    {

        if (model.Username != "" && model.Password != "")
        {
            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, 
            // set lockoutOnFailure: true
            var result = await _identityService.PasswordSignInAsync(model.Username, model.Password);
            if (result.Succeeded)
            {
                _logger.LogInformation("User logged in.");


                return Ok("User logged in.");
            }
            //if (result.RequiresTwoFactor)
            //{
            //    return BadRequest("RequiresTwoFactor");

            //}
            //if (result.IsLockedOut)
            //{

            //    return BadRequest("User account locked out.");

            //}
            else
            {
                //  ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return Ok("User Name Or PassWord Is Not Correct");
            }
        }
        else return BadRequest("null");

    }

    [Authorize]
    [HttpPost]
    [Route("User/Info")]
    public async Task<IActionResult> Info()
    {
        UserResponse response = new UserResponse();
        var user = await _identityService.GetUserWithRolesAsync(User.Identity.Name);
        var userInfo = await _identityService.GetUserInfoAsync(User.Identity.Name);

        response.name = user.User.FullName;
        response.phone = user.User.PhoneNumber;
        response.introduction = "I am a super hero";
        response.avatar = Url.Content("~/Images/User/" + user.User.UserName + ".jpeg");
        response.userrouter = DB.UserRouter.Where(x => x.UserId == userInfo.Id)?.SingleOrDefault()?.Router;
        response.defulateRedirect = DB.UserRouter.Where(x => x.UserId == userInfo.Id)?.SingleOrDefault()?.DefulateRedirect;
        response.roles = user.Roles.Select(s => s.ToLower()).ToArray();
        return Ok(response);
    }

    [Authorize]
    [HttpPost]
    [Route("User/Logout")]
    public async Task<IActionResult> Logout()
    {
        await _identityService.SignOutAsync();
        _logger.LogInformation("User logged out.");
        return Ok("User logged out.");

    }

    // POST User/Register
    [HttpPost]
    [Route("User/Register")]
    public async Task<ActionResult> Register(UserRegister model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var user = new IdentityUser() { UserName = model.UserName, Email = model.Email, PhoneNumber = model.PhoneNumber, PhoneNumberConfirmed = true, EmailConfirmed = true };

        (var result, _) = await _identityService.CreateUserAsync(user.UserName, model.Password, user.UserName, user.Email, user.PhoneNumber, user.UserName, "", "", true);

        if (!result.Succeeded)
        {
            return Ok(result);
        }

        return Ok();
    }

    [HttpPost]
    [Route("User/GetUsers")]
    public async Task<IActionResult> GetUsers()
    {
        var Users = await _identityService.GetUsersWithRolesAsync(0, 200, null, false, null);
        //var Users = (from x in DB.Users.ToList()
        //             select new
        //             {
        //                 x.Id,
        //                 x.Email,
        //                 x.UserName,
        //                 x.PhoneNumber,
        //                 avatar = Url.Content("~/Images/User/" + x.Id + ".jpeg"),
        //                 router = DB.UserRouter.Where(ur => ur.UserId == x.Id)?.SingleOrDefault()?.Router,
        //                 Redirect = DB.UserRouter.Where(ur => ur.UserId == x.Id)?.SingleOrDefault()?.DefulateRedirect,
        //                 Roles = (from R in DB.UserRoles.Where(ur => ur.UserId == x.Id).ToList()
        //                          let p = new
        //                          {
        //                              DB.Roles.Where(r => r.Id == R.RoleId).SingleOrDefault().Id,
        //                              DB.Roles.Where(r => r.Id == R.RoleId).SingleOrDefault().Name
        //                          }
        //                          select p).ToList(),
        //             }).ToList();
        return Ok(Users);
    }
    [HttpPost]
    [Route("User/GetUsersNames")]
    public async Task<IActionResult> GetUsersNames()
    {
        var Users = await _identityService.GetUsersWithRolesAsync(0, 200, null, false, null);

        // var Users = DB.Users.Select(x => new { x.UserName }).ToList();
        return Ok(Users);
    }
    [HttpPost]
    [Route("User/UnLockout")]
    public async Task<IActionResult> UnLockout(string UserId)
    {

        //var user = await _identityService.GetUserInfoByIdAsync(UserId);
        //var result = await _userManager.SetLockoutEnabledAsync(user, true);
        //if (result.Succeeded)
        //    return Ok(true);
        //else return Ok(false);
        return Ok(true);
    }
    [HttpPost]
    [Route("User/ChangePassword")]
    public async Task<IActionResult> ChangePassword(string OldPassword, string NewPassword)
    {
        var result = _identityService.ChangePasswordAsync(User.Identity.Name, OldPassword, NewPassword); // Get user id:
        if (result.IsCompleted)
            return Ok(true);
        else return Ok(false);
    }

    [HttpPost]
    [Route("User/AddRoleForUser")]
    public async Task<IActionResult> AddRoleForUser(string UserName, string RoleName)
    {
        var result = await _identityService.AddUserToRoleAsync(RoleName, UserName);

        if (result.Succeeded)
            return Ok(true);
        else return Ok(false);
    }
    [HttpPost]
    [Route("User/DeleteRoleForUser")]
    public async Task<IActionResult> DeleteRoleForUser(string UserName, string RoleName)
    {
        var user = await _identityService.GetUserWithRolesAsync(UserName);

        var result = await _identityService.AddUserToRolesAsync(user.Roles.ToArray(), UserName, true);

        if (result.Succeeded)
            return Ok(true);
        else return Ok(false);
    }

    public class Userlogin
    {
        public string Username { get; set; }
        public string Password { get; set; }

    }
    public class UserRegister
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }

    }
    public class UserResponse
    {
        public string Id { get; set; }
        public string name { get; set; }
        public string phone { get; set; }
        public string[] roles { get; set; }
        public string avatar { get; set; }
        public string userrouter { get; set; }
        public string defulateRedirect { get; set; }
        public string introduction { get; set; }

    }

}