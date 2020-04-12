using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.ComponentModel.DataAnnotations;
using Gooios.Infrastructure.Exceptions;

namespace Gooios.ActivityService.Filters
{
    public class ApiExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception?.InnerException is ValidationException)
            {
                context.Result = new ObjectResult(new { Message = context.Exception?.InnerException?.Message });
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.NotAcceptable;

                context.ExceptionHandled = true;
                return;
            }
            if (context.Exception?.InnerException is AppServiceException)
            {
                context.Result = new ObjectResult(new { Message = context.Exception?.InnerException?.Message });
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.NotAcceptable;
                context.ExceptionHandled = true;
                return;
            }
            if (context.Exception?.InnerException is ArgumentInvalidException)
            {
                context.Result = new ObjectResult(new { Message = context.Exception?.InnerException?.Message });
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.NotAcceptable;
                context.ExceptionHandled = true;
                return;
            }
            if (context.Exception?.InnerException is ApiException)
            {
                context.Result = new ObjectResult(new { Message = context.Exception?.InnerException?.Message });
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.NotAcceptable;
                context.ExceptionHandled = true;
                return;
            }
            else
            {
                context.Result = new ObjectResult(new { Message = "Inner server error. Please try again later." + context.Exception.Message });
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.ExceptionHandled = true;
                return;
            }

            //To do save the exception by context.Exception
        }
    }
}