using Gooios.UserService;
using Gooios.UserService.Configurations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.UserServiceHost.Filters
{
    public class ApiKeyAttribute:ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var config = IocProvider.GetService<IServiceConfigurationAgent>();
            context.HttpContext.Request.Headers.TryGetValue(config.ApiHeaderKey, out Microsoft.Extensions.Primitives.StringValues vals);

            if (vals.Count == 0)
            {
                context.Result = new BadRequestResult();
                return;
            }

            var val = vals.ToArray().GetValue(0)?.ToString();

            if (val != config.ApiHeaderValue) context.Result = new BadRequestResult();
            //base.OnActionExecuting(context);
        }
    }
}
