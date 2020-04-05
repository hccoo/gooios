using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NLog;
using System.Linq;

namespace Gooios.UserServiceHost.Filters
{
    public class LogFilter : IActionFilter
    {
        Logger logger = LogManager.GetLogger("LogFilter");
        public void OnActionExecuted(ActionExecutedContext context)
        {
            logger.Info("Hello logger£¡~~~~~");

            var ret = context.Result as ObjectResult;
            var objstr = JsonConvert.SerializeObject(ret);

            logger.Info($"result:{objstr}");
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var parameters = context.ActionArguments;
            var controller = context.Controller.ToString();
            var actionName = context.ActionDescriptor.DisplayName;

            var patams = string.Join(";", parameters.Select(o => string.Join("=", o.Key, o.Value)));
            logger.Info($"parameters:{patams}");
        }
    }
}