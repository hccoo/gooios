using Gooios.AuthorizationService.Configurations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.AuthorizationService.Attributes
{
    public class ApiKeyAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            context.HttpContext.Request.Headers.TryGetValue("gooiosapikey", out Microsoft.Extensions.Primitives.StringValues vals);

            if (vals.Count == 0)
            {
                context.Result = new BadRequestResult();
                return;
            }

            var val = vals.ToArray().GetValue(0)?.ToString();

            if (val != "6de960be9183367800160026c7e9f3d2") context.Result = new BadRequestResult();

        }
    }
}
