using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.UserServiceHost.Filters
{
    public class TokenAuthorizationFilter : AuthorizeAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var token = context.HttpContext.User.FindFirst("token")?.Value ?? "";
            var userName= context.HttpContext.User.FindFirst("username")?.Value ?? "";
            //var usService = IocProvider.GetService<IUserSessionAppService>();
            var result = true;//usService.Verify(userName, token);

            if (!result) context.Result = new RedirectResult("/User/Login");
        }
    }
}
