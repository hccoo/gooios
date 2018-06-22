using Gooios.LogService.Configurations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace Gooios.LogService.Filters
{
    public class ApiKeyFilter : IActionFilter
    {
        IServiceConfigurationProxy _config;
        public ApiKeyFilter(IServiceConfigurationProxy config)
        {
            _config = config;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // To do something...
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            context.HttpContext.Request.Headers.TryGetValue(_config.ApiHeaderKey,out Microsoft.Extensions.Primitives.StringValues vals);

            if(vals.Count==0)
            {
                context.Result = new BadRequestResult();
                return ; 
            }

            var val = vals.ToArray().GetValue(0)?.ToString();

            if(val != _config.ApiHeaderValue) context.Result = new BadRequestResult();
        }
    }
}