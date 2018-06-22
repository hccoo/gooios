using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Gooios.AuthorizationService.Models;
using Gooios.AuthorizationService.Models.RoleModels;

namespace Gooios.AuthorizationService.ApiControllers
{
    [Produces("application/json")]
    [Route("api/Role")]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Post([FromBody]AddRoleRequestModel model)
        {
            var result = await _roleManager.CreateAsync(new IdentityRole { Name=model.Name });
            if (result.Succeeded)
                return new OkResult();

            return new BadRequestObjectResult("创建角色失败.");
        }
    }
}