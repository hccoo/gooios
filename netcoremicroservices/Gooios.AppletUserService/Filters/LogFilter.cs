using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace Gooios.AppletUserService.Filters
{
    public class LogFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            var ret = context.Result as ObjectResult;

            var objstr = JsonConvert.SerializeObject(ret);

            //To do: log the data to db.
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var parameters = context.ActionArguments;
            var controller = context.Controller.ToString();
            var actionName = context.ActionDescriptor.DisplayName;

            //To do: log the parameters to db.
        }
    }
}