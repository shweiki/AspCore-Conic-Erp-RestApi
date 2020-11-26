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
    [Authorize]
    public class RoleController : Controller
    {

        private RoleManager<IdentityRole> _roleManager;
        private ConicErpContext DB = new ConicErpContext();


        public RoleController( RoleManager<IdentityRole> roleManager)
        {
  
            _roleManager = roleManager;

        }

        [HttpGet]
        [Route("Role/GetRoles")]
        public  IActionResult GetRoles()
        {
            var Roles = (from x in DB.Roles.ToList()
                         select new
                         {
                             x.Id,
                             x.Name,
                             x.NormalizedName,
                             x.ConcurrencyStamp,

                         }).ToList();
            return Ok(DB.Roles.ToList());
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
                else {
                    IsExsit.UserId = UserRouter.UserId;
                    IsExsit.Router = UserRouter.Router;
                }
                DB.SaveChanges();
                return Ok(UserRouter);
            }
            else return Ok(false);

        }
        [HttpPost]
        [Route("Role/Edit")]
        public IActionResult Edit(IdentityRole Role)
        {
            var role = DB.Roles.Where(x => x.Id == Role.Id).SingleOrDefault();
            role.Name = Role.Name;
            role.ConcurrencyStamp = Role.ConcurrencyStamp;
            role.NormalizedName = Role.NormalizedName;
            DB.SaveChanges();
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
}
