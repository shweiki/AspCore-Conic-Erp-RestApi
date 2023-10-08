using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace RestApi.Controllers;

[Authorize]
public class RoleController : Controller
{

    private RoleManager<IdentityRole> _roleManager;
    private readonly IApplicationDbContext DB;

    public RoleController(IApplicationDbContext dbcontext, RoleManager<IdentityRole> roleManager)
    {
        _roleManager = roleManager;
        DB = dbcontext;
    }

    [HttpGet]
    [Route("Role/GetRoles")]
    public IActionResult GetRoles()
    {
        var Roles = _roleManager.Roles.Where(x => x.Name != "SystemAdmin").ToList();
        return Ok(Roles);
    }

    [HttpPost]
    [Route("Role/AddRole")]
    public async Task<IActionResult> AddRole(IdentityRole Role)
    {
        Role.Id = Guid.NewGuid().ToString();
        await _roleManager.CreateAsync(Role);
        return Ok(Role.Id);
    }
    [HttpPost]
    [Route("Role/AddUserRouter")]
    public IActionResult AddUserRouter(UserRouter UserRouter)
    {
        if (UserRouter.Router != null && UserRouter.UserId != null)
        {
            var IsExsit = DB.UserRouter.Where(x => x.UserId == UserRouter.UserId).SingleOrDefault();
            if (IsExsit == null)
            {
                DB.UserRouter.Add(UserRouter);
            }
            else
            {
                IsExsit.UserId = UserRouter.UserId;
                IsExsit.Router = UserRouter.Router;
                IsExsit.DefulateRedirect = UserRouter.DefulateRedirect;
            }
            DB.SaveChanges();
            return Ok(UserRouter);
        }
        else return Ok(false);

    }
    [HttpPost]
    [Route("Role/Edit")]
    public async Task<IActionResult> Edit(IdentityRole Role)
    {
        await _roleManager.UpdateAsync(Role);

        //var role = DB.Roles.Where(x => x.Id == Role.Id).SingleOrDefault();
        //role.Name = Role.Name;
        //role.ConcurrencyStamp = Role.ConcurrencyStamp;
        //role.NormalizedName = Role.NormalizedName;
        //DB.SaveChanges();
        return Ok(true);

    }


    [HttpPost]
    [Route("Role/DeleteRole")]
    public async Task<IActionResult> DeleteRole(IdentityRole Role)
    {
        await _roleManager.DeleteAsync(Role);
        return Ok(Role);

    }

}
