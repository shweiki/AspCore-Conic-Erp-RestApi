using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AspCore_Conic_Erp_RestApi.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<UserController> _logger;
        private ConicErpContext DB = new ConicErpContext();

        public UserController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, ILogger<UserController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;

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
                var result = await _signInManager.PasswordSignInAsync(model.Username,
                                   model.Password, model.RememberMe, lockoutOnFailure: true);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    //var userID = User.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;

                    return Ok("User logged in.");
                }
                if (result.RequiresTwoFactor)
                {
                    return Ok("RequiresTwoFactor");

                }
                if (result.IsLockedOut)
                {

                    return Ok("User account locked out.");

                }
                else
                {
                    //  ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Ok("User Name Or PassWord Is Not Correct");
                }
            }
            else return Ok("Fack u");

        }

        [Authorize]
        [HttpPost]
        [Route("User/Info")]
        public async Task<IActionResult> Info()
        {
            var id = _userManager.GetUserId(User); // Get user id:
            UserResponse response = new UserResponse();
            IdentityUser user = await _userManager.FindByIdAsync(id);
            var roles = await _userManager.GetRolesAsync(user);
            roles.Add("Gest");
            response.Id = id;
            response.name = User.Identity.Name;
            response.phone = long.Parse(user.PhoneNumber);
            response.introduction = "I am a super hero";
            response.avatar = DB.FileData.Where(x => x.TableName == "User" && x.Fktable == long.Parse(user.PhoneNumber))?.ToList()?.LastOrDefault()?.File;
            // Url.Content("~/Images/User/" + long.Parse() + ".jpeg");
            response.userrouter = DB.UserRouter.Where(x => x.UserId == id)?.SingleOrDefault()?.Router;
            response.defulateRedirect = DB.UserRouter.Where(x => x.UserId == id)?.SingleOrDefault()?.DefulateRedirect;
            response.roles = roles.ToArray();
            return Ok(response);
        }

        [Authorize]
        [HttpPost]
        [Route("User/Logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            return Ok("User logged out.");

        }

        // POST User/Register
        [Route("User/Register")]
        public async Task<ActionResult> Register(UserRegister model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new IdentityUser() { UserName = model.UserName, Email = model.Email, PhoneNumber = model.PhoneNumber, PhoneNumberConfirmed = true, EmailConfirmed = true };

            IdentityResult result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return Ok(result);
            }

            return Ok();
        }

        [HttpPost]
        [Route("User/GetUsers")]
        public IActionResult GetUsers()
        {
            var Users = (from x in DB.Users.ToList()
                         select new
                         {
                             x.Id,
                             x.Email,
                             x.UserName,
                             x.PhoneNumber,
                             avatar = Url.Content("~/Images/User/" + x.Id + ".jpeg"),
                             router = DB.UserRouter.Where(ur => ur.UserId == x.Id)?.SingleOrDefault()?.Router,
                             Redirect = DB.UserRouter.Where(ur => ur.UserId == x.Id)?.SingleOrDefault()?.DefulateRedirect,
                             Roles = (from R in DB.UserRoles.Where(ur => ur.UserId == x.Id).ToList()
                                      let p = new
                                      {
                                          Id = DB.Roles.Where(r => r.Id == R.RoleId).SingleOrDefault().Id,
                                          Name = DB.Roles.Where(r => r.Id == R.RoleId).SingleOrDefault().Name
                                      }
                                      select p).ToList(),
                         }).ToList();
            return Ok(Users);
        }
        [HttpPost]
        [Route("User/UnLockout")]
        public async Task<IActionResult> UnLockout(string UserId)
        {
            
            IdentityUser user = await _userManager.FindByIdAsync(UserId);
            var result = await _userManager.SetLockoutEnabledAsync(user, true);
            if (result.Succeeded)
                return Ok(true);
            else return Ok(false);
        }
        [HttpPost]
        [Route("User/ChangePassword")]
        public async Task<IActionResult> ChangePassword(string OldPassword, string NewPassword)
        {
            var id = _userManager.GetUserId(User); // Get user id:
            IdentityUser user = await _userManager.FindByIdAsync(id);
            IdentityResult result = await _userManager.ChangePasswordAsync(user, OldPassword, NewPassword);
            if (result.Succeeded)
                return Ok(true);
            else return Ok(false);
        }

        [HttpPost]
        [Route("User/AddRoleForUser")]
        public async Task<IActionResult> AddRoleForUser(string UserName, string RoleName)
        {
            IdentityUser user = await _userManager.FindByNameAsync(UserName);

            await _userManager.AddToRoleAsync(user, RoleName);

            return Ok(true);
        }
        [HttpPost]
        [Route("User/DeleteRoleForUser")]
        public async Task<IActionResult> DeleteRoleForUser(string UserName, string RoleName)
        {
            IdentityUser user = await _userManager.FindByNameAsync(UserName);
            await _userManager.RemoveFromRoleAsync(user, RoleName);
            return Ok(true);

        }
        public string GetUserId()
        {
            var id = _userManager.GetUserId(User); // Get user id:

            return id;
        }
        public class Userlogin
        {
            public string Username { get; set; }
            public string Password { get; set; }
            public bool RememberMe { get; set; }

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
            public long phone { get; set; }
            public string[] roles { get; set; }
            public string avatar { get; set; }
            public string userrouter { get; set; }
            public string defulateRedirect { get; set; }
            public string introduction { get; set; }

        }


    }
}
