using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.ComponentModel.DataAnnotations;
using Gooios.Infrastructure.Exceptions;
using System;

namespace Gooios.ImagesService.Filters
{
    public class ApiExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is ValidationException)
            {
                context.Result = new ObjectResult(new { Message = context.Exception.Message });
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.NotAcceptable;

                context.ExceptionHandled = true;
                return;
            }
            if (context.Exception is AppServiceException)
            {
                context.Result = new ObjectResult(new { Message = context.Exception.Message });
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.NotAcceptable;
                context.ExceptionHandled = true;
                return;
            }
            if (context.Exception is ArgumentException)
            {
                context.Result = new ObjectResult(new { Message = context.Exception.Message });
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.NotAcceptable;
                context.ExceptionHandled = true;
                return;
            }
            if (context.Exception is ApiException)
            {
                context.Result = new ObjectResult(new { Message = context.Exception.Message });
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